namespace API.Configurations;

public sealed class JwtOptions
{
    public const string SectionName = "Security:Authentication:Jwt";

    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = default!;
    public string Key { get; init; } = default!;

    /// <summary>
    /// Tenta decodificar a chave se vier em Base64. Se falhar, usa como UTF8.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public byte[] GetKeyBytes()
    {
        try
        {
            return Convert.FromBase64String(Key);
        }
        catch
        {
            return Encoding.UTF8.GetBytes(Key);
        }
    }
}
