using MauiBankingExercise.Models;
using MauiBankingExercise.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiBankingExercise.ViewModels
{
    public class ClientListViewModel : INotifyPropertyChanged
    {
        private readonly BankingDatabaseService _service;
        private bool _isLoading;

        public ObservableCollection<Customer> Clients { get; set; }
        public ICommand SelectCustomerCommand { get; set; }


        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public ClientListViewModel()
        {
            _service = new BankingDatabaseService();
            Clients = new ObservableCollection<Customer>();
            SelectCustomerCommand = new Command<SelectionChangedEventArgs>(OnClientSelected);
            LoadClients();
        }

        private async void LoadClients()
        {
            IsLoading = true;
            try
            {
                var clients = _service.GetAllCustomers();
                Clients.Clear();

                if (clients != null)
                {
                    foreach (var client in clients)
                        Clients.Add(client);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load clients: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private Customer? _selectedClient;

        public Customer? SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;

                OnPropertyChanged(nameof(SelectedClient));
            }
        }




        private async void OnClientSelected(object obj)
        {
            if (SelectedClient != null)
            {
                var param = new ShellNavigationQueryParameters()
            {
                { "customerId", SelectedClient.CustomerId }
            };

                await AppShell.Current.GoToAsync($"customerview", param);
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

