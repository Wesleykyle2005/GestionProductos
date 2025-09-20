namespace GestionProductos.Services;

public interface IDbContextFactory
{
    GestionProductosContext Create();
}

public class DbContextFactory : IDbContextFactory
{
    public GestionProductosContext Create()
    {
        return new GestionProductosContext();
    }
}
