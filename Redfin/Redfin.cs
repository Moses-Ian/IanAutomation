using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IanAutomation.Redfin
{
    public class Redfin
    {
        public IWebDriver Driver;

        public Redfin()
        {
            string ProfilePath = @"C:\Users\IMoses\AppData\Local\Google\Chrome\User";
            ChromeOptions options = new ChromeOptions();
            options.AddArgument($"--user-data-dir={ProfilePath}");
            options.AddArgument("--incognito");
            Driver = new ChromeDriver(@"C:\WebDriver\chromedriver.exe", options);
            Driver.Navigate().GoToUrl("https://www.redfin.com/");
        }

        public Results Search(string query)
        {
            var searchInput = Driver.FindElement(By.CssSelector(".searchInputNode input#search-box-input"));
            searchInput.SendKeys(query);
            searchInput.Submit();
            return new Results(this, Driver);
        }

        public void Shutdown()
        {
            if (Driver != null)
                Driver.Quit();
        }
    }
}
