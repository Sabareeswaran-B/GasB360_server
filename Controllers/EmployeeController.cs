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
    public class EmployeeController : ControllerBase
    {
        private readonly GasB360Context _context;

        public EmployeeController(GasB360Context context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblEmployee>>> GetTblEmployees()
        {
            try
            {
                return await _context.TblEmployees.ToListAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblEmployee>> GetTblEmployee(Guid id)
        {
            try
            {
                var tblEmployee = await _context.TblEmployees.FindAsync(id);

                if (tblEmployee == null)
                {
                    return NotFound();
                }

                return tblEmployee;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblEmployee(Guid id, TblEmployee tblEmployee)
        {
            if (id != tblEmployee.EmployeeId)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(tblEmployee).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                if (!TblEmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }

            return NoContent();
        }

        // POST: api/Employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblEmployee>> PostTblEmployee(TblEmployee tblEmployee)
        {
            _context.TblEmployees.Add(tblEmployee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetTblEmployee",
                new { id = tblEmployee.EmployeeId },
                tblEmployee
            );
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblEmployee(Guid id)
        {
            try
            {
                var tblEmployee = await _context.TblEmployees.FindAsync(id);
                if (tblEmployee == null)
                {
                    return NotFound();
                }

                _context.TblEmployees.Remove(tblEmployee);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool TblEmployeeExists(Guid id)
        {
            return _context.TblEmployees.Any(e => e.EmployeeId == id);
        }
    }
}
