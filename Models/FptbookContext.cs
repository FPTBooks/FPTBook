using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FPTBook.Models;

public partial class FptbookContext : DbContext
{
    public FptbookContext()
    {
    }

    public FptbookContext(DbContextOptions<FptbookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:MyConnect");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasOne(d => d.Cat).WithMany(p => p.Books)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_Category");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasOne(d => d.Book).WithMany(p => p.Carts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Book");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_User");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Book).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Book");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
