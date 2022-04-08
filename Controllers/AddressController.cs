#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GasB360_server.Models;
using GasB360_server.Helpers;

namespace GasB360_server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize("customer")]
    public class AddressController : ControllerBase
    {
        private readonly GasB360Context _context;

        public AddressController(GasB360Context context)
        {
            _context = context;
        }

        // API To Get All Of The Customers Addresses
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCustomersAddresses()
        {
            try
            {
                var address = await _context.TblAddresses
                    .Where(x => x.Active == "true")
                    .ToListAsync();
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

        // API To Get The Customer Address By Passing AddressId As Parameter
        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetCustomerAddressById(Guid addressId)
        {
            try
            {
                var address = await _context.TblAddresses
                    .Where(x => x.Active == "true")
                    .Where(x => x.AddressId == addressId)
                    .FirstOrDefaultAsync();

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

        // API To Get The Customer Address By Passing CustomerId As Parameter
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

        //API To Update The Customer Address Details By Passing AddressId As Parameter
        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateCustomerAddress(
            Guid addressId,
            TblAddress tblAddress
        )
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

        //API To Add New Customer Address By Passing tblAddress Object As Parameter
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

        //API To Delete The Customer Address By Passing AddressId As Parameter
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteCustomerAddress(Guid addressId)
        {
            try
            {
                var tblAddress = await _context.TblAddresses
                    .Where(x => x.Active == "true")
                    .Where(x => x.AddressId == addressId)
                    .FirstOrDefaultAsync();
                if (tblAddress == null)
                {
                    return NotFound();
                }

                tblAddress.Active = "false";
                _context.Entry(tblAddress).State = EntityState.Modified;
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

        // Function To Check Whether The Address Already Exists or Not By Passing AddressId As Parameter
        private bool IsAddressExists(Guid addressId)
        {
            return _context.TblAddresses.Any(e => e.AddressId == addressId);
        }
    }
}
