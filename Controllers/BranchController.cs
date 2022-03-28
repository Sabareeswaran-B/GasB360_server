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
    public class BranchController : ControllerBase
    {
        private readonly GasB360Context _context;

        public BranchController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Branch
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblBranch>>> GetTblBranches()
        {
            try
            {
                return await _context.TblBranches.ToListAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Branch/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblBranch>> GetTblBranch(Guid id)
        {
            try
            {
                var tblBranch = await _context.TblBranches.FindAsync(id);

                if (tblBranch == null)
                {
                    return NotFound();
                }

                return tblBranch;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Branch/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblBranch(Guid id, TblBranch tblBranch)
        {
            if (id != tblBranch.BranchId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(tblBranch).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                if (!TblBranchExists(id))
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

        // POST: api/Branch
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblBranch>> PostTblBranch(TblBranch tblBranch)
        {
            try
            {
                _context.TblBranches.Add(tblBranch);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTblBranch", new { id = tblBranch.BranchId }, tblBranch);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/Branch/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblBranch(Guid id)
        {
            try
            {
                var tblBranch = await _context.TblBranches.FindAsync(id);
                if (tblBranch == null)
                {
                    return NotFound();
                }

                _context.TblBranches.Remove(tblBranch);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool TblBranchExists(Guid id)
        {
            return _context.TblBranches.Any(e => e.BranchId == id);
        }
    }
}
