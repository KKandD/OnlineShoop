using Codecool.CodecoolShop.Models;
using System.Collections.Generic;

namespace Codecool.CodecoolShop.Daos
{
    public interface IProductDao : IDao<Product>
    {
        IEnumerable<Product> GetAll(int page);
        IEnumerable<Product> GetBy(Supplier supplier, int page);
        IEnumerable<Product> GetBy(ProductCategory productCategory, int page);
        IEnumerable<Product> GetForPage(int page);

    }
}
