using Codecool.CodecoolShop.Models;
using System.Collections.Generic;

namespace Codecool.CodecoolShop.Daos
{
    public interface ICartItemDao : IDao<CartItem>
    {
        IEnumerable<CartItem> GetBy(Cart cart);
        decimal GetTotalAmount();
    }
}
