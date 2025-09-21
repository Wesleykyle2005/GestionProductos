using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProductos.Models;

[Table("Productos")]
public class Producto
{
    [Key]
    public int Codigo { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del producto debe estar entre 3 y 100 caracteres")]
    public required string Nombre { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser igual o mayor que 0.")]
    public int Existencia { get; set; } = 0;
    public bool Estado { get; set; } = true;
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre del producto debe estar entre 3 y 100 caracteres")]
    public string? NombreProveedor { get; set; }
    public virtual ObservableCollection<Opcion> Opciones { get; set; } = new();
}
