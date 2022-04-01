using BetterLessonAutomationProjectCSharp.src.PageObjects;
using BetterLessonAutomationProjectCSharp.src.TestData;
using BetterLessonAutomationProjectCSharp.src.TestObjects;
using BetterLessonAutomationProjectCSharp.src.WrapperFactory;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace BetterLessonAutomationProjectCSharp.src.TestCases.UserInterface
{
    [TestFixture]
    public class HomeTests : BaseTest
    {
        #region Tests

        [Test]
        public void HomePageLoad()
        {
            BrowserFactory.GoToPage(HomePage.Url);
            BrowserFactory.WaitForPageLoad(10);
            Assert.IsTrue(Page.Home.LogoImage.Displayed, "Page.Home.LogoImage.Displayed");
        }

        [Test]
        public void HomePageContent()
        {
            BrowserFactory.GoToPage(HomePage.Url);
            BrowserFactory.WaitForPageLoad(10);

            //There should be an Inn description paragraph
            Assert.AreEqual(HomePageTestData.InnDescriptionParagraph, Page.Home.InnDescriptionParagraph.Text);

            //There should be a photo
            Assert.AreEqual(HomePageTestData.RoomPictureSrc, Page.Home.RoomPicture.GetAttribute("src"));

            //There should be a room description
            Assert.AreEqual(HomePageTestData.RoomDescriptionParagraph, Page.Home.RoomDescriptionParagraph.Text);

            //There should be a booking button
            Assert.IsTrue(Page.Home.BookThisRoomButton.Displayed, "Page.Home.BookThisRoomButton.Displayed");
        }

        [Test]
        public void HomePageBookingContent()
        {
            BrowserFactory.GoToPage(HomePage.Url);
            BrowserFactory.WaitForPageLoad(10);

            //Click book this room
            Page.Home.BookThisRoomButton.Click();

            //A Cal should appear for the current month
            Assert.IsTrue(Page.Home.BookingCalendar.Displayed, "Page.Home.BookingCalendar.Displayed");

            //The firstname, lastname, email, phone fields, book button, cancel button, should exist
            Page.Home.ValidateBookingViewIsPresent();
        }

        [Test]
        public void HomePageBookingErrors()
        {
            BrowserFactory.GoToPage(HomePage.Url);
            BrowserFactory.WaitForPageLoad(10);

            //Click Book This Room
            Page.Home.BookThisRoomButton.Click();

            //Click Book without any fields filled
            Page.Home.BookingBookButton.Click();

            //Validate Error Messages
            var expectedErrorMessages = new List<string>()
            {
                "Firstname should not be blank",
                "size must be between 11 and 21",
                "size must be between 3 and 18",
                "must not be empty",
                "Lastname should not be blank",
                "must not be empty",
                "must not be null",
                "size must be between 3 and 30",
                "must not be null"
            };

            var actualErrorMessages = Page.Home.GetErrorMessages();

            //Sort the error messages since the site returns them in no particular order
            expectedErrorMessages.Sort();
            actualErrorMessages.Sort();

            //Validate they are correct
            Assert.AreEqual(expectedErrorMessages, actualErrorMessages);

            //Click Cancel
            Page.Home.CancelBookingInformation();

            //Validate that booking view isn't present
            Page.Home.ValidateBookingViewIsNotPresent();
        }

        [Test]
        public void HomePageBookingCancel()
        {
            BrowserFactory.GoToPage(HomePage.Url);
            BrowserFactory.WaitForPageLoad(10);

            //Click Book This Room
            Page.Home.BookThisRoomButton.Click();

            //Fill information
            var bookingInformation = new BookingInformation()
            {
                DateFrom = DateTime.Now.AddDays(1),
                DateTo = DateTime.Now.AddDays(2),
                FirstName = HomePageTestData.TestFirstName,
                LastName = HomePageTestData.TestLastName,
                Email = HomePageTestData.TestEmail,
                Phone = HomePageTestData.TestPhone
            };

            Page.Home.FillBookingInformation(bookingInformation);

            //Click Cancel
            Page.Home.CancelBookingInformation();

            //Validate that booking view isn't present
            Page.Home.ValidateBookingViewIsNotPresent();

            //Click Book This Room
            Page.Home.BookThisRoomButton.Click();

            //Validate that entered information isn't present
            Page.Home.ValidateBookingFormIsEmpty();
        }

        [Test]
        public void HomePageBookingSubmit()
        {
            BrowserFactory.GoToPage(HomePage.Url);
            BrowserFactory.WaitForPageLoad(10);

            //Click Book This Room
            Page.Home.BookThisRoomButton.Click();

            //Fill information
            var bookingInformation = new BookingInformation()
            {
                DateFrom = DateTime.Now.AddDays(3),
                DateTo = DateTime.Now.AddDays(4),
                FirstName = HomePageTestData.TestFirstName,
                LastName = HomePageTestData.TestLastName,
                Email = HomePageTestData.TestEmail,
                Phone = HomePageTestData.TestPhone
            };

            Page.Home.FillBookingInformation(bookingInformation);

            //Click Book
            Page.Home.SubmitBookingInformation();

            //Validate the success screen
            Page.Home.ValidateBookingSuccess(bookingInformation);
        }

        [Test]
        public void HomePageContactFormContent()
        {
            BrowserFactory.GoToPage(HomePage.Url);
            BrowserFactory.WaitForPageLoad(10);

            //There should be a contact form with name, email, phone, subject, message fields and a submit button
            Assert.IsTrue(Page.Home.ContactFormName.Displayed, "Page.Home.ContactFormName.Displayed");
            Assert.IsTrue(Page.Home.ContactFormEmail.Displayed, "Page.Home.ContactFormEmail.Displayed");
            Assert.IsTrue(Page.Home.ContactFormPhone.Displayed, "Page.Home.ContactFormPhone.Displayed");
            Assert.IsTrue(Page.Home.ContactFormSubject.Displayed, "Page.Home.ContactFormSubject.Displayed");
            Assert.IsTrue(Page.Home.ContactFormMessage.Displayed, "Page.Home.ContactFormMessage.Displayed");
            Assert.IsTrue(Page.Home.ContactFormSubmit.Displayed, "Page.Home.ContactFormSubmit.Displayed");

            //There should be inn name, address, phone, email test fields
            Assert.AreEqual(HomePageTestData.ContactFormInnName, Page.Home.ContactFormInnName.Text);
            Assert.AreEqual(HomePageTestData.ContactFormInnAddress, Page.Home.ContactFormInnAddress.Text);
            Assert.AreEqual(HomePageTestData.ContactFormInnPhone, Page.Home.ContactFormInnPhone.Text);
            Assert.AreEqual(HomePageTestData.ContactFormInnEmail, Page.Home.ContactFormInnEmail.Text);

            //There should be a map location image
            Assert.IsTrue(Page.Home.MapImage.Displayed, "Page.Home.MapImage.Displayed");
        }

        [Test]
        public void HomePageContactFormErrors()
        {
            BrowserFactory.GoToPage(HomePage.Url);
            BrowserFactory.WaitForPageLoad(10);

            //Click the Contact Form Submit Button
            Page.Home.SubmitContactFormInformation();

            //Validate Error Messages
            var expectedErrorMessages = new List<string>()
            {
                "must not be blank",
                "must not be blank",
                "must not be blank",
                "size must be between 20 and 2000",
                "size must be between 11 and 21",
                "must not be blank",
                "size must be between 5 and 100",
                "must not be blank"
            };

            var actualErrorMessages = Page.Home.GetErrorMessages();

            //Sort the error messages since the site returns them in no particular order
            expectedErrorMessages.Sort();
            actualErrorMessages.Sort();

            Assert.AreEqual(expectedErrorMessages, actualErrorMessages);
        }

        [Test]
        public void HomePageContactFormSubmit()
        {
            BrowserFactory.GoToPage(HomePage.Url);
            BrowserFactory.WaitForPageLoad(10);

            //Fill information
            var contactFormInformation = new ContactFormInformation()
            {
                Name = HomePageTestData.TestFullName,
                Email = HomePageTestData.TestEmail,
                Phone = HomePageTestData.TestPhone,
                Subject = HomePageTestData.TestSubject,
                Message = HomePageTestData.TestMessage
            };

            Page.Home.FillContactFormInformation(contactFormInformation);

            //Submit the contact form
            Page.Home.SubmitContactFormInformation();

            //Validate the form was submitted successfully
            Page.Home.ValidateContactFormSubmission();
        }

        #endregion Tests
    }
}
