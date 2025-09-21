using GestionProductos.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GestionProductos.Services;

public class OptionService : IOptionService
{
    private readonly IDbContextFactory _dbContextFactory;
    private readonly ILogger<IOptionService> _logger;

    public OptionService(IDbContextFactory dbContextFactory, ILogger<IOptionService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task<Opcion> AddOptionAsync(Opcion newOption)
    {
        if (newOption == null)
        {
            throw new ArgumentNullException(nameof(newOption));
        }

        try
        {
            using var context = _dbContextFactory.Create();
            context.Opciones.Add(newOption);
            await context.SaveChangesAsync();
            _logger.LogInformation("Nueva opción '{OptionName}' agregada al producto {ProductId}", newOption.Nombre, newOption.CodigoProducto);
            return newOption;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error de base de datos al agregar una nueva opción.");
            throw new InvalidOperationException("No fue posible crear la opción debido a un error en la base de datos.", ex);
        }
    }

    public async Task<Opcion> UpdateOptionAsync(Opcion optionToUpdate)
    {
        if (optionToUpdate == null)
        {
            throw new ArgumentNullException(nameof(optionToUpdate));
        }

        try
        {
            using var context = _dbContextFactory.Create();
            context.Opciones.Attach(optionToUpdate);
            context.Entry(optionToUpdate).State = EntityState.Modified;
            await context.SaveChangesAsync();
            _logger.LogInformation("Opción {OptionId} actualizada.", optionToUpdate.IdOpcion);
            return optionToUpdate;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Conflicto de concurrencia al actualizar la opción {OptionId}", optionToUpdate.IdOpcion);
            throw new InvalidOperationException("Los datos han sido modificados por otro usuario. Por favor, recarga y vuelve a intentarlo.", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error de base de datos al actualizar la opción {OptionId}", optionToUpdate.IdOpcion);
            throw new InvalidOperationException("No fue posible actualizar la opción debido a un error en la base de datos.", ex);
        }
    }

    public async Task DeleteOptionAsync(int optionId)
    {
        try
        {
            using var context = _dbContextFactory.Create();

            var optionToDelete = await context.Opciones.FindAsync(optionId);

            if (optionToDelete != null)
            {
                context.Opciones.Remove(optionToDelete);
                await context.SaveChangesAsync();
                _logger.LogInformation("Opción {OptionId} eliminada.", optionId);
            }
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error de base de datos al eliminar la opción {OptionId}", optionId);
            throw new InvalidOperationException("No fue posible eliminar la opción. Es posible que esté en uso.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al eliminar la opción {OptionId}", optionId);
            throw;
        }
    }
}
