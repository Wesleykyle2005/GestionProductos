using GestionProductos.Models;

namespace GestionProductos.Services;

public interface IOptionService
{
    Task<Opcion> AddOptionAsync(Opcion newOption);
    Task<Opcion> UpdateOptionAsync(Opcion optionToUpdate);
    Task DeleteOptionAsync(int optionId);

}
