﻿using OpenQA.Selenium;
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
using System.Diagnostics;

namespace IanAutomation.Apps.FlappyBird
{
    public class FlappyBird
    {
        public IWebDriver Driver;
        public Mat BirdImage;
        public Mat BirdImage_35;
        public Mat BirdImage_n35;
        public Mat TopPipeImage;
        public Mat TopPipeHalfImage;
        public Mat BottomPipeImage;
        public Mat BottomPipeHalfImage;
        public Mat ScoreBoxImage;
        
        public FlappyBird()
        {
            string ProfilePath = @"C:\Users\IMoses\AppData\Local\Google\Chrome\User";
            ChromeOptions options = new ChromeOptions();
            options.AddArgument($"--user-data-dir={ProfilePath}");
            Driver = new ChromeDriver(@"C:\WebDriver\chromedriver.exe", options);
            Driver.Navigate().GoToUrl("https://moses-ian.github.io/FlappyBirdWithComputerVision/");

            Thread.Sleep(1000);

            SetWindowSize();
            BirdImage = LoadImage(@"bird.png");
            BirdImage_35 = RotateImage(BirdImage, 35);
            BirdImage_n35 = RotateImage(BirdImage, -35);

            TopPipeImage = LoadImage(@"pipes_reverse.png");
            Rectangle roi = new Rectangle(0, TopPipeImage.Rows-100, TopPipeImage.Cols, 100);
            TopPipeImage = new Mat(TopPipeImage, roi);
            roi = new Rectangle(TopPipeImage.Cols / 2, TopPipeImage.Rows - 100, TopPipeImage.Cols / 2, 100);
            TopPipeHalfImage = new Mat(TopPipeImage, roi);

            BottomPipeImage = LoadImage(@"pipes.png");
            roi = new Rectangle(0, 0, BottomPipeImage.Cols, 100);
            BottomPipeImage = new Mat(BottomPipeImage, roi);
            roi = new Rectangle(BottomPipeImage.Cols / 2, 0, BottomPipeImage.Cols / 2, 100);
            BottomPipeHalfImage = new Mat(BottomPipeImage, roi);

            ScoreBoxImage = LoadImage(@"ScoreBox.png");
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
            Driver.FindElement(By.TagName("body")).SendKeys("r");
        }

        public void Pause()
        {
            Driver.FindElement(By.TagName("body")).SendKeys("p");
        }

        public void Unpause()
        {
            Driver.FindElement(By.TagName("body")).SendKeys("p");
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
            return new Size(Canvas.Size.Width, Canvas.Size.Height);
        }

        public void SetWindowSize()
        {
            Driver.Manage().Window.Size = new Size(Canvas.Size.Width, Canvas.Size.Height + 200);
        }

        public Point? DetectBird(Mat GameImage)
        {
            Point location_35 = DetectBird_35(GameImage);
            Point location_n35 = DetectBird_n35(GameImage);
            if (location_35.X < 30)
                return location_35;
            if (location_n35.X < 30) 
                return location_n35;
            return null;
        }

        private Point DetectBird_35(Mat GameImage) 
        {
            // Crop the game image to the left 200 pixels
            Rectangle roi = new Rectangle(0, 0, 200, GameImage.Rows);
            Mat croppedImage = new Mat(GameImage, roi);

            // Create the result matrix
            int resultCols = croppedImage.Cols - BirdImage_35.Cols + 1;
            int resultRows = croppedImage.Rows - BirdImage_35.Rows + 1;
            Mat result = new Mat(resultRows, resultCols, DepthType.Cv32F, 1);

            // Perform template matching
            CvInvoke.MatchTemplate(croppedImage, BirdImage_35, result, TemplateMatchingType.CcoeffNormed);

            // Normalize the result
            CvInvoke.Normalize(result, result, 0, 1, NormType.MinMax, DepthType.Default, null);

            // Find the location of the best match
            double minVal = 0, maxVal = 0;
            Point minLoc = new Point();
            Point maxLoc = new Point();
            CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

            // Clean up
            result.Dispose();

            // Return the best location
            return maxLoc;
        }

