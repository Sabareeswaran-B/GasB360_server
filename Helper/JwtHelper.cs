using System.IdentityModel.Tokens.Jwt;
using System.Text;
using GasB360_server.Services;
using GasB360_server.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using GasB360_server.Models;

namespace GasB360_server.Helpers;

public class JwtHelperCustomer
{
    private readonly RequestDelegate? _next;
    private readonly AppSettings? _appSettings;

    public JwtHelperCustomer(RequestDelegate next, IOptions<AppSettings> appsettings)
    {
        _next = next;
        _appSettings = appsettings.Value;
    }

    public async Task Invoke(HttpContext context, ICustomerService customerService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            attachUserToContext(customerService, context, token);
        await _next!(context);
    }

    private void attachUserToContext(ICustomerService customerService, HttpContext context, string token)
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

            context.Items["user"] = new AuthResponse(customer.CustomerId, customer.Role!.RoleType!, "");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
            // Sentry.SentrySdk.CaptureException(ex);
        }
    }
}
