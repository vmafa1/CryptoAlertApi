 using System.ComponentModel.DataAnnotations;

    namespace CryptoAlertApi.Models
    {

    public class AlertRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        // ✅ Optional Telegram message content
        public string TelegramMessage { get; set; }
    }
}


