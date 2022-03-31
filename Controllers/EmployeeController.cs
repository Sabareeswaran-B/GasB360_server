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

        private readonly ICustomerService _customerService;

        public EmployeeController(
            GasB360Context context,
            ICustomerService customerService,
            IEmployeeService EmployeeService
        )
        {
            _context = context;
            _customerService = customerService;
            _EmployeeService = EmployeeService;
        }
        
        // API To Get All Of The Employees
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employee = await _context.TblEmployees.ToListAsync();
                return Ok(
                    new { status = "success", message = "Get all customers", data = employee }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // API To Get The Employee By Passing EmployeeId As Parameter

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeById(Guid employeeId)
        {
            try
            {
                var tblEmployee = await _context.TblEmployees.FindAsync(employeeId);

                if (tblEmployee == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "get customer by id Successful",
                        data = tblEmployee
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Update The Employee Details By Passing EmployeeId As Parameter

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(Guid employeeId, TblEmployee tblEmployee)
        {
            if (employeeId != tblEmployee.EmployeeId)
            {
                return BadRequest();
            }

            try
            {
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(tblEmployee.Password);
                tblEmployee.Password = hashPassword;
                _context.Entry(tblEmployee).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                 return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all connection request successful.",
                        data = tblEmployee
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsEmployeeExists(employeeId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }
        //API To Get All The Connection Requests
        [HttpGet]
        public async Task<IActionResult> GetAllConnectionRequests()
        {
            try
            {
                var customer = await _context.TblCustomers
                    .Where(x => x.Active == "true")
                    .Where(x => x.Requested == "true")
                    .ToListAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all connection request successful.",
                        data = customer
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To EmployeeAdmin Accepting The Connection Request By Passing CustomerId As Parameter
        [HttpPut("{customerId}")]
        public async Task<IActionResult> AcceptCustomerConnection(Guid customerId)
        {
            try
            {
                TblCustomer customer = await _context.TblCustomers
                    .Where(c => c.Active == "true")
                    .Where(c => c.CustomerId == customerId)
                    .FirstOrDefaultAsync();
                customer.Requested = "false";
                customer.CustomerConnection += 1;
                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                 return Ok(
                    new
                    {
                        status = "success",
                        message = "Accept connection request successful.",
                        data = customer
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsEmployeeExists(customerId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }

           
        }

        //API To EmployeeAdmin Rejecting The Connection Request By Passing CustomerId As Parameter
        [HttpPut("{customerId}")]
        public async Task<IActionResult> RejectCustomerConnection(Guid customerId)
        {
            try
            {
                TblCustomer customer = await _context.TblCustomers
                    .Where(c => c.Active == "true")
                    .Where(c => c.CustomerId == customerId)
                    .FirstOrDefaultAsync();
                customer.Requested = "false";
                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                 return Ok(
                    new
                    {
                        status = "success",
                        message = "Reject Connection request successful.",
                        data = customer
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsEmployeeExists(customerId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }


        }
        //API To Employee Login By Passing AuthRequest Object As Parameter
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
                    new { status = "success", message = "Login Successful", data = response }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Add New Employee By Passing tblEmployee Object As Parameter

        [HttpPost]
        public async Task<IActionResult> AddNewEmployee(TblEmployee tblEmployee)
        {
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(tblEmployee.Password);
            tblEmployee.Password = hashPassword;
            _context.TblEmployees.Add(tblEmployee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetEmployeeById",
                new { employeeId = tblEmployee.EmployeeId },
                new
                {
                    status = "success",
                    message = "Add new employee Successful",
                    data = tblEmployee
                }
            );
        }

        //API To Delete The Employee By Passing EmployeeId As Parameter
        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(Guid employeeId)
        {
            try
            {
                var tblEmployee = await _context.TblEmployees.FindAsync(employeeId);
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
        // Function To Check Whether The Employee Already Exists or Not By Passing EmployeeId As Parameter
  
        private bool IsEmployeeExists(Guid employeeId)
        {
            return _context.TblEmployees.Any(e => e.EmployeeId == employeeId);
        }
    }
}
