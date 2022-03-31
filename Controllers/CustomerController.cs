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
        private readonly IMailService _mailService;

        public CustomerController(
            GasB360Context context,
            ICustomerService customerService,
            IMailService mailService
        )
        {
            _context = context;
            _customerService = customerService;
            _mailService = mailService;
        }

        //API To Get All Of The Customers
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _context.TblCustomers.ToListAsync();
                return Ok(
                    new { status = "success", message = "Get all customers", data = customers }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Get The Customer By Passing CustomerId As Parameter
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(Guid customerId)
        {
            try
            {
                var tblCustomer = await _context.TblCustomers.FindAsync(customerId);

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
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Update The Customer Details By Passing CustomerId As Parameter
        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, TblCustomer tblCustomer)
        {
            if (customerId != tblCustomer.CustomerId)
            {
                return BadRequest();
            }

            try
            {
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(tblCustomer.Password);
                tblCustomer.Password = hashPassword;
                _context.Entry(tblCustomer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                var customer = await _context.TblCustomers.FindAsync(customerId);
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Update customer successful.",
                        data = customer
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsCustomerExists(customerId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }

        //API To Update Request Connection By Passing CustomerId As Parameter
        [HttpPut("{customerId}")]
        public async Task<IActionResult> RequestConnection(Guid customerId)
        {
            try
            {
                TblCustomer customer = await _context.TblCustomers
                    .Where(c => c.Active == "true")
                    .Where(c => c.CustomerId == customerId)
                    .FirstOrDefaultAsync();
                customer.Requested = "true";
                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Request connection successful.",
                        data = customer
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsCustomerExists(customerId))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }
        //API To Customer Login By Passing AuthRequest Object As Parameter
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
                    new { status = "success", message = "Login Successful", data = response }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Add New Customer By Passing tblCustomer Object As Parameter
        [HttpPost]
        public async Task<IActionResult> AddNewCustomer(TblCustomer tblCustomer)
        {
            try
            {
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(tblCustomer.Password);
                tblCustomer.Password = hashPassword;
                _context.TblCustomers.Add(tblCustomer);
                await _context.SaveChangesAsync();

                var mail = new MailRequest();
                mail.ToEmail = tblCustomer.CustomerEmail;
                mail.Subject = "Welcome to GasB360";
                mail.Body =
                    $"<h3>Hello {tblCustomer.CustomerName},</h3><p>Thank you for register. We are glad to be on your service. Get a connection and order from us.</p><br><p>Thankyou</p><br><p>GasB360</p>";
                await _mailService.SendEmailAsync(mail);

                return CreatedAtAction(
                    "GetCustomerById",
                    new { customerId = tblCustomer.CustomerId },
                    new {
                        status = "success",
                        message = "Add new customer successful",
                        data = tblCustomer
                    }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //API To Delete The Customer By Passing CustomerId As Parameter
        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomerById(Guid customerId)
        {
            try
            {
                var tblCustomer = await _context.TblCustomers.FindAsync(customerId);
                if (tblCustomer == null)
                {
                    return NotFound();
                }

                _context.TblCustomers.Remove(tblCustomer);
                await _context.SaveChangesAsync();

                return Ok(
                    new { status = "success", message = "Delete customer by id successful." }
                );
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }
        //Function To Check Whether The Customer Already Exists Or Not By Passing CustomerId As Parameter
        private bool IsCustomerExists(Guid customerId)
        {
            return _context.TblCustomers.Any(e => e.CustomerId == customerId);
        }
    }
}
