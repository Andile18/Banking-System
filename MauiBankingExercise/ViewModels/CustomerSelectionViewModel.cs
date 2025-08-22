using MauiBankingExercise.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiBankingExercise.Views;


namespace MauiBankingExercise.ViewModels
{
    public class CustomerSelectionViewModel : BaseViewModel
    {
        public ObservableCollection<Customer> Customers { get; }

        private Customer? _selectedCustomer;
        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (SetProperty(ref _selectedCustomer, value) && value != null)
                    OnCustomerSelected(value);
            }
        }

        public CustomerSelectionViewModel()
        {
            Customers = new ObservableCollection<Customer>
        {
            new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe",  Email="john.doe@email.com" },
            new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email="jane.smith@email.com" }
        };
        }

        private async void OnCustomerSelected(Customer customer)
        {
            await Shell.Current.GoToAsync(nameof(CustomerDashboardPage),
                new Dictionary<string, object> { { "Customer", customer } });
        }
    }
}
 