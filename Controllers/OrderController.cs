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
        public async Task<ActionResult<IEnumerable<TblOrder>>> GetTblOrders()
        {
            // return await _context.TblOrders.ToListAsync();
             try
           {
               return await _context.TblOrders.ToListAsync();
           }
           catch (System.Exception ex)
           {
               Console.WriteLine(ex);
               return BadRequest(new{status="Order GET request Failed",message = ex.Message});
           }
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblOrder>> GetTblOrder(Guid id)
        {
           
              try
           {
                var tblOrder = await _context.TblOrders.FindAsync(id);

            if (tblOrder == null)
            {
                return NotFound();
            }

            return tblOrder;
           }
           catch (System.Exception ex)
           {
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblOrder(Guid id, TblOrder tblOrder)
        {
            if (id != tblOrder.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(tblOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                if (!TblOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(new{status="Failed",message = ex.Message});
                }
            }

            return NoContent();
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblOrder>> PostTblOrder(TblOrder tblOrder)
        {
              try
           {
                _context.TblOrders.Add(tblOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblOrder", new { id = tblOrder.OrderId }, tblOrder);
           }
           catch (System.Exception ex)
           {
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblOrder(Guid id)
        {
           
            try
           {
               var tblOrder = await _context.TblOrders.FindAsync(id);
            if (tblOrder == null)
            {
                return NotFound();
            }

            _context.TblOrders.Remove(tblOrder);
            await _context.SaveChangesAsync();

            return NoContent();
           }
           catch (System.Exception ex)
           {
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        private bool TblOrderExists(Guid id)
        {
            return _context.TblOrders.Any(e => e.OrderId == id);
        }
    }
}
