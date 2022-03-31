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
    public class FilledProductController : ControllerBase
    {
        private readonly GasB360Context _context;

        public FilledProductController(GasB360Context context)
        {
            _context = context;
        }

        // API To Get All Of The Filled Products
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

        // API To Get The Filled Product By Passing FilledProductId As Parameter
        [HttpGet("{filledProductId}")]
        public async Task<IActionResult> GetFilledProductById(Guid filledProductId)
        {
            try
            {
                var tblFilledProduct = await _context.TblFilledProducts.FindAsync(filledProductId);

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

        //API To Update The Filled Product Details By Passing FilledProductId As Parameter

        [HttpPut("{filledProductId}")]
        public async Task<IActionResult> UpdateFilledProduct(
            Guid filledProductId,
            TblFilledProduct tblFilledProduct
        )
        {
            if (filledProductId != tblFilledProduct.FilledProductId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(tblFilledProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var updatedFilledProduct = await _context.TblFilledProducts.FindAsync(
                    filledProductId
                );
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
                if (!isFilledProductExists(filledProductId))
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

        //API To Add Filled Product In The Stock By Passing FilledProductId/StockToAdd As Parameter
        [HttpPut("{filledProductId}/{stocksToAdd}")]
        public async Task<IActionResult> AddFilledProductStock(
            Guid filledProductId,
            int stocksToAdd
        )
        {
            try
            {
                var tblFilledProduct = await _context.TblFilledProducts.FindAsync(filledProductId);
                tblFilledProduct.FilledProductQuantity += stocksToAdd;
                _context.Entry(tblFilledProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Filled product stock added successfully.",
                        data = tblFilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!isFilledProductExists(filledProductId))
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
        //API To Remove Filled Product In The Stock By Passing FilledProductId/StockToRemove As Parameter
        [HttpPut("{filledProductId}/{stocksToRemove}")]
        public async Task<IActionResult> RemoveFilledProductStock(
            Guid filledProductId,
            int stocksToRemove
        )
        {
            try
            {
                var tblFilledProduct = await _context.TblFilledProducts.FindAsync(filledProductId);
                if (tblFilledProduct.FilledProductQuantity > stocksToRemove)
                {
                    tblFilledProduct.FilledProductQuantity -= stocksToRemove;
                }
                else
                {
                    tblFilledProduct.Active = "false";
                }
                _context.Entry(tblFilledProduct).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Filled product stock removed successfully",
                        data = tblFilledProduct
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!isFilledProductExists(filledProductId))
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

        //API To Add New Filled Product  By Passing tblFilledProduct Object As Parameter

        [HttpPost]
        public async Task<IActionResult> AddNewFilledProduct(TblFilledProduct tblFilledProduct)
        {
            try
            {
                _context.TblFilledProducts.Add(tblFilledProduct);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetFilledProductById",
                    new { filledProductId = tblFilledProduct.FilledProductId },
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

        //API To Delete The Filled Product By Passing FilledProductId As Parameter
        [HttpDelete("{filledProductId}")]
        public async Task<IActionResult> DeleteFilledProduct(Guid filledProductId)
        {
            try
            {
                var tblFilledProduct = await _context.TblFilledProducts.FindAsync(filledProductId);
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
        // Function To Check Whether The FilledProduct Already Exists or Not By Passing FilledProductId As Parameter

        private bool isFilledProductExists(Guid filledProductId)
        {
            return _context.TblFilledProducts.Any(e => e.FilledProductId == filledProductId);
        }
    }
}
