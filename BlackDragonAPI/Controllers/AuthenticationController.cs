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
   /// Authenticates a user using the provided login credentials and returns a JWT token if authentication is
   /// successful.
   /// </summary>
   /// <remarks>The user's email address must be registered, and the password must be correct for
   /// authentication to succeed. This endpoint is typically used to obtain a token for subsequent authenticated
   /// requests.</remarks>
   /// <param name="request">The login request containing the user's email and password to be validated.</param>
   /// <returns>An IActionResult that contains a JWT token if authentication succeeds; otherwise, an Unauthorized result.</returns>
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


    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromServices] TokenBlacklistContext db)
    {
        // Since JWTs are stateless, logout can be handled on the client side by simply deleting the token.
        // Optionally, you could implement token blacklisting on the server side if needed.
        var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        db.Tokens.Add(new TokenBlacklist {  Token = token, RevokedAt = DateTime.UtcNow});
        await db.SaveChangesAsync();
        return Ok();
    }   

    /// <summary>
    /// Deletes the user account associated with the specified email address.
    /// </summary>
    /// <remarks>The specified email address must correspond to an existing user. Ensure that the email
    /// provided is valid and registered in the system before calling this method.</remarks>
    /// <param name="email">The email address of the user to delete. This parameter cannot be null or empty.</param>
    /// <returns>An IActionResult that indicates the result of the delete operation. Returns Ok if the user is successfully
    /// deleted, NotFound if no user exists with the specified email address, or BadRequest with error details if the
    /// deletion fails.</returns>
    [HttpDelete("delete/{email}")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound();
        }
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result.Errors);
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
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

