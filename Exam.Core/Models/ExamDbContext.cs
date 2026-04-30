using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Exam.Core.Models;

public partial class ExamDbContext : DbContext
{
    public ExamDbContext()
    {
    }

    public ExamDbContext(DbContextOptions<ExamDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bom> Boms { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ExamDb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bom>(entity =>
        {
            entity.HasKey(e => e.BomId).HasName("PK__Bom__7D5F6A37501CBB12");

            entity.ToTable("Bom");

            entity.HasIndex(e => new { e.ProductId, e.MaterialId }, "UQ_Product_Material").IsUnique();

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MaterialId).HasMaxLength(10);
            entity.Property(e => e.ProductId).HasMaxLength(10);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Material).WithMany(p => p.Boms)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bom_Material");

            entity.HasOne(d => d.Product).WithMany(p => p.Boms)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bom_Product");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Material__C50610F7D03FD414");

            entity.ToTable("Material");

            entity.Property(e => e.MaterialId).HasMaxLength(10);
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MaterialName).HasMaxLength(100);
            entity.Property(e => e.Stock).HasDefaultValue(0);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCFF45D58F6");

            entity.Property(e => e.OrderId).HasMaxLength(20);
            entity.Property(e => e.Applicant).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderTitle).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__OrderDet__135C316D5441DDEC");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasMaxLength(20);
            entity.Property(e => e.ProductId).HasMaxLength(10);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6CD7F829B11");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CF844BDAA");

            entity.ToTable("User");

            entity.HasIndex(e => e.Account, "UQ__User__B0C3AC46B6457C65").IsUnique();

            entity.Property(e => e.Account).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
