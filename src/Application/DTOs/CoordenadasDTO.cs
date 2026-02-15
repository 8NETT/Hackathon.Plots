namespace Application.DTOs;

public sealed record CoordenadasDTO
{
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}
