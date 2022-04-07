#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GasB360_server.Models;
using GasB360_server.Helpers;

namespace GasB360_server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize("admin")]
    public class RoleController : ControllerBase
    {
        private readonly GasB360Context _context;

        public RoleController(GasB360Context context)
        {
            _context = context;
        }

        // API To Get All Of The Roles
        [HttpGet]
        [AllowAnonymous]
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

        // API To Get The Role By Passing RoleId As Parameter
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

        //API To Update The Role Details By Passing RoleId As Parameter

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

        //API To Add New Role By Passing tblRole Object As Parameter

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

        //API To Delete The Role By Passing RoleId As Parameter
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
        // Function To Check Whether The Role Already Exists or Not By Passing RoleId As Parameter

        private bool IsRoleExists(Guid roleId)
        {
            return _context.TblRoles.Any(e => e.RoleId == roleId);
        }
    }
}
