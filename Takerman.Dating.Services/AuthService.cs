using Microsoft.AspNetCore.Identity;
using Takerman.Dating.Data;
using Takerman.Dating.Models.Enum;
using Takerman.Dating.Services.Abstraction;
using Takerman.Dating.Services.Authentication;
using Microsoft.OpenApi.Extensions;


namespace Takerman.Dating.Services
{
    public class AuthService : IAuthService
    {
        public async Task<User> AddUserFromSocial(UserManager<User> userManager, DefaultContext context, CreateUserFromSocialLogin model, LoginProvider loginProvider)
        {
            //CHECKS IF THE USER HAS NOT ALREADY BEEN LINKED TO AN IDENTITY PROVIDER
            var user = await userManager.FindByLoginAsync(loginProvider.GetDisplayName(), model.LoginProviderSubject);

            if (user is not null)
                return user; //USER ALREADY EXISTS.

            user = await userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Avatar = model.ProfilePicture,
                    Password = string.Empty,
                    Gender = Gender.Male
                };

                await userManager.CreateAsync(user);

                //EMAIL IS CONFIRMED; IT IS COMING FROM AN IDENTITY PROVIDER
                user.IsActive = true;

                await userManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            }

            UserLoginInfo userLoginInfo = null;
            switch (loginProvider)
            {
                case LoginProvider.Google:
                    {
                        userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
                    }
                    break;
                case LoginProvider.Facebook:
                    {
                        userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
                    }
                    break;
                default:
                    break;
            }

            //ADDS THE USER TO AN IDENTITY PROVIDER
            var result = await userManager.AddLoginAsync(user, userLoginInfo);

            if (result.Succeeded)
                return user;

            else
                return null;
        }
    }
}