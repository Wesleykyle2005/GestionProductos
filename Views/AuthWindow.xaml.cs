using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using GestionProductos.Common;
using GestionProductos.ViewModels;
using System.Windows;

namespace GestionProductos.Views;

/// <summary>
/// Lógica de interacción para AuthWindow.xaml
/// </summary>
public partial class AuthWindow : Window, IRecipient<LoginSuccessMessage>
{
    public AuthWindow()
    {
        InitializeComponent();
        DataContext = Ioc.Default.GetService<AuthViewModel>();
        WeakReferenceMessenger.Default.Register<LoginSuccessMessage>(this);
    }

    public void Receive(LoginSuccessMessage message)
    {
        Dispatcher.Invoke(() =>
        {
            var productWindow = new ProductWindow();
            productWindow.Show();
            this.Close();
        });
    }
}