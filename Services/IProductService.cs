using GestionProductos.Models;

namespace GestionProductos.Services;

public interface IProductService
{
    Task<IEnumerable<Producto>> GetAllProductsAsync();
    Task<IEnumerable<Producto>> SearchAsync(string? name = null, bool? isActive = null);
}