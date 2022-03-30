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
        public async Task<ActionResult<IEnumerable<TblDelivery>>> GetTblDeliveries()
        {
            return await _context.TblDeliveries.ToListAsync();
        }

        // GET: api/Delivery/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblDelivery>> GetTblDelivery(Guid id)
        {
            var tblDelivery = await _context.TblDeliveries.FindAsync(id);

            if (tblDelivery == null)
            {
                return NotFound();
            }

            return tblDelivery;
        }

        // PUT: api/Delivery/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblDelivery(Guid id, TblDelivery tblDelivery)
        {
            if (id != tblDelivery.DeliveryId)
            {
                return BadRequest();
            }

            _context.Entry(tblDelivery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblDeliveryExists(id))
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

        // POST: api/Delivery
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblDelivery>> PostTblDelivery(TblDelivery tblDelivery)
        {
            _context.TblDeliveries.Add(tblDelivery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblDelivery", new { id = tblDelivery.DeliveryId }, tblDelivery);
        }

        // DELETE: api/Delivery/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblDelivery(Guid id)
        {
            var tblDelivery = await _context.TblDeliveries.FindAsync(id);
            if (tblDelivery == null)
            {
                return NotFound();
            }

            _context.TblDeliveries.Remove(tblDelivery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblDeliveryExists(Guid id)
        {
            return _context.TblDeliveries.Any(e => e.DeliveryId == id);
        }
    }
}
