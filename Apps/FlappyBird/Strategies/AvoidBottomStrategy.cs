// This strategy has a best score of 6

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using System.Drawing;
using System.Diagnostics;

namespace IanAutomation.Apps.FlappyBird.Strategies
{
    public class AvoidBottomStrategy : IStrategy
    {
        public FlappyBird Page;
        public Stopwatch FlapStopwatch;
        //public Stopwatch RestartStopwatch;
        public int DistanceBuffer = 20;

        public AvoidBottomStrategy(FlappyBird Page)
        {
            SetPage(Page);
            FlapStopwatch = new Stopwatch();
            FlapStopwatch.Start();
            //RestartStopwatch = new Stopwatch();
            //RestartStopwatch.Start();
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

            List<Point> BottomPipes = Page.DetectBottomPipes(GameImage);
            var closestPipe = DetermineClosestPipe(BottomPipes);

            if (BirdLocation.Value.Y + Page.BirdImage.Height + DistanceBuffer > closestPipe.Y)
            {
                Page.Flap();
                FlapStopwatch.Restart();
            }

            if (FlapStopwatch.ElapsedMilliseconds > 400)
            {
                Page.Flap();
                FlapStopwatch.Restart();
            }
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
    }
}
