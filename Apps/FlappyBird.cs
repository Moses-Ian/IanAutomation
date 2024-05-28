using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;

namespace IanAutomation.Apps
{
    public class FlappyBird
    {
        public IWebDriver Driver;

        public FlappyBird()
        {
            string ProfilePath = @"C:\Users\IMoses\AppData\Local\Google\Chrome\User";
            ChromeOptions options = new ChromeOptions();
            options.AddArgument($"--user-data-dir={ProfilePath}");
            Driver = new ChromeDriver(@"C:\WebDriver\chromedriver.exe", options);
            Driver.Navigate().GoToUrl("https://moses-ian.github.io/FlappyBirdWithComputerVision/");

            Thread.Sleep(1000);

            SetWindowSize();
        }

        public IWebElement Canvas
        {
            get
            {
                return Driver.FindElement(By.TagName("canvas"));
            }
        }

        public void Shutdown()
        {
            if (Driver != null)
                Driver.Quit();
        }

        public void Flap()
        {
            Driver.FindElement(By.TagName("body")).SendKeys(" ");
        }

        public void Restart()
        {
            Driver.Navigate().Refresh();
        }

        public void GetScreenshot(Mat image)
        {
            string js = "return arguments[0].toDataURL('image/png').substring(22);";
            string base64Image = (string)((IJavaScriptExecutor)Driver).ExecuteScript(js, Canvas);
            byte[] pixels = Convert.FromBase64String(base64Image);
            CvInvoke.Imdecode(pixels, ImreadModes.Color, image);
        }

        public Size GetCanvasSize()
        {
            return new Size( Canvas.Size.Width, Canvas.Size.Height );
        }

        public void SetWindowSize()
        {
            Driver.Manage().Window.Size = new Size(Canvas.Size.Width, Canvas.Size.Height + 200);
        }
    }
}
