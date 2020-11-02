using System;
using Basket.API.Data.Interface;
using StackExchange.Redis;

namespace Basket.API.Data
{
    public class BasketContext : IBasketContext
    {
        private readonly IConnectionMultiplexer _redisConnection;
        public IDatabase Redis { get; }

        public BasketContext(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            Redis = _redisConnection.GetDatabase();
        }
    }
}
