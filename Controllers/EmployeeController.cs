#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GasB360_server.Models;
using GasB360_server.Services;
using GasB360_server.Helpers;

namespace GasB360_server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly GasB360Context _context;

        private readonly IEmployeeService _EmployeeService;

        public EmployeeController(GasB360Context context, IEmployeeService EmployeeService)
        {
            _context = context;
            _EmployeeService = EmployeeService;
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
                Sentry.SentrySdk.CaptureException(ex);
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
                Sentry.SentrySdk.CaptureException(ex);
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
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(tblEmployee.Password);
                tblEmployee.Password = hashPassword;
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

        [HttpPost]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            try
            {
                var Employee = await _context.TblEmployees
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.EmployeeEmail == request.Email);
                if (Employee == null)
                    return BadRequest(new { status = "failed", message = "Email not found!" });
                var verify = BCrypt.Net.BCrypt.Verify(request.Password, Employee!.Password);
                if (!verify)
                    return BadRequest(new { status = "failed", message = "Incorrect password!" });
                var response = _EmployeeService.Authenticate(Employee);
                return Ok(
                    new { status = "success", message = "Login Successfull", data = response }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // POST: api/Employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblEmployee>> PostTblEmployee(TblEmployee tblEmployee)
        {
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(tblEmployee.Password);
            tblEmployee.Password = hashPassword;
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
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool TblEmployeeExists(Guid id)
        {
            return _context.TblEmployees.Any(e => e.EmployeeId == id);
        }
    }
}
