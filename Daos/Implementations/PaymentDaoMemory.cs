using System.Collections.Generic;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    public class PaymentDaoMemory : IPaymentDao
    {
        private List<Payment> data = new List<Payment>();
        private static PaymentDaoMemory instance = null;

        private PaymentDaoMemory()
        {

        }

        public static PaymentDaoMemory GetInstance()
        {
            if (instance == null)
            {
                instance = new PaymentDaoMemory();
            }

            return instance;
        }

        public void Add(Payment item)
        {
            item.Id = data.Count + 1;
            data.Add(item);
        }

        public void Remove(int id)
        {
            data.Remove(this.Get(id));
        }

        public Payment Get(int id)
        {
            return data.Find(x => x.Id == id);
        }

        public IEnumerable<Payment> GetAll()
        {
            return data;
        }
    }
}