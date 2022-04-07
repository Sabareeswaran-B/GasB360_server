#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GasB360_server.Models;
using System.Net.Http.Headers;

namespace GasB360_server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly GasB360Context _context;

        public OrderController(GasB360Context context)
        {
            _context = context;
        }

        // API To Get All Of The Orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _context.TblOrders.ToListAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all orders successful.",
                        data = orders
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // API To Get The Order By Passing OrderId As Parameter
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            try
            {
                var order = await _context.TblOrders.FindAsync(orderId);

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get order by id successful.",
                        data = order
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // API To Get The Ordesr By Customer By Passing CustomerId As Parameter
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerId(Guid customerId)
        {
            try
            {
                var customerOrders = await _context.TblOrders
                    .Where(a => a.Active == "true")
                    .Where(a => a.CustomerId == customerId)
                    .ToListAsync();

                if (customerOrders == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get Orders by customer id successful.",
                        data = customerOrders
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // API To Get The Orders By Employee By Passing EmployeeId As Parameter

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetOrderByEmployeeId(Guid employeeId)
        {
            try
            {
                var orders = await _context.TblOrders
                    .Where(a => a.Active == "true")
                    .Where(a => a.OrderStatus != "Delivered")
                    .Where(a => a.EmployeeId == employeeId)
                    .Include(x => x.Address)
                    .Include(x => x.Customer)
                    .Include(x => x.Employee)
                    .Include(x => x.FilledProduct)
                    .Include(x => x.FilledProduct.ProductCategory)
                    .ToListAsync();

                if (orders == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get Orders by Employee id successful.",
                        data = orders
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Update The Order Details By Passing OrderId As Parameter

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, TblOrder tblOrder)
        {
            if (orderId != tblOrder.OrderId)
            {
                return BadRequest();
            }
            try
            {
                _context.Entry(tblOrder).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var Order = await _context.TblOrders.FindAsync(orderId);
                return Ok(
                    new { status = "success", message = "Update Order successful.", data = Order }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsOrderExists(orderId))
                {
                    return NotFound();
                }
                else
                {
                    Sentry.SentrySdk.CaptureException(ex);
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }

        //API To Check Order Delivery By OTP  By Passing OrderId And Otp As Parameter
        [HttpGet("{orderId}/{inputOtp}")]
        public async Task<IActionResult> OrderDeliveryCheckByOtp(Guid orderId,int inputOtp){
            try{
                var order = await _context.TblOrders.FindAsync(orderId);
                if(order.OrderOtp==inputOtp){
                order.OrderStatus="Delivered";
                _context.Entry(order).State = EntityState.Modified;
                var tblDelivery = new TblDelivery();
                tblDelivery.OrderId=orderId;
                _context.TblDeliveries.Add(tblDelivery);
                await _context.SaveChangesAsync();
                await AddUnfilledProduct(order.FilledProductId);
                await RemovefilledProduct(order.FilledProductId);
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Delivery By Otp successfull.",
                    }
                );
                }
                else{
                return BadRequest(new{status = "Failed",message="wrong Otp"});
                }

            }
            
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }

            
            

        }

        //API To Add New Order By Passing tblOrder Object As Parameter

        [HttpPost]
        public async Task<IActionResult> AddNewOrder(TblOrder tblOrder)
        {
            try
            {
                tblOrder.OrderOtp = OrderOtpGenerator();
                tblOrder.EmployeeId = await AssignEmployeeId();
                var customer = await _context.TblCustomers.FindAsync(tblOrder.CustomerId);

                //Send Otp to the customer
                using var client = new HttpClient();
                var sentID = "TXTIND";
                var message = $"Your order from GasB360 is placed successfully, OTP for your order is, \n OTP : {tblOrder.OrderOtp}";
                var phone = customer.CustomerPhone;
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded")
                );
                client.DefaultRequestHeaders.Add(
                    "authorization",
                    "xOKaf9FuYNR2O1KlfcoYtIXyS1ALi5ymXAgZt4Mb88zNvMH0lmAidKugEMN9"
                );
                var res = await client.PostAsJsonAsync(
                    "https://www.fast2sms.com/dev/bulkV2",
                    new { sender_id = sentID, message = message, numbers = phone, route = "v3" }
                );
                _context.TblOrders.Add(tblOrder);

                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetOrderById",
                    new { orderId = tblOrder.OrderId },
                    new
                    {
                        status = "success",
                        message = "Add new order successful.",
                        data = tblOrder
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Delete The Order By Passing OrderId As Parameter
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            try
            {
                var tblOrder = await _context.TblOrders.FindAsync(orderId);
                if (tblOrder == null)
                {
                    return NotFound();
                }

                _context.TblOrders.Remove(tblOrder);
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Delete order by id successful." });
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // Function To Check Whether The Order Already Exists or Not By Passing OrderId As Parameter
        private bool IsOrderExists(Guid orderId)
        {
            return _context.TblOrders.Any(e => e.OrderId == orderId);
        }

        // Function To Generate an OTP And Assign On Creating Order Request
        private int OrderOtpGenerator()
        {
            Random random = new Random();
            int otp = random.Next(100000);
            return otp;
        }

        //Function To Assign The EmployeeId Under Certain Condition On Creating Order Request
        private async Task<Guid> AssignEmployeeId()
        {
            var employee = await _context.TblEmployees
                .Where(x => x.Active == "true")
                .Include(x => x.Role)
                .Where(x => x.Role.RoleType == "delivery")
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefaultAsync();
            return employee.EmployeeId;
        }

        //Function To Add A UnFilled Product After Successfull Delivery By Passing FilledProductId As Parameter
        private async Task AddUnfilledProduct(Guid? filledProductId)
        {
            var filledProduct = await _context.TblFilledProducts
                .Where(x => x.Active == "true")
                .Where(x => x.FilledProductId == filledProductId)
                .Include(x => x.ProductCategory)
                .FirstOrDefaultAsync();
            var unfilledProduct = await _context.TblUnfilledProducts
                .Where(x => x.ProductCategoryId == filledProduct.ProductCategoryId)
                .FirstOrDefaultAsync();
            unfilledProduct.UnfilledProductQuantity += 1;
            _context.Entry(unfilledProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        //Function To Remove A Filled Product After Successfull Delivery By Passing FilledProductId As Parameter
        private async Task RemovefilledProduct(Guid? filledProductId)
        {
            var unfilledProduct = await _context.TblFilledProducts
                .Where(x => x.Active == "true")
                .Where(x => x.FilledProductId == filledProductId)
                .FirstOrDefaultAsync();

            if (unfilledProduct.FilledProductQuantity >= 2)
                unfilledProduct.FilledProductQuantity -= 1;
            else
                unfilledProduct.Active = "false";

            _context.Entry(unfilledProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        
    }
}
