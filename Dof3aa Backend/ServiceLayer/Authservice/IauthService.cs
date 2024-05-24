using Core.entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Authservice
{
    public interface IauthService
    {
        Task<AppUser> GetCurentUser();
        Task<string> GenerateToken(string username);

        Task<AppUser> GetUserByEmailasync(string email);
        Task<AppUser> GetUserByUsernameasync(string username);

          Task<IdentityResult> ConfirmEmailAsync(AppUser user, string token);
            
        Task<string> GeneratePasswordToken(AppUser user); 

        Task<IdentityResult> CreateUser(AppUser user,string password);

        Task<bool> CheckPasswordAsync(AppUser user, string password);

        Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string password);
    }
}
