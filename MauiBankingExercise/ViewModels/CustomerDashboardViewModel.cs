using MauiBankingExercise.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace MauiBankingExercise.ViewModels
{
    public class CustomerDashboardViewModel : BaseViewModel
    {
        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        public ObservableCollection<Transaction> Transactions { get; set; }

        public ICommand DepositCommand { get; }
        public ICommand WithdrawCommand { get; }

        public CustomerDashboardViewModel()
        {
            Balance = 15000; // Starting balance
            Transactions = new ObservableCollection<Transaction>();

            DepositCommand = new Command<decimal>(Deposit);
            WithdrawCommand = new Command<decimal>(Withdraw);
        }

        private void Deposit(decimal amount)
        {
            Balance += amount;
            Transactions.Insert(0, new Transaction
            {
                TransactionType = 
                Amount = amount,
                BalanceAfter = Balance,
                TransactionDate = DateTime.Now
            });
        }

        private void Withdraw(decimal amount)
        {
            if (amount <= Balance) // Prevent overdraft
            {
                Balance -= amount;
                Transactions.Insert(0, new Transaction
                {
                    TransactionType = "Withdrawal",
                    Amount = amount,
                    BalanceAfter = Balance,
                    TransactionDate = DateTime.Now
                });
            }
        }
    }
}
