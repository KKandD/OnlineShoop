using System;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.Implementations;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Blazored.LocalStorage;


namespace Codecool.CodecoolShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuthentication")
               .AddCookie("CookieAuthentication", config =>
               {
                   config.Cookie.Name = "UserLoginCookie";
                   config.LoginPath = "/Account/SignIn";
               });

            services.AddDistributedMemoryCache();
            services.AddBlazoredLocalStorage();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            services.AddScoped<IPasswordHasher<SingUpUserModel>, PasswordHasher<SingUpUserModel>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Product/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Cart}/{action=Basket}/{id?}");

                endpoints.MapControllerRoute(
                    name: "order",
                    pattern: "{controller=Checkout}/{action=Index}/{id?}");
            });

            SetupInMemoryDatabases();
        }

        private void SetupInMemoryDatabases()
        {
            IProductDao productDataStore = ProductDaoMemory.GetInstance();
            IProductCategoryDao productCategoryDataStore = ProductCategoryDaoMemory.GetInstance();
            ISupplierDao supplierDataStore = SupplierDaoMemory.GetInstance();
            ICartDao cartDataStore = CartDaoMemory.GetInstance();
            ICartItemDao cartItemDataStore = CartItemDaoMemory.GetInstance();
            IAccountDao accountDao = AccountDaoMemory.GetInstance();
            PasswordHasher<SingUpUserModel> passwordHasher = new PasswordHasher<SingUpUserModel>();
            AccountService accountService = new AccountService(accountDao, passwordHasher);
            accountService.RegisterUser(new SingUpUserModel { ConfirmPassword = "test", Email = "test@codecool.pl", Id = 1, Name = "Test", Password = "test" });

            Supplier amazon = new Supplier{Name = "Amazon", Description = "Digital content and services"};
            supplierDataStore.Add(amazon);
            Supplier lenovo = new Supplier{Name = "Lenovo", Description = "Computers"};
            supplierDataStore.Add(lenovo);
            Supplier brother = new Supplier { Name = "Brother", Description = "Printers" };
            supplierDataStore.Add(brother);
            Supplier canon = new Supplier { Name = "Canon", Description = "Printers" };
            supplierDataStore.Add(canon);
            Supplier polaroid = new Supplier { Name = "Polaroid", Description = "Printers" };
            supplierDataStore.Add(polaroid);
            ProductCategory tablet = new ProductCategory {Name = "Tablet", Department = "Hardware", Description = "A tablet computer, commonly shortened to tablet, is a thin, flat mobile computer with a touchscreen display." };
            productCategoryDataStore.Add(tablet);
            ProductCategory printer = new ProductCategory { Name = "Printer", Department = "Hardware", Description = "A device transfering text or graphic information to paper" };
            productCategoryDataStore.Add(printer);
            ProductCategory monitor = new ProductCategory { Name = "Monitor", Department = "Hardware", Description = "Output device that displays information in pictorial form" };
            productCategoryDataStore.Add(monitor);
            productDataStore.Add(new Product { Name = "Amazon Fire", DefaultPrice = 49.9m, Currency = "USD", Description = "Fantastic price. Large content ecosystem. Good parental controls. Helpful technical support.", ProductCategory = tablet, Supplier = amazon });
            productDataStore.Add(new Product { Name = "Lenovo IdeaPad Miix 700", DefaultPrice = 479.0m, Currency = "USD", Description = "Keyboard cover is included. Fanless Core m5 processor. Full-size USB ports. Adjustable kickstand.", ProductCategory = tablet, Supplier = lenovo });
            productDataStore.Add(new Product { Name = "Amazon Fire HD 8", DefaultPrice = 89.0m, Currency = "USD", Description = "Amazon's latest Fire HD 8 tablet is a great value for media consumption.", ProductCategory = tablet, Supplier = amazon });
            productDataStore.Add(new Product { Name = "Brother J315W", DefaultPrice = 59.0m, Currency = "USD", Description = "A4 colour multifunction inkjet printer", ProductCategory = printer, Supplier = brother });
            productDataStore.Add(new Product { Name = "Canon i-Sensys LBP113W", DefaultPrice = 109.0m, Currency = "USD", Description = "Canon i-SENSYS LBP113w Mono Laser Printer, A compact, high-speed, Wi-Fi laser printer ideal for home or small offices", ProductCategory = printer, Supplier = canon });
            productDataStore.Add(new Product { Name = "Polaroid playsmart 3D", DefaultPrice = 199.0m, Currency = "USD", Description = "Multiple ways to send your model to print via SD Card, USB, Mobile App (Wi-Fi)", ProductCategory = printer, Supplier = polaroid });
            productDataStore.Add(new Product { Name = "Lenovo T32p-20", DefaultPrice = 585.0m, Currency = "USD", Description = "31.5inch UHD(3840 x 2160) 3 - side Near - edgeless display. USB Type-C one cable solution. HDMI, DP ports with USB hub", ProductCategory = monitor, Supplier = lenovo });
            Cart cart = new Cart();
            cartDataStore.Add(cart);
        }
    }
}
