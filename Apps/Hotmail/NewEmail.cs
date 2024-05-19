using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IanAutomation.Apps.Hotmail
{

    public class NewEmail
    {
        public IWebDriver Driver;
        
        public NewEmail(IWebDriver Driver)
        {
            this.Driver = Driver;
        }

        public string To
        {
            set
            {
                string XPath = @"//*[@aria-label='To']";
                Driver.FindElement(By.XPath(XPath)).SendKeys(value);
            }
        }

        public void Send()
        {
            string XPath = @"//span[contains(text(),'Send')]";
            Driver.FindElement(By.XPath(XPath)).Click();
        }
    }
}
