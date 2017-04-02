using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemtenta_Alisina_Housela.webshop
{
    public class Basket : IBasket
    {
        public decimal TotalCost { get; set; }


        public void AddProduct(Product product, int amount)
        {
            if (product == null || amount < 0 || amount == int.MaxValue)
            {
                throw new NotValidAmountOrProductException();
            }
            if (product.Price == decimal.MinusOne)
            {
                throw new NotValidPriceException();
            }
            while (amount > 0)
            {
                TotalCost += product.Price;
                amount--;
            }
        }

        public void RemoveProduct(Product product, int amount)
        {
            if (product == null || amount < 0 || amount == int.MaxValue)
            {
                throw new NotValidAmountOrProductException();
            }
            while (amount > 0)
            {
                TotalCost -= product.Price;
                amount--;
            }
        }
    }
}