        private Point DetectBird_n35(Mat GameImage) 
        {
            // Crop the game image to the left 200 pixels
            Rectangle roi = new Rectangle(0, 0, 200, GameImage.Rows);
            Mat croppedImage = new Mat(GameImage, roi);

            // Create the result matrix
            int resultCols = croppedImage.Cols - BirdImage_n35.Cols + 1;
            int resultRows = croppedImage.Rows - BirdImage_n35.Rows + 1;
            Mat result = new Mat(resultRows, resultCols, DepthType.Cv32F, 1);

            // Perform template matching
            CvInvoke.MatchTemplate(croppedImage, BirdImage_n35, result, TemplateMatchingType.CcoeffNormed);

            // Normalize the result
            CvInvoke.Normalize(result, result, 0, 1, NormType.MinMax, DepthType.Default, null);

            // Find the location of the best match
            double minVal = 0, maxVal = 0;
            Point minLoc = new Point();
            Point maxLoc = new Point();
            CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

            // Determine the best match location
            Point matchLoc = maxLoc;

            // Clean up
            result.Dispose();

            return matchLoc;
        }

        public void AnnotateBird(Mat GameImage, Point? BirdLocation)
        {
            if (BirdLocation == null)
                return;

            // Draw a rectangle around the matched region
            Rectangle matchRect = new Rectangle(BirdLocation.Value, BirdImage.Size);
            CvInvoke.Rectangle(GameImage, matchRect, new MCvScalar(0, 0, 255), 2);
        }

        private Mat LoadImage(string relativePath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string imagePath = Path.Combine(basePath, relativePath);
            return CvInvoke.Imread(imagePath);
        }

        private Mat RotateImage(Mat image, double angle)
        {
            PointF center = new PointF(image.Width / 2, image.Height / 2);
            Mat rotationMatrix = new Mat();
            CvInvoke.GetRotationMatrix2D(center, angle, 1.0, rotationMatrix);
            Mat rotatedImage = new Mat();
            CvInvoke.WarpAffine(image, rotatedImage, rotationMatrix, image.Size, Inter.Linear, Warp.Default, BorderType.Reflect101);

            return rotatedImage;
        }

        public List<Point> DetectTopPipes(Mat GameImage)
        {
            double threshold = 0.95;
            List<Point> points = new List<Point>();

            // Crop the game image to the top 300 pixels
            Rectangle roi = new Rectangle(0, 0, GameImage.Cols, 300);
            Mat croppedImage = new Mat(GameImage, roi);

            // Create the result matrix
            int resultCols = croppedImage.Cols - TopPipeImage.Cols + 1;
            int resultRows = croppedImage.Rows - TopPipeImage.Rows + 1;
            Mat result = new Mat(resultRows, resultCols, DepthType.Cv32F, 1);

            // Perform template matching
            CvInvoke.MatchTemplate(croppedImage, TopPipeImage, result, TemplateMatchingType.CcoeffNormed);

            // Normalize the result
            CvInvoke.Normalize(result, result, 0, 1, NormType.MinMax, DepthType.Default, null);

            while (true)
            {
                // Find the location of the best match
                double minVal = 0, maxVal = 0;
                Point minLoc = new Point();
                Point maxLoc = new Point();
                CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                // If the best match value is less than the threshold, stop searching
                if (maxVal < threshold)
                    break;

                // Determine the best match location
                points.Add(maxLoc);

                // Set the matched region to zero in the result matrix to find other matches
                Rectangle matchRect = new Rectangle(maxLoc, TopPipeImage.Size);
                CvInvoke.Rectangle(result, matchRect, new MCvScalar(0), -1);
            }

            // Clean up
            result.Dispose();

            return points;
        }

        public void AnnotateTopPipes(Mat GameImage, List<Point> Points)
        {
            foreach (Point p in Points)
            {
                // Draw a rectangle around the matched region
                Rectangle matchRect = new Rectangle(p, TopPipeImage.Size);
                CvInvoke.Rectangle(GameImage, matchRect, new MCvScalar(0, 0, 255), 2);
            }
        }

