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
    public class AddressController : ControllerBase
    {
        private readonly GasB360Context _context;

        public AddressController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Address
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblAddress>>> GetTblAddresses()
        {
            try
            {
                return await _context.TblAddresses.ToListAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Address/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblAddress>> GetTblAddress(Guid id)
        {
            try
            {
                var tblAddress = await _context.TblAddresses.FindAsync(id);

                if (tblAddress == null)
                {
                    return NotFound();
                }

                return tblAddress;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Address/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblAddress(Guid id, TblAddress tblAddress)
        {
            if (id != tblAddress.AddressId)
            {
                return BadRequest();
            }

            _context.Entry(tblAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                if (!TblAddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    Console.WriteLine(ex);
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }

            return NoContent();
        }

        // POST: api/Address
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblAddress>> PostTblAddress(TblAddress tblAddress)
        {
            try
            {
                _context.TblAddresses.Add(tblAddress);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetTblAddress",
                    new { id = tblAddress.AddressId },
                    tblAddress
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/Address/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblAddress(Guid id)
        {
            try
            {
                var tblAddress = await _context.TblAddresses.FindAsync(id);
                if (tblAddress == null)
                {
                    return NotFound();
                }

                _context.TblAddresses.Remove(tblAddress);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool TblAddressExists(Guid id)
        {
            return _context.TblAddresses.Any(e => e.AddressId == id);
        }
    }
}
