using MailKit.Net.Smtp;
using MimeKit;

namespace NetReach.Api.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.hostinger.com";
        private readonly int _smtpPort = 465;
        private readonly string _senderEmail = "info@netreach.site";
        private readonly string _senderPassword = "Losha55**"; // User needs to fill this

        public async Task SendProductEmailAsync(string recipientEmail, string productName, List<string> items)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("NetReach", _senderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = $"Your {productName} Purchase - NetReach";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h2>Thank you for your purchase!</h2>
                    <p>You have successfully purchased {items.Count} {productName}(s).</p>
                    <p>Here are your items:</p>
                    <pre style='background: #f4f4f4; padding: 10px; border: 1px solid #ddd;'>{string.Join("\n", items)}</pre>
                    <p>If you have any questions, feel free to contact us.</p>
                    <p>Best regards,<br>NetReach Team</p>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, true);
                await client.AuthenticateAsync(_senderEmail, _senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email Error: {ex.Message}");
            }
        }
    }
}
