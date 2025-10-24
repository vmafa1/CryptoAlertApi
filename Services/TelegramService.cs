using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoAlertApi.Services
{


    public class TelegramService
    {
        private readonly HttpClient _http;

        public TelegramService(HttpClient http)
        {
            _http = http;
        }

        public async Task SendTelegramAlert(string message)
        {
            var botToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
            var chatId = Environment.GetEnvironmentVariable("TELEGRAM_CHAT_ID");

            if (string.IsNullOrEmpty(botToken) || string.IsNullOrEmpty(chatId))
                throw new InvalidOperationException("Telegram credentials are missing from environment variables.");

            var url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={Uri.EscapeDataString(message)}";
            await _http.GetAsync(url);
        }
    }
}
