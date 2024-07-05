using Microsoft.AspNetCore.Mvc;
using Stripe;
using TestRESTAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent(dtoStripe amounts)
    {
   
        var options = new PaymentIntentCreateOptions
        {
            Amount = amounts.amount, // amount in cents
            Currency = "SAR",
            PaymentMethodTypes = new List<string> { "card" },
        };
        var service = new PaymentIntentService();
        var paymentIntent = service.Create(options);

        return Ok(new { clientSecret = paymentIntent.ClientSecret });
    }
}
