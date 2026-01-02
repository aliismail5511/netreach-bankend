using Microsoft.AspNetCore.Mvc;
using NetReach.Api.Models;
using NetReach.Api.Services;

namespace NetReach.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NowPayController : ControllerBase
    {
        private readonly CryptomusService _cryptomusService;

        public NowPayController(CryptomusService cryptomusService)
        {
            _cryptomusService = cryptomusService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PaymentRequest request)
        {
            if (request == null || request.Price <= 0)
            {
                return BadRequest("Invalid request");
            }

            // Generate a unique order ID that includes product info
            var orderId = $"{Guid.NewGuid()}|{request.Email}|{request.Type}|{request.Quantity}";
            
            // In a real scenario, this should be your public URL
            var callbackUrl = "https://ek0bryxqm4.execute-api.eu-north-1.amazonaws.com/api/Payment/Webhook";

            var paymentUrl = await _cryptomusService.CreateInvoiceAsync(request.Price, orderId, callbackUrl);

            if (string.IsNullOrEmpty(paymentUrl))
            {
                return StatusCode(500, "Failed to create payment");
            }

            return Ok(new { url = paymentUrl });
        }
    }
}
