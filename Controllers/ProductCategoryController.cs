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
                var productcategory  =  await _context.TblProductCategories.ToListAsync();
                 return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all product category successful.",
                        data = productcategory
                    }
                );
           }
           catch (System.Exception ex)
           {
               
               return BadRequest(new{status=" Failed",message = ex.Message});
           }
        }

        // GET: api/ProductCategory/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCategoryById(Guid id)
        {
                try
           {
               var productcategory = await _context.TblProductCategories.FindAsync(id);

            if (productcategory == null)
            {
                return NotFound();
            }

            return Ok(
                    new
                    {
                        status = "success",
                        message = "Get product category by id successful.",
                        data =productcategory
                    }
                );
           }
           catch (System.Exception ex)
           {
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // PUT: api/ProductCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductCategory(Guid id, TblProductCategory tblProductCategory)
        {
            if (id != tblProductCategory.ProductId)
            {
                return BadRequest();
            }


            try
            {
            _context.Entry(tblProductCategory).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var productcategory =await _context.TblProductCategories.FindAsync(id);
                 return Ok(
                    new
                    {
                        status = "success",
                        message = "Update product category successful.",
                        data = productcategory
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!TblProductCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                   return BadRequest(new{status="Failed",message = ex.Message});

                }
            }

        
        }

        // POST: api/ProductCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblProductCategory>> AddProductCategory(TblProductCategory tblProductCategory)
        {
          
            try
           {
             _context.TblProductCategories.Add(tblProductCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductCategoryById",
             new { id = tblProductCategory.ProductId },
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
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // DELETE: api/ProductCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(Guid id)
        {
           

              try
           {
                var tblProductCategory = await _context.TblProductCategories.FindAsync(id);
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
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        private bool TblProductCategoryExists(Guid id)
        {
            return _context.TblProductCategories.Any(e => e.ProductId == id);
        }
    }
}
