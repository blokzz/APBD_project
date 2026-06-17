using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace APBD_PROJEKT.Services;
using APBD_PROJEKT.Dtos.Auth;
using  APBD_PROJEKT.Models;
using Microsoft.EntityFrameworkCore;
using APBD_PROJEKT.Data;
public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string> Login(LoginDto dto)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Login == dto.Login);

        if (employee == null || !BCrypt.Net.BCrypt.Verify(dto.Password, employee.PasswordHash))
            throw new UnauthorizedAccessException("Nieprawidłowy login lub hasło");

        return GenerateToken(employee);
    }

    public async Task Register(RegisterDto dto)
    {
        var exists = await _context.Employees.AnyAsync(e => e.Login == dto.Login);
        if (exists)
            throw new InvalidOperationException("Login zajęty");

        _context.Employees.Add(new Employee
        {
            Login = dto.Login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        });
        await _context.SaveChangesAsync();
    }

    private string GenerateToken(Employee employee)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, employee.Login),
            new Claim(ClaimTypes.Role, employee.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}