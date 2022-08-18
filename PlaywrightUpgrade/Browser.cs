using Microsoft.Playwright;

namespace PlaywrightUpgrade
{
    internal class Browser
    {
        public Browser(IPage page)
        {
            Page = page;
        }

        private IPage Page { get; }

        public async Task LoginToAccountSummaryAsync()
        {
            await Page.GotoAsync("https://online.lloydsbank.co.uk/personal/logon/login.jsp?WT.ac=TopLink/Navigation/Personal/");

            await LogonWithUsernameAndPasswordAsync();
        }

        private async Task LogonWithUsernameAndPasswordAsync()
        {
            await Page.Locator("input[name=\"frmLogin\\:strCustomerLogin_userID\"]").ClickAsync();
            await Page.Locator("input[name=\"frmLogin\\:strCustomerLogin_userID\"]").FillAsync("BadUserName");

            await Page.Locator("input[name=\"frmLogin\\:strCustomerLogin_pwd\"]").ClickAsync();
            await Page.Locator("input[name=\"frmLogin\\:strCustomerLogin_pwd\"]").FillAsync("BadPassword");

            await Page.Locator("input[alt=\"Continue\"]").ClickAsync();
        }
    }
}