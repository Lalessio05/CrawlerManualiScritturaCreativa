using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CrawlerManualiScritturaCreativa
{

    internal class Program
    {

        static WebDriver driver = new ChromeDriver();
        static readonly WebDriverWait wait = new(driver, TimeSpan.FromSeconds(5));


        static string ImgXpath => @"(.//img[@class='img-fluid'])[1]";
        static string LinksXpath => @"//ol//li//a";
        static List<string> XpathDaEliminare =>
            [
                @".//div[@class='panel__body']/div[@class='panel__block']/..",
                @"//*[@id='section-header']/div",
                @"//*[@id='section-blog_sidebar']/div",
                @"(//div[@class='panel__body']//p)[last()]",
                @"(//div[@class='panel__body']//p)[last()]",
                @"(//div[@class='panel__body']//p)[last()]",
                @"(//div[@class='panel__body']//p)[last()]",
                @".//footer",
        ];
        static void CrawlPage(string link, string nome, int numeroCapitolo)
        {
            driver.Navigate().GoToUrl(link);
            wait.Until(x => driver.FindElement(By.XPath(ImgXpath)).Displayed);

            foreach (var xpath in XpathDaEliminare)
            {
                driver.ExecuteScript($"var elementToRemove = document.evaluate(\r\n    \"{xpath}\",  \r\n    document,                              \r\n    null,                                  \r\n    XPathResult.FIRST_ORDERED_NODE_TYPE,   \r\n    null                                   \r\n).singleNodeValue;\r\n\r\nelementToRemove.parentNode.removeChild(elementToRemove);\r\n");
            }

            var a = driver.Print(new PrintOptions());
            a.SaveAsFile($@"C:\Users\catri\Downloads\Nuova cartella\{numeroCapitolo}.{nome}.pdf".Replace("?", "").Replace(": ", "_ ").Replace("\"", "'"));
        }
        static void Main()
        {
            string url = @"https://www.agenziaduca.it/manuale-gratuito-scrittura-creativa";
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);
            var links = driver.FindElements(By.XPath(LinksXpath)).Select(x => x.GetAttribute("href")).ToList();
            var names = driver.FindElements(By.XPath(LinksXpath)).Select(x => x.Text).ToList();
            foreach (var link in links)
            {
                CrawlPage(link, names[links.IndexOf(link)], links.IndexOf(link) + 1);
            }
            driver.Quit();
        }
    }
}
