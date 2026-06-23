using Microsoft.EntityFrameworkCore;
using Practica_INA.Data.Models;

namespace Practica_INA.Data.Context
{
    public class PracticaContext : DbContext
    {
        public PracticaContext(DbContextOptions<PracticaContext> options) : base(options)
        {
        }

        public DbSet<CategoriaProducto> CategoriasProducto { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.CategoriaProducto)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CategoriaProducto>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.Nombre)
                .IsUnique();
        }
    }
}
