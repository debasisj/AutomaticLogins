using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace AutomaticLogins
{
    public class InitDriver
    {

        private static IWebDriver webDriver;

        public static void Init(string driverTyper, string userId, string pwd, AppVals appVals, string url = null, string envVal = null)
        {
            try
            {
                var test1 = url;
                var test2 = envVal;
                string path = Path.Combine(Environment.CurrentDirectory, @"Drivers\");
                int WaitTimeInSeconds = 2000;
                //string application = 
                switch (driverTyper.ToLower())
                {
                    case "chrome":
                        webDriver = new ChromeDriver(path);
                        break;

                    case "firefox":
                        webDriver = new FirefoxDriver(path);
                        break;

                    case "ie":
                        webDriver = new InternetExplorerDriver(path);
                        break;
                    default:
                        throw new NotSupportedException("This browser is not supportd");
                       
                }
                var findBy = appVals.findBy;
                By userIdFindBy = null;
              
                By pwdFindBy = null;
                By selectPathFindBy = null;
                By selectValFindBy = null;
                By submitFindBy = null;

                switch (findBy)
                {
                    case "css":
                        userIdFindBy = By.CssSelector(appVals.userId);
                        pwdFindBy = By.CssSelector(appVals.password);
                        selectPathFindBy = By.CssSelector(appVals.selectPath);
                        selectValFindBy = By.CssSelector(appVals.selectBy);
                        submitFindBy = By.CssSelector(appVals.submit);
                        break;
                    case "id":
                        userIdFindBy = By.Id(appVals.userId);
                        pwdFindBy = By.Id(appVals.password);
                        selectPathFindBy = By.Id(appVals.selectPath);
                        selectValFindBy = By.Id(appVals.selectBy);
                        submitFindBy = By.Id(appVals.submit);
                        break;
                    case "xpath":
                        userIdFindBy = By.XPath(appVals.userId);
                        pwdFindBy = By.XPath(appVals.password);
                        selectPathFindBy = By.XPath(appVals.selectPath);
                        selectValFindBy = By.XPath(appVals.selectBy);
                        submitFindBy = By.XPath(appVals.submit);

                        break;

                    default:
                        break;
                }
                if (url == null)
                {
                    webDriver.Navigate().GoToUrl(appVals.url);
                }
                else
                {
                    webDriver.Navigate().GoToUrl(url);
                }
              
                Thread.Sleep(WaitTimeInSeconds);
                webDriver.Manage().Window.Maximize();
               
                webDriver.FindElement(userIdFindBy).SendKeys(userId);
                webDriver.FindElement(pwdFindBy).SendKeys(pwd);

                if(appVals.selectEnv)
                {
                    webDriver.FindElement(selectPathFindBy).Click();
                    var items = webDriver.FindElements(selectValFindBy);
                    string textTofind = envVal == null ? appVals.defaultSelectEnv : envVal;
                    foreach (var item in items)
                    {
                        if (item.Text.Contains(textTofind))
                        {
                            item.Click();
                            break;
                        }
                    }
                }
                webDriver.FindElement(submitFindBy).Click();
                Thread.Sleep(WaitTimeInSeconds);
    
                //webDriver.Close();
                //webDriver.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
