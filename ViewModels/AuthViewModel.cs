using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GestionProductos.Common;
using GestionProductos.Models;
using GestionProductos.Services;
using Microsoft.Extensions.Logging;

namespace GestionProductos.ViewModels;

public class AuthViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthViewModel> _logger;
    public AuthViewModel(IUserService userService, ILogger<AuthViewModel> logger)
    {
        _userService = userService;
        _logger = logger;

        LoginCommand = new AsyncRelayCommand<string>(DoLoginAsync);
        RegisterCommand = new RelayCommand<string>(DoRegister);
        ToggleModeCommand = new RelayCommand(() => IsLoginMode = !IsLoginMode);
    }

    private bool _isLoginMode = true;
    public bool IsLoginMode
    {
        get => _isLoginMode;
        set => SetProperty(ref _isLoginMode, value);
    }

    public string NombreUsuario { get => _nombreUsuario; set => SetProperty(ref _nombreUsuario, value); }
    private string _nombreUsuario = string.Empty;
    public string Apellido { get => _apellido; set => SetProperty(ref _apellido, value); }
    private string _apellido = string.Empty;
    public string Correo { get => _correo; set => SetProperty(ref _correo, value); }
    private string _correo = string.Empty;
    public string Telefono { get => _telefono; set => SetProperty(ref _telefono, value); }
    private string _telefono = string.Empty;
    public string? Error { get => _error; set => SetProperty(ref _error, value); }
    private string? _error;
    public Usuario? CurrentUser { get => _currentUser; set => SetProperty(ref _currentUser, value); }
    private Usuario? _currentUser;

    public IAsyncRelayCommand<string> LoginCommand { get; }
    public IRelayCommand<string> RegisterCommand { get; }
    public IRelayCommand ToggleModeCommand { get; }

    private void DoRegister(string password)
    {
        Error = null;
        CurrentUser = null;

        try
        {
            var user = _userService.Register(NombreUsuario, string.IsNullOrWhiteSpace(Apellido) ? null : Apellido, Correo, Telefono, password);
            CurrentUser = user;
            IsLoginMode = true;
        }
        catch (System.ComponentModel.DataAnnotations.ValidationException vex)
        {
            Error = vex.Message;
        }
        catch (System.InvalidOperationException ioex)
        {
            Error = ioex.Message;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error en registro");
            Error = "No se pudo completar el registro. Inténtalo más tarde.";
        }
    }
    private async Task DoLoginAsync(string? password)
    {
        if (string.IsNullOrWhiteSpace(Correo) || string.IsNullOrWhiteSpace(password))
        {
            Error = "El correo y la contraseña son obligatorios.";
            return;
        }

        Error = null;

        try
        {
            var user = await _userService.LoginAsync(Correo, password);
            if (user == null)
            {
                Error = "Credenciales inválidas.";
                return;
            }
            CurrentUser = user;

            WeakReferenceMessenger.Default.Send(new LoginSuccessMessage(true));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en login");
            Error = "No se pudo iniciar sesión. Inténtalo más tarde.";
        }
    }
}
