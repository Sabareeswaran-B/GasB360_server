#nullable disable
using System.Security.AccessControl;
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
    public class ProductCategoryController : ControllerBase
    {
        private readonly GasB360Context _context;

        public ProductCategoryController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/ProductCategory
        [HttpGet]
        public async Task<IActionResult> GetAllProductCategories()
        {
            // return await _context.TblProductCategories.ToListAsync();
            try
            {
                var productCategory = await _context.TblProductCategories.ToListAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all product category successful.",
                        data = productCategory
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/ProductCategory/5
        [HttpGet("{productCategoryId}")]
        public async Task<IActionResult> GetProductCategoryById(Guid productCategoryId)
        {
            try
            {
                var productCategory = await _context.TblProductCategories.FindAsync(
                    productCategoryId
                );

                if (productCategory == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get product category by id successful.",
                        data = productCategory
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/ProductCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{productCategoryId}")]
        public async Task<IActionResult> UpdateProductCategory(
            Guid productCategoryId,
            TblProductCategory tblProductCategory
        )
        {
            if (productCategoryId != tblProductCategory.ProductId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(tblProductCategory).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var productCategory = await _context.TblProductCategories.FindAsync(
                    productCategoryId
                );
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Update product category successful.",
                        data = productCategory
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsProductCategoryExists(productCategoryId))
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

        // POST: api/ProductCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddProductCategory(
            TblProductCategory tblProductCategory
        )
        {
            try
            {
                _context.TblProductCategories.Add(tblProductCategory);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetProductCategoryById",
                    new { productCategoryId = tblProductCategory.ProductId },
                    new
                    {
                        status = "success",
                        message = "Add new product category successful.",
                        data = tblProductCategory
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/ProductCategory/5
        [HttpDelete("{productCategoryId}")]
        public async Task<IActionResult> DeleteProductCategory(Guid productCategoryId)
        {
            try
            {
                var tblProductCategory = await _context.TblProductCategories.FindAsync(
                    productCategoryId
                );
                if (tblProductCategory == null)
                {
                    return NotFound();
                }

                _context.TblProductCategories.Remove(tblProductCategory);
                await _context.SaveChangesAsync();

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Delete product category by id successful."
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool IsProductCategoryExists(Guid productCategoryId)
        {
            return _context.TblProductCategories.Any(e => e.ProductId == productCategoryId);
        }
    }
}
