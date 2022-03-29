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
    public class CustomerController : ControllerBase
    {
        private readonly GasB360Context _context;
        private readonly ICustomerService _customerService;

        public CustomerController(GasB360Context context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }

        // GET: api/Customer
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblCustomer>>> GetTblCustomers()
        {
            try
            {
                return await _context.TblCustomers.ToListAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Customer/5
        [Authorize("admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTblCustomer(Guid id)
        {
            try
            {
                var tblCustomer = await _context.TblCustomers.FindAsync(id);

                if (tblCustomer == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "get customer by id Successful",
                        data = tblCustomer
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblCustomer(Guid id, TblCustomer tblCustomer)
        {
            if (id != tblCustomer.CustomerId)
            {
                return BadRequest();
            }

            try
            {
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(tblCustomer.Password);
                tblCustomer.Password = hashPassword;
                _context.Entry(tblCustomer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                if (!TblCustomerExists(id))
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
                var customer = await _context.TblCustomers
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.CustomerEmail == request.Email);
                if (customer == null)
                    return BadRequest(new { status = "failed", message = "Email not found!" });
                var verify = BCrypt.Net.BCrypt.Verify(request.Password, customer!.Password);
                if (!verify)
                    return BadRequest(new { status = "failed", message = "Incorrect password!" });
                var response = _customerService.Authenticate(customer);
                return Ok(
                    new { status = "success", message = "Login Successfull", data = response }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblCustomer>> PostTblCustomer(TblCustomer tblCustomer)
        {
            try
            {
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(tblCustomer.Password);
                tblCustomer.Password = hashPassword;
                _context.TblCustomers.Add(tblCustomer);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetTblCustomer",
                    new { id = tblCustomer.CustomerId },
                    tblCustomer
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblCustomer(Guid id)
        {
            try
            {
                var tblCustomer = await _context.TblCustomers.FindAsync(id);
                if (tblCustomer == null)
                {
                    return NotFound();
                }

                _context.TblCustomers.Remove(tblCustomer);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool TblCustomerExists(Guid id)
        {
            return _context.TblCustomers.Any(e => e.CustomerId == id);
        }
    }
}
