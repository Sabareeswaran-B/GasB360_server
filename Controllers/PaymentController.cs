using GasB360_server.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace GasB360_server.Controllers;

public class PaymentsController : Controller
{
    private readonly GasB360Context _context;

    public PaymentsController(GasB360Context contex)
    {
        _context = contex;
        StripeConfiguration.ApiKey =
            "sk_test_51KlVTMSCjFUU5tTaAtk6bcQRG5SwsDL2tOUg1OOYD99j1IqzKYETrlOApY5kHsirh4VEqEBli3p89Gd0nYPEGBEn00rykos0QL";
    }

    [HttpPost("create-checkout-session")]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] TblOrder tblOrder)
    {
        try
        {
            var UnitAmount = tblOrder.OrderTotalprice.ToString() + "00";
            var filledProduct = await _context.TblFilledProducts.FindAsync(
                tblOrder.FilledProductId
            );
            filledProduct!.ProductCategory = await _context.TblProductCategories.FindAsync(
                filledProduct.ProductCategoryId
            );
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = long.Parse(UnitAmount),
                            Currency = "inr",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = filledProduct.ProductCategory!.ProductName,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl =
                    $"http://localhost:4200/order?customerId={tblOrder.CustomerId}&filledProductId={tblOrder.FilledProductId}&addressId={tblOrder.AddressId}&orderTotalprice={tblOrder.OrderTotalprice}",
                CancelUrl = $"http://localhost:4200/orders/address",
            };

            var service = new SessionService();
            Session session = service.Create(options);
            Console.WriteLine(session.Url);

            Response.Headers.Add("Location", session.Url);
            return Ok(
                new { status = "success", message = "Payment url created", data = session.Url }
            );
        }
        catch (System.Exception ex)
        {
            Sentry.SentrySdk.CaptureException(ex);
            return BadRequest(new { status = "failed", message = ex.Message });
        }
    }
}
