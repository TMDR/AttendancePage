using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AttendancePage.Models
{
    public partial class dbcontext : DbContext
    {
        public dbcontext()
        {
        }

        public dbcontext(DbContextOptions<dbcontext> options)
            : base(options)
        {
        }

        public virtual DbSet<DateTime11> DateTime11 { get; set; }
        public virtual DbSet<Worker> Worker { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=Rayland5;database=workers");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<DateTime11>(entity =>
            {
                entity.ToTable("date_time11", "workers");

                entity.HasIndex(e => e.Email)
                    .HasName("SOMESHIT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.TimeIn).HasColumnName("time_in");

                entity.Property(e => e.TimeOut).HasColumnName("time_out");
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.ToTable("worker", "workers");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Psswrd)
                    .HasColumnName("psswrd")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.RatePerHour).HasColumnName("ratePerHour");
            });
        }
    }
}
