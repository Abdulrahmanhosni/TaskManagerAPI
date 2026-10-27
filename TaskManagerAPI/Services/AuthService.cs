using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManagerAPI.Data;
using TaskManagerAPI.DTOs.Auth;
using TaskManagerAPI.Exceptions;
using TaskManagerAPI.Models;
using TaskManagerAPI.Settings;

namespace TaskManagerAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings  _jwt;

        public AuthService(AppDbContext context, IOptions< JwtSettings> jwtOpthions)
        {
            _context = context;
            _jwt = jwtOpthions.Value;
        }
        public async Task RegisterAsync(RegisterDto dto)
        {
            bool chic = await _context.Users.Where(e =>  e.Email == dto.Email).AnyAsync();
            if (chic == true) throw new ConflictException("Email already exists");
            var user = new User { Name =  dto.Name, Email = dto.Email,
             Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)};
             _context.Users.Add(user);
             await _context.SaveChangesAsync(); 
        }
        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.Where(e => e.Email == dto.Email).FirstOrDefaultAsync();
            if (user == null) throw new NotFoundException("Email not found");
            var password = BCrypt.Net.BCrypt.Verify(dto.Password , user.Password);
            if (!password) throw new UnauthorizedException("Invalid password");
            var key = _jwt.Key;
            var issuer = _jwt.Issuer;
            var audience = _jwt.Audience;
            var claims = new[]
            {
                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                  new Claim(ClaimTypes.Email,user.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
                (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes),
                signingCredentials: credentials


                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
