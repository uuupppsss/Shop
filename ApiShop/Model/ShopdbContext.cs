using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using ShopLib;

namespace ApiShop.Model;

public partial class ShopdbContext : DbContext
{
    public ShopdbContext()
    {
    }

    public ShopdbContext(DbContextOptions<ShopdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Basketitem> Basketitems { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productimage> Productimages { get; set; }

    public virtual DbSet<Productsize> Productsizes { get; set; }

    public virtual DbSet<Producttype> Producttypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3307;user=root;password=root;database=shopdb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Basketitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("basketitem");

            entity.HasIndex(e => e.ProductId, "fk_basketitem_product");

            entity.HasIndex(e => e.UserId, "fk_basketitem_user");

            entity.Property(e => e.Count).HasMaxLength(255);
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.Size).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Basketitems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_basketitem_product");

            entity.HasOne(d => d.User).WithMany(p => p.Basketitems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_basketitem_user");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("brand");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.StatusId, "fk_order_status");

            entity.HasIndex(e => e.UserId, "fk_order_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasPrecision(19, 2);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.RecieveDate).HasColumnType("datetime");
            entity.Property(e => e.StatusId).HasColumnName("Status_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("fk_order_status");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_order_user");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orderitem");

            entity.HasIndex(e => e.ProductId, "fk_orderitem_product");

            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            entity.Property(e => e.Size).HasMaxLength(255);

            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_orderitem_product");
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orderstatus");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.BrandId, "fk_product_brand");

            entity.HasIndex(e => e.TypeId, "fk_product_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("Brand_id");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(19, 2);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.TypeId).HasColumnName("Type_id");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("fk_product_brand");

            entity.HasOne(d => d.Type).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("fk_product_type");
        });

        modelBuilder.Entity<Productimage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("productimages");

            entity.HasIndex(e => e.ProductId, "fk_productimages_product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image).HasColumnType("blob");
            entity.Property(e => e.ProductId).HasColumnName("Product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Productimages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_productimages_product");
        });

        modelBuilder.Entity<Productsize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("productsizes");

            entity.HasIndex(e => e.ProductId, "fk_productsizes_product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasMaxLength(255);
            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.Size).HasMaxLength(255);

            entity.HasOne(d => d.Product).WithMany(p => p.Productsizes)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_productsizes_product");
        });

        modelBuilder.Entity<Producttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("producttype");

            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.RoleId, "fk_user_role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasColumnName("Role_id");
            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_user_role");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("userrole");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
