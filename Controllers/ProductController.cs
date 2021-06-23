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
using Microsoft.AspNetCore.Http;

namespace Codecool.CodecoolShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        public ProductService ProductService { get; set; }
        public BasketService BasketService { get; set; }

        public ProductController(ILogger<ProductController> logger)
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

        public IActionResult Index(int page = 1)
        {
            var categoryId = 0;
            if(Request.Cookies.Any(c=> c.Key == "categoryId"))
            {
                categoryId = HttpContext.Session.GetInt32("categoryId").HasValue ? HttpContext.Session.GetInt32("categoryId").Value : 0;
            }

            var supplierId = 0;
            if (Request.Cookies.Any(c => c.Key == "supplierId"))
            {
                supplierId = HttpContext.Session.GetInt32("supplierId").HasValue ? HttpContext.Session.GetInt32("supplierId").Value : 0;
            }

            bool userLogged = false;
            if (Request.Cookies.Any(c => c.Key == "UserLoginCookie"))
            {
                userLogged = true;
            }

            foreach (var c in Request.Cookies)
            {
                Console.WriteLine($"Cookie: {c.Key} \t {c.Value}");
            }

            var indexViewModel = FillIndexViewModel(categoryId, supplierId, userLogged, page);


            return View(indexViewModel);
        }

        [HttpPost]
        public IActionResult Index(int categoryId, int supplierId, bool userLogged, int page = 1)
        {
            HttpContext.Session.SetInt32("categoryId", categoryId);
            HttpContext.Session.SetInt32("supplierId", supplierId);

            var indexViewModel = FillIndexViewModel(categoryId, supplierId, userLogged, page);

            return View(indexViewModel);
        }

        private IndexViewModel FillIndexViewModel(int categoryId=0, int supplierId=0, bool userLogged = false, int page = 1)
        {
            int pageSize = 6;
            var indexViewModel = new IndexViewModel();
            var productsCount = 0;

            if (categoryId>0 && supplierId>0)
            {
                indexViewModel.Products = ProductService.GetProductsForCategoryAndSupplier(categoryId, supplierId, page).ToList();
                productsCount = ProductService.GetProductsForCategoryAndSupplier(categoryId, supplierId, 0).ToList().Count;
            }
            else if(categoryId>0)
            {
                indexViewModel.Products = ProductService.GetProductsForCategory(categoryId, page).ToList();
                productsCount = ProductService.GetProductsForCategory(categoryId, 0).ToList().Count;
            }
            else if (supplierId > 0)
            {
                indexViewModel.Products = ProductService.GetProductsForSupplier(supplierId, page).ToList();
                productsCount = ProductService.GetProductsForSupplier(supplierId, 0).ToList().Count;
            }
            else
            {
                indexViewModel.Products = ProductService.GetAllProducts(page).ToList();
                productsCount = ProductService.GetAllProducts(0).ToList().Count;
            }
            

            var selectList = new SelectList(ProductService.GetAllCategories().ToList(), "Id", "Name", categoryId);
            //int totalQuantity = ProductService.GetAllProducts().ToList().Count;
            
            indexViewModel.Categories = selectList;
            indexViewModel.Suppliers = new SelectList(ProductService.GetAllSuppliers().ToList(), "Id", "Name", supplierId);
            indexViewModel.CurrentPage = page;
            //indexViewModel.PageSize = 6;
            indexViewModel.IsNextPageAvailable = page < GetNumberOfPages(productsCount, 6) ? true : false;
            indexViewModel.NextPage = page + 1;
            indexViewModel.IsPrevPageAvailable = page - 1 > 0;
            indexViewModel.PrevPage = page - 1;

            indexViewModel.CartItems = BasketService.GetItemsForCart(1).ToList();

            indexViewModel.UserLogged = userLogged;


            return indexViewModel;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddToCart(int id)
        {
            var product = ProductService.GetAllProducts().Where(x => x.Id == id).SingleOrDefault();

            if (product != null)
            {
                BasketService.AddToBasket(1, product);
            }

            return RedirectToAction("index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private int GetNumberOfPages(int count, int pageSize)
        {
            var numberOfPages = count / pageSize;
            if (count % pageSize == 0)
                return numberOfPages; 
            else 
                return numberOfPages + 1;
        }
    }
}
