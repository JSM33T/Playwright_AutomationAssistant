using Microsoft.Playwright;

namespace PlayWrightBotAssist
{
    public interface IPWBAssist
    {
        public Task Initialize();
        public Task NavigateTo(string url);
        public Task CleanUp();
        public Task<IPage> GetPageObject();
    }
}
