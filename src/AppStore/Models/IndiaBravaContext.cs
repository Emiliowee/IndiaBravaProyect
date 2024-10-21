using Microsoft.EntityFrameworkCore;

public class IndiaBravaContext : DbContext
{
    public IndiaBravaContext(DbContextOptions<IndiaBravaContext> options)
        : base(options)
    {
    }

    public DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Agregar datos semilla
        modelBuilder.Entity<Producto>().HasData(
            new Producto { IDProducto = 3, Nombre = "Producto 1", Precio = 10.99m, ContenidoNeto = 500, UnidadMedida = "g", Stock = 100, Imagen = "/images/producto1.jpg" },
            new Producto { IDProducto = 4, Nombre = "Producto 2", Precio = 15.99m, ContenidoNeto = 750, UnidadMedida = "ml", Stock = 50, Imagen = "/images/producto2.jpg" }
            
        );
    }
}
