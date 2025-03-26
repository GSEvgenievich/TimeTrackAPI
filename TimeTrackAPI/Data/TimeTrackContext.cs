using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TimeTrackAPI.Models;

namespace TimeTrackAPI.Data;

public partial class TimeTrackContext : DbContext
{
    public TimeTrackContext()
    {
    }

    public TimeTrackContext(DbContextOptions<TimeTrackContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=TimeTrackerDB;Username=postgres;Password=Ger357siezap1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("assignments_pkey");

            entity.Property(e => e.AssignmentId).HasDefaultValueSql("nextval('assignments_id_seq'::regclass)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Shift).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.ShiftId)
                .HasConstraintName("assignments_shiftid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("assignments_statusid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("assignments_userid_fkey");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("Location_pkey");

            entity.Property(e => e.LocationId).HasDefaultValueSql("nextval('\"Location_LocationId_seq\"'::regclass)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.LocationName).HasMaxLength(150);
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("notifications_pkey");

            entity.Property(e => e.NotificationId).HasDefaultValueSql("nextval('notifications_id_seq'::regclass)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.NotificationIsRead).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notifications_userid_fkey");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.SalaryId).HasName("salaries_pkey");

            entity.Property(e => e.SalaryId).HasDefaultValueSql("nextval('salaries_id_seq'::regclass)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.SalaryAmount).HasPrecision(10, 2);
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.User).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("salaries_userid_fkey");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("shifts_pkey");

            entity.Property(e => e.ShiftId).HasDefaultValueSql("nextval('shifts_id_seq'::regclass)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.ShiftIsAvailable).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Location).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("shifts_locationid_fkey");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("status_pkey");

            entity.Property(e => e.StatusId).HasDefaultValueSql("nextval('status_statusid_seq'::regclass)");
            entity.Property(e => e.StatusName).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.HasIndex(e => e.UserEmail, "users_email_key").IsUnique();

            entity.HasIndex(e => e.UserName, "users_username_key").IsUnique();

            entity.Property(e => e.UserId).HasDefaultValueSql("nextval('users_id_seq'::regclass)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.UserEmail).HasMaxLength(100);
            entity.Property(e => e.UserFirstName).HasMaxLength(50);
            entity.Property(e => e.UserLastName).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.UserPasswordHash).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
