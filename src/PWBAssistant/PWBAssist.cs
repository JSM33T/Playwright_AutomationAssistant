using Microsoft.Playwright;
using PlayWrightBotAssist;
using PlayWrightBotAssist.Assistants;

namespace PWBAssistant
{
    public class PWBAssist : IPWBAssist
    {
        private  IPlaywright _playwright;
        private  IBrowser _browser;
        private  IPage _page;
        public async Task Initialize()
        {
            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = false,
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
                Args = new[]
                {
                    "--no-sandbox",
                    "--disable-setuid-sandbox",
                    "--disable-extensions",
                    "--disable-gpu",
                    "--disable-dev-shm-usage",
                    "--disable-infobars",
                    "--disable-blink-features=AutomationControlled",
                    "--start-maximized",
                    "--num-raster-threads=1",
                    "--disable-features=site-per-process",
                    "--disable-renderer-backgrounding",
                    "--disable-background-timer-throttling",
                    "--disable-backgrounding-occluded-windows",
                    "--disable-background-networking",
                    "--no-zygote",
                    "--disable-notifications"
                }
            };

            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(launchOptions);
            _page = await _browser.NewPageAsync();
        }

        public async Task NavigateTo(string url) => await NavigationAssist.NavigateTo(_page, url);

        public async Task CleanUp() => await _browser.CloseAsync();

        public async Task<IPage> GetPageObject() => _page;
    }
}
