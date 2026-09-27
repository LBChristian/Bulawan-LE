using BlogDataLibrary.Data;
using BlogDataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ISqlData _db;
        private readonly IConfiguration _config;

        public LoginController(ISqlData db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost]
        public IActionResult Login(UserLogin login)
        {
            var user = _db.Authenticate(login.Username, login.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = GenerateToken(user);

            return Ok(new
            {
                token = token
            });
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("/api/controller/register")]
        public ActionResult Register([FromBody] UserModel user)
        {
            _db.Register(user.UserName, user.FirstName, user.LastName, user.Password);
            return Ok("User registered.");
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            string userIdStr = user.Id.ToString();

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userIdStr)
    };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class UserLogin
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}