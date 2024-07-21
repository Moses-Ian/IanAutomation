// This strategy has a best score of 2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace IanAutomation.Apps.FlappyBird.Strategies
{
    public class HoverStrategy : IStrategy
    {
        public FlappyBird Page;
        public Stopwatch FlapStopwatch;
        public Stopwatch RestartStopwatch;
        
        public HoverStrategy(FlappyBird Page)
        {
            SetPage(Page);
            FlapStopwatch = new Stopwatch();
            FlapStopwatch.Start();
            RestartStopwatch = new Stopwatch();
            RestartStopwatch.Start();
        }

        public void SetPage(FlappyBird Page)
        {
            this.Page = Page;
        }

        public BirdAction Strategize()
        {
            BirdAction action = BirdAction.Nothing;
            if (FlapStopwatch.ElapsedMilliseconds > 400)
            {
                //Page.Flap();
                action = BirdAction.Flap;
                FlapStopwatch.Restart();
            }
            if (RestartStopwatch.ElapsedMilliseconds > 4000)
            {
                Page.Restart();
                RestartStopwatch.Restart();
            }
            return action;
        }
    }
}
