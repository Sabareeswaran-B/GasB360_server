using GasB360_server.Helpers;
using GasB360_server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace GasB360_server.Controllers;

public class PaymentsController : Controller
{
    private readonly GasB360Context _context;

    public PaymentsController(GasB360Context contex, IOptions<AppSettings> appSettings)
    {
        _context = contex;
        StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
    }

    [HttpPost("create-checkout-session")]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] TblOrder tblOrder)
    {
        try
        {
            var customer = await _context.TblCustomers.FindAsync(tblOrder.CustomerId);
            if (customer!.CustomerConnection < 1)
            {
                return BadRequest(new { status = "falied", message = "Get a connection first" });
            }
            var orders = await Task.FromResult(
                _context.TblOrders
                    .Where(x => x.CustomerId == tblOrder.CustomerId)
                    .OrderBy(x => x.OrderDate)
                    .LastOrDefault()
            );
            if (orders != null)
            {
                if (orders.OrderDate != null)
                {
                    var lastOrderDate = (DateTime)orders!.OrderDate! - DateTime.Now;
                    if (lastOrderDate.TotalDays <= 30)
                    {
                        return BadRequest(
                            new { status = "falied", message = "Already ordered for this month." }
                        );
                    }
                }
            }
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
                    $"https://gasb360.herokuapp.com/order?customerId={tblOrder.CustomerId}&filledProductId={tblOrder.FilledProductId}&addressId={tblOrder.AddressId}&orderTotalprice={tblOrder.OrderTotalprice}",
                CancelUrl = $"https://gasb360.herokuapp.com/orders/address",
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
