using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codecool.CodecoolShop.Services
{
    public class CheckoutService
    {
        private readonly ICheckoutDao checkoutDao;

        public CheckoutService(ICheckoutDao checkoutDao)
        {
            this.checkoutDao = checkoutDao;
        }
        public void Add(Checkout checkout)
        {  
            checkoutDao.Add(checkout);
        }
        public Checkout Get(int id)
        {
            return this.checkoutDao.Get(id);
        }
    }
}
