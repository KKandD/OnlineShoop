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
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MimeKit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Codecool.CodecoolShop.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;
        public BasketService BasketService { get; set; }
        public PaymentService PaymentService { get; set; }
        public CheckoutService CheckoutService { get; set; }

        
        public IHostingEnvironment _env;



        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
            BasketService = new BasketService(
                CartItemDaoMemory.GetInstance(),
                CartDaoMemory.GetInstance());
            PaymentService = new PaymentService(
                PaymentDaoMemory.GetInstance());
            CheckoutService = new CheckoutService(
                CheckoutDaoMemory.GetInstance());
        }

        public IActionResult Index()
        {
            //int id = 1;
            //var cartItems = BasketService.GetItemsForCart(id).ToList();
            //CartViewModel cartViewModel = new CartViewModel { CartId = id, CartItems = cartItems };
            //Order order = new Order { cart = cartViewModel };
            decimal totalAmount = BasketService.GetTotalAmount();
            //Checkout checkout = CheckoutService.Get(id);
            Payment payment = new Payment { totalAmount = totalAmount};
            //PaymentService.Add(payment);
            return View("Payment", payment);
        }
        [HttpPost]
        public IActionResult Proceed(Payment payment)
        {
            PaymentService.Add(payment);
            SendEmail();
            return View("Order Confirmation");
        }

        public IActionResult Details()
        {
            int id = 1;
            Checkout checkout = CheckoutService.Get(id);
            decimal totalAmount = BasketService.GetTotalAmount();
            Payment payment = PaymentService.GetPayment(id);
            List<CartItem> items = BasketService.GetItemsForCart(id).ToList();
            OrderDetails orderDetails = new OrderDetails { Checkout = checkout, TotalAmount = totalAmount, Items = items , Payment = payment};
            return View("OrderDetails", orderDetails);
        }

        public void SendEmail()
        {
            var fromAddress = new MailAddress("email1", "Best Shoop EU");
            var toAddress = new MailAddress("email2", "To Name");
            const string fromPassword = "Password";
            const string subject = "Kozackie zamówienie";
            const string body = "Siemanko!\n" +
                "\nDzięki za zamówienie w naszym sklepie.\n" +
                "Gratulujemy dokonania najlepszego wyboru.\n" +
                "\nSzacowany czas dostawy: Niedługo";


            //var pathToFile = @"wwwroot\Templates\Email.cshtml";

            //var builder = new BodyBuilder();

            //using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            //{
            //    builder.HtmlBody = SourceReader.ReadToEnd();
            //}
            ////{0} : Subject  
            ////{1} : DateTime  
            ////{2} : Email  
            ////{3} : Username  
            ////{4} : Password  
            ////{5} : Message  
            ////{6} : callbackURL  

            //string messageBody = string.Format(builder.HtmlBody,
            //    subject,
            //    String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now)
            //    );

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                EnableSsl = true,
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }

}





