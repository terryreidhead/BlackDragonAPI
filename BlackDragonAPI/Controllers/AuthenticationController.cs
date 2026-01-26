using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlackDragonAPI.Models;

namespace BlackDragonAPI.Controllers;   

[ApiController]
[Route("api/[controller]")]

public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
                private readonly IConfiguration _configuration;
        public AuthenticationController(
            UserManager<IdentityUser> userManager,
         
            IConfiguration configuration)
        {
            _userManager = userManager;
            
            _configuration = configuration;
        }
    /// <summary>
    /// Registers a new user account using the specified registration details.
    /// </summary>
    /// <remarks>This method should be called with an HTTP POST request. Ensure that the request body contains
    /// valid registration data to avoid validation errors.</remarks>
    /// <param name="request">The registration information containing the user's email address and password. The email must be in a valid
    /// format, and the password must meet the application's security requirements.</param>
    /// <returns>An IActionResult that indicates the outcome of the registration operation. Returns Ok if the registration is
    /// successful; otherwise, returns BadRequest with the encountered errors.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new IdentityUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Unauthorized();

        var valid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!valid)
            return Unauthorized();

        var token = GenerateJwtToken(user);
        return Ok(new { token });
        }
    /// <summary>
    /// Generates a JSON Web Token (JWT) that contains claims identifying the specified user.
    /// </summary>
    /// <remarks>The generated token is valid for 30 minutes and includes claims for the user's ID and email
    /// address.</remarks>
    /// <param name="user">The user for whom the JWT is generated. This parameter must not be null.</param>
    /// <returns>A string that represents the generated JWT, which can be used for authentication purposes.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the JWT configuration key 'Jwt:Key' is missing or empty.</exception>
    private string GenerateJwtToken(IdentityUser user)
    {
        var keyString = _configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(keyString))
            throw new InvalidOperationException("Missing JWT configuration: Jwt:Key");

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

