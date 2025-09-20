using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProductos.Models;
[Table("Opciones")]
public class Opcion
{
    [Key]
    public int IdOpcion { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "El nombre de la opción debe tener entre 1 y 50 caracteres")]
    public required string Nombre { get; set; }

    [Required]
    [ForeignKey("Producto")]
    public int CodigoProducto { get; set; }

    public bool Estado { get; set; } = true;

    public Producto Producto { get; set; }
}
