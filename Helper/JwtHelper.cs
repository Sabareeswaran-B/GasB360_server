using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GasB360_server.Services;
using GasB360_server.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using GasB360_server.Models;

namespace GasB360_server.Helpers;

public class JwtHelper
{
    private readonly RequestDelegate? _next;
    private readonly AppSettings? _appSettings;

    public JwtHelper(RequestDelegate next, IOptions<AppSettings> appsettings)
    {
        _next = next;
        _appSettings = appsettings.Value;
    }

    public async Task Invoke(
        HttpContext context,
        ICustomerService customerService,
        IEmployeeService employeeService
    )
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            attachUserToContext(customerService, employeeService, context, token);
        await _next!(context);
    }

    private void attachUserToContext(
        ICustomerService customerService,
        IEmployeeService employeeService,
        HttpContext context,
        string token
    )
    {
        try
        {
            var key = Encoding.ASCII.GetBytes(_appSettings!.Secret!);
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                },
                out SecurityToken validateToken
            );

            var jwtToken = (JwtSecurityToken)validateToken;
            var userID = Guid.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == "Id")!.Value);
            var customer = customerService.GetById(userID);
            if (customer != null)
            {
                context.Items["user"] = new AuthResponse(
                    customer.CustomerId,
                    customer.Role!.RoleType!,
                    ""
                );
            }
            else
            {
                var Employee = employeeService.GetById(userID);
                context.Items["user"] = new AuthResponse(
                    Employee.EmployeeId,
                    Employee.Role!.RoleType!,
                    ""
                );
            }
        }
        catch (System.Exception ex)
        {
            Sentry.SentrySdk.CaptureException(ex);
            // Sentry.SentrySdk.CaptureException(ex);
        }
    }
}