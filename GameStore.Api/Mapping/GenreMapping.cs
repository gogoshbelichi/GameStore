using GameStore.Api.Contracts;
using GameStore.Domain.Entities;

namespace GameStore.Api.Mapping;

public static class GenreMapping
{
    public static GenreContract ToGenreContract(this Genre genre)
    {
        return new GenreContract(genre.Id, genre.Name);
    }
}