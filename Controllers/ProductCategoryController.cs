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
using GasB360_server.Helpers;

namespace GasB360_server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductCategoryController : ControllerBase
    {
        private readonly GasB360Context _context;

        public ProductCategoryController(GasB360Context context)
        {
            _context = context;
        }

        // API To Get All Of The Product Categories
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductCategories()
        {
            try
            {
                 var productCategory = from ai in _context.TblProductCategories select new {
                    ProductId = ai.ProductId,
                    ProductName = ai.ProductName ,
                    ProductWeight = ai.ProductWeight,
                    ProductPrice = ai.ProductPrice,
                    TypeId = ai.TypeId,
                    Active = ai.Active,
                    type = ai.Type,
                    
                };
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

        // API To Get The Product Category By Passing ProductCategoryId As Parameter
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

        // API To Get The Product Category and the product details By Passing ProductCategoryId As Parameter
        [HttpGet("{productCategoryId}")]
        public async Task<IActionResult> GetProductDetailsById(Guid productCategoryId)
        {
            try
            {
                var productCategory = await _context.TblProductCategories
                    .Where(x => x.ProductId == productCategoryId)
                    .Include(x => x.Type)
                    .Include(x => x.TblFilledProducts)
                    .FirstOrDefaultAsync();
                
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

        //API To Update The Product Category Details By Passing ProductCategoryId As Parameter

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

        //API To Add New Product Category By Passing tblProductCategory Object As Parameter

        [HttpPost]
        public async Task<IActionResult> AddProductCategory(TblProductCategory tblProductCategory)
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

        //API To Delete The Product Category By Passing ProductCategoryId As Parameter
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

        // Function To Check Whether The Product Category Already Exists or Not By Passing ProductCategoryId As Parameter

        private bool IsProductCategoryExists(Guid productCategoryId)
        {
            return _context.TblProductCategories.Any(e => e.ProductId == productCategoryId);
        }
    }
}
