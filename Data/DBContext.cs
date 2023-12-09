using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;

public class NotesDbContext : DbContext
{
    //public NotesDbContext(DbContextOptions<NotesDbContext> options) : base (options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if(!options.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.Development.json")
               .Build();

            string? connectionString = configuration.GetConnectionString("MainDB");
            Console.WriteLine(connectionString);
            options.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
     
//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        // define relationships Between User and Notes entities
//        modelBuilder.Entity<User>()
//            .HasMany(u => u.notes)
//            .WithOne(n => n.user)
//            .HasForeignKey(n => n.user_id);
//    }
}