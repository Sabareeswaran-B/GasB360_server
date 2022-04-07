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

        //API To Get All Of The Deliveries
        [HttpGet]
        public async Task<IActionResult> GetAllDeliveries()
        {
            try
            {
                var deliveries = await _context.TblDeliveries.ToListAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all Deliverys successful.",
                        data = deliveries
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }



        }

        //API To Get Delivery By Passing DeliveryId As Parameter
        [HttpGet("{deliveryId}")]
        public async Task<IActionResult> GetDeliveryById(Guid deliveryId)
        {
            try
            {
                var delivery = await _context.TblDeliveries.FindAsync(deliveryId);

                if (delivery == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get order by id successful.",
                        data = delivery
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Get Deliveries By Employee By Passing EmployeeId As Parameter
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetDeliveriesByEmployeeId(Guid employeeId)
        {
            try
            {
                var deliveries = await _context.TblDeliveries
                    .Where(a => a.Active == "true")
                    .Include(a => a.Order)
                    .Where(a => a.Order.EmployeeId == employeeId)
                    .Include(x => x.Order.Address)
                    .Include(x => x.Order.Customer)
                    .Include(x => x.Order.Employee)
                    .Include(x => x.Order.FilledProduct)
                    .Include(x =>x.Order.FilledProduct.ProductCategory)

                    .ToListAsync();

                if (deliveries == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get Deliveries by Employee id successful.",
                        data = deliveries
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Update The Delivery Details By Passing DeliveryId As Parameter
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
                var Delivery = await _context.TblDeliveries.FindAsync(deliveryId);
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Update Delivery successful.",
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
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }

        //API To Add New Delivery By Passing tblDelivery Object As Parameter
        [HttpPost]
        public async Task<IActionResult> AddNewDelivery(TblDelivery tblDelivery)
        {
            try
            {
            
                _context.TblDeliveries.Add(tblDelivery);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetDeliveryById",
                    new { deliveryId = tblDelivery.DeliveryId },
                    new
                    {
                        status = "success",
                        message = "Add new order successful.",
                        data = tblDelivery
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }


        }

        //API To Delete The Delivery By Passing DeliveryId As Parameter
        [HttpDelete("{deliveryId}")]
        public async Task<IActionResult> DeleteDelivery(Guid deliveryId)
        {
            try
            {
                var tblDelivery = await _context.TblDeliveries.FindAsync(deliveryId);
                if (tblDelivery == null)
                {
                    return NotFound();
                }

                _context.TblDeliveries.Remove(tblDelivery);
                await _context.SaveChangesAsync();

                return Ok(
                    new { status = "success", message = "Delete delivery by id successful." }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }
        //Function To Check Whether Delivery Already Exists Or Not By Passing DeliveryId As Parameter
        private bool IsDeliveryExists(Guid deliveryId)
        {
            return _context.TblDeliveries.Any(e => e.DeliveryId == deliveryId);
        }
    }
}
