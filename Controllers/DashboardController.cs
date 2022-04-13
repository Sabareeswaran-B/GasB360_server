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
    public IActionResult GetAdminDashboard()
    {
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
            return Ok(
                new
                {
                    status = "success",
                    message = "Get admin dashboard successfull",
                    data = new
                    {
                        productCategory = dataSet.Tables[0],
                        FilledProduct = dataSet.Tables[1],
                        UnfilledProducts = dataSet.Tables[2],
                        Customer = dataSet.Tables[3],
                        CustomerRequestCount = dataSet.Tables[4],
                        Employee = dataSet.Tables[5],
                        Order = dataSet.Tables[6],
                        Delivery = dataSet.Tables[7],
                        customerCount = dataSet.Tables[3].Rows.Count,
                        employeeCount = dataSet.Tables[5].Rows.Count,
                        orderCount = dataSet.Tables[6].Rows.Count,
                        deliveryCount = dataSet.Tables[7].Rows.Count,
                    }
                }
            );
        }
        catch (System.Exception ex)
        {
            Sentry.SentrySdk.CaptureException(ex);
            return BadRequest(new { status = "failed", message = ex.Message });
        }
    }
}
