using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace atprotosharp;
public class API
{
    IHttpRequestHandler _httpClient;

    public API(IHttpRequestHandler httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ServerConfig?> GetServerParameters()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var response = await _httpClient.HttpGetAsync(Constants.DescribeServerEndpoint);
        var result =  JsonSerializer.Deserialize<ServerConfig>(response,options);
        return result;
    }

    public async Task<AuthenticationResponse> Authenticate(string identifier, string password)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var requestPayload = new AuthenticationRequest()
        {
            identifier = identifier,
            password = password
        };

        var response = await _httpClient.HttpPostAsync(Constants.AuthenticationEndpoint,
            new StringContent(JsonSerializer.Serialize(requestPayload), Encoding.UTF8, "application/json"));
        var result = JsonSerializer.Deserialize<AuthenticationResponse>(response, options);
        return result;
    }
}

public static class Constants
{
    public const string DescribeServerEndpoint = "https://bsky.social/xrpc/com.atproto.server.describeServer";
    public const string AuthenticationEndpoint = "https://bsky.social/xrpc/com.atproto.server.createSession";
}

public class AuthenticationRequest
{
    public string identifier { get; set; }
    public string password { get; set; }
}

public class AuthenticationResponse
{
    public string Did { get; set; }
    public string Handle { get; set; }
    public string Email { get; set; }
    public string AccessJwt { get; set; }
    public string RefreshJwt { get; set; }
}
