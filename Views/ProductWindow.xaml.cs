using CommunityToolkit.Mvvm.DependencyInjection;
using GestionProductos.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GestionProductos.Views
{
    public partial class ProductWindow : Window
    {
        public ProductWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetService<ProductViewModel>();
        }
    }
}