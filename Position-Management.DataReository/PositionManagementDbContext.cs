using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Position_Management.DataRepository
{
    public class PositionManagementDbContext : DbContext
    {
        public PositionManagementDbContext(DbContextOptions<PositionManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transactions"); 

                entity.HasKey(t => t.TransactionID);

                entity.Property(t => t.TransactionID)
                      .ValueGeneratedOnAdd();

                entity.Property(t => t.TradeID)
                      .IsRequired();

                entity.Property(t => t.Version)
                      .IsRequired();

                entity.Property(t => t.SecurityCode)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(t => t.Quantity)
                      .IsRequired();

                entity.Property(t => t.InsertUpdateCancel)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(t => t.BuySell)
                      .HasMaxLength(4)
                      .IsRequired();
            });
        }
    }
}

