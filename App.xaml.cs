using CommunityToolkit.Mvvm.DependencyInjection;
using GestionProductos.Services;
using GestionProductos.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace GestionProductos;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        services.AddLogging(b =>
        {
            b.ClearProviders();
            b.AddDebug();
            b.SetMinimumLevel(LogLevel.Information);
        });

        services.AddSingleton<IDbContextFactory, DbContextFactory>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<AuthViewModel>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ProductViewModel>();

        var provider = services.BuildServiceProvider();
        Ioc.Default.ConfigureServices(services.BuildServiceProvider());

        var logger = provider.GetRequiredService<ILogger<App>>();

        this.DispatcherUnhandledException += (s, args) =>
        {
            logger.LogError(args.Exception, "Excepción no controlada en el hilo de UI");
            MessageBox.Show("Ocurrió un error inesperado. Inténtalo más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };

        AppDomain.CurrentDomain.UnhandledException += (s, args2) =>
        {
            if (args2.ExceptionObject is Exception ex)
                logger.LogCritical(ex, "Excepción no controlada en AppDomain");
        };

        TaskScheduler.UnobservedTaskException += (s, args3) =>
        {
            logger.LogError(args3.Exception, "Excepción no observada en tarea");
            args3.SetObserved();
        };

    }
}
