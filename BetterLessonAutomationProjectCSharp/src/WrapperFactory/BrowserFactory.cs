using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BetterLessonAutomationProjectCSharp.src.WrapperFactory
{
    /// <summary>
    /// The Browser Factory for easy driver initialization
    /// </summary>
    public static class BrowserFactory
    {
        /// <summary>
        /// A stopwatch for keeping track of the execution time of tests
        /// </summary>
        public static readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        private static readonly IDictionary<string, IWebDriver> Drivers = new Dictionary<string, IWebDriver>();
        private static IWebDriver _driver;

        /// <summary></summary>
        public static IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method InitBrowser.");
                }
                return _driver;
            }
            private set => _driver = value;
        }

        /// <summary>
        /// Initializes the driver based on the supplied WebDriver enum
        /// </summary>
        /// <param name="webDriver">The Browser enum</param>
        public static void InitBrowser(WebDriver webDriver)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            switch (webDriver)
            {
                case WebDriver.Chrome:
                    Driver = new ChromeDriver(path);
                    Drivers.Add("Chrome", Driver);
                    break;
                case WebDriver.Firefox:
                    Driver = new FirefoxDriver(path);
                    Drivers.Add("Firefox", Driver);
                    break;
                case WebDriver.InternetExplorer:
                    Driver = new InternetExplorerDriver(path);
                    Drivers.Add("InternetExplorer", Driver);
                    break;
                case WebDriver.Edge:
                    Driver = new EdgeDriver(path);
                    Drivers.Add("Edge", Driver);
                    break;
                case WebDriver.Opera:
                    Driver = new OperaDriver(path);
                    Drivers.Add("Opera", Driver);
                    break;
                case WebDriver.Safari:
                    Driver = new SafariDriver(path);
                    Drivers.Add("Safari", Driver);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(webDriver), webDriver, null);
            }

            MaximizeWindow();
            SetImplicitWait(5);
        }

        /// <summary>
        /// Initializes the driver based on the supplied WebDriver enum
        /// </summary>
        /// <param name="webDriver">The Browser enum</param>
        public static void InitBrowserHeadless(WebDriver webDriver)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            switch (webDriver)
            {
                case WebDriver.Chrome:
                    List<string> args = new List<string> { "headless", "window-size=1920,1080" };
                    Driver = new ChromeDriver(path, SetChromeOptions(args));
                    Drivers.Add("Chrome", Driver);
                    break;
                case WebDriver.Firefox:
                    throw new NotSupportedException();
                case WebDriver.InternetExplorer:
                    throw new NotSupportedException();
                case WebDriver.Edge:
                    throw new NotSupportedException();
                case WebDriver.Opera:
                    throw new NotSupportedException();
                case WebDriver.Safari:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(webDriver), webDriver, null);
            }

            MaximizeWindow();
            SetImplicitWait(5);
        }

        /// <summary>
        /// Close all active drivers for the session
        /// </summary>
        public static void CloseAllDrivers()
        {
            foreach (string key in Drivers.Keys)
            {
                Drivers[key].Close();
                Drivers[key].Quit();
            }

            Drivers.Clear();
        }

        /// <summary>
        /// Set the Browser to be used for the WebDriver
        /// </summary>
        public enum WebDriver
        {
            /// <summary>The Chrome Browser</summary>
            Chrome,
            /// <summary>The Firefox Browser</summary>
            Firefox,
            /// <summary>The Internet Explorer Browser</summary>
            InternetExplorer,
            /// <summary>The Edge Browser</summary>
            Edge,
            /// <summary>The Opera Browser</summary>
            Opera,
            /// <summary>The Safari Browser</summary>
            Safari
        }

        #region Basic Browser Operations

        /// <summary>
        /// Go to page by URL
        /// </summary>
        /// <param name="url"></param>
        public static void GoToPage(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Gets the title of the current page
        /// </summary>
        /// <returns></returns>
        public static string GetPageTitle()
        {
            return _driver.Title;
        }

        /// <summary>
        /// Gets the url of the current page
        /// </summary>
        /// <returns></returns>
        public static string GetPageUrl()
        {
            return _driver.Url;
        }

        /// <summary>
        /// Gets the source code of the current page
        /// </summary>
        /// <returns></returns>
        public static string GetPageSource()
        {
            return _driver.PageSource;
        }

        #endregion Basic Browser Operations

        #region Advanced Browser Operations

        /// <summary>
        /// Accepts the current popup
        /// </summary>
        public static void AcceptPopup()
        {
            IAlert alert = _driver.SwitchTo().Alert();
            alert.Accept();
        }

        /// <summary>
        /// Dismisses the current popup
        /// </summary>
        public static void DismissPopup()
        {
            IAlert alert = _driver.SwitchTo().Alert();
            alert.Dismiss();
        }

        /// <summary>
        /// Switches to the first browser tab
        /// </summary>
        public static void SwitchToFirstTab()
        {
            ReadOnlyCollection<string> windowHandles = _driver.WindowHandles;
            string firstTab = windowHandles.First();
            _driver.SwitchTo().Window(firstTab);
        }

        /// <summary>
        /// Switches to the last browser tab
        /// </summary>
        public static void SwitchToLastTab()
        {
            ReadOnlyCollection<string> windowHandles = _driver.WindowHandles;
            string lastTab = windowHandles.Last();
            _driver.SwitchTo().Window(lastTab);
        }

        /// <summary>
        /// Switches to the next browser tab unless you're already at the end
        /// </summary>
        public static void SwitchToNextTab()
        {
            ReadOnlyCollection<string> windowHandles = _driver.WindowHandles;
            int currentIndex = windowHandles.IndexOf(_driver.CurrentWindowHandle);

            if (currentIndex < windowHandles.Count - 1)
            {
                _driver.SwitchTo().Window(windowHandles[currentIndex + 1]);
            }
        }

        /// <summary>
        /// Switches to the previous browser tab unless you're already at the beginning
        /// </summary>
        public static void SwitchToPreviousTab()
        {
            ReadOnlyCollection<string> windowHandles = _driver.WindowHandles;
            int currentIndex = windowHandles.IndexOf(_driver.CurrentWindowHandle);

            if (currentIndex > 0)
            {
                _driver.SwitchTo().Window(windowHandles[currentIndex - 1]);
            }
        }

        /// <summary>
        /// Uses the browser back button
        /// </summary>
        public static void GoBack()
        {
            _driver.Navigate().Back();
        }

        /// <summary>
        /// Uses the browser forward button
        /// </summary>
        public static void GoForward()
        {
            _driver.Navigate().Forward();
        }

        /// <summary>
        /// Uses the browser refresh button
        /// </summary>
        public static void Refresh()
        {
            _driver.Navigate().Refresh();
        }

        /// <summary>
        /// Scrolls to a hyperlink and focuses it
        /// </summary>
        /// <param name="link">The hyperlink</param>
        public static void FocusLink(string link)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].focus();", link);
        }

        /// <summary>
        /// Maximizes the browser window
        /// </summary>
        private static void MaximizeWindow()
        {
            _driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Sets the implicit wait for element visibility and presence
        /// </summary>
        /// <param name="seconds">The number of seconds to wait</param>
        private static void SetImplicitWait(int seconds)
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Adds a cookie key and value to the driver
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddCookie(string key, string value)
        {
            Cookie cookie = new Cookie("key", "value");
            _driver.Manage().Cookies.AddCookie(cookie);
        }

        /// <summary>
        /// Gets all cookies
        /// </summary>
        /// <returns>ReadOnlyCollection of type Cookie</returns>
        public static ReadOnlyCollection<Cookie> GetCookies()
        {
            return _driver.Manage().Cookies.AllCookies;
        }

        /// <summary>
        /// Deletes a cookie by name
        /// </summary>
        /// <param name="name">The name of the cookie</param>
        public static void DeleteCookie(string name)
        {
            _driver.Manage().Cookies.DeleteCookieNamed(name);
        }

        /// <summary>
        /// Deletes all cookies
        /// </summary>
        public static void DeleteCookies()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
        }

        /// <summary>
        /// Takes a screenshot and saves it at the supplied path
        /// </summary>
        /// <param name="path">The path to save the image at</param>
        public static void TakeScreenshot(string path)
        {
            Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
        }

        /// <summary>
        /// Waits for the page to load using document ready state and the supplied timeout
        /// </summary>
        /// <param name="timeout">The number of seconds to wait</param>
        public static void WaitForPageLoad(int timeout = 30)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeout));
            wait.Until((x) => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        /// <summary>
        /// Switches to an iframe by index
        /// </summary>
        /// <param name="index">the index of the iframe</param>
        public static void SwitchToFrameByIndex(int index)
        {
            _driver.SwitchTo().Frame(index);
        }

        /// <summary>
        /// Switches to an iframe by its name
        /// </summary>
        /// <param name="name">the name of the iframe</param>
        public static void SwitchToFrameByName(string name)
        {
            _driver.SwitchTo().Frame(name);
        }

        /// <summary>
        /// Switches to an iframe by element
        /// </summary>
        /// <param name="element">the element for the iframe</param>
        public static void SwitchToFrameByElement(IWebElement element)
        {
            _driver.SwitchTo().Frame(element);
        }

        /// <summary>
        /// Switches to the default iframe
        /// </summary>
        public static void SwitchToDefaultFrame()
        {
            _driver.SwitchTo().DefaultContent();
        }

        #endregion Advanced Browser Operations

        #region Advanced Browser Configurations

        /// <summary>
        /// Sets the firefox profile by username
        /// </summary>
        /// <param name="userName">the username to use</param>
        public static void UseFirefoxProfile(string userName)
        {
            FirefoxProfileManager profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile(userName);
            FirefoxOptions options = new FirefoxOptions { Profile = profile };
            _driver = new FirefoxDriver(options);
        }

        /// <summary>
        /// Create a firefox profile and set the network proxy settings
        /// </summary>
        /// <param name="networkProxyType"></param>
        /// <param name="networkProxyHttp"></param>
        /// <param name="networkProxyHttpPort"></param>
        public static void SetFirefoxHttpProxy(int networkProxyType = 1, string networkProxyHttp = "myproxy.com", int networkProxyHttpPort = 3239)
        {
            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("network.proxy.type", networkProxyType);
            profile.SetPreference("network.proxy.http", networkProxyHttp);
            profile.SetPreference("network.proxy.http_port", networkProxyHttpPort);
            FirefoxOptions options = new FirefoxOptions { Profile = profile };
            _driver = new FirefoxDriver(options);
        }

        /// <summary>
        /// Create a chrome options and set the network proxy settings
        /// </summary>
        /// <param name="proxyKind"></param>
        /// <param name="isAutoDetect"></param>
        /// <param name="httpProxy"></param>
        /// <param name="sslProxy"></param>
        public static void SetChromeHttpProxy(ProxyKind proxyKind = ProxyKind.Manual, bool isAutoDetect = false, string httpProxy = "", string sslProxy = "127.0.0.1:3239")
        {
            ChromeOptions options = new ChromeOptions();
            Proxy proxy = new Proxy
            {
                Kind = proxyKind,
                IsAutoDetect = isAutoDetect,
                HttpProxy = httpProxy,
                SslProxy = sslProxy
            };
            options.Proxy = proxy;
            options.AddArgument("ignore-certificate-errors");
            _driver = new ChromeDriver(options);
        }

        /// <summary>
        /// Accept all certificates for firefox, currently not implemented
        /// </summary>
        public static void AcceptAllCertificatesFirefox()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Accept all certificates for chrome
        /// </summary>
        public static void AcceptAllCertificatesChrome()
        {
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            _driver = new ChromeDriver(options);
        }

        /// <summary>
        /// Set Chrome options, currently not implemented
        /// </summary>
        public static ChromeOptions SetChromeOptions(List<string> args)
        {
            var chromeOptions = new ChromeOptions();
            foreach (var arg in args)
            {
                chromeOptions.AddArgument(arg);
            }
            return chromeOptions;
        }

        /// <summary>
        /// Disables javascript for Firefox within options
        /// </summary>
        /// <param name="userName"></param>
        public static void DisableJavascriptFirefox(string userName)
        {
            FirefoxProfileManager profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile(userName);
            profile.SetPreference("javascript.enabled", false);
            FirefoxOptions options = new FirefoxOptions { Profile = profile };
            _driver = new FirefoxDriver(options);
        }

        /// <summary>
        /// Set the default page load timeout for the driver
        /// </summary>
        /// <param name="timeout"></param>
        public static void SetDefaultPageLoadTimeout(int timeout)
        {
            _driver.Manage().Timeouts().PageLoad = new TimeSpan(timeout);
        }

        /// <summary>
        /// start firefox with plugins at the specified path
        /// </summary>
        /// <param name="pluginPath"></param>
        public static void StartFirefoxWithPlugins(string pluginPath = @"C:extensionsLocationextension.xpi")
        {
            FirefoxProfile profile = new FirefoxProfile();
            profile.AddExtension(pluginPath);
            FirefoxOptions options = new FirefoxOptions { Profile = profile };
            _driver = new FirefoxDriver(options);
        }

        /// <summary>
        /// Start chrome unpacked, currently not implemented
        /// </summary>
        public static void StartChromeUnpacked()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Start chrome packed, currently not implemented
        /// </summary>
        public static void StartChromePacked()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set the default download save location for firefox
        /// </summary>
        /// <param name="downloadFolderPath"></param>
        /// <param name="browserDownloadFolderList"></param>
        /// <param name="browserDownloadManagerAlertOnExeOpen"></param>
        public static void ChangeDefaultSaveLocation(string downloadFolderPath = @"c:temp", int browserDownloadFolderList = 2, bool browserDownloadManagerAlertOnExeOpen = false)
        {
            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("browser.download.folderList", browserDownloadFolderList);
            profile.SetPreference("browser.download.dir", downloadFolderPath);
            profile.SetPreference("browser.download.manager.alertOnEXEOpen", browserDownloadManagerAlertOnExeOpen);
            profile.SetPreference("browser.helperApps.neverAsk.saveToDisk",
                @"application/msword, application/binary, application/ris, text/csv, image/png, application/pdf,
            text / html, text / plain, application / zip, application / x - zip, application / x - zip - compressed, application / download,
            application / octet - stream");
            FirefoxOptions options = new FirefoxOptions { Profile = profile };
            _driver = new FirefoxDriver(options);
        }

        #endregion Advanced Browser Configurations
    }
}
