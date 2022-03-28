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
    public class RoleController : ControllerBase
    {
        private readonly GasB360Context _context;

        public RoleController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblRole>>> GetTblRoles()
        {
            return await _context.TblRoles.ToListAsync();
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblRole>> GetTblRole(Guid id)
        {
            var tblRole = await _context.TblRoles.FindAsync(id);

            if (tblRole == null)
            {
                return NotFound();
            }

            return tblRole;
        }

        // PUT: api/Role/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblRole(Guid id, TblRole tblRole)
        {
            if (id != tblRole.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(tblRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblRoleExists(id))
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

        // POST: api/Role
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblRole>> PostTblRole(TblRole tblRole)
        {
            _context.TblRoles.Add(tblRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblRole", new { id = tblRole.RoleId }, tblRole);
        }

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblRole(Guid id)
        {
            var tblRole = await _context.TblRoles.FindAsync(id);
            if (tblRole == null)
            {
                return NotFound();
            }

            _context.TblRoles.Remove(tblRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblRoleExists(Guid id)
        {
            return _context.TblRoles.Any(e => e.RoleId == id);
        }
    }
}
