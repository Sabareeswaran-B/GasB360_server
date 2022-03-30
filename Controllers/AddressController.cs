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
    public class AddressController : ControllerBase
    {
        private readonly GasB360Context _context;

        public AddressController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Address
        [HttpGet]
        public async Task<IActionResult> GetAllCustomerAddresses()
        {
            try
            {
                var address = await _context.TblAddresses.ToListAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all customer addresses successful.",
                        data = address
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Address/5
        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetCustomerAddressById(Guid addressId)
        {
            try
            {
                var address = await _context.TblAddresses.FindAsync(addressId);

                if (address == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get customer address by id successful.",
                        data = address
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Address/5
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetAddressByCustomerId(Guid customerId)
        {
            try
            {
                var address = await _context.TblAddresses
                    .Where(a => a.Active == "true")
                    .Where(a => a.CustomerId == customerId)
                    .ToListAsync();

                if (address == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get addresses by customer id successful.",
                        data = address
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Address/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAddress(Guid addressId, TblAddress tblAddress)
        {
            if (addressId != tblAddress.AddressId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(tblAddress).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var address = await _context.TblAddresses.FindAsync(addressId);
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Update customer address successful.",
                        data = address
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsAddressExists(addressId))
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

        // POST: api/Address
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddNewCustomerAddress(TblAddress tblAddress)
        {
            try
            {
                _context.TblAddresses.Add(tblAddress);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetAddressByCustomerId",
                    new { customerId = tblAddress.AddressId },
                    new
                    {
                        status = "success",
                        message = "Add new customer address successful.",
                        data = tblAddress
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/Address/5
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteCustomerAddress(Guid addressId)
        {
            try
            {
                var tblAddress = await _context.TblAddresses.FindAsync(addressId);
                if (tblAddress == null)
                {
                    return NotFound();
                }

                _context.TblAddresses.Remove(tblAddress);
                await _context.SaveChangesAsync();

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Delete customer address by id successful."
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool IsAddressExists(Guid addressId)
        {
            return _context.TblAddresses.Any(e => e.AddressId == addressId);
        }
    }
}
