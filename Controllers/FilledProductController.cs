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
    public class FilledProductController : ControllerBase
    {
        private readonly GasB360Context _context;

        public FilledProductController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/FilledProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblFilledProduct>>> GetTblFilledProducts()
        {
            return await _context.TblFilledProducts.ToListAsync();
        }

        // GET: api/FilledProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblFilledProduct>> GetTblFilledProduct(Guid id)
        {
            var tblFilledProduct = await _context.TblFilledProducts.FindAsync(id);

            if (tblFilledProduct == null)
            {
                return NotFound();
            }

            return tblFilledProduct;
        }

        // PUT: api/FilledProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblFilledProduct(Guid id, TblFilledProduct tblFilledProduct)
        {
            if (id != tblFilledProduct.FilledProductId)
            {
                return BadRequest();
            }

            _context.Entry(tblFilledProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblFilledProductExists(id))
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

        // POST: api/FilledProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblFilledProduct>> PostTblFilledProduct(TblFilledProduct tblFilledProduct)
        {
            _context.TblFilledProducts.Add(tblFilledProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblFilledProduct", new { id = tblFilledProduct.FilledProductId }, tblFilledProduct);
        }

        // DELETE: api/FilledProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblFilledProduct(Guid id)
        {
            var tblFilledProduct = await _context.TblFilledProducts.FindAsync(id);
            if (tblFilledProduct == null)
            {
                return NotFound();
            }

            _context.TblFilledProducts.Remove(tblFilledProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblFilledProductExists(Guid id)
        {
            return _context.TblFilledProducts.Any(e => e.FilledProductId == id);
        }
    }
}
