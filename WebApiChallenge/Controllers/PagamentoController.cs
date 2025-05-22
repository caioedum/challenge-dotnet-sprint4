using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace WebApiChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoController : ControllerBase
    {
        private readonly IStripeClient _stripeClient;

        public PagamentoController(IStripeClient stripeClient)
        {
            _stripeClient = stripeClient;
        }

        [HttpPost("criar-sessao")]
        public async Task<IActionResult> CriarSessao()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "brl",
                        UnitAmount = 5000,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Consulta Odontológica"
                        }
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = "https://challenge-dotnet.com/sucesso",
                CancelUrl = "https://challenge-dotnet.com/cancelado"
            };

            var service = new SessionService(_stripeClient);
            var session = await service.CreateAsync(options);

            return Ok(new { sessionId = session.Id, url = session.Url });
        }
    }
}
