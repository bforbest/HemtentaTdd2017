using System;
using Hemtenta_Alisina_Housela.webshop;
using Xunit;
using Moq;

namespace Hemtenta_Alisina_Housela_Test
{
    public class WebshopTest
    {
        public class WebshopTests
        {
            private decimal totalCost;

            private Mock<IBilling> mockBilling;
            private WebShop webShop;
            private Basket basket;
            private Product product;

            public WebshopTests()
            {
                totalCost = 0;
                mockBilling = new Mock<IBilling>();
                basket = new Basket();
                webShop = new WebShop(basket);
                product = new Product { Name = "Car", Price = 300000 };
            }

            [Fact]
            public void InvalidValues_AddedToBasket_Throws_NotValidAmountOrProduct()
            {
                Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(product, int.MinValue));
                Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(product, int.MaxValue));
                Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(product, -1));
                Assert.Throws<NotValidAmountOrProductException>(() => basket.AddProduct(null, 1));
            }

            [Fact]
            public void NotValidPrice_Product_Throws()
            {
                var product = new Product { Price = decimal.MinusOne };

                Assert.Throws<NotValidPriceException>(() => basket.AddProduct(product, 2));
            }

            [Fact]
            public void SingleProduct_AddedToBasket()
            {
                basket.AddProduct(product, 1);

                totalCost = basket.TotalCost;

                Assert.Equal(product.Price * 1, totalCost);
            }

            [Fact]
            public void MultipleProduct_Added_To_Basket()
            {
                decimal sum = 0;

                basket.AddProduct(product, 3);

                for (int i = 0; i < 3; i++)
                {
                    sum += product.Price;
                }

                totalCost = basket.TotalCost;

                Assert.Equal(sum, totalCost);
            }

            [Fact]
            public void RemoveInvalidProductsFromBasket_Throws_NotValidAmountOrProductException()
            {
                Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(product, int.MinValue));
                Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(product, int.MaxValue));
                Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(product, -1));
                Assert.Throws<NotValidAmountOrProductException>(() => basket.RemoveProduct(null, 1));
               
            }

            [Fact]
            public void RemoveSingleProductFromBasket()
            {
                decimal sum = 0;

                basket.AddProduct(product, 2);

                for (int i = 0; i < 2; i++)
                {
                    sum += product.Price;
                }

                basket.RemoveProduct(product, 1);

                for (int i = 0; i < 1; i++)
                {
                    sum -= product.Price;
                }

                totalCost = basket.TotalCost;

                Assert.Equal(sum, totalCost);
            }

            [Fact]
            public void RemoveMultipleProductFromBasket()
            {
                decimal sum = 0;

                basket.AddProduct(product, 4);

                for (int i = 0; i < 4; i++)
                {
                    sum += product.Price;
                }

                basket.RemoveProduct(product, 3);

                for (int i = 0; i < 3; i++)
                {
                    sum -= product.Price;
                }

                totalCost = basket.TotalCost;

                Assert.Equal(sum, totalCost);
            }

            [Fact]
            public void NullValuePassed_ToWebShop_Checkout_Throws()
            {
                Assert.Throws<BillingIsNullException>(() => webShop.Checkout(null));
            }

            [Fact]
            public void Checkout_Success()
            {
                basket.AddProduct(product, 3);
                totalCost = basket.TotalCost;

                mockBilling.Setup(b => b.Balance).Returns(totalCost);

                webShop.Checkout(mockBilling.Object);

                Assert.Equal(true, mockBilling.Object.Balance == totalCost);
                mockBilling.Verify((IBilling x) => x.Pay(totalCost), Times.Exactly(1));

            }
        }
    }
}
