using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hemtenta_Alisina_Housela.webshop
{
    public class WebShop : IWebshop
    {
        private IBasket _basket;
        private IBilling _billing;
        public WebShop(IBasket basket)
        {
            _basket = basket;
        }
        public IBasket Basket
        {
            get
            {
                return _basket;
            }
        }

        public void Checkout(IBilling billing)
        {
            if (billing == null)
            {
                throw new BillingIsNullException();
            }
            _billing = billing;

            if (_billing.Balance == Basket.TotalCost)
            {
                _billing.Pay(_billing.Balance);
            }
        }
    }

    [Serializable]
    public class BillingIsNullException : Exception
    {
        public BillingIsNullException()
        {
        }

        public BillingIsNullException(string message) : base(message)
        {
        }

        public BillingIsNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BillingIsNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public interface IWebshop
    {
        IBasket Basket { get; }

        void Checkout(IBilling billing);
    }
    public interface IBasket
    {
        void AddProduct(Product p, int amount);
        void RemoveProduct(Product p, int amount);
        decimal TotalCost { get; }
       
    }

    // Mocka
    public interface IBilling
    {
        decimal Balance { get; set; }
        void Pay(decimal amount);
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
    public class NotValidAmountOrProductException : Exception { }
    public class NotValidPriceException : Exception { }
}
