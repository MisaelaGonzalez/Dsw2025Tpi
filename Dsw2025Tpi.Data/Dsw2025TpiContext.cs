using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2025Tpi.Data;
// Hereda de DbContext, lo que significa que representa la conexión a la base de datos
// y se encarga de mapear las clases C# (entidades) a tablas SQL.
public class Dsw2025TpiContext : DbContext
{
    //Este constructor permite inyectar las opciones de configuración del contexto
    //(cadena de conexión, proveedor de base de datos, etc.). 
    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options) : base(options)
    {
       
    }
    // Este método se ejecuta cuando EF está construyendo el modelo de datos. Se usa para aplicar
    // configuraciones personalizadas sobre cómo se mapean las entidades a las tablas.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var dbProduct = modelBuilder.Entity<Product>().ToTable("Products"); // Le dice a EF Core que la entidad Product
                                                                            // se mapeará a una tabla llamada Products

        dbProduct.Property(p => p.Sku) 
            .IsRequired() // campo obligatorio
            .HasMaxLength(50); //longitud maxima
        dbProduct.HasIndex(p => p.Sku)
            .IsUnique(); // sku: indice unico
        dbProduct.Property(p => p.InternalCode)
            .IsRequired()
            .HasMaxLength(50);
        dbProduct.Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();
        dbProduct.Property(p => p.Description)
            .HasMaxLength(300);
        dbProduct.Property(p => p.CurrentUnitPrice)
            .HasPrecision(15, 2) // precision decimal
            .IsRequired();
        dbProduct.Property(p => p.StockCuantity)
            .HasMaxLength(10);

        var dbCustomer = modelBuilder.Entity<Customer>().ToTable("Customers");
        dbCustomer.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();
        dbCustomer.Property(c => c.Email)
            .HasMaxLength(100)
            .IsRequired();
        dbCustomer.Property(c => c.PhoneNumber)
            .HasMaxLength(10)
            .IsRequired();

        var dbOrder = modelBuilder.Entity<Order>().ToTable("Orders");
        dbOrder.Property(o => o.ShippingAddress)
            .HasMaxLength(200)
            .IsRequired();
        dbOrder.Property(o => o.BillingAddress)
            .HasMaxLength(200)
            .IsRequired();
        dbOrder.Property(o => o.Notes)
            .HasMaxLength(500);
        dbOrder.Ignore(o => o.TotalAmount); // Se ignora la propiedad TotalAmount, que es calculada
                                            // (no se guarda en la base de datos).


        var dbOrderItem = modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
        dbOrderItem.Property(oi => oi.UnitPrice)
            .HasPrecision(15, 2)
            .IsRequired();
        dbOrderItem.Property(oi => oi.Quantity)
            .IsRequired();
        dbOrderItem.Ignore(oi => oi.Subtotal); // Tambien se ignora la propiedad Subtotal

    }
    // Estas propiedades indican qué entidades se deben mapear como tablas en la base de datos:
    // Cada DbSet<T> representa una tabla en la base de datos.
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}


