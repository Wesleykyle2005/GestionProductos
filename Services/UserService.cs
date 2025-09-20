using GestionProductos.Models;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GestionProductos.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ILogger<UserService> _logger;
        public UserService(IDbContextFactory dbContextFactory, ILogger<UserService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public bool IsEmailTaken(string correo)
        {
            using var context = _dbContextFactory.Create();
            return context.Usuarios.Any(u => u.Correo == correo);
        }

        public bool IsUsernameTaken(string nombreUsuario)
        {
            using var context = _dbContextFactory.Create();
            return context.Usuarios.Any(u => u.NombreUsuario == nombreUsuario);
        }
        public bool IsPhoneTaken(string telefono)
        {
            using var context = _dbContextFactory.Create();
            return context.Usuarios.Any(u => u.Telefono == telefono);
        }

        public Usuario Register(string nombreUsuario, string? apellido, string correo, string telefono, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario)) throw new ValidationException("El nombre de usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(correo)) throw new ValidationException("El correo es obligatorio.");
            if (string.IsNullOrWhiteSpace(telefono)) throw new ValidationException("El teléfono es obligatorio.");
            if (string.IsNullOrWhiteSpace(password)) throw new ValidationException("La contraseña es obligatoria.");

            using var context = _dbContextFactory.Create();

            if (IsEmailTaken(correo))
                throw new InvalidOperationException("El correo ya está registrado.");
            if (IsUsernameTaken(nombreUsuario))
                throw new InvalidOperationException("El nombre de usuario ya está registrado.");
            if (IsPhoneTaken(telefono))
                throw new InvalidOperationException("El teléfono ya está registrado.");

            var hashString = BCrypt.Net.BCrypt.HashPassword(password);
            var hashBytes = Encoding.UTF8.GetBytes(hashString);

            var usuario = new Usuario
            {
                NombreUsuario = nombreUsuario,
                Apellido = string.IsNullOrWhiteSpace(apellido) ? null : apellido,
                Correo = correo,
                Telefono = telefono,
                ContrasenaHash = hashBytes,
                FechaCreacion = DateTime.Now
            };

            context.Usuarios.Add(usuario);

            try
            {
                context.SaveChanges();
                _logger.LogInformation("Usuario registrado correctamente: {Correo}", correo);
                return usuario;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                    .SelectMany(v => v.ValidationErrors)
                    .ToList();

                var detailsForLog = string.Join("; ", errors.Select(v => $"{v.PropertyName}: {v.ErrorMessage}"));
                _logger.LogWarning(ex, "Validación de entidad fallida en Register: {Details}", detailsForLog);

                var detailsForUser = string.Join(System.Environment.NewLine, errors.Select(v => v.ErrorMessage));
                throw new System.ComponentModel.DataAnnotations.ValidationException(
                    $"Datos inválidos.{System.Environment.NewLine}{detailsForUser}"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en Register");
                throw new Exception("No se pudo completar el registro. Inténtalo más tarde.");
            }
        }

        public Usuario? Login(string correo, string password)
        {
            try
            {
                using var ctx = _dbContextFactory.Create();
                var usuario = ctx.Usuarios.FirstOrDefault(u => u.Correo == correo);
                if (usuario == null)
                {
                    _logger.LogInformation("Intento de login con correo inexistente: {Correo}", correo);
                    return null;
                }

                var hashString = Encoding.UTF8.GetString(usuario.ContrasenaHash);
                var ok = BCrypt.Net.BCrypt.Verify(password, hashString);

                if (!ok)
                {
                    _logger.LogInformation("Intento de login con contraseña inválida para {Correo}", correo);
                    return null;
                }

                _logger.LogInformation("Login correcto para {Correo}", correo);
                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en Login para {Correo}", correo);
                return null;
            }
        }
    }
}