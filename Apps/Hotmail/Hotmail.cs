using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace IanAutomation.Apps.Hotmail
{

    public class Hotmail
    {
        public IWebDriver Driver;

        public Hotmail(string downloadDirectory = "")
        {
            string ProfilePath = @"C:\Users\IMoses\AppData\Local\Google\Chrome\User";
            ChromeOptions options = new ChromeOptions();
            options.AddArgument($"--user-data-dir={ProfilePath}");
            options.AddArgument("--incognito");
            // all of this crap was my attempt to prevent the "Save As" dialog from appearing
            //options.AddUserProfilePreference("download.default_directory", downloadDirectory);
            //options.AddUserProfilePreference("download.prompt_for_download", false);
            //options.AddUserProfilePreference("download.directory_upgrade", true);
            //options.AddUserProfilePreference("safebrowsing.enabled", true);
            //options.AddArgument("--disable-features=download-bubble");
            //options.AddArgument("--disable-features=download-bubble-v2");
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

        /// <summary>
        /// Parses the information that is available from the list view
        /// </summary>
        public Email ParseEmail(IWebElement EmailElement)
        {
            string XPath = $"//*[@id='{EmailElement.GetAttribute("id")}']//span[@title='']";
            var SubjectElement = EmailElement.FindElement(By.XPath(XPath));

            return new Email()
            {
                IsRead = !EmailElement.GetAttribute("aria-label").Contains("Unread"),
                Subject = SubjectElement.Text,
                Element = EmailElement
            };
        }

        /// <summary>
        /// Opens the email and parses that information. If it was unread before this method was called, this method will mark it unread after returning to the main page.
        /// </summary>
        public Email OpenAndParse(IWebElement EmailElement)
        {
            throw new NotImplementedException();
        }

        public void MarkRead(Email email)
        {
            // if it's aleady read, return
            if (email.IsRead)
                return;

            new Actions(Driver)
                .ContextClick(email.Element)
                .Perform();
            Thread.Sleep(2000);
            string XPath = @"//*[text()='Mark as read']";
            Driver.FindElement(By.XPath(XPath)).Click();
            email.IsRead = !email.Element.GetAttribute("aria-label").Contains("Unread");
        }

        /// <summary>
        /// Broken. I can't stop the "Save As" dialog from popping up.
        /// </summary>
        public void Save(Email email)
        {
            new Actions(Driver)
                .ContextClick(email.Element)
                .Perform();
            Thread.Sleep(2000);
            string XPath = @"//*[text()='Save as']";
            Driver.FindElement(By.XPath(XPath)).Click();
        }

        /// <summary>
        /// Broken. I can't stop the "Save As" dialog from popping up.
        /// </summary>
        public void DownloadAttachments(Email email)
        {
            throw new NotImplementedException();
        }

        public void Forward(Email email, string To)
        {
            new Actions(Driver)
                .ContextClick(email.Element)
                .Perform();
            Thread.Sleep(2000);
            string XPath = @"//*[text()='Forward']";
            Driver.FindElement(By.XPath(XPath)).Click();
            var NewEmail = new NewEmail(Driver);
            Thread.Sleep(2000);
            NewEmail.To = To;
            NewEmail.Send();
        }

        /// <summary>
        /// Deletes the email on the page. Please destroy the Email object yourself.
        /// </summary>
        public void Delete(Email email)
        {
            new Actions(Driver)
                .ContextClick(email.Element)
                .Perform();
            Thread.Sleep(2000);
            string XPath = @"//*[text()='Delete']";
            Driver.FindElement(By.XPath(XPath)).Click();
        }

        public struct Email
        {
            public bool IsRead;
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

