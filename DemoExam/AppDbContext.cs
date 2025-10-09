using DemoExam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExam
{
    public class AppDbContext : DbContext
    {
        public static string host = "localhost";
        public static string port = "5432";
        public static string database = "demo_exam";
        public static string username = "postgres";
        public static string password = "postgres";
        public string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

        public DbSet<User> User { get; set; }
        public DbSet<Tovar>? Tovar { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
