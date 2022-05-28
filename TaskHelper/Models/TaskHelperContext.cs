using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaskHelper.Models
{
    public partial class TaskHelperContext : DbContext
    {
        public TaskHelperContext()
        {
        }

        public TaskHelperContext(DbContextOptions<TaskHelperContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SatusTask> SatusTasks { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TaskHelper;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SatusTask>(entity =>
            {
                entity.HasKey(e => e.StatusTaskId);

                entity.ToTable("SatusTask");

                entity.Property(e => e.StatusTaskId).HasColumnName("StatusTaskID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.TaskId).HasColumnName("TaskID");

                entity.Property(e => e.AcceptorId).HasColumnName("AcceptorID");

                entity.Property(e => e.CreatorId).HasColumnName("CreatorID");

                entity.Property(e => e.Describtion).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PublicDate).HasColumnType("datetime");

                entity.Property(e => e.StatusTaskId).HasColumnName("StatusTaskID");

                entity.HasOne(d => d.Acceptor)
                    .WithMany(p => p.TaskAcceptors)
                    .HasForeignKey(d => d.AcceptorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_AcceptorID");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.TaskCreators)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_CreatorID");

                entity.HasOne(d => d.StatusTask)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.StatusTaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_SatusTask");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Patronymic).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.SecondName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
