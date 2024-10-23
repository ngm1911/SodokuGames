using BlazorSodokuApp.Server.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BlazorSodokuApp.Server.Database.DbContext
{
    public class SudokuContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public SudokuContext(DbContextOptions<SudokuContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                          .LogTo(x => Console.WriteLine(x),
                                    events: new List<EventId>() { RelationalEventId.CommandExecuted })
                          .EnableDetailedErrors()
                          .EnableSensitiveDataLogging();
        }

        public DbSet<SudokuGame> SudokuGames { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SudokuGame>().ToTable("SudokuGames");
        }
    }
}
