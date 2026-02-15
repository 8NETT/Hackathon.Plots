using Application.DTOs;
using Domain.ValueObjects;

namespace Application.Mapping;

internal static class CoordenadasMapper
{
    public static CoordenadasDTO ToDTO(this Coordenadas coordenadas) => new()
    {
        Latitude = coordenadas.Latitude,
        Longitude = coordenadas.Longitude
    };
}
