using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Contracts;

public record UpdateGameContract(
    [Required][StringLength(50)]string Name,
    [Required][StringLength(25)]string Genre,
    [Required][Range(1, 100)]decimal Price,
    DateOnly ReleaseDate);