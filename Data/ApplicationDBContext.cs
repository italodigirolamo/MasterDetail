using MasterDetail.Models;
using Microsoft.EntityFrameworkCore;

namespace MasterDetail.Data
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        
        }

        public DbSet<Cliente> Clientes { get; set; } = null!;

        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<Pedido> Pedidos { get; set; }  = null !;
        public DbSet<PedidoDetalle> PedidosDetalle { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .HasMany(p=>p.Detalles)
                .WithOne(d=>d.Pedido)
                .HasForeignKey(d=>d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
