using System.Collections.Generic;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Services
{
    public class BasketService
    {
        private readonly ICartItemDao cartItemDao;
        private readonly ICartDao cartDao;

        public BasketService(ICartItemDao cartItemDao, ICartDao cartDao)
        {
            this.cartDao = cartDao;
            this.cartItemDao = cartItemDao;
        }

        public Cart GetCart(int cartId)
        {
            return this.cartDao.Get(cartId);
        }

        public IEnumerable<CartItem> GetItemsForCart(int cartId)
        {
            Cart cart = this.cartDao.Get(cartId);
            return this.cartItemDao.GetBy(cart);
        }

        public void AddToBasket(int cartId, Product product)
        {
            var cart = this.GetCart(cartId);

            var cartItem = new CartItem()
            {
                Product = product,
                Quantity = 1,
                Cart = cart

            };

            this.cartItemDao.Add(cartItem);
        }

        public void RemoveFromCart(int cartId, int cartItemId)
        {
            var cart = GetCart(cartId);
            cartItemDao.Remove(cartItemId);
        }

        public decimal GetTotalAmount()
        {
            //var cart = GetCart(cartId);
            //var items = this.GetItemsForCart(id);

            return cartItemDao.GetTotalAmount();
            
        }
    }
}
