using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.ViewModel;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    public class ProductDaoMemory : IProductDao
    {
        private List<Product> data = new List<Product>();
        private static ProductDaoMemory instance = null;

        private ProductDaoMemory()
        {
        }

        public static ProductDaoMemory GetInstance()
        {
            if (instance == null)
            {
                instance = new ProductDaoMemory();
            }

            return instance;
        }

        public void Add(Product item)
        {
            item.Id = data.Count + 1;
            data.Add(item);
        }

        public void Remove(int id)
        {
            data.Remove(this.Get(id));
        }

        public Product Get(int id)
        {
            return data.Find(x => x.Id == id);
        }
        public IEnumerable<Product> GetAll()
        {
            return GetAll(0);
        }
        public IEnumerable<Product> GetAll(int page)
        {
            if (page == 0)
                return data;
            else
                return data.Skip((page - 1) * 6).Take(6);
        }

        public IEnumerable<Product> GetBy(Supplier supplier, int page)
        {
            if(page == 0)
                return data.Where(x => x.Supplier.Id == supplier.Id);
            else
                return data.Where(x => x.Supplier.Id == supplier.Id).Skip((page - 1) * 6).Take(6);
        }

        public IEnumerable<Product> GetBy(ProductCategory productCategory, int page)
        {
            if (page == 0)
                return data.Where(x => x.ProductCategory.Id == productCategory.Id);
            else
                return data.Where(x => x.ProductCategory.Id == productCategory.Id).Skip((page - 1) * 6).Take(6);
            
        }
        public IEnumerable<Product> GetForPage(int page)
        {
            return data.Skip((page - 1) * 6).Take(6);
        }
        public int GetNumberOfPages(int pageSize)
        {
            var numberOfPages = data.ToList().Count / pageSize;
            if (data.ToList().Count % pageSize == 0)
            { return numberOfPages; }
            else { return numberOfPages + 1; }
        }
    }
}
