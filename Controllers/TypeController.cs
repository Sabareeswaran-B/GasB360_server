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
              try
           {
              return await _context.TblTypes.ToListAsync();
           }
           catch (System.Exception ex)
           {
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // GET: api/Type/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblType>> GetTblType(Guid id)
        {
          
               try
           {
               var tblType = await _context.TblTypes.FindAsync(id);

            if (tblType == null)
            {
                return NotFound();
            }

            return tblType;
           }
           catch (System.Exception ex)
           {
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
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


            try
            {
            _context.Entry(tblType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                if (!TblTypeExists(id))
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

        // POST: api/Type
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblType>> PostTblType(TblType tblType)
        {
               try
           {
            _context.TblTypes.Add(tblType);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTblType", new { id = tblType.TypeId }, tblType);
           }
           catch (System.Exception ex)
           {   
            return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // DELETE: api/Type/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblType(Guid id)
        {
           
                 try
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
           catch (System.Exception ex)
           {   
            return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        private bool TblTypeExists(Guid id)
        {
            return _context.TblTypes.Any(e => e.TypeId == id);
        }
    }
}
