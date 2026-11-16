using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ASPCoreFirstApp.Models;

public class ApplicationDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        mb.Entity<Movie>()
            .HasOne(m => m.Genre)
            .WithMany(g => g.Movies)
            .HasForeignKey(m => m.GenreId)
            .OnDelete(DeleteBehavior.SetNull);

        var path = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "genres.json");
        var genres = JsonSerializer.Deserialize<List<Genre>>(
                         File.ReadAllText(path),
                         new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                     ?? new List<Genre>();

        mb.Entity<Genre>().HasData(genres);

        var action = Guid.Parse("84ca0bcd-082c-49cb-aa77-ea2f1f5f8285");
        var sciFi = Guid.Parse("3f2b8c4e-5a1d-4e7b-9c60-1d2e3f4a5b6c");

        mb.Entity<Movie>().HasData(
            new Movie { Id = 1, Name = "Inception", GenreId = sciFi, IsAvailable = true },
            new Movie { Id = 2, Name = "Gladiator", GenreId = action, IsAvailable = true },
            new Movie { Id = 3, Name = "Interstellar", GenreId = sciFi, IsAvailable = true }
        );
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie>? Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
}