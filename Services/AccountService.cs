using Codecool.CodecoolShop.Daos.Implementations;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;

namespace Codecool.CodecoolShop.Services
{
    public class AccountService
    {
        private readonly IAccountDao accountDao;
        private readonly IPasswordHasher<SingUpUserModel> passwordHasher;

        public AccountService(IAccountDao accountDao, IPasswordHasher<SingUpUserModel> passwordHasher)
        {
            this.accountDao = accountDao;
            this.passwordHasher = passwordHasher;

        }

        public void RegisterUser(SingUpUserModel singUpUser)
        {
            var newUser = new SingUpUserModel()
            {
                Email = singUpUser.Email,
                Password = singUpUser.Password
            };

            var hashedPassword = this.passwordHasher.HashPassword(newUser, singUpUser.Password);
            newUser.Password = hashedPassword;

            accountDao.Add(newUser);
        }

        public bool LoginUser(SingInUserModel userModel)
        {

            var user = accountDao.GetAll().Where(x => x.Email.Equals(userModel.Email)).SingleOrDefault();
            if (user != null)
            {
                var result = passwordHasher.VerifyHashedPassword(user, user.Password, userModel.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

