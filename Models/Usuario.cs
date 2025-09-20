using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProductos.Models;

[Table("Usuarios")]
public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe estar entre 3 y 100 caracteres")]
    public required string NombreUsuario { get; set; }

    [StringLength(100, MinimumLength = 3, ErrorMessage = "El apellido debe estar entre 3 y 100 caracteres")]
    public string? Apellido { get; set; }

    [Required]
    public required byte[] ContrasenaHash { get; set; }

    [Required]
    [StringLength(100)]
    public required string Correo { get; set; }
    [Required]
    public required string Telefono { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}
