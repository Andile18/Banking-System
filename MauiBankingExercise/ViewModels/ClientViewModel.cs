using MauiBankingExercise.Models;
using MauiBankingExercise.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiBankingExercise.ViewModels
{

    public class ClientViewModel: BaseViewModel
    {
        private readonly BankingDatabaseService _service;
        private Customer _customer;
        private Account _selectedAccount;
        private int _customerId;

        public ObservableCollection<Account> Accounts { get; } = new();
        public ObservableCollection<Transaction> RecentTransactions { get; } = new();

        public ICommand RefreshCommand { get; }
        public ICommand ViewTransactionsCommand { get; }

        public int CustomerId
        {
            get => _customerId;
            set
            {
                if (_customerId != value)
                {
                    _customerId = value;
                    OnPropertyChanged();
                    _ = LoadCustomerData();
                }
            }
        }

        public Customer Customer
        {
            get => _customer;
            set { _customer = value; OnPropertyChanged(); OnPropertyChanged(nameof(CustomerName)); }
        }

        public string CustomerName => Customer == null ? "Unknown" : $"{Customer.FirstName} {Customer.LastName}";

        public Account SelectedAccount
        {
            get => _selectedAccount;
            set { _selectedAccount = value; OnPropertyChanged(); _ = LoadAccountTransactions(); }
        }

        public ClientViewModel()
        {
            _service = new BankingDatabaseService();
            RefreshCommand = new Command(async () => await LoadCustomerData());
            ViewTransactionsCommand = new Command<Account>(async acc => await ViewAccountTransactions(acc));
        }

        public async Task LoadCustomerData()
        {
            if (CustomerId == 0) return;

            try
            {
                Customer = _service.GetCustomer(CustomerId);

                Accounts.Clear();
                var accounts = _service.GetCustomerAccounts(CustomerId);
                foreach (var acc in accounts)
                {
                    acc.AccountType ??= _service.GetAccountType(acc.AccountTypeId);
                    Accounts.Add(acc);
                }

                if (Accounts.Any())
                    SelectedAccount = Accounts.First();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load: {ex.Message}", "OK");
            }
        }

        private async Task LoadAccountTransactions()
        {
            if (SelectedAccount == null) return;

            try
            {
                RecentTransactions.Clear();
                var transactions = _service.GetAccountTransactions(SelectedAccount.AccountId);
                foreach (var tx in transactions)
                {
                    tx.TransactionType ??= _service.GetTransactionType(tx.TransactionTypeId);
                    RecentTransactions.Add(tx);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load transactions: {ex.Message}", "OK");
            }
        }

        private async Task ViewAccountTransactions(Account account)
        {
            if (account == null) return;

            try
            {
                var txs = _service.GetAccountTransactions(account.AccountId);
                var list = string.Join("\n", txs.Select(t =>
                    $"{t.TransactionDate:MM/dd/yyyy} - {_service.GetTransactionTypeName(t.TransactionTypeId)}: {t.Amount:C}"));

                await Shell.Current.DisplayAlert("Transactions",
                    $"Account: {account.AccountNumber}\nBalance: {account.AccountBalance:C}\n\n{list}", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
 }
