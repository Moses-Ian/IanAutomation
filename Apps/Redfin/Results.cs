using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;

namespace IanAutomation.Apps.Redfin
{
    public class Results
    {
        public IWebDriver Driver;
        public Redfin Redfin;
        private static Regex number = new Regex(@"[\d\.]*");

        public Results(Redfin redfin, IWebDriver driver)
        {
            Driver = driver;
            Redfin = redfin;
        }

        public void SetMode(SearchMode mode)
        {
            // click the "For Sale" dropdown
            Driver.FindElement(By.CssSelector(".ExposedSearchModeFilter")).Click();
            switch (mode.SaleType)
            {
                case SaleType.ForSale:
                    // click the "For sale" radio button
                    Driver.FindElement(By.Id("for-sale")).Click();
                    Thread.Sleep(500);

                    // click the dropdown chevron
                    var element = Driver.FindElement(By.CssSelector(".ForSaleSection .Accordion__heading"));
                    if (element.GetAttribute("aria-expanded") == "false")
                        element.Click();
                    Thread.Sleep(500);

                    // click the checkboxes
                    SetComingSoon(mode.ComingSoon);
                    SetActive(mode.Active);
                    SetUnderContractPending(mode.UnderContractPending);

                    break;
                case SaleType.ForRent:
                    Driver.FindElement(By.Id("forRent")).Click();
                    break;
                case SaleType.Sold:
                    Driver.FindElement(By.Id("sold")).Click();
                    // I can't be assed to set this
                    break;

            }
        }

        public void SetComingSoon(bool Checked)
        {
            var element = Driver.FindElement(By.Id("comingSoon"));
            var isChecked = bool.Parse(element.GetAttribute("checked"));
            if (isChecked && Checked)
                return;
            if (!isChecked && !Checked)
                return;
            element.Click();
        }

        public void SetActive(bool Checked)
        {
            var element = Driver.FindElement(By.Id("active"));
            var isChecked = bool.Parse(element.GetAttribute("checked"));
            if (isChecked && Checked)
                return;
            if (!isChecked && !Checked)
                return;
            element.Click();
        }

        public void SetUnderContractPending(bool Checked)
        {
            var element = Driver.FindElement(By.XPath(@"//input[@id='underContractPending']/parent::span/parent::span"));
            var isChecked = element.GetAttribute("class").Contains("checked");
            if (isChecked && Checked)
                return;
            if (!isChecked && !Checked)
                return;
            element.Click();
        }

        public void SetHomeType(HomeType homeType)
        {
            // click the home button
            var element = Driver.FindElement(By.CssSelector(".ExposedPropertyTypeFilter"));
            element.Click();

            if (homeType.House)
                Driver.FindElement(By.XPath(@"//span/*[name()='svg' and contains(@class, ' house')]/parent::*")).Click();
            if (homeType.Townhouse)
                Driver.FindElement(By.XPath(@"//span/*[name()='svg' and contains(@class, 'townhouse')]/parent::*")).Click();
            if (homeType.Condo)
                Driver.FindElement(By.XPath(@"//span/*[name()='svg' and contains(@class, 'condo')]/parent::*")).Click();
            if (homeType.Land)
                Driver.FindElement(By.XPath(@"//span/*[name()='svg' and contains(@class, 'land')]/parent::*")).Click();
            if (homeType.MultiFamily)
                Driver.FindElement(By.XPath(@"//span/*[name()='svg' and contains(@class, 'multi-family')]/parent::*")).Click();
            if (homeType.Mobile)
                Driver.FindElement(By.XPath(@"//span/*[name()='svg' and contains(@class, 'mobile-home')]/parent::*")).Click();
            if (homeType.CoOp)
                Driver.FindElement(By.XPath(@"//span/*[name()='svg' and contains(@class, 'co-op')]/parent::*")).Click();
            if (homeType.Other)
                Driver.FindElement(By.XPath(@"//span/*[name()='svg' and contains(@class, 'castle')]/parent::*")).Click();
        }

        public List<Listing> GetListings(int MaxPages = int.MaxValue, int MaxListings = int.MaxValue)
        {
            List<Listing> listings = new List<Listing>();

            int pageNumber = 1;
            while (pageNumber <= MaxPages && listings.Count < MaxListings)
            {
                listings.AddRange(GetListingsOnPage());
                if (IsLastPage())
                    break;
                ClickNext();
                pageNumber++;
            }

            return listings.Count < MaxListings ? listings : listings.GetRange(0, MaxListings);
        }

        public List<Listing> GetListingsOnPage()
        {
            var prices = Driver.FindElements(By.CssSelector(".bp-Homecard__Price--value"));
            var bedrooms = Driver.FindElements(By.CssSelector(".bp-Homecard__Stats--beds"));
            var bathrooms = Driver.FindElements(By.CssSelector(".bp-Homecard__Stats--baths"));
            var sqfts = Driver.FindElements(By.XPath(@"//span[contains(@class, 'bp-Homecard__Stats--sqft')]/*[1]"));
            var addresses = Driver.FindElements(By.CssSelector(".bp-Homecard__Address"));
            var urls = Driver.FindElements(By.CssSelector(".link-and-anchor"));

            var listings = new List<Listing>(prices.Count);

            for (int i = 0; i < prices.Count; i++)
            {
                try
                {
                    listings.Add(new Listing()
                    {
                        Price = decimal.Parse(prices[i].Text, NumberStyles.Currency),
                        Bedrooms = decimal.Parse(number.Match(bedrooms[i].Text).Value),
                        Bathrooms = decimal.Parse(number.Match(bathrooms[i].Text).Value),
                        Sqft = sqfts[i].Text == "—" ? 0 : decimal.Parse(sqfts[i].Text.Replace(",", "")),
                        Address = addresses[i].Text,
                        Url = urls[i].GetAttribute("href")
                    });
                }
                catch { } // if we can't parse a listing correctly, just skip it
            }

            return listings;
        }

        public void ClickNext()
        {
            Driver.FindElement(By.CssSelector(".PageArrow__direction--next")).Click();
        }

        public void ClickPrevious()
        {
            Driver.FindElement(By.CssSelector(".PageArrow__direction--previous")).Click();
        }

        public bool IsFirstPage()
        {
            return Driver.FindElement(By.CssSelector(".PageArrow__direction--previous")).GetAttribute("class").Contains("PageArrow--hidden");
        }

        public bool IsLastPage()
        {
            return Driver.FindElement(By.CssSelector(".PageArrow__direction--next")).GetAttribute("class").Contains("PageArrow--hidden");
        }

    }

    public class SearchMode
    {
        public SaleType SaleType;
        public bool ComingSoon = false;
        public bool Active = false;
        public bool UnderContractPending = false;
        public SoldType SoldType;

    }

    public enum SaleType
    {
        ForSale,
        ForRent,
        Sold
    }

    public enum SoldType
    {
        Last1Week,
        Last1Month,
        Last3Months,
        Last5Months,
        Last1Year,
        Last2Years,
        Last5Years
    }

    public class HomeType
    {
        public bool House;
        public bool Townhouse;
        public bool Condo;
        public bool Land;
        public bool MultiFamily;
        public bool Mobile;
        public bool CoOp;
        public bool Other;
    }

    public class Listing
    {
        public decimal Price;
        public decimal Bedrooms;
        public decimal Bathrooms;
        public decimal Sqft;
        public string? Address;
        public string? Url;
        public decimal PricePerSqft
        {
            get
            {
                if (Sqft == 0)
                    return 0;
                return Math.Round(Price / Sqft, 0);
            }
        }
    }
}
