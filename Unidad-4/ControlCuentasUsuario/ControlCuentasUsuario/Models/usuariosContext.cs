using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ControlCuentasUsuario.Models
{
    public partial class usuariosContext : DbContext
    {
        public usuariosContext()
        {
        }

        public usuariosContext(DbContextOptions<usuariosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cuentum> Cuenta { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=usuarios", x => x.ServerVersion("5.7.18-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cuentum>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PRIMARY");

                entity.ToTable("cuenta");

                entity.Property(e => e.IdUser).HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(256)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Token).HasColumnType("int(11)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
