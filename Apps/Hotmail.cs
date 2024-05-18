using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace IanAutomation.Apps
{

    public class Hotmail
    {
        public IWebDriver Driver;
        
        public Hotmail()
        {
            string ProfilePath = @"C:\Users\IMoses\AppData\Local\Google\Chrome\User";
            ChromeOptions options = new ChromeOptions();
            options.AddArgument($"--user-data-dir={ProfilePath}");
            options.AddArgument("--incognito");
            Driver = new ChromeDriver(@"C:\WebDriver\chromedriver.exe", options);
            Driver.Navigate().GoToUrl("https://www.hotmail.com/");
        }

        public void SignIn(string username, string password)
        {
            Driver.FindElement(By.Id("mectrl_headerPicture")).Click();
            Thread.Sleep(1000);
            Driver.FindElement(By.Id("i0116")).SendKeys(username);
            Driver.FindElement(By.Id("idSIButton9")).Click();
            Thread.Sleep(2000);
            Driver.FindElement(By.Id("i0118")).SendKeys(password);
            Driver.FindElement(By.Id("idSIButton9")).Click();
            Thread.Sleep(1000);
            Driver.FindElement(By.Id("declineButton")).Click();
            Thread.Sleep(1000);
        }

        public List<Email> GetEmails(int Count)
        {
            string XPath = @"//div[@class='jGG6V gDC9O' and not(contains(@aria-label,'Pinned'))]";
            var EmailElements = Driver.FindElements(By.XPath(XPath));
            var result = new List<Email>();
            foreach (var EmailElement in EmailElements)
            {
                result.Add(ParseEmail(EmailElement));
                if (result.Count == Count)
                    break;
            }

            return result;
        }

        private Email ParseEmail(IWebElement EmailElement)
        {
            string XPath = $"//*[@id='{EmailElement.GetAttribute("id")}']//span[@title='']";
            var SubjectElement = EmailElement.FindElement(By.XPath(XPath));
            
            return new Email()
            {
                Subject = SubjectElement.Text,
                Element = EmailElement
            };
        }

        public struct Email
        {
            public string Subject;
            public IWebElement Element;
        }

        public void Shutdown()
        {
            if (Driver != null)
                Driver.Quit();
        }
    }

}

