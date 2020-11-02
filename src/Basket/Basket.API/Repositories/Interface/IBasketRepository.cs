using System;
using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositories.Interface
{
    public interface IBasketRepository
    {
        Task<BasketCart> GetBasketCart(string userName);
        Task<BasketCart> UpdateBasketCart(BasketCart basketCart);
        Task<bool> DeleteBasket(string username);
    }
}
