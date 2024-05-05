using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace IanAutomation
{
    public class TheAutomationChallenge
    {
        public IWebDriver Driver;

        public TheAutomationChallenge()
        {
            string ProfilePath = @"C:\Users\IMoses\AppData\Local\Google\Chrome\User";
            ChromeOptions options = new ChromeOptions();
            options.AddArgument($"--user-data-dir={ProfilePath}");
            Driver = new ChromeDriver(@"C:\WebDriver\chromedriver.exe", options);
            Driver.Navigate().GoToUrl("https://www.theautomationchallenge.com/");
        }

        public void Shutdown()
        {
            if (Driver != null)
                Driver.Quit();
        }

        public void Start()
        {
            Driver.FindElement(By.XPath(@"//button[text()='Start']")).Click();
        }

        public void Submit()
        {
            var buttons = Driver.FindElements(By.XPath(@"//button[text()='Submit']"));
            foreach (var button in buttons)
            {
                try
                {
                    button.Click();
                    return;
                }
                catch { }
            }
            throw new Exception("Failed to find Submit");
        }

        public void ClickReCAPTCHA()
        {
            Driver.FindElement(By.XPath(@"//button[text()='presentation']")).Click();
        }

        public string EIN
        {
            set
            {
                EnterValue("EIN", value);
            }
        }

        public string CompanyName
        {
            set
            {
                EnterValue("Company Name", value);
            }
        }

        public string Sector
        {
            set
            {
                EnterValue("Sector", value);
            }
        }

        public string Address
        {
            set
            {
                EnterValue("Address", value);
            }
        }

        public string AutomationTool
        {
            set
            {
                EnterValue("Automation Tool", value);
            }
        }

        public string AnnualSaving
        {
            set
            {
                EnterValue("Annual Saving", value);
            }
        }

        public string Date
        {
            set
            {
                EnterValue("Date", value);
            }
        }

        public void EnterValue(string Label, string Value)
        {
            foreach (var element in Select1(Label))
            {
                try
                {
                    element.SendKeys(Value);
                    return;
                } catch { }
            }
            foreach (var element in Select2(Label))
            {
                try
                {
                    element.SendKeys(Value);
                    return;
                } catch { }
            }
            foreach (var element in Select3(Label))
            {
                try
                {
                    element.SendKeys(Value);
                    return;
                } catch { }
            }
            foreach (var element in Select4(Label))
            {
                try
                {
                    element.SendKeys(Value);
                    return;
                } catch { }
            }
            foreach (var element in Select5(Label))
            {
                try
                {
                    element.SendKeys(Value);
                    return;
                } catch { }
            }
            foreach (var element in Select6(Label))
            {
                try
                {
                    element.SendKeys(Value);
                    return;
                } catch { }
            }
            foreach (var element in Select7(Label))
            {
                try
                {
                    element.SendKeys(Value);
                    return;
                } catch { }
            }
            Console.WriteLine($"Got here at {Label}");
            throw new Exception($"Failed to find {Label}");
        }

        private ReadOnlyCollection<IWebElement> Select1(string Label)
        {
            return Driver.FindElements(By.XPath(@"//div[@class='bubble-r-box']/div[@class='bubble-element Text']/div[@class='content' and text()='" + Label + @"']/../following-sibling::input"));
        }
        
        private ReadOnlyCollection<IWebElement> Select2(string Label)
        {
            return Driver.FindElements(By.XPath(@"//div[div/div[text()='" + Label + @"']]/following-sibling::div//input"));
        }

        private ReadOnlyCollection<IWebElement> Select3(string Label)
        {
            return Driver.FindElements(By.XPath(@"//div[div/div[text()='" + Label + @"']]/preceding-sibling::div//input"));
        }

        private ReadOnlyCollection<IWebElement> Select4(string Label)
        {
            return Driver.FindElements(By.XPath(@"//div[div[text()='" + Label + @"']]/following-sibling::input"));
        }

        private ReadOnlyCollection<IWebElement> Select5(string Label)
        {
            return Driver.FindElements(By.XPath(@"//div[div[text()='" + Label + @"']]/preceding-sibling::input"));
        }

        private ReadOnlyCollection<IWebElement> Select6(string Label)
        {
            return Driver.FindElements(By.XPath(@"//div[div/div[text()='" + Label + @"']]/following-sibling::div//input"));
        }

        private ReadOnlyCollection<IWebElement> Select7(string Label)
        {
            return Driver.FindElements(By.XPath(@"//div[div/div/div[text()='" + Label + @"']]/following-sibling::div//input"));
        }

    }
}