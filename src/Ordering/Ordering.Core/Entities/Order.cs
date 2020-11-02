using System;
using System.Globalization;
using System.Net.Mail;

namespace Ordering.Core.Entities
{
    public class Order : Entity
    {
        // Billing Infor
        public string UserName { get; set; }
        public string TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

    }

    public enum PaymentMethod
    {
        CreditCard = 1,
        DebitCard = 2,
        Paypal = 3
    }
}
