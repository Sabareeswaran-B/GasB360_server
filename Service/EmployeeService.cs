using System.IdentityModel.Tokens.Jwt;
using GasB360_server.Helpers;
using GasB360_server.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace GasB360_server.Services;

public interface IEmployeeService
{
    AuthResponse Authenticate(TblEmployee model);
    IEnumerable<TblEmployee> GetAll();
    TblEmployee GetById(Guid Id);
}

public class EmployeeService : IEmployeeService
{
    private readonly GasB360Context _context;

    private readonly AppSettings _appSettings;

    public EmployeeService(GasB360Context context, IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    public AuthResponse Authenticate(TblEmployee Employee)
    {
        var token = GenerateJwtToken(Employee);
        return new AuthResponse(Employee.EmployeeId, Employee.Role!.RoleType!, token);
    }

    public IEnumerable<TblEmployee> GetAll()
    {
        return _context.TblEmployees.ToList();
    }

    public TblEmployee GetById(Guid Id)
    {
        var Employee =
            from c in _context.TblEmployees
            where c.Active == "true"
            join r in _context.TblRoles on c.RoleId equals r.RoleId
            select new TblEmployee
            {
                EmployeeId = c.EmployeeId,
                EmployeeName = c.EmployeeName,
                EmployeeEmail = c.EmployeeEmail,
                EmployeePhone = c.EmployeePhone,
                RoleId = c.RoleId,
                Role = r,
            };
        return Employee.FirstOrDefault(x => x.EmployeeId == Id)!;
    }

    private string GenerateJwtToken(TblEmployee Employee)
    {
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("Id", Employee.EmployeeId.ToString()) }),
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
