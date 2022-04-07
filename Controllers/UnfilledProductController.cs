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
    [Authorize("admin", "delivery")]
    public class UnfilledProductController : ControllerBase
    {
        private readonly GasB360Context _context;

        public UnfilledProductController(GasB360Context context)
        {
            _context = context;
        }

        // API To Get All Of The UnfilledProducts
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUnfilledProducts()
        {
            try
            {
                var unfilledproducts = await _context.TblUnfilledProducts
                    .Include(x => x.Branch)
                    .Include(x => x.ProductCategory)
                    .Include(x => x.ProductCategory.Type)
                    .ToListAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all unfilled products successful.",
                        data = unfilledproducts
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // API To Get The UnFilledProduct By Passing UnFilledProductId As Parameter
        [HttpGet("{unfilledProductId}")]
        public async Task<IActionResult> GetUnfilledProductById(Guid unfilledProductId)
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

        //API To Update The UnFilledProduct Details By Passing UnFilledProductId As Parameter

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

        //API To Add UnFilled Product In The Stock By Passing UnFilledProductId/StockToAdd As Parameter

        [HttpPut("{unFilledProductId}/{stocksToAdd}")]
        public async Task<IActionResult> AddUnFilledProductStock(
            Guid unFilledProductId,
            int stocksToAdd
        )
        {
            try
            {
                var unFilledProduct = await _context.TblUnfilledProducts.FindAsync(
                    unFilledProductId
                );
                unFilledProduct.UnfilledProductQuantity += stocksToAdd;
                _context.Entry(unFilledProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Unfilled product stock added successfully",
                        data = unFilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsUnfilledProductExists(unFilledProductId))
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

        //API To Remove UnFilled Product In The Stock By Passing UnFilledProductId/StockToRemove As Parameter

        [HttpPut("{unFilledProductId}/{stocksToRemove}")]
        public async Task<IActionResult> RemoveUnFilledProductStock(
            Guid unFilledProductId,
            int stocksToRemove
        )
        {
            try
            {
                var unFilledProduct = await _context.TblUnfilledProducts.FindAsync(
                    unFilledProductId
                );
                if (unFilledProduct.UnfilledProductQuantity > stocksToRemove)
                {
                    unFilledProduct.UnfilledProductQuantity -= stocksToRemove;
                }
                else
                {
                    unFilledProduct.Active = "false";
                }
                _context.Entry(unFilledProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Unfilled product stock removed successfully",
                        data = unFilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsUnfilledProductExists(unFilledProductId))
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

        //API To Add New UnFilled Product  By Passing tblUnFilledProduct Object As Parameter

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

        //API To Delete The UnFilled Product By Passing UnFilledProductId As Parameter
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

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Delete unfilled product by id successful."
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // Function To Check Whether The UnFilledProduct Already Exists or Not By Passing UnFilledProductId As Parameter

        private bool IsUnfilledProductExists(Guid unfilledProductId)
        {
            return _context.TblUnfilledProducts.Any(e => e.UnfilledProductId == unfilledProductId);
        }
    }
}
