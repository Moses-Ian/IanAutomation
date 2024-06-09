using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using IanNet;
using System.Drawing;

namespace IanAutomation.Apps.FlappyBird.Strategies
{
    public class ToyNeuralNetworkStrategy : IStrategy
    {
        public FlappyBird Page;
        public ToyNeuralNetwork Net;

        public ToyNeuralNetworkStrategy(FlappyBird Page)
        {
            SetPage(Page);
            Net = new ToyNeuralNetwork(4, 4, 1);
        }

        public void SetPage(FlappyBird Page)
        {
            this.Page = Page;
        }

        public void Strategize()
        {
            Mat GameImage = new Mat();
            Page.GetScreenshot(GameImage);

            if (Page.IsGameOver(GameImage))
                Page.Restart();

            Point? BirdLocation = Page.DetectBird(GameImage);

            if (BirdLocation == null)
                return;

            List<Point> BottomHalfPipes = Page.DetectBottomHalfPipes(GameImage);
            Point closestBottomHalfPipe = DetermineClosestPipe(BottomHalfPipes);
            List<Point> TopHalfPipes = Page.DetectTopHalfPipes(GameImage);
            Point closestTopHalfPipe = DetermineClosestPipe(TopHalfPipes);

            float[] inputs = new float[] 
            { 
                BirdLocation.Value.Y / GameImage.Height, 
                closestBottomHalfPipe.X / GameImage.Width, 
                closestBottomHalfPipe.Y / GameImage.Height, 
                closestTopHalfPipe.Y / GameImage.Height
            };
            float[] outputs = Net.Forward(inputs);
            if (outputs[0] > 0.5)
                Page.Flap();
        }

        private Point DetermineClosestPipe(List<Point> BottomPipes)
        {
            // the point itself is the top-left corner
            Point closestPoint = new Point(int.MaxValue, int.MaxValue);
            foreach (Point p in BottomPipes)
            {
                if (p.X < closestPoint.X)
                    closestPoint = p;
            }

            return closestPoint;
        }

        public void SetToyNeuralNetwork(ToyNeuralNetwork toyNeuralNetwork)
        {
            Net = toyNeuralNetwork;
        }

    }
}
