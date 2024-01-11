using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;
using StoreManager.Facade.Interfaces.Services;
using StoreManager.Models;
using StoreManager.Web.Interfaces;
using System.Net;
using System.Net.Mail;

namespace StoreManager.Web.Controllers;

public class CustomerController : Controller
{
    private readonly ICustomerAccountService _customerAccountService;
    private readonly ICustomerQueryService _customerQueryService;
    private readonly ICustomerCommandService _customerCommandService;
    private readonly IApiHelper _apiHelper;

    public CustomerController(ICustomerAccountService customerAccountService, ICustomerQueryService customerQueryService, ICustomerCommandService customerCommandService, IApiHelper apiHelper)
    {
        _customerAccountService = customerAccountService;
        _customerQueryService = customerQueryService;
        _customerCommandService = customerCommandService;
        _apiHelper = apiHelper;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            if (await _apiHelper.Login(model.Username, model.Password))
            {
                return RedirectToAction("Index", "Home");
            }
        }

        return BadRequest("Login faild");
    }

    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var verificationToken = GenerateVerificationToken();

            _customerAccountService.Register(model.Username, model.Password,
                    new Customer
                    {
                        DisplayName = model.Username,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateOfBirth = model.DateOfBirth,
                        Email = model.Email,
                        VerificationToken = verificationToken,
                        TokenExpireTime = DateTime.Now.AddMinutes(30),
                    }
                );
            SendVerificationEmail(model.Email, verificationToken);

            return Ok("Registered. Check your email for verification.");
        }

        return BadRequest("Registration failed");
    }

    private string GenerateVerificationToken() => Guid.NewGuid().ToString();

    private void SendVerificationEmail(string email, string verificationToken)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("sendertest90@gmail.com", "djgegughplatrcbh"),
            EnableSsl = true,
        };
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var mailMessage = new MailMessage
        {
            From = new MailAddress("sendertest90@gmail.com", "StoreManager"),
            Subject = "Account Verification",
            Body = $"Click the following link to verify your account:{baseUrl}/customer/Verify?token={verificationToken}",
            IsBodyHtml = false,
        };

        mailMessage.To.Add(email);

        smtpClient.Send(mailMessage);
    }

    [HttpGet]
    public IActionResult Verify(string token)
    {
        var customer = _customerQueryService
            .Set(x => x.VerificationToken == token)
            .Include(x => x.AccountDetails)
            .SingleOrDefault();

        if (customer == null) return NotFound();

        if (!customer.AccountDetails!.IsVerified)
        {
            if (customer.TokenExpireTime < DateTime.Now)
            {
                ResendVerificationEmail(customer);
                return Ok("Verification has been resent to your email.");
            }

            customer.AccountDetails.IsVerified = true;
            customer.VerificationToken = null;
            _customerCommandService.Update(customer);

            return Ok("Successfully verified");
        }

        return BadRequest("Verification failed or already verified");
    }

    private void ResendVerificationEmail(Customer customer)
    {
        if (customer.Email == null) throw new ArgumentNullException();

        var token = GenerateVerificationToken();

        customer.VerificationToken = token;
        customer.TokenExpireTime = DateTime.Now.AddMinutes(30);
        _customerCommandService.Update(customer);

        SendVerificationEmail(customer.Email, token);
    }
}
