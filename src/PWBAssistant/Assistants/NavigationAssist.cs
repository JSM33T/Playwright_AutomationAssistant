using Microsoft.Playwright;

namespace PlayWrightBotAssist.Assistants
{
    public static class NavigationAssist
    {
        public static async Task NavigateTo(IPage page, string url)
        {
            await page.GotoAsync(url);
        }
    }
}
