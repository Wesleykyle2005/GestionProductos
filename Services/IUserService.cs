using GestionProductos.Models;

namespace GestionProductos.Services
{
    public interface IUserService
    {
        Task<Usuario?> LoginAsync(string correo, string password);
        Usuario Register(string nombreUsuario, string? apellido, string correo, string telefono, string password);
        bool IsEmailTaken(string correo);
        bool IsUsernameTaken(string nombreUsuario);
        bool IsPhoneTaken(string telefono);
    }
}