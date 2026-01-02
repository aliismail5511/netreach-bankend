using Microsoft.AspNetCore.Mvc;
using NetReach.Api.Services;
using Newtonsoft.Json;

namespace NetReach.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly EmailService _emailService;
        private readonly CryptomusService _cryptomusService;

        public PaymentController(ProductService productService, EmailService emailService, CryptomusService cryptomusService)
        {
            _productService = productService;
            _emailService = emailService;
            _cryptomusService = cryptomusService;
        }

        [HttpPost("Webhook")]
        public async Task<IActionResult> Webhook()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            
            // Log the webhook for debugging
            Console.WriteLine($"Webhook received: {body}");

            dynamic data = JsonConvert.DeserializeObject(body)!;
            string status = data.status;
            string orderId = data.order_id;

            // Check if payment is successful
            if (status == "paid" || status == "completed")
            {
                // Parse orderId: {Guid}|{Email}|{Type}|{Quantity}
                var parts = orderId.Split('|');
                if (parts.Length == 4)
                {
                    string email = parts[1];
                    int type = int.Parse(parts[2]);
                    int quantity = int.Parse(parts[3]);

                    var items = _productService.GetRandomItems(type, quantity);
                    var productName = _productService.GetProductName(type);

                    await _emailService.SendProductEmailAsync(email, productName, items);
                }
            }

            return Ok();
        }
    }
}
