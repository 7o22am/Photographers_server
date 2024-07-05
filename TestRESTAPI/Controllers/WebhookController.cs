using Microsoft.AspNetCore.Mvc;
using Stripe;

[Route("webhook")]
[ApiController]
public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], "your_webhook_secret");

        if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            // Handle successful payment here
        }

        return Ok();
    }
}
