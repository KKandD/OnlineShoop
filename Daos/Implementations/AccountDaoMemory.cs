using System.Collections.Generic;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    public class AccountDaoMemory: IAccountDao
    {
        private List<SingUpUserModel> data = new List<SingUpUserModel>();
        private static AccountDaoMemory instance = null;

        private AccountDaoMemory()
        {

        }

        public static AccountDaoMemory GetInstance()
        {
            if (instance == null)
            {
                instance = new AccountDaoMemory();
            }

            return instance;
        }

        public void Add(SingUpUserModel user)
        {
            data.Add(user);
        }

        public void Remove(int id)
        {
            data.Remove(this.Get(id));
        }

        public SingUpUserModel Get(int id)
        {
            return data.Find(x => x.Id == id);
        }

        public IEnumerable<SingUpUserModel> GetAll()
        {
            return data;
        }
    }
}