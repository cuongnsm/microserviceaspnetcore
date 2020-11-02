using System;
using System.Threading.Tasks;
using Basket.API.Data.Interface;
using Basket.API.Entities;
using Basket.API.Repositories.Interface;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context;
        }

        public async Task<BasketCart> GetBasketCart(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;
            var basket = await _context.Redis.StringGetAsync(userName);

            if (basket.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasketCart(BasketCart basketCart)
        {
            if (basketCart == null && string.IsNullOrEmpty(basketCart.UserName)) return null;
            var isUpdated = await _context.Redis.StringSetAsync(basketCart.UserName,
                JsonConvert.SerializeObject(basketCart));
            if (!isUpdated)
            {
                return null;
            }
            return await GetBasketCart(basketCart.UserName);
        }

        public async Task<bool> DeleteBasket(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;

            return await _context.Redis.KeyDeleteAsync(username);
        }
    }
}
