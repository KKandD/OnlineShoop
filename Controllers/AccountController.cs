using Codecool.CodecoolShop.Daos.Implementations;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Codecool.CodecoolShop.Models
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        public AccountService accountService { get; set; }

        public AccountController(ILogger<AccountController> logger, IPasswordHasher<SingUpUserModel> passwordHasher)
        {
            _logger = logger;
            accountService = new AccountService(
                AccountDaoMemory.GetInstance(),
                passwordHasher);
        }

        [Route("signup")]
        public IActionResult Signup()
        {
            return View();
        }

        [Route("signup")]
        [HttpPost]
        public IActionResult Signup(SingUpUserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.Clear();
                ModelState.AddModelError("", "Wrong! Try again");
                return View();
            }
            ModelState.Clear();
            accountService.RegisterUser(userModel);

            SingInUserModel user = new SingInUserModel();
            user.Email = userModel.Email;
            user.Password = userModel.Password;

            // redirect to signin
            return Signin(user);

            //return RedirectToAction("Index", "Product");
        }

        [Route("signin")]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost("signin")]
        public ActionResult Signin(SingInUserModel userModel)
        {
            var result = accountService.LoginUser(userModel);

            if (result)
            {
                var userClaims = new List<Claim>();
                userClaims.Add(new Claim(ClaimTypes.Name, userModel.Email));
                userClaims.Add(new Claim(ClaimTypes.Email, userModel.Email));

                var identity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { identity });
                HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("Index", "Product");
            }

            return View();
        }

        [HttpGet("signout")]
        public ActionResult Singout(SingInUserModel userModel)
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Product");
        }
    }
}