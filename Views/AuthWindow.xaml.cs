using CommunityToolkit.Mvvm.DependencyInjection;
using GestionProductos.ViewModels;
using System.Windows;

namespace GestionProductos.Views;

/// <summary>
/// Lógica de interacción para AuthWindow.xaml
/// </summary>
public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
        DataContext = Ioc.Default.GetService<AuthViewModel>();
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is AuthViewModel vm)
        {
            vm.LoginCommand.Execute(LoginPasswordBox.Password);
            if (vm.CurrentUser != null)
            {
                Close();
            }
        }
    }

    private void Register_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is AuthViewModel vm)
        {
            vm.RegisterCommand.Execute(SignupPasswordBox.Password);
        }
    }
}