using System;
using Xunit;
using Hemtenta_Alisina_Housela.bank;
namespace Hemtenta_Alisina_Housela_Test
{
    public class AccountTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        [InlineData(null)]
        public void AmountInvalidParameter_Throws_IllegalAmountException(double amount)
        {
            var account = new Account();
            Assert.Throws<IllegalAmountException>(() => account.Deposit(amount));
            Assert.Throws<IllegalAmountException>(() => account.TransferFunds(new Account(),amount));
            Assert.Throws<IllegalAmountException>(() => account.Withdraw(amount));

        }
        [Fact]
        public void DepositeSuccessful()
        {
            var account = new Account();
            Assert.Equal(0, account.Amount);
            account.Deposit(3.3);
            Assert.Equal(3.3, account.Amount);
        }
        [Fact]
        public void WithdrawSuccessful()
        {
            var account = new Account();
            Assert.Equal(0, account.Amount);
            account.Deposit(600);
            Assert.Equal(600, account.Amount);
            account.Withdraw(300);
            Assert.Equal(300, account.Amount);
        }
        [Fact]
        public void WithDrawInsufficientFunds_Throws_InsufficientFundsException()
        {
            var account = new Account();
            Assert.Throws<InsufficientFundsException>(() => account.Withdraw(3));
        }
        [Fact]
        public void TransferInsufficientFunds_Throws_InsufficientFundsException()
        {
            var account = new Account();
            Assert.Throws<InsufficientFundsException>(() => account.TransferFunds(new Account(),3));
        }
        [Fact]
        public void TranfserFailer_Throws_OperationNotPermittedException()
        {
            var account = new Account();
            Assert.Throws<OperationNotPermittedException>(() => account.TransferFunds(null, 3));
        }
        [Fact]
        public void TransferSufficientFunds()
        {
            var account = new Account();
            account.Deposit(500);
            var account2 = new Account();
            account.TransferFunds(account2, 500);
            Assert.Equal(500, account2.Amount);
            Assert.Equal(0, account.Amount);
        }

    }
}
