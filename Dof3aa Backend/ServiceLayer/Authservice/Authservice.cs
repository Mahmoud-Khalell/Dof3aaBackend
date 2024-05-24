using Core.entities;
using InfraStructure_Layer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Authservice
{
    public class Authservice : IauthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly HttpContextAccessor httpContext;

        public IConfiguration Iconfig { get; }

        public Authservice(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,HttpContextAccessor httpContext,IConfiguration iconfig)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpContext = httpContext;
            Iconfig = iconfig;
        }
        private  IQueryable<AppUser> GetSpecification()
        {
            return userManager.Users.Include(e => e.UserGroups).Include(e => e.UserNotifications);

        }
        public async Task<AppUser> GetCurentUser()
        {
            var claim = httpContext.HttpContext?.User?.Claims?.FirstOrDefault(e => e.Type == "Username");
            if (claim == null) return null;
            var username=claim.Value;
            if (username == null) return null;
            return await userManager.Users.Include(e=>e.UserGroups).Include(e=>e.UserNotifications).FirstOrDefaultAsync(e=>e.UserName == username);
            

        }

        public  async Task<string> GenerateToken(string username)
        {
            var user=await userManager.FindByNameAsync(username);
            var claim = new List<Claim>();
            claim.Add(new Claim("Username", username));
            claim.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Iconfig["JWT:Secret"]));
            SigningCredentials signingCreden = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: Iconfig["JWT:issuer"],
                audience: Iconfig["JWT:audience"],
                claims: claim,
                signingCredentials: signingCreden



            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }


        public async Task<AppUser?> GetUserByEmailasync(string email)
        {
            return await GetSpecification().FirstOrDefaultAsync(x=>x.Email==email);

        }

        public async Task<AppUser?> GetUserByUsernameasync(string username)
        {
            return await GetSpecification().FirstOrDefaultAsync(x => x.UserName == username);
        }


        public async Task<IdentityResult> ConfirmEmailAsync(AppUser user, string token)
        {
            return await userManager.ConfirmEmailAsync(user, token);
        }


        public async Task<string> GeneratePasswordToken(AppUser user)
        {
            return await userManager.GeneratePasswordResetTokenAsync(user);
        }


        public async Task<IdentityResult> CreateUser(AppUser user, string password)
        {
             return await userManager.CreateAsync(user, password);

        }


        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }


        public async Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string password)
        {
            return await userManager.ResetPasswordAsync(user, token, password);

        }
    }
}
