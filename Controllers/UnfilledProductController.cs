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
    public class UnfilledProductController : ControllerBase
    {
        private readonly GasB360Context _context;

        public UnfilledProductController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/UnfilledProduct
        [HttpGet]
        public async Task<IActionResult> GetAllUnfilledProducts()
        {
            try
            {
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all unfilled products successful.",
                        data = await _context.TblUnfilledProducts.ToListAsync()
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/UnfilledProduct/5
        [HttpGet("{unfilledProductId}")]
        public async Task<IActionResult> GetUnfilledProductById(
            Guid unfilledProductId
        )
        {
            try
            {
                var tblUnfilledProduct = await _context.TblUnfilledProducts.FindAsync(
                    unfilledProductId
                );

                if (tblUnfilledProduct == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get unfilled product by id successful.",
                        data = tblUnfilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/UnfilledProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{unfilledProductId}")]
        public async Task<IActionResult> UpdateUnfilledProduct(
            Guid unfilledProductId,
            TblUnfilledProduct tblUnfilledProduct
        )
        {
            if (unfilledProductId != tblUnfilledProduct.UnfilledProductId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(tblUnfilledProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var updatedUnfilledProduct = await _context.TblUnfilledProducts.FindAsync(
                    unfilledProductId
                );
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Update unfilled product successful.",
                        data = updatedUnfilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsUnfilledProductExists(unfilledProductId))
                {
                    return NotFound();
                }
                else
                {
                    Sentry.SentrySdk.CaptureException(ex);
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }

        // POST: api/UnfilledProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddNewUnfilledProduct(
            TblUnfilledProduct tblUnfilledProduct
        )
        {
            try
            {
                _context.TblUnfilledProducts.Add(tblUnfilledProduct);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetUnfilledProductById",
                    new { unFilledProductId = tblUnfilledProduct.UnfilledProductId },
                    new
                    {
                        status = "success",
                        message = "Add new unfilled product successful.",
                        data = tblUnfilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/UnfilledProduct/5
        [HttpDelete("{unFilledProductId}")]
        public async Task<IActionResult> DeleteUnfilledProduct(Guid unFilledProductId)
        {
            try
            {
                var tblUnfilledProduct = await _context.TblUnfilledProducts.FindAsync(
                    unFilledProductId
                );
                if (tblUnfilledProduct == null)
                {
                    return NotFound();
                }

                _context.TblUnfilledProducts.Remove(tblUnfilledProduct);
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Delete unfilled product by id successful." });
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool IsUnfilledProductExists(Guid id)
        {
            return _context.TblUnfilledProducts.Any(e => e.UnfilledProductId == id);
        }
    }
}
