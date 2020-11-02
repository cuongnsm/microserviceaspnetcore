using System;
using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class BasketCart
    {
        public string UserName { get; set; }
        public List<BasketCartItem> Items { get; set; }

        public BasketCart(string userName)
        {
            UserName = userName;
        }

        public BasketCart()
        {

        }

        public decimal TotolPrice
        {
            get
            {
                return Items.Aggregate(0.00m, (acc, x) => acc + x.Price);
            }
        }
    }
}
