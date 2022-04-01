using OpenQA.Selenium;
using BetterLessonAutomationProjectCSharp.src.WrapperFactory;
using System.Collections.Generic;
using BetterLessonAutomationProjectCSharp.src.TestObjects;
using OpenQA.Selenium.Interactions;
using System.Linq;
using NUnit.Framework;
using BetterLessonAutomationProjectCSharp.src.TestData;
using System.Diagnostics;

namespace BetterLessonAutomationProjectCSharp.src.PageObjects
{
    public class HomePage
    {
        private IWebDriver driver = BrowserFactory.Driver;

        /// <summary>
        /// The HomePage Url
        /// </summary>
        public const string Url = "https://automationintesting.online/#/";

        #region Elements

        /// <summary>
        /// The Logo Image Element
        /// </summary>
        public IWebElement LogoImage => driver.FindElement(By.ClassName("hotel-logoUrl"));

        /// <summary>
        /// The Inn Description Element
        /// </summary>
        public IWebElement InnDescriptionParagraph => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[3]/div[2]/p"));

        public IWebElement InnHeader => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[4]/div[2]/h2"));

        public IWebElement RoomPicture => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[5]/div/div/div[2]/img"));

        public IWebElement RoomHeader => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[5]/div/div/div[3]/h3"));

        public IWebElement RoomAccessibilityIcon => driver.FindElement(By.ClassName("wheelchair"));

        public IWebElement RoomDescriptionParagraph => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[5]/div/div/div[3]/p"));

        public IWebElement RoomFeaturesList => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[5]/div/div/div[3]/ul"));

        public IWebElement BookThisRoomButton => driver.FindElement(By.ClassName("openBooking"));

        public IWebElement BookingCalendarToolbarTodayButton => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[5]/div/div[2]/div[2]/div/div[1]/span[1]/button[1]"));

        public IWebElement BookingCalendarToolbarBackButton => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[5]/div/div[2]/div[2]/div/div[1]/span[1]/button[2]"));

        public IWebElement BookingCalendarToolbarNextButton => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[5]/div/div[2]/div[2]/div/div[1]/span[1]/button[3]"));

        public IWebElement BookingCalendarToolbarDate => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[5]/div/div[2]/div[2]/div/div[1]/span[2]"));

        public IWebElement BookingCalendar => driver.FindElement(By.ClassName("rbc-month-view"));

        public IWebElement BookingCalendarDay => BookingCalendar.FindElement(By.ClassName("rbc-button-link"));

        public IWebElement BookingFirstName => driver.FindElement(By.ClassName("room-firstname"));

        public IWebElement BookingLastName => driver.FindElement(By.ClassName("room-lastname"));

        public IWebElement BookingEmail => driver.FindElement(By.ClassName("room-email"));

        public IWebElement BookingPhone => driver.FindElement(By.ClassName("room-phone"));

        public IWebElement BookingBookButton => driver.FindElement(By.ClassName("book-room")).FindElement(By.XPath($"//button[normalize-space() = 'Book']"));

        public IWebElement BookingCancelButton => driver.FindElement(By.ClassName("book-room")).FindElement(By.XPath($"//button[normalize-space() = 'Cancel']"));

        public List<IWebElement> ErrorMessages => driver.FindElement(By.ClassName("alert-danger")).FindElements(By.TagName("p")).ToList();

        public IWebElement BookingSuccessHeader => driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div[2]/h3"));

        public IWebElement BookingSuccessDescription => driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div[2]/p[1]"));

        public IWebElement BookingSuccessDates => driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div[2]/p[2]"));

        public IWebElement BookingSuccessCloseButton => driver.FindElement(By.XPath("/html/body/div[4]/div/div/div[2]/div/button"));

        /// <summary>
        /// Returns a number greater than 0 if present
        /// </summary>
        public int BookingViewIsPresent => driver.FindElements(By.XPath("//*[@id='root']/div/div/div[5]/div/div[2]")).Count;

        public IWebElement ContactFormName => driver.FindElement(By.Id("name"));

        public IWebElement ContactFormEmail => driver.FindElement(By.Id("email"));

        public IWebElement ContactFormPhone => driver.FindElement(By.Id("phone"));

        public IWebElement ContactFormSubject => driver.FindElement(By.Id("subject"));

        public IWebElement ContactFormMessage => driver.FindElement(By.Id("description"));

        public IWebElement ContactFormSubmit => driver.FindElement(By.Id("submitContact"));

        public IWebElement ContactFormInnName => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[6]/div[3]/p[1]"));

        public IWebElement ContactFormInnAddress => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[6]/div[3]/p[2]"));

        public IWebElement ContactFormInnPhone => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[6]/div[3]/p[3]"));

        public IWebElement ContactFormInnEmail => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[6]/div[3]/p[4]"));

        public IWebElement MapImage => driver.FindElement(By.ClassName("pigeon-overlays"));

        public IWebElement ContactFormSubmissionSuccess => driver.FindElement(By.XPath("//*[@id='root']/div/div/div[6]/div[2]/div"));

        public IWebElement ContactFormSubmissionSuccessHeader => ContactFormSubmissionSuccess.FindElement(By.TagName("h2"));

        public IWebElement ContactFormSubmissionSuccessDescription => ContactFormSubmissionSuccess.FindElements(By.TagName("p")).ToList()[0];

