namespace atprotosharp;

public class AuthenticationResponse : ErrorHandling
{

    public string? Did { get; set; }
    public string? Handle { get; set; }
    public string? Email { get; set; }
    public string? AccessJwt { get; set; }
    public string? RefreshJwt { get; set; }

    public AuthenticationResponse() : base()
    {

    }

    public AuthenticationResponse(bool success, string errorMessage)
        : base(success, errorMessage)
    {

    }
}
