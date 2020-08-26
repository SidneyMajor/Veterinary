﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Models;

namespace Veterinary.Helpers
{
    public class UserHelper: IUserHelper
    {
        //Manager User
        private readonly UserManager<User> _userManager;
        //Manager Login
        private readonly SignInManager<User> _signInManager;
        //Manager role
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }



        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return await _userManager.CreateAsync(user);
            }
            return await _userManager.CreateAsync(user, password);
        }



        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> AddPasswordAsync(User user, string password)
        {
            return await _userManager.AddPasswordAsync(user, password);
        }


        public async Task<IdentityResult> ChangePasswordAsync(User user, string olpassword, string newpassword)
        {
            return await _userManager.ChangePasswordAsync(user, olpassword, newpassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
        
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }



        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);           
        }



        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }



        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                 model.Username,
                 model.Password,
                 model.RememberMe,
                 false);
        }



        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }



        //public async Task<IdentityResult> UpdateUserAsync(User user)
        //{
        //    return await _userManager.UpdateAsync(user);
        //}
    }
}
