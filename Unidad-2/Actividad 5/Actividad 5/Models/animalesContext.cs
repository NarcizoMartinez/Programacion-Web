using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Actividad_5.Models
{
    public partial class animalesContext : DbContext
    {
        public animalesContext()
        {
        }

        public animalesContext(DbContextOptions<animalesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Clase> Clase { get; set; }
        public virtual DbSet<Especies> Especies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=animales", x => x.ServerVersion("5.7.18-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clase>(entity =>
            {
                entity.ToTable("clase");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasColumnType("text")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Especies>(entity =>
            {
                entity.ToTable("especies");

                entity.HasIndex(e => e.IdClase)
                    .HasName("fk_especie_clase_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Especie)
                    .IsRequired()
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Habitat)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.IdClase).HasColumnType("int(11)");

                entity.Property(e => e.Observaciones)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Peso).HasColumnType("double(7,2)");

                entity.Property(e => e.Tamaño).HasColumnType("int(11)");

                entity.HasOne(d => d.IdClaseNavigation)
                    .WithMany(p => p.Especies)
                    .HasForeignKey(d => d.IdClase)
                    .HasConstraintName("fk_especie_clase");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
