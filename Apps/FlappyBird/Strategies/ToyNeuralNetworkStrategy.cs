﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using IanNet;
using System.Drawing;
using System.Diagnostics;

namespace IanAutomation.Apps.FlappyBird.Strategies
{
    public class ToyNeuralNetworkStrategy : IStrategy
    {
        public FlappyBird Page;
        public ToyNeuralNetwork Net;
        public int? previousY = null;
        public int? birdX = null;
        public float? previousVelocityY = null;
        public Stopwatch velocityStopwatch;

        // You get better results by adjusting which pipes you ignore for being too far back
        public int BirdLocationOffset = 25;

        public ToyNeuralNetworkStrategy(FlappyBird Page, Stopwatch VelocityStopwatch)
        {
            SetPage(Page);
            Net = new ToyNeuralNetwork(5, 5, 1);
            velocityStopwatch = VelocityStopwatch;
        }

        public void SetPage(FlappyBird Page)
        {
            this.Page = Page;
        }

        public BirdAction Strategize()
        {
            BirdAction action = BirdAction.Nothing;

            Mat GameImage = new Mat();
            Page.GetScreenshot(GameImage);

            // check for game over
            if (Page.IsGameOver(GameImage))
                return BirdAction.Restart;

            Point? BirdLocation = Page.DetectBird(GameImage);
            
            if (BirdLocation == null)
                return BirdAction.Nothing;

            List<Point> BottomHalfPipes = Page.DetectBottomHalfPipes(GameImage);
            Point closestBottomHalfPipe = DetermineClosestPipe(BottomHalfPipes, BirdLocation);
            List<Point> TopHalfPipes = Page.DetectTopHalfPipes(GameImage);
            Point closestTopHalfPipe = DetermineClosestPipe(TopHalfPipes, BirdLocation);

            float velocityY;
            if (previousY != null)
                velocityY = ((float)(BirdLocation.Value.Y - previousY.Value)) / velocityStopwatch.ElapsedMilliseconds;
            else
                velocityY = 0;
            
            //float[] inputs = new float[] 
            //{ 
            //    0,
            //    0,
            //    0, 
            //    0, 
            //    0
            //};
            
            float[] inputs = new float[]
            {
                (float) BirdLocation.Value.Y / GameImage.Height,
                (float) velocityY / GameImage.Height,
                (float) closestBottomHalfPipe.X / GameImage.Width, 
                (float) closestBottomHalfPipe.Y / GameImage.Height, 
                (float) closestTopHalfPipe.Y / GameImage.Height
            };
            float[] outputs = Net.Forward(inputs);

            // return true if the page should flap
            if (outputs[0] > 0.5)
                action = BirdAction.Flap;

            previousY = BirdLocation.Value.Y;
            previousVelocityY = velocityY;
            velocityStopwatch.Restart();
            return action;
        }

        private Point DetermineClosestPipe(List<Point> BottomPipes, Point? BirdLocation)
        {
            // the point itself is the top-left corner
            Point closestPoint = new Point(int.MaxValue, 1);
            foreach (Point p in BottomPipes)
            {
                if (p.X < closestPoint.X)
                    if (BirdLocation == null || (p.X > BirdLocation.Value.X + BirdLocationOffset))
                        closestPoint = p;
            }

            return closestPoint;
        }

        public void SetToyNeuralNetwork(ToyNeuralNetwork toyNeuralNetwork)
        {
            Net = toyNeuralNetwork;
            velocityStopwatch.Restart();
        }
    }
}
