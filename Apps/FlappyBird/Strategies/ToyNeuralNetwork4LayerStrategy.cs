using System;
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
    public class ToyNeuralNetwork4LayerStrategy : IStrategy
    {
        public FlappyBird Page;
        public ToyNeuralNetwork4Layer Net;
        public int? previousY = null;
        public int? birdX = null;
        public float? previousVelocityY = null;
        public Stopwatch velocityStopwatch;

        public Point? BirdLocation;
        public Point closestBottomHalfPipe;
        public Point closestTopHalfPipe;
        public float velocityY;
        public float[] inputs;
        public float output;

        // You get better results by adjusting which pipes you ignore for being too far back
        public int BirdLocationOffset = 25;

        public ToyNeuralNetwork4LayerStrategy(FlappyBird Page, Stopwatch VelocityStopwatch)
        {
            SetPage(Page);
            Net = new ToyNeuralNetwork4Layer(5, 5, 5, 1, learningRate: 1f);
            velocityStopwatch = VelocityStopwatch;
            CvInvoke.NamedWindow("Flappy Bird");

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

            // find the bird location and the score card
            var task_GameOver = Task.Run(() => Page.IsGameOver(GameImage));
            var task_BirdLocation = Task.Run(() => Page.DetectBird(GameImage));
            Task.WaitAll(task_GameOver, task_BirdLocation);

            bool isGameOver = task_GameOver.Result;
            BirdLocation = task_BirdLocation.Result;
            
            // check for game over
            if (isGameOver)
                return BirdAction.Restart;

            if (BirdLocation == null && previousY == null)
                return BirdAction.Flap;
            else if (BirdLocation == null)
                return BirdAction.Nothing;

            // find the pipes
            var task_BottomPipes = Task.Run(() =>
            {
                List<Point> BottomHalfPipes = Page.DetectBottomHalfPipes(GameImage);
                return DetermineClosestPipe(BottomHalfPipes, BirdLocation);
            });
            var task_TopPipes = Task.Run(() =>
            {
                List<Point> TopHalfPipes = Page.DetectTopHalfPipes(GameImage);
                return DetermineClosestPipe(TopHalfPipes, BirdLocation);
            });
            Task.WaitAll(task_BottomPipes, task_TopPipes);

            closestBottomHalfPipe = task_BottomPipes.Result;
            closestTopHalfPipe = task_TopPipes.Result;

            long elapsedMilliseconds = velocityStopwatch.ElapsedMilliseconds;

            if (previousY != null)
                velocityY = ((float)(previousY.Value - BirdLocation.Value.Y)) / elapsedMilliseconds;
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

            inputs = new float[]
            {
                (float) BirdLocation.Value.Y / GameImage.Height,
                (float) velocityY / GameImage.Height,
                (float) closestBottomHalfPipe.X / GameImage.Width,
                (float) closestBottomHalfPipe.Y / GameImage.Height,
                (float) closestTopHalfPipe.Y / GameImage.Height
            };
            float[] outputs = Net.Forward(inputs);
            output = outputs[0];

            // return true if the page should flap
            if (output > 0.5)
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

        public void SetToyNeuralNetwork(ToyNeuralNetwork4Layer toyNeuralNetwork)
        {
            Net = toyNeuralNetwork;
            velocityStopwatch.Restart();
        }

    }
}
