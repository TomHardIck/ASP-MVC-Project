using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApp.Models
{
    public partial class ChinaStockContext : DbContext
    {
        public ChinaStockContext()
        {
        }

        public ChinaStockContext(DbContextOptions<ChinaStockContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Package> Packages { get; set; } = null!;
        public virtual DbSet<PackagesOfUser> PackagesOfUsers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.IdPackage);

                entity.ToTable("Package");

                entity.Property(e => e.IdPackage).HasColumnName("ID_Package");

                entity.Property(e => e.PackageName)
                    .IsUnicode(false)
                    .HasColumnName("Package_Name");

                entity.Property(e => e.ProductArt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Product_Art");

                entity.Property(e => e.ProductCount).HasColumnName("Product_Count");

                entity.Property(e => e.ProductLink)
                    .IsUnicode(false)
                    .HasColumnName("Product_Link");

                entity.Property(e => e.IsFinished)
                    .IsUnicode(false)
                    .HasColumnName("IsFinished");

                entity.Property(e => e.TypeDelivery)
                    .IsUnicode(false)
                    .HasColumnName("TypeDelivery");

                entity.Property(e => e.ProductPrice).HasColumnName("Product_Price");
                entity.Property(e => e.TrackNumber).HasMaxLength(20).IsUnicode(false).HasColumnName("TrackNumber");
            });

            modelBuilder.Entity<PackagesOfUser>(entity =>
            {
                entity.HasKey(e => e.IdPackageUser);

                entity.ToTable("Packages_Of_Users");

                entity.Property(e => e.IdPackageUser).HasColumnName("ID_Package_User");

                entity.Property(e => e.IdenticalNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Identical_Number");

                entity.Property(e => e.PackageId).HasColumnName("Package_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("User");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Phone_Number");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.Identical_Number)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Identical_Number");

                entity.Property(e => e.TgLink)
                    .IsUnicode(false)
                    .HasColumnName("TG_Link");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PK_Role");

                entity.ToTable("UserRole");

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.RoleName)
                    .IsUnicode(false)
                    .HasColumnName("Role_Name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
