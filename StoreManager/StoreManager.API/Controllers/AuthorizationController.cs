using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StoreManager.API.JwtToken;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Services;
using StoreManager.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreManager.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly ICustomerAccountService _customerAccountService;
    private readonly IConfiguration _configuration;

    public AuthorizationController(ICustomerAccountService accountService, IConfiguration configuration)
    {
        _customerAccountService = accountService;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login(LoginModel model)
    {
        try
        {
            _customerAccountService.Login(model.Username, model.Password);

            return Ok(GetToken(model.Username));
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"{ex.Message}");
            return Unauthorized();
        }
    }

    [HttpPost]
    [Route("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        var token = HttpContext.Request.Headers["Authorization"];
        TokenBlackList.RevokeToken(token!);

        return Ok("Logout successful");
    }

    [HttpPost]
    [Route("register")]
    public IActionResult Register(RegisterModel model)
    {
        _customerAccountService.Register(model.Username, model.Password,
            new Customer
            {
                DisplayName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email
            });

        return Ok();
    }

    private string GetToken(string username)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Email, username),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        tokenHandler.WriteToken(token);

        return tokenHandler.WriteToken(token);
    }
}
