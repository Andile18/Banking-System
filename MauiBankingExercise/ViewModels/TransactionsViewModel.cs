using MauiBankingExercise.Models;
using MauiBankingExercise.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiBankingExercise.ViewModels
{
    public class TransactionViewModel : BaseViewModel
    {
        private readonly BankingDatabaseService _service;
        private Account _selectedAccount;
        private string _selectedTransactionType;
        private string _transactionAmount;

        public ObservableCollection<string> TransactionTypes { get; } = new() { "Deposit", "Withdrawal" };

        public ICommand SubmitTransactionCommand { get; }

        public Account SelectedAccount
        {
            get => _selectedAccount;
            set { _selectedAccount = value; OnPropertyChanged(); }
        }

        public string SelectedTransactionType
        {
            get => _selectedTransactionType;
            set { _selectedTransactionType = value; OnPropertyChanged(); }
        }

        public string TransactionAmount
        {
            get => _transactionAmount;
            set { _transactionAmount = value; OnPropertyChanged(); }
        }

        public TransactionViewModel()
        {
            _service = new BankingDatabaseService();
            SubmitTransactionCommand = new Command(async () => await SubmitTransaction(), CanSubmitTransaction);
        }

        private async Task SubmitTransaction()
        {
            if (!CanSubmitTransaction()) return;

            if (!decimal.TryParse(TransactionAmount, out var amount) || amount <= 0)
            {
                await Shell.Current.DisplayAlert("Error", "Enter a valid amount > 0", "OK");
                return;
            }

            string typeLabel = SelectedTransactionType;
            int typeId = typeLabel == "Deposit" ? 1 : 2;

            try
            {
                _service.AddTransaction(SelectedAccount.AccountId, amount, typeId);
                TransactionAmount = "";
                SelectedTransactionType = null;
                await Shell.Current.DisplayAlert("Success", $"{typeLabel}: {amount:C} completed!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private bool CanSubmitTransaction() =>
            SelectedAccount != null &&
            !string.IsNullOrWhiteSpace(SelectedTransactionType) &&
            !string.IsNullOrWhiteSpace(TransactionAmount);
    }
}

