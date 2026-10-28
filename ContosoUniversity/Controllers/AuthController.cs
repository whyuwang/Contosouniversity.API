using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ContosoUniversity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SchoolContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenViewModel>> Login([FromBody] LoginViewModel loginViewModel)
        {
            var person = await _context.People
                .AsNoTracking()
                .Where(p => p.Username == loginViewModel.Username && p.Password == loginViewModel.Password)
                .FirstOrDefaultAsync();
            if (person == null)
            {
                return Unauthorized();
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            string role = "Student";
            if (person is Instructor)
            {
                role = "Instructor";
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, person.FullName),
                new Claim(ClaimTypes.Role, role)
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:Expires"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return Ok(new TokenViewModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
