namespace atprotosharp;

public class AuthenticationResponse
{
    public string? Did { get; set; }
    public string? Handle { get; set; }
    public string? Email { get; set; }
    public string? AccessJwt { get; set; }
    public string? RefreshJwt { get; set; }
}
