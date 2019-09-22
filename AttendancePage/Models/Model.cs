using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AttendancePage.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int Age { get; set; }
    }
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    //public partial class MyDbContext : DbContext
    //{
    //    public MyDbContext()
    //    {
    //    }

    //    public MyDbContext(DbContextOptions<MyDbContext> options)
    //        : base(options)
    //    {
    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        if (!optionsBuilder.IsConfigured)
    //        {
    //            optionsBuilder.UseMySql("Server=localhost;User Id=root;Password=Rayland5;Database=workers");
    //        }
    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    { }
    //}
}