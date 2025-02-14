using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Domain.DbData;

public class GameStoreContext(DbContextOptions<GameStoreContext> options)
    : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Fighter" },
            new { Id = 2, Name = "FPS" },
            new { Id = 3, Name = "Simulator" },
            new { Id = 4, Name = "Survival Horror" },
            new { Id = 5, Name = "Sandbox" }
        );
    }
}