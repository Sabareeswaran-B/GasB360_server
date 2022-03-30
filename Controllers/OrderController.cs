#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GasB360_server.Models;

namespace GasB360_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly GasB360Context _context;

        public OrderController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
           {
               var orders= await _context.TblOrders.ToListAsync();
               return Ok(
                   new
                    {
                        status = "success",
                        message = "Get all orders successfull.",
                        data = orders
                    }
               );
            
           }
           catch (System.Exception ex)
           {
               Sentry.SentrySdk.CaptureException(ex);
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // GET: api/Order/5
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
                        message = "Get order by id successfull.",
                        data = order
                    }
                );
           }
           catch (System.Exception ex)
           {
               Sentry.SentrySdk.CaptureException(ex);
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // GET: api/Order/5
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetOrderByCustomerId(Guid customerId)
        {
            try
            {
                var Customerorders = await _context.TblOrders
                    .Where(a => a.Active == "true")
                    .Where(a => a.CustomerId == customerId)
                    .ToListAsync();

                if (Customerorders == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get Orders by customer id successfull.",
                        data = Customerorders
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Order/5
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetOrderByEmployeeId(Guid employeeId)
        {
            try
            {
                var Orders = await _context.TblOrders
                    .Where(a => a.Active == "true")
                    .Where(a => a.EmployeeId == employeeId)
                    .ToListAsync();

                if (Orders == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get Orders by Employee id successfull.",
                        data = Orders
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                var Order=await _context.TblOrders.FindAsync(id);
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Update Order successfull.",
                        data = Order
                    }
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
                    return BadRequest(new{status="Failed",message = ex.Message});
                }
            }
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddNewOrder(TblOrder tblOrder)
        {
            try
           {
                _context.TblOrders.Add(tblOrder);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetOrderById", new { orderId = tblOrder.OrderId },
                new
                    {
                        status = "success",
                        message = "Add new order successfull.",
                        data = tblOrder
                    } );
           }
           catch (System.Exception ex)
           {
               Sentry.SentrySdk.CaptureException(ex);
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // DELETE: api/Order/5
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

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Delete order by id successfull."
                    }
                );
           }
           catch (System.Exception ex)
           {
               Sentry.SentrySdk.CaptureException(ex);
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        private bool IsOrderExists(Guid orderId)
        {
            return _context.TblOrders.Any(e => e.OrderId == orderId);
        }
    }
}
