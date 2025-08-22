using MauiBankingExercise.ViewModels;

namespace MauiBankingExercise.Views;

public partial class CustomerDashboardPage : ContentPage
{
    int count = 0;
    public CustomerDashboardPage()
        {
            InitializeComponent();
            BindingContext = new CustomerDashboardViewModel();
        }
    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;
        
    }
}
