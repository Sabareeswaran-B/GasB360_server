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
    public class RoleController : ControllerBase
    {
        private readonly GasB360Context _context;

        public RoleController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var role = await _context.TblRoles.ToListAsync();
                return Ok(
                    new { status = "success", message = "Get all role successful.", data = role }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "Failed", message = ex.Message });
            }
        }

        // GET: api/Role/5
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(Guid roleId)
        {
            try
            {
                var tblRole = await _context.TblRoles.FindAsync(roleId);

                if (tblRole == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get role by id successful.",
                        data = tblRole
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "Failed", message = ex.Message });
            }
        }

        // PUT: api/Role/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(Guid roleId, TblRole tblRole)
        {
            if (roleId != tblRole.RoleId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(tblRole).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var role = await _context.TblRoles.FindAsync(roleId);
                return Ok(
                    new { status = "success", message = "Update role successful.", data = role }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsRoleExists(roleId))
                {
                    return NotFound();
                }
                else
                {
                    Sentry.SentrySdk.CaptureException(ex);
                    return BadRequest(new { status = "Failed", message = ex.Message });
                }
            }


        }

        // POST: api/Role
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddRole(TblRole tblRole)
        {
            try
            {
                _context.TblRoles.Add(tblRole);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetRoleById",
                    new { roleId = tblRole.RoleId },
                    new { status = "success", message = "Add new role successful.", data = tblRole }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "Failed", message = ex.Message });
            }
        }

        // DELETE: api/Role/5
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            try
            {
                var tblRole = await _context.TblRoles.FindAsync(roleId);
                if (tblRole == null)
                {
                    return NotFound();
                }

                _context.TblRoles.Remove(tblRole);
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Delete role by id successful." });
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "Failed", message = ex.Message });
            }
        }

        private bool IsRoleExists(Guid roleId)
        {
            return _context.TblRoles.Any(e => e.RoleId == roleId);
        }
    }
}