        public List<Point> DetectTopHalfPipes(Mat GameImage)
        {
            double threshold = 0.95;
            List<Point> points = new List<Point>();

            // Crop the game image to the top 300 pixels
            Rectangle roi = new Rectangle(0, 0, GameImage.Cols, 300);
            Mat croppedImage = new Mat(GameImage, roi);

            // Create the result matrix
            int resultCols = croppedImage.Cols - TopPipeHalfImage.Cols + 1;
            int resultRows = croppedImage.Rows - TopPipeHalfImage.Rows + 1;
            Mat result = new Mat(resultRows, resultCols, DepthType.Cv32F, 1);

            // Perform template matching
            CvInvoke.MatchTemplate(croppedImage, TopPipeHalfImage, result, TemplateMatchingType.CcoeffNormed);

            // Normalize the result
            CvInvoke.Normalize(result, result, 0, 1, NormType.MinMax, DepthType.Default, null);

            while (true)
            {
                // Find the location of the best match
                double minVal = 0, maxVal = 0;
                Point minLoc = new Point();
                Point maxLoc = new Point();
                CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                // If the best match value is less than the threshold, stop searching
                if (maxVal < threshold)
                    break;

                // Determine the best match location
                points.Add(maxLoc);

                // Set the matched region to zero in the result matrix to find other matches
                Rectangle matchRect = new Rectangle(maxLoc, TopPipeHalfImage.Size);
                CvInvoke.Rectangle(result, matchRect, new MCvScalar(0), -1);
            }

            // Clean up
            result.Dispose();

            return points;
        }

        public void AnnotateTopHalfPipes(Mat GameImage, List<Point> Points)
        {
            foreach (Point p in Points)
            {
                // Draw a rectangle around the matched region
                Rectangle matchRect = new Rectangle(p, TopPipeHalfImage.Size);
                CvInvoke.Rectangle(GameImage, matchRect, new MCvScalar(0, 0, 255), 2);
            }
        }

        public List<Point> DetectBottomPipes(Mat GameImage)
        {
            double threshold = 0.95;
            List<Point> points = new List<Point>();

            // Crop the game image to the bottom 300 pixels
            Rectangle roi = new Rectangle(0, 300, GameImage.Cols, 300);
            Mat croppedImage = new Mat(GameImage, roi);

            // Create the result matrix
            int resultCols = croppedImage.Cols - BottomPipeImage.Cols + 1;
            int resultRows = croppedImage.Rows - BottomPipeImage.Rows + 1;
            Mat result = new Mat(resultRows, resultCols, DepthType.Cv32F, 1);

            // Perform template matching
            CvInvoke.MatchTemplate(croppedImage, BottomPipeImage, result, TemplateMatchingType.CcoeffNormed);

            // Normalize the result
            CvInvoke.Normalize(result, result, 0, 1, NormType.MinMax, DepthType.Default, null);

            while (true)
            {
                // Find the location of the best match
                double minVal = 0, maxVal = 0;
                Point minLoc = new Point();
                Point maxLoc = new Point();
                CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                // If the best match value is less than the threshold, stop searching
                if (maxVal < threshold)
                    break;

                // Determine the best match location
                Point actualPoint = new Point(maxLoc.X, maxLoc.Y + 300);
                points.Add(actualPoint);

                // Set the matched region to zero in the result matrix to find other matches
                Rectangle matchRect = new Rectangle(maxLoc, BottomPipeImage.Size);
                CvInvoke.Rectangle(result, matchRect, new MCvScalar(0), -1);
            }

            // Clean up
            result.Dispose();

            return points;
        }

        public void AnnotateBottomPipes(Mat GameImage, List<Point> Points)
        {
            foreach (Point p in Points)
            {
                // Draw a rectangle around the matched region
                Rectangle matchRect = new Rectangle(p, BottomPipeImage.Size);
                CvInvoke.Rectangle(GameImage, matchRect, new MCvScalar(126, 255, 0), 2);
            }
        }

