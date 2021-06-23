using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Codecool.CodecoolShop.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using ServiceStack.Host;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;

namespace Codecool.CodecoolShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        public ProductService ProductService { get; set; }
        public BasketService BasketService { get; set; }

        public int itemsCount = 0;

        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
            ProductService = new ProductService(
                ProductDaoMemory.GetInstance(),
                ProductCategoryDaoMemory.GetInstance(),
                SupplierDaoMemory.GetInstance());
            BasketService = new BasketService(
                CartItemDaoMemory.GetInstance(),
                CartDaoMemory.GetInstance());
        }
        
        // GET: ShoppingController
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var product = ProductService.GetAllProducts().Where(x => x.Id == id).SingleOrDefault();

            if (product != null)
            {
                BasketService.AddToBasket(1, product);
                HttpContext.Session.SetInt32("UserItemsCount", itemsCount += 1);
            }

            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult RemoveFromCart(int id)
        {
            var cartId = 1;
            var items = BasketService.GetItemsForCart(1).ToList();

            foreach(CartItem cartItem in items)
            {
                if (cartItem.Id == id)
                {
                    if(cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                    }
                    else if (cartItem.Quantity == 1)
                    {
                        BasketService.RemoveFromCart(cartId, cartItem.Id);
                        items = BasketService.GetItemsForCart(cartId).ToList();
                    }
                }
            }

            CartViewModel cartViewModel = CreateView(cartId, items);

            return View("Basket", cartViewModel);
        }

        public CartViewModel CreateView(int cartId, List<CartItem> items)
        {
            return new CartViewModel
            {
                UserLogged = Request.Cookies.Any(c => c.Key == "UserLoginCookie"),
                CartId = cartId,
                CartItems = items
            };
        }

        [HttpGet]
        public IActionResult IncreaseQuantity(int id)
        {
            int cartId = 1;
            var items = BasketService.GetItemsForCart(cartId).ToList();
            foreach (CartItem cartItem in items)
            {
                if (cartItem.Id == id)
                {
                    cartItem.Quantity++;
                }
            }

            CartViewModel cartViewModel = CreateView(cartId, items);
            return View("Basket", cartViewModel);
        }

        [Authorize]
        public IActionResult Basket()
        {
            int cartId = 1;
            var items = BasketService.GetItemsForCart(cartId);

            CartViewModel cartViewModel = CreateView(cartId, items.ToList());

            return View(cartViewModel);
        }


        public ActionResult CartSummary()
        {
            ViewData["CartCount"] = 3; // count Qty in your cart
            return PartialView("CartSummary");
        }

        public int UserItemsCount()
        {
            var userItemsCount = HttpContext.Session.GetInt32("UserItemsCount");
             return ViewBag.count = userItemsCount;
        }

        //public IActionResult UserItemsCoun()
        //{
        //    int cartId = 1;
        //    var items = BasketService.GetItemsForCart(cartId);
        //    CartViewModel cartViewModel = CreateView(cartId, items.ToList());
        //    var itemsCount = cartViewModel.CartItems.Count();
        //    HttpContext.Items.Add("Key", itemsCount);
        //    return View();
        //}


        //public int UserItemsCount()
        //{
        //    int cartId = 1;
        //    var items = BasketService.GetItemsForCart(cartId);
        //    CartViewModel cartViewModel = CreateView(cartId, items.ToList());
        //    var itemsCount = cartViewModel.CartItems.Count();

        //    return itemsCount;
        //}



        /*
        // GET: ShoppingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShoppingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShoppingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShoppingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShoppingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShoppingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        
        // POST: ShoppingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        */
    }
}
