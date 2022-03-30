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
    public class ProductCategoryController : ControllerBase
    {
        private readonly GasB360Context _context;

        public ProductCategoryController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/ProductCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblProductCategory>>> GetTblProductCategories()
        {
            // return await _context.TblProductCategories.ToListAsync();
             try
           {
               return await _context.TblProductCategories.ToListAsync();
           }
           catch (System.Exception ex)
           {
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // GET: api/ProductCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblProductCategory>> GetTblProductCategory(Guid id)
        {
                try
           {
               var tblProductCategory = await _context.TblProductCategories.FindAsync(id);

            if (tblProductCategory == null)
            {
                return NotFound();
            }

            return tblProductCategory;
           }
           catch (System.Exception ex)
           {
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // PUT: api/ProductCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblProductCategory(Guid id, TblProductCategory tblProductCategory)
        {
            if (id != tblProductCategory.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(tblProductCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/ProductCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblProductCategory>> PostTblProductCategory(TblProductCategory tblProductCategory)
        {
          
            try
           {
                _context.TblProductCategories.Add(tblProductCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblProductCategory", new { id = tblProductCategory.ProductId }, tblProductCategory);
           }
           catch (System.Exception ex)
           {
               
               return BadRequest(new{status="Failed",message = ex.Message});
           }
        }

        // DELETE: api/ProductCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblProductCategory(Guid id)
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

            return NoContent();
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