        public List<Point> DetectBottomHalfPipes(Mat GameImage)
        {
            double threshold = 0.95;
            List<Point> points = new List<Point>();

            // Crop the game image to the bottom 300 pixels
            Rectangle roi = new Rectangle(0, 300, GameImage.Cols, 300);
            Mat croppedImage = new Mat(GameImage, roi);

            // Create the result matrix
            int resultCols = croppedImage.Cols - BottomPipeHalfImage.Cols + 1;
            int resultRows = croppedImage.Rows - BottomPipeHalfImage.Rows + 1;
            Mat result = new Mat(resultRows, resultCols, DepthType.Cv32F, 1);

            // Perform template matching
            CvInvoke.MatchTemplate(croppedImage, BottomPipeHalfImage, result, TemplateMatchingType.CcoeffNormed);

            // Normalize the result
            CvInvoke.Normalize(result, result, 0, 1, NormType.MinMax, DepthType.Default, null);

            while (true)
            {
                // Find the location of the best match
                double minVal = 0, maxVal = 0;
                Point minLoc = new Point();
                Point maxLoc = new Point();
                CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                // If the best match value is less than the threshold, stop searching
                if (maxVal < threshold)
                    break;

                // Determine the best match location
                Point actualPoint = new Point(maxLoc.X, maxLoc.Y + 300);
                points.Add(actualPoint);

                // Set the matched region to zero in the result matrix to find other matches
                Rectangle matchRect = new Rectangle(maxLoc, BottomPipeHalfImage.Size);
                CvInvoke.Rectangle(result, matchRect, new MCvScalar(0), -1);
            }

            // Clean up
            result.Dispose();

            return points;
        }

        public void AnnotateBottomHalfPipes(Mat GameImage, List<Point> Points)
        {
            foreach (Point p in Points)
            {
                // Draw a rectangle around the matched region
                Rectangle matchRect = new Rectangle(p, BottomPipeHalfImage.Size);
                CvInvoke.Rectangle(GameImage, matchRect, new MCvScalar(126, 255, 0), 2);
            }
        }

        public bool IsGameOver(Mat GameImage)
        {
            return DetectScoreBox(GameImage) != null;
        }

        public Point? DetectScoreBox(Mat GameImage)
        {
            // Create the result matrix
            int resultCols = GameImage.Cols - ScoreBoxImage.Cols + 1;
            int resultRows = GameImage.Rows - ScoreBoxImage.Rows + 1;
            Mat result = new Mat(resultRows, resultCols, DepthType.Cv32F, 1);

            // Perform template matching
            CvInvoke.MatchTemplate(GameImage, ScoreBoxImage, result, TemplateMatchingType.CcoeffNormed);

            // Normalize the result
            CvInvoke.Normalize(result, result, 0, 1, NormType.MinMax, DepthType.Default, null);

            // Find the location of the best match
            double minVal = 0, maxVal = 0;
            Point minLoc = new Point();
            Point maxLoc = new Point();
            CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

            // Clean up
            result.Dispose();

            // the location should always be ( 36, 256 )
            if (maxLoc.X < 35 || maxLoc.X > 37 || maxLoc.Y < 255 || maxLoc.Y > 257)
                return null;

            // Return the best location
            return maxLoc;
        }

        public void AnnotateScoreBox(Mat GameImage, Point? ScoreBoxLocation)
        {
            if (ScoreBoxLocation == null)
                return;

            // Draw a rectangle around the matched region
            Rectangle matchRect = new Rectangle(ScoreBoxLocation.Value, ScoreBoxImage.Size);
            CvInvoke.Rectangle(GameImage, matchRect, new MCvScalar(0, 255, 0), 2);
        }

        /// <summary>
        /// Cheat the game by adjusting the parameters!
        /// </summary>
        public void SetPipeMinGap(int PipeMinGap)
        {
            string js = $"pipeMinGap = {PipeMinGap}";
            ((IJavaScriptExecutor)Driver).ExecuteScript(js, Canvas);
        }

        /// <summary>
        /// Cheat the game by adjusting the parameters!
        /// </summary>
        public void SetPipeMaxGap(int PipeMaxGap)
        {
            string js = $"pipeMaxGap = {PipeMaxGap}";
            ((IJavaScriptExecutor)Driver).ExecuteScript(js, Canvas);
        }
    }
}
