namespace GameStore.Api.Contracts;

public record GameDetailsContract(
    int Id,
    string Name,
    int Genre,
    decimal Price,
    DateOnly ReleaseDate);
