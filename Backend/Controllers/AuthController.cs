using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hydra.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("token")]
    public IActionResult Token([FromBody] LoginRequest req)
    {
        // NOTE: Replace with real validation
        if (req.Username == "test" && req.Password == "test")
        {
            var jwtKey = _config["Jwt:Key"] ?? "ReplaceWithStrongKey";
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] { new Claim(ClaimTypes.Name, req.Username) };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        return Unauthorized();
    }
}

public record LoginRequest(string Username, string Password);
