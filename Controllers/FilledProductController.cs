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
    public class FilledProductController : ControllerBase
    {
        private readonly GasB360Context _context;

        public FilledProductController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/FilledProduct
        [HttpGet]
        public async Task<IActionResult> GetAllFilledProducts()
        {
            var filledproducts = await _context.TblFilledProducts.ToListAsync();
            return Ok(
                new
                {
                    status = "success",
                    message = "Get all filled products successful.",
                    data = filledproducts
                }
            );
        }

        // GET: api/FilledProduct/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilledProductById(Guid id)
        {
            try
            {
                var tblFilledProduct = await _context.TblFilledProducts.FindAsync(id);

                if (tblFilledProduct == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get filled product by id successful.",
                        data = tblFilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/FilledProduct/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFilledProduct(
            Guid id,
            TblFilledProduct tblFilledProduct
        )
        {
            if (id != tblFilledProduct.FilledProductId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(tblFilledProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var updatedFilledProduct = await _context.TblFilledProducts.FindAsync(id);
                return Ok(
                    new
                    {
                        status = "success",
                        message = "update filled product by id successful.",
                        data = updatedFilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!TblFilledProductExists(id))
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

        // POST: api/FilledProduct
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddNewFilledProduct(
            TblFilledProduct tblFilledProduct
        )
        {
            try
            {
                _context.TblFilledProducts.Add(tblFilledProduct);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetFilledProductById",
                    new { id = tblFilledProduct.FilledProductId },
                    new
                    {
                        status = "success",
                        message = "Add new filled product successful.",
                        data = tblFilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/FilledProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilledProduct(Guid id)
        {
            try
            {
                var tblFilledProduct = await _context.TblFilledProducts.FindAsync(id);
                if (tblFilledProduct == null)
                {
                    return NotFound();
                }

                _context.TblFilledProducts.Remove(tblFilledProduct);
                await _context.SaveChangesAsync();

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Delete filled product by id successful.",
                        data = tblFilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool TblFilledProductExists(Guid id)
        {
            return _context.TblFilledProducts.Any(e => e.FilledProductId == id);
        }
    }
}
