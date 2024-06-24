using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using TestRESTAPI.Data.Models;
using TestRESTAPI.Models;

namespace TestRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromEmail = "7o22am@gmail.com";
            var fromPassword = "h022@m&..//";

            var message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            message.Subject = subject;
            message.To.Add(email);
            message.Body = $"<html><body>{htmlMessage}</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, "hzwv qqub oibv crby")
            };
            smtpClient.UseDefaultCredentials = false;
            await smtpClient.SendMailAsync(message);
        }


        public AccountController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
        }

        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration configuration;

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser([FromForm] dtoNewUser user) 
        {
            string randomString = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 5).
                Select(s => s[new Random().Next(s.Length)]).ToArray());

            using var stream = new MemoryStream();
            await user.image.CopyToAsync(stream);

            if (ModelState.IsValid) 
            {
                AppUser appUser = new()
                {
                        UserName = user.email,
                        Email = user.email,
                        FullName = user.fullname,
                        verfiyCode = randomString ,
                        addries = user.addries,
                        EmailConfirmed = false ,
                         gender = user.gender,
                         image = stream.ToArray(),
                         link = user.link,
                         location = user.location,
                         PhoneNumber = user.phoneNumber,
                         title = user.title,
                          salary = user.salary,
                          typeOfCam = user.typeOfCam,
                           typeOfUser = user.typeOfUser,
                            NationalId = user.NationalId,
                            Nationality= user.Nationality,
                     
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, user.password);
                if (result.Succeeded)
                {
                   await SendEmailAsync(user.email, "Confirm Email", "Your Code is ( " + randomString + " )");
                    return Ok("Success");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(dtoLogin login)
        { 
            if (ModelState.IsValid) 
            {
                AppUser? user = await _userManager.FindByEmailAsync(login.email);
                if (user.EmailConfirmed == false ) {
                    return Unauthorized();
                }

                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, login.password))
                    {
                        var claims = new List<Claim>();
                        //claims.Add(new Claim("name", "value"));
                        claims.Add(new Claim(ClaimTypes.Name, user.Email));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                        //signingCredentials
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: configuration["JWT:Issuer"],
                            audience: configuration["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: sc
                            );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                        };
                        return Ok(_token);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email is invalid");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(dtoConfirmEmail confirmEmail)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByEmailAsync(confirmEmail.email);
                if (user != null)
                {
                    if (confirmEmail.code == user.verfiyCode) {

                        user.EmailConfirmed = true;
                       await _userManager.UpdateAsync(user);
                        return Ok("User Confirm Sucessfully");
                        
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email is invalid");
                }
            }
            return BadRequest(ModelState);
        }


        [HttpPatch("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromForm] dtoNewUser user)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(user.email);
                if (appUser == null)
                {
                    return NotFound("User not found.");
                }

                if (user.fullname != null)
                {
                    appUser.FullName = user.fullname;
                }

                if (user.addries != null)
                {
                    appUser.addries = user.addries;
                }

                if (user.gender != null)
                {
                    appUser.gender = user.gender;
                }

                if (user.image != null)
                {
                    using var stream = new MemoryStream();
                    await user.image.CopyToAsync(stream);
                    appUser.image = stream.ToArray();
                }

                if (user.link != null)
                {
                    appUser.link = user.link;
                }

                if (user.location != null)
                {
                    appUser.location = user.location;
                }

                if (user.phoneNumber != null)
                {
                    appUser.PhoneNumber = user.phoneNumber;
                }

                if (user.title != null)
                {
                    appUser.title = user.title;
                }

                if (user.salary != null)
                {
                    appUser.salary = user.salary;
                }

                if (user.typeOfCam != null)
                {
                    appUser.typeOfCam = user.typeOfCam;
                }

                if (user.typeOfUser != null)
                {
                    appUser.typeOfUser = user.typeOfUser;
                }

                if (user.NationalId != null)
                {
                    appUser.NationalId = user.NationalId;
                }

                if (user.Nationality != null)
                {
                    appUser.Nationality = user.Nationality;
                }

                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    return Ok("User updated successfully.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }

            return BadRequest(ModelState);
        }


        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(dtoDeleteUser user)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(user.email);
                if (appUser == null)
                {
                    return NotFound("User not found.");
                }
 
                var result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded)
                {
                    return Ok("User Deleted successfully.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(dtoDeleteUser user)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(user.email);
                if (appUser == null)
                {
                    return NotFound("User not found.");
                }

                    return Ok(appUser);
               
                 
            }

            return BadRequest(ModelState);
        }


    }
}
