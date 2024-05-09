using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IanAutomation.Apps
{
    [Obsolete("Automation is blocked. Use Redfin instead.")]
    public class Zillow
    {
        public IWebDriver Driver;

        public Zillow()
        {
            string ProfilePath = @"C:\Users\IMoses\AppData\Local\Google\Chrome\User";
            ChromeOptions options = new ChromeOptions();
            options.AddArgument($"--user-data-dir={ProfilePath}");
            options.AddArgument("--incognito");
            Driver = new ChromeDriver(@"C:\WebDriver\chromedriver.exe", options);
            Driver.Navigate().GoToUrl("https://www.zillow.com/");
        }

        public void Search(string query)
        {
            var searchInput = Driver.FindElement(By.CssSelector("input[type='text']"));
            searchInput.SendKeys(query);
            searchInput.Submit();
        }

        public void Shutdown()
        {
            if (Driver != null)
                Driver.Quit();
        }
    }
}
