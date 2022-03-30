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
    [Route("[controller]/[action]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly GasB360Context _context;

        public DeliveryController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Delivery
        [HttpGet]
        public async Task<IActionResult> GetAllDeliveries()
        {
            try{
                var deliveries= await _context.TblDeliveries.ToListAsync();
                return Ok(
                   new
                    {
                        status = "success",
                        message = "Get all Deliverys successfull.",
                        data = deliveries
                    }
               );
            }
            catch (System.Exception ex)
           {
               Sentry.SentrySdk.CaptureException(ex);
               return BadRequest(new{status="Failed",message = ex.Message});
           }

            

        }

        // GET: api/Delivery/5
        [HttpGet("{deliveryId}")]
        public async Task<IActionResult> GetDeliveryById(Guid deliveryId)
        {
            try{
                var delivery = await _context.TblDeliveries.FindAsync(deliveryId);

                if (delivery == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get order by id successfull.",
                        data = delivery
                    }
                );

            }
            catch (System.Exception ex)
           {
               Sentry.SentrySdk.CaptureException(ex);
               return BadRequest(new{status="Failed",message = ex.Message});
           }
            
        }

        // GET: api/Delivery/5

        // Remainder : 
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetDeliveriesByEmployeeId(Guid employeeId)
        {
            try
            {
                var Deliveries = await _context.TblDeliveries
                    .Where(a => a.Active == "true")
                    .Include(a=>a.Order)
                    .Where(a => a.Order.EmployeeId== employeeId)
                    .ToListAsync();

                if (Deliveries== null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get Deliveries by Employee id successfull.",
                        data = Deliveries
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }


        // PUT: api/Delivery/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{deliveryId}")]
        public async Task<IActionResult> UpdateDelivery(Guid deliveryId, TblDelivery tblDelivery)
        {
            if (deliveryId != tblDelivery.DeliveryId)
            {
                return BadRequest();
            }
            try
            {
                _context.Entry(tblDelivery).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var Delivery=await _context.TblDeliveries.FindAsync(deliveryId);
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Update Delivery successfull.",
                        data = Delivery
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (IsDeliveryExists(deliveryId))
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

        // POST: api/Delivery
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddNewDelivery(TblDelivery tblDelivery)
        {
            try{
                _context.TblDeliveries.Add(tblDelivery);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDeliveryById", new { deliveryId = tblDelivery.DeliveryId },
                new
                    {
                        status = "success",
                        message = "Add new order successfull.",
                        data = tblDelivery
                    }  );

            }
            catch (System.Exception ex)
           {
               Sentry.SentrySdk.CaptureException(ex);
               return BadRequest(new{status="Failed",message = ex.Message});
           }

            
        }

        // DELETE: api/Delivery/5
        [HttpDelete("{deliveryId}")]
        public async Task<IActionResult> DeleteDelivery(Guid deliveryId)
        {
            try{
                var tblDelivery = await _context.TblDeliveries.FindAsync(deliveryId);
            if (tblDelivery == null)
            {
                return NotFound();
            }

            _context.TblDeliveries.Remove(tblDelivery);
            await _context.SaveChangesAsync();

            return Ok(
                    new
                    {
                        status = "success",
                        message = "Delete delivery by id successfull."
                    }
                );

            }
            catch (System.Exception ex)
           {
               Sentry.SentrySdk.CaptureException(ex);
               return BadRequest(new{status="Failed",message = ex.Message});
           }
            
        }

        private bool IsDeliveryExists(Guid deliveryId)
        {
            return _context.TblDeliveries.Any(e => e.DeliveryId == deliveryId);
        }
    }
}
