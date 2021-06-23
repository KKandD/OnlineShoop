using System.Collections.Generic;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    class CartDaoMemory : ICartDao
    {
        private List<Cart> data = new List<Cart>();
        private static CartDaoMemory instance = null;

        private CartDaoMemory()
        {
        }

        public static CartDaoMemory GetInstance()
        {
            if (instance == null)
            {
                instance = new CartDaoMemory();
            }

            return instance;
        }

        public void Add(Cart item)
        {
            item.Id = data.Count + 1;
            data.Add(item);
        }

        public void Remove(int id)
        {
            data.Remove(this.Get(id));
        }

        public Cart Get(int id)
        {
            return data.Find(x => x.Id == id);
        }

        public IEnumerable<Cart> GetAll()
        {
            return data;
        }
    }
}
