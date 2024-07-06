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
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Castle.Core.Smtp;
using TestRESTAPI.Data;
using Microsoft.Extensions.Options;

namespace TestRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// send email 
        protected async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
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
            catch (Exception ex)
            {

            }
           
        }
        public AccountController(UserManager<AppUser> userManager, IConfiguration configuration , IOptions<AppSetting> appSettings)
        {
            _userManager = userManager;
            this.configuration = configuration;
            _appSettings = appSettings.Value;
        }
        private readonly AppSetting _appSettings;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration configuration;
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser( dtoNewUser user)
        {
            string randomString = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", 5).
                Select(s => s[new Random().Next(s.Length)]).ToArray());

            if (ModelState.IsValid)
            {
                Boolean ans = false;
                if (user.EmailConfirmed != null)
                {
                      ans =true;
                }
                        
                AppUser appUser = new()
                {
                    UserName = user.email,
                    Email = user.email,
                   FullName = user.fullname,
                   verfiyCode = randomString,
                    addries = user.addries,
                    EmailConfirmed = ans,
                    gender = user.gender,
                    link = user.lastWork,
                    perHourTask =user.perHourTask,
                    location = user.location,
                    PhoneNumber = user.phoneNumber,
                    title = user.title,
                    salary = user.salary,
                    typeOfCam = user.typeOfCam,
                    typeOfUser = user.typeOfUser,
                    NationalId = user.NationalId,
                    Nationality = user.Nationality,
                    idTokn = user.idTokn,
                    provider = user.provider,

                };


                IdentityResult result = await _userManager.CreateAsync(appUser, user.password);
                if (result.Succeeded)
                {
                    await SendEmailAsync(user.email, "Confirm Email", "Your Code is ( " + randomString + " )");

                    return Ok(new { respone = "Sucess"});
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
                AppUser? user = await _userManager.FindByEmailAsync(login.email.ToString());
                if (user.EmailConfirmed == false ) {
 
                    return Ok(new { respone = "Confirm email" });
                }

                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user , login.password))
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
                            respone = "Sucess",
                            id = user.Id,
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                        };

                        return Ok(_token);
                    }
                    else
                    {

                        return Ok(new { respone = "Password Error" });
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

                        return Ok(new { respone = "User Confirm Sucessfully" });
                       
                        
                    }
                    else
                    {
                        return Ok(new { respone = "Code Error" });
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
        public async Task<IActionResult> UpdateUser( dtoUpdateUser user)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByEmailAsync(user.email);
                if (appUser == null)
                {
                    return NotFound("User not found.");
                }

                if (user.fullname != "")
                {
                    appUser.FullName = user.fullname;
                }

                if (user.addries != "")
                {
                    appUser.addries = user.addries;
                }

                if (user.gender != "")
                {
                    appUser.gender = user.gender;
                }
     

                if (user.lastWork != "")
                {
                    appUser.link = user.lastWork;
                }

                if (user.location != "")
                {
                    appUser.location = user.location;
                }

                if (user.phoneNumber != "")
                {
                    appUser.PhoneNumber = user.phoneNumber;
                }

                if (user.title != "")
                {
                    appUser.title = user.title;
                }

                if (user.salary != "")
                {
                    appUser.salary = user.salary;
                }

                if (user.typeOfCam != "")
                {
                    appUser.typeOfCam = user.typeOfCam;
                }

                if (user.typeOfUser != "")
                {
                    appUser.typeOfUser = user.typeOfUser;
                }
                if (user.Nationality != "")
                {
                    appUser.Nationality = user.Nationality;
                }



                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    return Ok(new { respone = "Sucess" });
                    
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

        [HttpPatch("UpdateRate")]
        public async Task<IActionResult> UpdateRate(dtoUpdateUser user)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByIdAsync(user.id);
                if (appUser == null)
                {
                    return NotFound("User not found.");
                }

                float x = (float)user.rate;
                float y = float.Parse(appUser.rate);
                x =( x + y )/ 2;

                appUser.rate = x.ToString();
              
                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    return Ok(new { respone = "Sucess" });

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

        [HttpPost("GetUser/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByIdAsync(id);
                if (appUser == null)
                {
                    return NotFound("User not found.");
                }

                    return Ok(appUser);
               
                 
            }

            return BadRequest(ModelState);
        }


        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            if (ModelState.IsValid)
            {
                var usersWithType = await _userManager.Users.Where(u => u.typeOfUser != null).ToListAsync();
                if (usersWithType == null)
                {
                    return NotFound("User not found.");
                }

                return Ok(usersWithType);


            }

            return BadRequest(ModelState);
        }


        [HttpPost("Search")]
        public async Task<IActionResult> Search( dtoSearch search)
        {
            if (ModelState.IsValid)
            {
                var usersSearch = await _userManager.Users.Where(u => (u.gender == search.gender || search.gender =="")
                && (u.perHourTask == search.perHourTask || search.perHourTask == "")
                && (u.typeOfCam == search.typeOfCam || search.typeOfCam == "")
                &&   (u.location.Contains(search.location) || search.location == "")).ToListAsync();
                if (usersSearch == null)
                {
                    return NotFound(" Not found Users");
                }

                 return Ok( new { respone = usersSearch });


            }

            return BadRequest(ModelState);
        }



        [HttpPatch("ChangeImage")]
        public async Task<IActionResult> ChangeImage( [FromForm] dtoImage NewImage)
        {

            if (ModelState.IsValid)
            {
              
                var appUser = await _userManager.FindByEmailAsync( NewImage.email);

                using var stream = new MemoryStream();
                await NewImage.image.CopyToAsync(stream);


                appUser.image = stream.ToArray();

                var respone = await _userManager.UpdateAsync(appUser);
                if (respone.Succeeded)
                {
                    return Ok(new { respone = "Sucess" });

                }
                else
                {
                    foreach (var error in respone.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }

            }

            return BadRequest(ModelState);
        }


        [HttpGet("changePassword")]
        public async Task<IActionResult> ChangePassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return Ok(new { response = "User not found" });
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordLink = $"{_appSettings.BaseUrl}/api/Account/resetPassword?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

                await  SendEmailAsync(user.Email, "Password Reset", $"Please reset your password by clicking here: <a href='{resetPasswordLink}'>link</a>");

                return Ok(new { response = "ok" });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, new { response = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] dtoForgetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Ok(new { response = "Password reset email sent. Please check your email." });
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { response = "Password has been reset successfully." });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }
    }

}
