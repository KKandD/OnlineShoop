using Codecool.CodecoolShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    public class CheckoutDaoMemory : ICheckoutDao
    {
        private List<Checkout> data = new List<Checkout>();
        private static CheckoutDaoMemory instance = null;
        public static CheckoutDaoMemory GetInstance()
        {
            if (instance == null)
            {
                instance = new CheckoutDaoMemory();
            }

            return instance;
        }

        public void Add(Checkout item)
        {
            item.Id = data.Count + 1;
            data.Add(item);
        }


        public Checkout Get(int id)
        {
            return data.Find(x => x.Id == id);
        }

        public IEnumerable<Checkout> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
