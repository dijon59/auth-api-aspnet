using Microsoft.AspNetCore.Mvc;
using BackendApi.Data;
using BackendApi.models;
using BackendApi.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
namespace BackendApi.controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext ctx;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        ctx = context;
        _configuration = configuration;
    }

    public string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("register")]
    public ActionResult<UserResponse> Register([FromBody] RegisterHttpRequest request)
    {
        // check if user exit
        var checkedUser = ctx.Users.FirstOrDefault(user => user.Email.ToLower() == request.Email.ToLower());

        if (checkedUser != null)
        {
            return BadRequest(new { message = "User already exist"});
        }

        var _user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email.ToLower(),
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
        };

        ctx.Users.Add(_user);
        ctx.SaveChanges();

        return Ok(new UserResponse
        {
            Email = _user.Email,
            FirstName = _user.FirstName,
            LastName = _user.LastName,
        });
    }

    [HttpPost("login")]
    public ActionResult<AuthResponse> Login([FromBody] LoginHTTPRequest request)
    {

        try
        {
            // check if user exist
            var user = ctx.Users.FirstOrDefault(user => user.Email.ToLower() == request.Email.ToLower());

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // verify password
            bool isVerified = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (isVerified)
            {
                var token = GenerateJwtToken(user);

                return Ok(new AuthResponse
                {
                    Token = token,
                    Username = user.FirstName,
                });
            }

            else
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, new { message = "An error occured during login" });
        }
    }

    [Authorize]
    [HttpGet("user")]
    public ActionResult<UserResponse> GetUserDetails()
    {
        try
        {
            // Get user Id from Claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            // check if user exist
            var user = ctx.Users.Find(Guid.Parse(userId));

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new UserResponse
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            });
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, new { message = "An error occured while retrieving user details" });
        }
    }
}
