using GameStore.Api.Endpoints;
using GameStore.Domain.DbData;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connectionString);

var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

await DataExtensions.MigrateDbAsync(app);

app.Run();