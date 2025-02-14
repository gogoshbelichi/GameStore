using GameStore.Api.Contracts;
using GameStore.Api.Mapping;
using GameStore.Domain.DbData;
using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();
        
        //get full list
        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Games
            .Include(g => g.Genre)
            .Select(game => game.ToGameSummaryContract())
            .AsNoTracking()
            .ToListAsync());

        //get one
        group.MapGet("/{id:int}", async (int id, GameStoreContext dbContext) => {
                Game? game = await dbContext.Games.FindAsync(id);
    
                return game is null ?
                    Results.NotFound() : Results.Ok(game.ToGameDetailsContract());
        }).WithName(GetGameEndpointName);

        //create
        group.MapPost("/", async (CreateGameContract newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            
            return Results.CreatedAtRoute(GetGameEndpointName,
                new { id = game.Id }, game.ToGameDetailsContract());
        });

        //update game info
        group.MapPut("/{id:int}", async (int id, UpdateGameContract updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                .CurrentValues
                .SetValues(updatedGame.ToEntity(id));
            await dbContext.SaveChangesAsync();
            
            return Results.NoContent();
        });

        //delete record
        group.MapDelete("/{id:int}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });
        
        return group;
    }
}