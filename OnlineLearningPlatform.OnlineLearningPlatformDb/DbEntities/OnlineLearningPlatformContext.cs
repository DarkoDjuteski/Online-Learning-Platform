using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlineLearningPlatform.OnlineLearningPlatformDb.DbEntities;

public partial class OnlineLearningPlatformContext : DbContext
{
    public OnlineLearningPlatformContext()
    {
    }

    public OnlineLearningPlatformContext(DbContextOptions<OnlineLearningPlatformContext> options)
        : base(options)
    {
    }

    //public virtual DbSet<Basket> Baskets { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public DbSet<CustomerBasket> CustomerBaskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }


    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=DJUTESKI-D;Database=OnlineLearningPlatform;User ID=admin;Password=admin;Encrypt=False;Trusted_Connection=True;MultipleActiveResultSets=true;");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Basket>(entity =>
        {
            entity.HasKey(e => e.BasketId).HasName("PK__Basket__8FDA77B54A6EEE22");

            entity.ToTable("Basket");

            entity.HasOne(d => d.Course).WithMany(p => p.Baskets)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Basket__CourseId__3B75D760");

            entity.HasOne(d => d.User).WithMany(p => p.Baskets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Basket__UserId__3C69FB99");
        });

        modelBuilder.Entity<BasketItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BasketIt__3214EC0794BA89B7");

            entity.ToTable("BasketItem");

            entity.Property(e => e.OldUnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            //entity.HasOne(d => d.CustomerBasket).WithMany(p => p.Items)
            //    .HasForeignKey(d => d.CustomerBasketId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__BasketIte__Custo__44FF419A");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71A7C1228BC2");

            entity.ToTable("Course");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<CustomerBasket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC074B19BAE8");

            entity.ToTable("CustomerBasket");

            entity.Property(e => e.BuyerId).HasMaxLength(50);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A382EBEF1FC");

            entity.ToTable("Payment");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__UserId__3F466844");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C5F97CF15");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
