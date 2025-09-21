using GestionProductos.Models;
using System.Data.Entity;
using Microsoft.Extensions.Logging;


namespace GestionProductos.Services;

public class ProductService : IProductService
{

    private readonly IDbContextFactory _dbContextFactory;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IDbContextFactory dbContextFactory, ILogger<ProductService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }


    public async Task<IEnumerable<Producto>> GetAllProductsAsync()
    {
        try
        {
            using var context = _dbContextFactory.Create();
            return await context.Productos.Include(p => p.Opciones).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los productos");
            throw new Exception("No se pudo obtener los productos en este momento.", ex);
        }
    }


    public async Task<IEnumerable<Producto>> SearchAsync(string? productName = null, bool? isActive = null)
    {
        try
        {
            using var context = _dbContextFactory.Create();
            var query = context.Productos
                               .Include(p => p.Opciones)
                               .AsNoTracking()
                               .AsQueryable();

            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(p => p.Nombre.Contains(productName));
            }
            if (isActive.HasValue)
            {
                query = query.Where(p => p.Estado == isActive.Value);
            }
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar productos. Criterios: name={Name}, isActive={IsActive}", productName, isActive);
            throw new Exception("No se pudo completar la búsqueda de productos.", ex);
        }
    }
}
