using GameStore.Api.Contracts;
using GameStore.Api.Mapping;
using GameStore.Domain.DbData;
using GameStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

//list is not safe. it is not protected from simultaneous access.
//so it's just a lesson case
    static readonly List<GameContract> games =
    [
        new (
            0,
            "Street Fighter II",
            "Fighter",
            19.99M,
            new DateOnly(1992, 07, 15)),
    
        new (
            1,
            "Counter-Strike 2",
            "FPS",
            14.99M,
            new DateOnly(2023, 09, 27)),
    
        new (
            2,
            "The Sims 4",
            "Simulator",
            24.99M,
            new DateOnly(2014, 09, 2)),
    
        new (
            3,
            "Resident Evil 2: Remake",
            "Survival Horror",
            39.99M,
            new DateOnly(2019, 08, 12))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();
        
        //get full list
        group.MapGet("/", () => games);

        //get one
        group.MapGet("/{id:int}", (int id) =>
            {
                GameContract? game = games.Find(g => g.Id == id);
    
                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpointName);

        //create
        group.MapPost("/", (CreateGameContract newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.GenreId);
            
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            
            return Results.CreatedAtRoute(GetGameEndpointName,
                new { id = game.Id }, game.ToContract());
        });

        //update game info
        group.MapPut("/{id:int}", (int id, UpdateGameContract updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameContract(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate);
        
            return Results.NoContent();
        });

        //delete record
        group.MapDelete("/{id:int}", (int id) =>
        {
            games.RemoveAll(g => g.Id == id);
            return Results.NoContent();
        });
        
        return group;
    }
}