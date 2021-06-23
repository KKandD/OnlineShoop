using Codecool.CodecoolShop.Daos.Implementations;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Services
{
    public class PaymentService
    {
        private readonly IPaymentDao paymentDao;


        public PaymentService(IPaymentDao paymentDao)
        {
            this.paymentDao = paymentDao;

        }

        public void Add(Payment payment)
        {

            this.paymentDao.Add(payment);
        }

        public Payment GetPayment(int id)
        {
            return this.paymentDao.Get(id);
        }
    }
}

