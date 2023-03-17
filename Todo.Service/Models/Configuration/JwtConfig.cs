namespace Todo.Service.Models.Configuration;

/// <summary>
/// JWT config model
/// </summary>
public class JwtConfig
{
    /// <summary>
    /// 
    /// </summary>
    public string? Issuer { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Audience { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Key { get; set; }
}