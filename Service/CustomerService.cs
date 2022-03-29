using System.IdentityModel.Tokens.Jwt;
using GasB360_server.Helpers;
using GasB360_server.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace GasB360_server.Services;

public interface ICustomerService
{
    AuthResponse Authenticate(TblCustomer model);
    IEnumerable<TblCustomer> GetAll();
    TblCustomer GetById(Guid Id);
}

public class CustomerService : ICustomerService
{
    private readonly GasB360Context _context;

    private readonly AppSettings _appSettings;

    public CustomerService(GasB360Context context, IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    public AuthResponse Authenticate(TblCustomer customer)
    {
        var token = GenerateJwtToken(customer);
        return new AuthResponse(customer.CustomerId, customer.Role!.RoleType!, token);
    }

    public IEnumerable<TblCustomer> GetAll()
    {
        return _context.TblCustomers.ToList();
    }

    public TblCustomer GetById(Guid Id)
    {
        var customer =
            from c in _context.TblCustomers
            where c.Active == "true"
            join r in _context.TblRoles on c.RoleId equals r.RoleId
            select new TblCustomer
            {
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerEmail = c.CustomerEmail,
                CustomerPhone = c.CustomerPhone,
                RoleId = c.RoleId,
                TypeId = c.TypeId,
                CustomerConnection = c.CustomerConnection,
                AllowedLimit = c.AllowedLimit,
                Requested = c.Requested,
                Role = r,
                Type = c.Type
            };
        return customer.FirstOrDefault(x => x.CustomerId == Id)!;
    }

    private string GenerateJwtToken(TblCustomer customer)
    {
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("Id", customer.CustomerId.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
