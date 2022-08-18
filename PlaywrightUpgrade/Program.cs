using System.Text;
using Microsoft.Playwright;

namespace PlaywrightUpgrade
{
    internal partial class Program
    {
        private static IPage? _page;

        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Installing browsers");

                // The following line installs the default browsers. If you only need a subset of browsers,
                // you can specify the list of browsers you want to install among: chromium, chrome,
                // chrome-beta, msedge, msedge-beta, msedge-dev, firefox, and webkit.
                // var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "webkit", "chrome" });
                // If you need to install dependencies, you can add "--with-deps"
                var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
                if (exitCode != 0)
                {
                    Console.WriteLine("Failed to install browsers");
                    Environment.Exit(exitCode);
                }

                Console.WriteLine("Browsers installed");

                using var playwright = await Playwright.CreateAsync();
                await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });

                // Open new page
                _page = await browser.NewPageAsync();

                var browserInstance = new Browser(_page);

                await browserInstance.LoginToAccountSummaryAsync();

                var workingAsExpected = _page.Url == "https://online.lloydsbank.co.uk/personal/logon/login.jsp?messageKey=IB:9210358&mobile=false";

                Console.WriteLine($"workingAsExpected = {workingAsExpected}");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(string.Empty);
                Console.WriteLine(FormatStackTrace(ex));
                Console.WriteLine(string.Empty);
                Console.WriteLine("Press any key to continue ...");
                Console.ReadKey();
            }
            finally
            {
                _page?.CloseAsync();
            }

            static string FormatStackTrace(Exception ex)
            {
                var s = new StringBuilder();

                while (ex != null)
                {
                    s.AppendLine("Exception type: " + ex.GetType().FullName);
                    s.AppendLine("Message       : " + ex.Message);
                    s.AppendLine("Stacktrace:");
                    s.AppendLine(ex.StackTrace!.Replace(" in ", Environment.NewLine + "\tin ").Replace(":line ", Environment.NewLine + "\tline "));
                    s.AppendLine();

                    ex = ex.InnerException!;
                }

                var stackTrace = s.ToString();

                return stackTrace;
            }
        }
    }
}