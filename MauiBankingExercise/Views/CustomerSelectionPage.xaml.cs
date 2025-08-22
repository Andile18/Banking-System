using MauiBankingExercise.ViewModels;

namespace MauiBankingExercise.Views;

public partial class CustomerSelectionPage : ContentPage
{
    public CustomerSelectionPage()
    {
        InitializeComponent();
        BindingContext = new CustomerSelectionViewModel(); // attach VM
    }
}
