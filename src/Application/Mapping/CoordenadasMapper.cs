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

    public static Coordenadas ToValueObject(this CoordenadasDTO dto) =>
        new(dto.Latitude, dto.Longitude);
}
