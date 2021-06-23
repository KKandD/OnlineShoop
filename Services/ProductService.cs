using System;
using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Services
{
    public class ProductService
    {
        private readonly IProductDao productDao;
        private readonly IProductCategoryDao productCategoryDao;
        private readonly ISupplierDao supplierDao;

        public ProductService(IProductDao productDao, IProductCategoryDao productCategoryDao, ISupplierDao supplierDao)
        {
            this.productDao = productDao;
            this.productCategoryDao = productCategoryDao;
            this.supplierDao = supplierDao;
        }

        public ProductCategory GetProductCategory(int categoryId)
        {
            return this.productCategoryDao.Get(categoryId);
        }

        public IEnumerable<Product> GetProductsForCategory(int categoryId, int page = 0)
        {
            ProductCategory category = this.productCategoryDao.Get(categoryId);
            return this.productDao.GetBy(category, page);
        }

        public IEnumerable<Product> GetProductsForSupplier(int supplierId, int page = 0)
        {
            Supplier supplier = this.supplierDao.Get(supplierId);
            return this.productDao.GetBy(supplier, page);
        }

        public IEnumerable<Product> GetAllProducts(int page = 0)
        {
            return this.productDao.GetAll(page);
        }

        public IEnumerable<Product> GetProductsForPage(int page)
        {
            return this.productDao.GetForPage(page);
        }

        public IEnumerable<ProductCategory> GetAllCategories()
        {
            return this.productCategoryDao.GetAll();
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return this.supplierDao.GetAll();
        }

        public IEnumerable<Product> GetProductsForCategoryAndSupplier(int categoryId, int supplierId, int page)
        {
            ProductCategory category = this.productCategoryDao.Get(categoryId);
            return this.productDao.GetBy(category, page).Where(product => product.Supplier.Id==supplierId);
        }

    }
}
