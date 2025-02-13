using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Contracts;

public record CreateGameContract(
    [Required][StringLength(50)]string Name,
    [Required][StringLength(25)]string Genre,
    [Required][Range(1, 100)]decimal Price,
    DateTime ReleaseDate);