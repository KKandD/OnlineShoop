using System.Collections.Generic;
using Codecool.CodecoolShop.Models;
using System.Linq;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    class CartItemDaoMemory : ICartItemDao
    {
        private List<CartItem> data = new List<CartItem>();
        private static CartItemDaoMemory instance = null;

        private CartItemDaoMemory()
        {
        }

        public static CartItemDaoMemory GetInstance()
        {
            if (instance == null)
            {
                instance = new CartItemDaoMemory();
            }

            return instance;
        }

        public void Add(CartItem item)
        {
            if (data.Count == 0)
            {
                item.Id = data.Count + 1;
                data.Add(item);
            }
            else
            {
                if (data.Any(x => x.Product.Id == item.Product.Id))
                {
                    var result = data.First(x => x.Product.Id == item.Product.Id);
                    result.Quantity += 1;
                }
                else
                {
                    item.Id = data.Count + 1;
                    data.Add(item);
                }
            }
        }

        public void Remove(int id)
        {
            data.Remove(this.Get(id));
        }

        public CartItem Get(int id)
        {
            return data.Find(x => x.Id == id);
        }

        public IEnumerable<CartItem> GetAll()
        {
            return data;
        }

        public IEnumerable<CartItem> GetBy(Cart cart)
        {
            return data.Where(x => x.Cart.Id == cart.Id);
        }
        public decimal GetTotalAmount()
        {
            decimal total = 0;
            foreach (var element in data)
            {
                var itemTotal = (element.Product.DefaultPrice * element.Quantity);
                total += itemTotal;
            }
            return total;
        }
    }
}

