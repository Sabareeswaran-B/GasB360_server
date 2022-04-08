#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GasB360_server.Models;
using GasB360_server.Helpers;
using System.Data.SqlClient;
using System.Data;

namespace GasB360_server.Controllers;

[Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DashBoardController : ControllerBase
    {
        private readonly GasB360Context _context;

        public DashBoardController(GasB360Context context)
        {
            _context = context;
        }

        // API To Get All the apis needed for the admin dashboard
        [HttpGet]
        public IActionResult GetAdminDashboard() {
            try
            {
                SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString());
                SqlCommand command = new SqlCommand();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                DataSet dataSet = new DataSet();
                command = new SqlCommand("proc_admin_dashboard", connection);
                command.CommandType = CommandType.StoredProcedure;
                dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataSet);
                connection.Close();

                return Ok(new {
                    status="success",
                    message="Get admin dashboard successfull",
                    data = dataSet.Tables
                });
            }
            catch (System.Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }
    }