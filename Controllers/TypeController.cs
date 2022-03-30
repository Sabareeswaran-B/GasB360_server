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
    public class TypeController : ControllerBase
    {
        private readonly GasB360Context _context;

        public TypeController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Type
        [HttpGet]
        public async Task<IActionResult> GetAllTypes()
        {
            try
            {
                var type = await _context.TblTypes.ToListAsync();
                return Ok(
                    new { status = "success", message = "Get all type successful.", data = type }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "Failed", message = ex.Message });
            }
        }

        // GET: api/Type/5
        [HttpGet("{typeId}")]
        public async Task<IActionResult> GetTypeById(Guid typeId)
        {
            try
            {
                var tblType = await _context.TblTypes.FindAsync(typeId);

                if (tblType == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get type by id successful.",
                        data = tblType
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "Failed", message = ex.Message });
            }
        }

        // PUT: api/Type/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{typeId}")]
        public async Task<IActionResult> updateType(Guid typeId, TblType tblType)
        {
            if (typeId != tblType.TypeId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(tblType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var type = await _context.TblTypes.FindAsync(typeId);
                return Ok(
                    new { status = "success", message = "Update type successful.", data = type }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsTypeExists(typeId))
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

        // POST: api/Type
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddType(TblType tblType)
        {
            try
            {
                _context.TblTypes.Add(tblType);
                await _context.SaveChangesAsync();
                return CreatedAtAction(
                    "GetTypeById",
                    new { typeId = tblType.TypeId },
                    new { status = "success", message = "Add new type successful.", data = tblType }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "Failed", message = ex.Message });
            }
        }

        // DELETE: api/Type/5
        [HttpDelete("{typeId}")]
        public async Task<IActionResult> DeleteType(Guid typeId)
        {
            try
            {
                var tblType = await _context.TblTypes.FindAsync(typeId);
                if (tblType == null)
                {
                    return NotFound();
                }

                _context.TblTypes.Remove(tblType);
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Delete type by id successful." });
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "Failed", message = ex.Message });
            }
        }

        private bool IsTypeExists(Guid typeId)
        {
            return _context.TblTypes.Any(e => e.TypeId == typeId);
        }
    }
}
