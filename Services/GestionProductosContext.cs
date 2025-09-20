using GestionProductos.Models;
using System.Data.Entity;

namespace GestionProductos.Services;

public class GestionProductosContext : DbContext
{
    public GestionProductosContext() : base("name=GestionProductosContext")
    {
        Productos = Set<Producto>();
        Usuarios = Set<Usuario>();
        Opciones = Set<Opcion>();
    }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Opcion> Opciones { get; set; }
}
