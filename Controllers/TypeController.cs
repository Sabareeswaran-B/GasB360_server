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

        // API To Get All Of The Types
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

        // API To Get The Type By Passing TypeId As Parameter
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

        //API To Update The Type Details By Passing TypeId As Parameter

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

        //API To Add New Type By Passing tblType Object As Parameter

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

        //API To Delete The Type By Passing TypeId As Parameter
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
        // Function To Check Whether The Type Already Exists or Not By Passing TypeId As Parameter

        private bool IsTypeExists(Guid typeId)
        {
            return _context.TblTypes.Any(e => e.TypeId == typeId);
        }
    }
}
