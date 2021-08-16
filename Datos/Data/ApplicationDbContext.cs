using Datos.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Data
{
    public class ApplicationDbContext : IdentityDbContext<AplicationUser, UserRole, string>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }       
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Parqueo> Parqueos { get; set; }
        public virtual DbSet<TipoVehiculo> TipoVehiculos { get; set; }
        public virtual DbSet<Vehiculo> Vehiculos { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Cedula);

                entity.ToTable("Cliente");

                entity.Property(e => e.Cedula)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Parqueo>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("Parqueo");

                entity.Property(e => e.FechaIngreso).HasColumnType("datetime");

                entity.Property(e => e.FechaSalida).HasColumnType("datetime");

                entity.Property(e => e.PlacaVehiculo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoVehiculo>(entity =>
            {
                entity.HasKey(e => e.Codigo);

                entity.ToTable("TipoVehiculo");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(e => e.Placa)
                    .HasName("PK_Vehiculo_1");

                entity.ToTable("Vehiculo");

                entity.Property(e => e.Placa)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CedulaCliente)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Chasis)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.CedulaClienteNavigation)
                    .WithMany(p => p.Vehiculos)
                    .HasForeignKey(d => d.CedulaCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehiculo_Cliente");

                entity.HasOne(d => d.TipoVehiculoNavigation)
                    .WithMany(p => p.Vehiculos)
                    .HasForeignKey(d => d.TipoVehiculo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehiculo_TipoVehiculo");
            });

        }
    }
}