﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Models;

namespace Veterinary.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task LogoutAsync();

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task<IdentityResult> ChangePasswordAsync(User user, string olpassword, string newpassword);

        //Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

       Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<User> GetUserByIdAsync(string userId);

       // Task<string> GeneratePasswordResetTokenAsync(User user);

        //Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    }
}
