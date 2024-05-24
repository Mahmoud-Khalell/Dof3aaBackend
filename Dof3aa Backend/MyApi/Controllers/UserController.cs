using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using MimeKit;
using ServiceLayer.Authservice;
using ServiceLayer.DocumentServices;
using ServiceLayer.CourceService;
using PresentationLayer.Services;
using PresentationLayer.DTO;
using ServiceLayer.EmailService;
namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IauthService authService;
        private readonly ICourceService iCourceService;

        public UserController(IauthService AuthService,ICourceService ICourceService)
        {
            authService = AuthService;
            
            iCourceService = ICourceService;
        }


        #region Confirm Email
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user =await authService.GetUserByEmailasync(email);
            if (user == null)
                return NotFound("Invalid User");

            var result = await authService.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok("Email Confirmed successfully");
            return BadRequest(result.Errors.FirstOrDefault());
        }
        #endregion

        #region Send Email Confirmation
        [HttpGet("SendEmailConfirmation")]
        public async Task<IActionResult> SendconfirmEmail(string email)
        {
            var user =await authService.GetUserByEmailasync(email);
            if (user == null)
                return NotFound();

            var token=authService.GenerateToken(user.UserName);
            

            var link = Url.Action("ConfirmEmail", "User", new { email = email, token = token }, Request.Scheme, Request.Host.ToString());
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dof3aa","icpcfinalist@gmail.com"));
            message.To.Add(new MailboxAddress("",email));
            message.Subject = "Confirm Email";

            var builder = new BodyBuilder();
            builder.HtmlBody = EmailService.GetEmailBodyForEmailConfirm(link); 
            message.Body = builder.ToMessageBody();
            EmailService.SendEmail(message);
            return Ok();


        }
        #endregion

        #region Update Password
        [HttpGet("UpdatePassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await authService.GetUserByEmailasync(email);
            if (user == null)
                return NotFound();
            var token = await authService.GeneratePasswordToken(user);
            
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dof3aa", "icpcfinalist@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Token for updateing Password";

            var builder = new BodyBuilder();
            builder.HtmlBody = EmailService.GetEmailBodyForPasswordReset(token);
            message.Body = builder.ToMessageBody();
            EmailService.SendEmail(message);
            return Ok();
        }

        #endregion

        #region Update Password Confirm
        [HttpGet("UpdatePasswordConfirm")]
        public async Task<IActionResult> UpdatePasswordConfirm(string email, string token, string newPassword)
        {
            var user = await authService.GetUserByEmailasync(email);
            if (user == null)
                return NotFound();
            var result = await authService.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Errors.FirstOrDefault());
        }

        #endregion

        #region Register
        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromForm] RegisterationDTO registerationDTO)
        {
            if (ModelState.IsValid)
            {
                var user =await Mapper.RegisDTO2User(registerationDTO);

                var result = await authService.CreateUser(user, registerationDTO.Password);
                if (result.Succeeded)
                {
                    //confirm email
                    SendconfirmEmail(user.Email);
                    return Ok();
                }
                else
                {
                    DocumentService.DeleteFile(user.ImageUrl);
                    return BadRequest(result.Errors.FirstOrDefault());
                }
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region Login
        [HttpPost("Login")]
        [EnableCors]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
        {
            if (ModelState.IsValid == false)
                return Unauthorized(ModelState);
            var user=await authService.GetUserByUsernameasync(loginDTO.UserName);

            if (user == null)
                return NotFound("Username not found");
            if (user.EmailConfirmed == false)
                return NotFound("Email not confirmed");

            bool check = await authService.CheckPasswordAsync(user, loginDTO.Password);
            if (check == false)
                return NotFound("Password is wrong");

            var token =await authService.GenerateToken(loginDTO.UserName);
            return Ok(token);
        }
        #endregion


        #region Get User Info
        [HttpGet("GetUserInfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserInfo()
        {
            var user =await authService.GetCurentUser();
            if (user == null)
                return NotFound();

            var userDTO = Mapper.User2UserDTO(user);
            return Ok(userDTO);
        }
        #endregion

        #region update user info
        [HttpGet("GetRoles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRole(int CourceId)
        {

            var user = await authService.GetCurentUser();
            if (user == null)
                return Unauthorized();

            var role=await iCourceService.GetRole( CourceId,user.UserName);   
            
            if(role==0)
                return NotFound();
            return Ok(role);

        }
        #endregion


        




    }
}
