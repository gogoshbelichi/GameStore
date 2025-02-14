using GameStore.Api.Contracts;
using GameStore.Api.Mapping;
using GameStore.Domain.DbData;
using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Genres
                .Select(genre => genre.ToGenreContract())
                .AsNoTracking()
                .ToListAsync()
        );
        return group;
    }
}