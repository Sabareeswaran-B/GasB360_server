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
    public class TypeController : ControllerBase
    {
        private readonly GasB360Context _context;

        public TypeController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblType>>> GetTblTypes()
        {
            return await _context.TblTypes.ToListAsync();
        }

        // GET: api/Type/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblType>> GetTblType(Guid id)
        {
            var tblType = await _context.TblTypes.FindAsync(id);

            if (tblType == null)
            {
                return NotFound();
            }

            return tblType;
        }

        // PUT: api/Type/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblType(Guid id, TblType tblType)
        {
            if (id != tblType.TypeId)
            {
                return BadRequest();
            }

            _context.Entry(tblType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblTypeExists(id))
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

        // POST: api/Type
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblType>> PostTblType(TblType tblType)
        {
            _context.TblTypes.Add(tblType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblType", new { id = tblType.TypeId }, tblType);
        }

        // DELETE: api/Type/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblType(Guid id)
        {
            var tblType = await _context.TblTypes.FindAsync(id);
            if (tblType == null)
            {
                return NotFound();
            }

            _context.TblTypes.Remove(tblType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblTypeExists(Guid id)
        {
            return _context.TblTypes.Any(e => e.TypeId == id);
        }
    }
}
