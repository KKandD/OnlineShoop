using Codecool.CodecoolShop.Daos.Implementations;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.ViewModel;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codecool.CodecoolShop.Controllers
{
    public class CheckoutController : Controller
    {
        public BasketService BasketService { get; set; }
        public CheckoutService CheckoutService { get; set; }

        public CheckoutController()
        {
            BasketService = new BasketService(
                CartItemDaoMemory.GetInstance(),
                CartDaoMemory.GetInstance());
            CheckoutService = new CheckoutService(CheckoutDaoMemory.GetInstance());
        }

        public IActionResult Index(int id)
        {
            var cartItmes = BasketService.GetItemsForCart(id);
            ViewBag.ItemsCount = cartItmes.ToList().Count();
            ViewBag.Items = cartItmes.ToList();

            var basemodel = new BaseViewModel
            {
                UserLogged = Request.Cookies.Any(c => c.Key == "UserLoginCookie"),

            };

            var checkout = new Checkout
            {
                //UserLogged = Request.Cookies.Any(c => c.Key == "UserLoginCookie"),
            };

            return View("CheckoutWithPayment");
        }
        public IActionResult Checkout(Checkout checkout)
        {
            CheckoutService.Add(checkout);
            return RedirectToAction("Index", "Payment");
        }
    }
}
