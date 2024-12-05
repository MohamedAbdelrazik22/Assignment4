using System;
namespace Assignment4
{
    // Base Account Class
    public class Account
    {
        public string Name { get; set; }
        public double Balance { get; set; }

        public Account(string name = "Unnamed Account", double balance = 0.0)
        {
            Name = name;
            Balance = balance;
        }

        public virtual bool Deposit(double amount)
        {
            if (amount > 0)
            {
                Balance += amount;
                return true;
            }
            return false;
        }

        public virtual bool Withdraw(double amount)
        {
            if (Balance - amount >= 0)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public virtual string ToString()
        {
            return $"{Name}: Balance = {Balance:C}";
        }

        public static double operator +(Account acc1, Account acc2)
        {
            return acc1.Balance + acc2.Balance;
        }
    }

    // SavingsAccount Class
    public class SavingsAccount : Account
    {
        public double InterestRate { get; set; }

        public SavingsAccount(string name = "Unnamed Savings Account", double balance = 0.0, double interestRate = 0.0)
            : base(name, balance)
        {
            InterestRate = interestRate;
        }

        public override bool Deposit(double amount)
        {
            if (base.Deposit(amount))
            {
                Balance += (amount * InterestRate / 100);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Interest Rate = {InterestRate}%";
        }
    }

    // CheckingAccount Class
    public class CheckingAccount : Account
    {
        private const double WithdrawalFee = 1.50;

        public CheckingAccount(string name = "Unnamed Checking Account", double balance = 0.0)
            : base(name, balance)
        {
        }

        public override bool Withdraw(double amount)
        {
            if (base.Withdraw(amount + WithdrawalFee))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Withdrawal Fee = {WithdrawalFee:C}";
        }
    }

    // TrustAccount Class
    public class TrustAccount : SavingsAccount
    {
        private const double BonusThreshold = 5000.0;
        private const double BonusAmount = 50.0;
        private const int MaxWithdrawals = 3;
        private int WithdrawalCount = 0;
        private DateTime LastWithdrawalDate { get; set; }

        public TrustAccount(string name = "Unnamed Trust Account", double balance = 0.0, double interestRate = 0.0)
            : base(name, balance, interestRate)
        {
            LastWithdrawalDate = DateTime.MinValue;
        }

        public override bool Deposit(double amount)
        {
            if (amount >= BonusThreshold)
            {
                Balance += BonusAmount;
            }
            return base.Deposit(amount);
        }

        public override bool Withdraw(double amount)
        {
            if (LastWithdrawalDate.Year < DateTime.Now.Year)
            {
                WithdrawalCount = 0;
                LastWithdrawalDate = DateTime.Now;
            }
            if (WithdrawalCount >= MaxWithdrawals || amount > Balance * 0.2)
            {
                return false;
            }
            if (base.Withdraw(amount))
            {
                WithdrawalCount++;
                return true;
            }
           

            return false;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Withdrawals this year = {WithdrawalCount}/{MaxWithdrawals}";
        }
    }

    // AccountUtil Class (Unchanged)
    public static class AccountUtil
    {
        // Utility helper functions for Account class
        public static void Display(List<Account> accounts)
        {
            Console.WriteLine("\n=== Accounts ==================================");
            foreach (var acc in accounts)
            {
                Console.WriteLine(acc);
            }
        }

        public static void Deposit(List<Account> accounts, double amount)
        {
            Console.WriteLine("\n=== Depositing to Accounts ====================");
            foreach (var acc in accounts)
            {
                if (acc.Deposit(amount))
                    Console.WriteLine($"Deposited {amount:C} to {acc}");
                else
                    Console.WriteLine($"Failed Deposit of {amount:C} to {acc}");
            }
        }

        public static void Withdraw(List<Account> accounts, double amount)
        {
            Console.WriteLine("\n=== Withdrawing from Accounts =================");
            foreach (var acc in accounts)
            {
                if (acc.Withdraw(amount))
                    Console.WriteLine($"Withdrew {amount:C} from {acc}");
                else
                    Console.WriteLine($"Failed Withdrawal of {amount:C} from {acc}");
            }
        }
    }

    // Main Program
    public class Program
    {
        static void Main()
        {
            // Create accounts
            var accounts = new List<Account>
        {
            new Account("Account1", 1000),
            new Account("Account2", 2000)
        };

            // Overloading +
            Console.WriteLine($"Combined Balance: {accounts[0] + accounts[1]:C}");

            // Other account types
            var savings = new SavingsAccount("Savings1", 5000, 3.0);
            var checking = new CheckingAccount("Checking1", 1500);
            var trust = new TrustAccount("Trust1", 10000, 5.0);

            var accountList = new List<Account> { savings, checking, trust };
            AccountUtil.Display(accountList);

            AccountUtil.Deposit(accountList, 6000);
            AccountUtil.Withdraw(accountList, 2000);

        }
    }

}
