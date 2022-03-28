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
    public class CustomerController : ControllerBase
    {
        private readonly GasB360Context _context;

        public CustomerController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblCustomer>>> GetTblCustomers()
        {
            return await _context.TblCustomers.ToListAsync();
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblCustomer>> GetTblCustomer(Guid id)
        {
            var tblCustomer = await _context.TblCustomers.FindAsync(id);

            if (tblCustomer == null)
            {
                return NotFound();
            }

            return tblCustomer;
        }

        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblCustomer(Guid id, TblCustomer tblCustomer)
        {
            if (id != tblCustomer.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(tblCustomer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblCustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblCustomer>> PostTblCustomer(TblCustomer tblCustomer)
        {
            _context.TblCustomers.Add(tblCustomer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblCustomer", new { id = tblCustomer.CustomerId }, tblCustomer);
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblCustomer(Guid id)
        {
            var tblCustomer = await _context.TblCustomers.FindAsync(id);
            if (tblCustomer == null)
            {
                return NotFound();
            }

            _context.TblCustomers.Remove(tblCustomer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblCustomerExists(Guid id)
        {
            return _context.TblCustomers.Any(e => e.CustomerId == id);
        }
    }
}
