using System;
using System.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace OPeLaskuGrab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IWebDriver driver = new FirefoxDriver();

        public MainWindow()
        {
            InitializeComponent();

            driver.Navigate().GoToUrl("https://www.op.fi/");
            //driver.FindElement(By.Name("USERID")).SendKeys("123456");
            //driver.FindElement(By.Name("PASSWORD")).SendKeys("7890");
            //driver.FindElement(By.Id("op-login-btn-sidebar")).Click();


        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);


                DateTime day = new DateTime(2021, 7, 1); //Koodaa alku tähän
                DateTime end = new DateTime(2021, 12, 31); // Koodaa loppu tähän

                for (; day < end; day = day.AddDays(1))
                {
                    int c = 1;
                    while (true)
                    {
                        System.Console.WriteLine("Päivä:" + day.ToString("dd.MM.yyyy"));
                        System.Threading.Thread.Sleep(1000);
                        driver.Navigate().GoToUrl("https://www.op.fi/auth/paivittaiset/e-laskut/e-laskuarkisto");
                        //driver.FindElement(By.Id("dynamic-navigation-link-item-id-45")).Click();
                        driver.FindElement(By.Name("alkupvm")).Clear();
                        driver.FindElement(By.Name("alkupvm")).SendKeys(day.ToString("dd.MM.yyyy"));
                        driver.FindElement(By.Name("loppupvm")).Clear();
                        driver.FindElement(By.Name("loppupvm")).SendKeys(day.AddDays(1).ToString("dd.MM.yyyy"));

                        System.Console.WriteLine("Hakunappi");
                        driver.FindElement(By.ClassName("HakuNappi")).Click();

                        IWebElement elem;
                        try
                        {
                            elem = driver.FindElement(
                                By.XPath("//table[contains(@class, 'wd-hakutulos-taulukko')]/tbody/tr[" + c +
                                         "]/td[2]/div/a"));
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine(ex);
                            break;
                        }

                        if (elem == null)
                            break;

                        Download(elem);

                        c++;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

            }
        }


        void Download(IWebElement e)
        {
            System.Console.WriteLine("Download1");
            // Klikkaa riviä listassa
            e.Click();
            System.Console.WriteLine("Download2");
            driver.FindElement(By.ClassName("opux-action-link")).Click();

            //Expander
            var elems0 = driver.FindElements(By.TagName("a"));
            foreach (var el in elems0)
            {
                if (el.Text == "Laskun tallennus omalle koneelle")
                {
                    System.Console.WriteLine("Download3");
                    el.Click();
                    break;
                }
            }
            System.Threading.Thread.Sleep(500);
            var elems = driver.FindElements(By.ClassName("opux-action-link"));
            foreach (var el in elems)
            {
                if (el.Text == "Tallenna lasku omalle koneelle")
                {
                    System.Console.WriteLine("Download3");
                    el.Click();
                    break;
                }
            }
        }
    }

}
