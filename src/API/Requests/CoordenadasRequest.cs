namespace API.Requests;

public sealed record CoordenadasRequest
{
    [FromBody]
    public double? Latitude { get; set; }

    [FromBody]
    public double? Longitude { get; set; }
}
