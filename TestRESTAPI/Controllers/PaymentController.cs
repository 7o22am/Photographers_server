using Microsoft.AspNetCore.Mvc;
using Stripe;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    [HttpPost("create-payment-intent")]
    public ActionResult CreatePaymentIntent()
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = 1099, // amount in cents
            Currency = "usd",
            PaymentMethodTypes = new List<string> { "card" },
        };
        var service = new PaymentIntentService();
        var paymentIntent = service.Create(options);

        return Ok(new { clientSecret = paymentIntent.ClientSecret });
    }
}
