namespace Domain.ValueObjects;

public sealed record Coordenadas : IEquatable<Coordenadas>
{
    public double Latitude { get; }
    public double Longitude { get; }

    private Coordenadas() { }

    public Coordenadas(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentOutOfRangeException("Latitude deve estar entre -90 e 90.");
        if (longitude < -180 || longitude > 180)
            throw new ArgumentOutOfRangeException("Longitude deve estar entre -180 e 180.");

        Latitude = latitude;
        Longitude = longitude;
    }
}
