using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemtenta_Alisina_Housela.bank
{
    public class Account : IAccount
    {
        private double amount;
        public double Amount
        {
            get
            {
                return amount;
            }
        }

        public void Deposit(double amount)
        {
            if (double.IsNaN(amount) || double.IsInfinity(amount) || amount < 1)
            {
                throw new IllegalAmountException();
            }
            this.amount += amount;
        }

        public void TransferFunds(IAccount destination, double amount)
        {
            if (destination == null)
            {
                throw new OperationNotPermittedException();
            }
            if(double.IsNaN(amount) || double.IsInfinity(amount) || amount < 1)
            {
                throw new IllegalAmountException();
            }
            Withdraw(amount);
            destination.Deposit(amount);
        }

        public void Withdraw(double amount)
        {
            if (double.IsNaN(amount) || double.IsInfinity(amount) || amount < 1)
            {
                throw new IllegalAmountException();
            }

            if (amount > this.amount)
            {
                throw new InsufficientFundsException();
            }

            this.amount -= amount;
        }
    }
    public interface IAccount
    {
        // behöver inte testas
        double Amount { get; }

        // Sätter in ett belopp på kontot
        void Deposit(double amount);

        // Gör ett uttag från kontot
        void Withdraw(double amount);

        // Överför ett belopp från ett konto till ett annat
        void TransferFunds(IAccount destination, double amount);
    }

    // Kastas när beloppet på kontot inte tillåter
    // ett uttag eller en överföring
    public class InsufficientFundsException : Exception { }

    // Kastas för ogiltiga siffror
    public class IllegalAmountException : Exception { }

    // Kastas om en operation på kontot inte tillåts av någon
    // anledning som inte de andra exceptions täcker in
    public class OperationNotPermittedException : Exception { }
}