        public IWebElement ContactFormSubmissionSuccessSubject => ContactFormSubmissionSuccess.FindElements(By.TagName("p")).ToList()[1];

        public IWebElement ContactFormSubmissionSuccessTimeline => ContactFormSubmissionSuccess.FindElements(By.TagName("p")).ToList()[2];

        #endregion Elements

        #region Methods

        public void FillBookingInformation(BookingInformation bookingInformation)
        {
            //Drag and drop to set date
            var DateFromElement = Page.Home.BookingCalendarDay.FindElement(By.XPath($"//button[normalize-space() = '{bookingInformation.DateFrom.ToString("dd")}']"));
            var DateToElement = Page.Home.BookingCalendarDay.FindElement(By.XPath($"//button[normalize-space() = '{bookingInformation.DateTo.ToString("dd")}']"));
            Actions action = new Actions(driver);
            action.DragAndDrop(DateFromElement, DateToElement).Perform();
            Page.Home.BookingFirstName.SendKeys(bookingInformation.FirstName);
            Page.Home.BookingLastName.SendKeys(bookingInformation.LastName);
            Page.Home.BookingEmail.SendKeys(bookingInformation.Email);
            Page.Home.BookingPhone.SendKeys(bookingInformation.Phone);
        }

        public void SubmitBookingInformation()
        {
            Page.Home.BookingBookButton.Click();
        }

        public void CancelBookingInformation()
        {
            Page.Home.BookingCancelButton.Click();
        }

        public void ValidateBookingViewIsPresent()
        {
            Assert.IsTrue(Page.Home.BookingFirstName.Displayed, "Page.Home.BookingFirstName.Displayed");
            Assert.IsTrue(Page.Home.BookingLastName.Displayed, "Page.Home.BookingLastName.Displayed");
            Assert.IsTrue(Page.Home.BookingEmail.Displayed, "Page.Home.BookingEmail.Displayed");
            Assert.IsTrue(Page.Home.BookingPhone.Displayed, "Page.Home.BookingPhone.Displayed");
            Assert.IsTrue(Page.Home.BookingBookButton.Displayed, "Page.Home.BookingBookButton.Displayed");
            Assert.IsTrue(Page.Home.BookingCancelButton.Displayed, "Page.Home.BookingCancelButton.Displayed");
        }

        public void ValidateBookingViewIsNotPresent()
        {
            Assert.AreEqual(Page.Home.BookingViewIsPresent, 0, "The Booking View was present but it wasn't supposed to be.");
        }

        public void ValidateBookingFormIsEmpty()
        {
            Assert.AreEqual("", Page.Home.BookingFirstName.GetAttribute("innerText"));
            Assert.AreEqual("", Page.Home.BookingLastName.GetAttribute("innerText"));
            Assert.AreEqual("", Page.Home.BookingEmail.GetAttribute("innerText"));
            Assert.AreEqual("", Page.Home.BookingPhone.GetAttribute("innerText"));
        }

        public void ValidateBookingSuccess(BookingInformation bookingInformation)
        {
            Assert.AreEqual(HomePageTestData.BookingSuccessHeader, Page.Home.BookingSuccessHeader.Text);
            Assert.AreEqual(HomePageTestData.BookingSuccessDescription, Page.Home.BookingSuccessDescription.Text);
            Assert.AreEqual($"{bookingInformation.DateFrom.ToString("yyyy-MM-dd")} - {bookingInformation.DateTo.ToString("yyyy-MM-dd")}", Page.Home.BookingSuccessDates.Text);
            Assert.IsTrue(Page.Home.BookingSuccessCloseButton.Displayed);
        }

        public void FillContactFormInformation(ContactFormInformation contactFormInformation)
        {
            Page.Home.ContactFormName.SendKeys(contactFormInformation.Name);
            Page.Home.ContactFormEmail.SendKeys(contactFormInformation.Email);
            Page.Home.ContactFormPhone.SendKeys(contactFormInformation.Phone);
            Page.Home.ContactFormSubject.SendKeys(contactFormInformation.Subject);
            Page.Home.ContactFormMessage.SendKeys(contactFormInformation.Message);
        }

        public void SubmitContactFormInformation()
        {
            Page.Home.ContactFormSubmit.Click();
        }

        public List<string> GetErrorMessages()
        {
            var errorMessages = new List<string>();

            foreach (var errorMessageParagraph in Page.Home.ErrorMessages)
            {
                errorMessages.Add(errorMessageParagraph.GetAttribute("innerText"));
            }

            return errorMessages;
        }

        public void ValidateContactFormSubmission()
        {
            Assert.AreEqual(HomePageTestData.ContactFormSubmissionSuccessHeader, Page.Home.ContactFormSubmissionSuccessHeader.GetAttribute("innerText"));
            Assert.AreEqual(HomePageTestData.ContactFormSubmissionSuccessDescription, Page.Home.ContactFormSubmissionSuccessDescription.GetAttribute("innerText"));
            Assert.AreEqual(HomePageTestData.ContactFormSubmissionSuccessSubject, Page.Home.ContactFormSubmissionSuccessSubject.GetAttribute("innerText"));
            Assert.AreEqual(HomePageTestData.ContactFormSubmissionSuccessTimeline, Page.Home.ContactFormSubmissionSuccessTimeline.GetAttribute("innerText"));
        }

        #endregion Methods
    }
}
