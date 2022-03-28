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
    public class UnfilledProductController : ControllerBase
    {
        private readonly GasB360Context _context;

        public UnfilledProductController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/UnfilledProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblUnfilledProduct>>> GetTblUnfilledProducts()
        {
            return await _context.TblUnfilledProducts.ToListAsync();
        }

        // GET: api/UnfilledProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblUnfilledProduct>> GetTblUnfilledProduct(Guid id)
        {
            var tblUnfilledProduct = await _context.TblUnfilledProducts.FindAsync(id);

            if (tblUnfilledProduct == null)
            {
                return NotFound();
            }

            return tblUnfilledProduct;
        }

        // PUT: api/UnfilledProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblUnfilledProduct(Guid id, TblUnfilledProduct tblUnfilledProduct)
        {
            if (id != tblUnfilledProduct.UnfilledProductId)
            {
                return BadRequest();
            }

            _context.Entry(tblUnfilledProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblUnfilledProductExists(id))
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

        // POST: api/UnfilledProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblUnfilledProduct>> PostTblUnfilledProduct(TblUnfilledProduct tblUnfilledProduct)
        {
            _context.TblUnfilledProducts.Add(tblUnfilledProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblUnfilledProduct", new { id = tblUnfilledProduct.UnfilledProductId }, tblUnfilledProduct);
        }

        // DELETE: api/UnfilledProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblUnfilledProduct(Guid id)
        {
            var tblUnfilledProduct = await _context.TblUnfilledProducts.FindAsync(id);
            if (tblUnfilledProduct == null)
            {
                return NotFound();
            }

            _context.TblUnfilledProducts.Remove(tblUnfilledProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblUnfilledProductExists(Guid id)
        {
            return _context.TblUnfilledProducts.Any(e => e.UnfilledProductId == id);
        }
    }
}
