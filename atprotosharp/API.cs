using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace atprotosharp;
public class API
{
    private string _serverUrl;
    private readonly IHttpRequestHandler _httpClient;
    private readonly JsonSerializerOptions _defaultJsonSerializerOptions;
    private AuthenticationResponse? _session;


    public API(IHttpRequestHandler httpClient, string serverUrl = Constants.DefaultServerUrl)
    {
        _httpClient = httpClient;
        _serverUrl = serverUrl;
        _defaultJsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    public API(string serverUrl = Constants.DefaultServerUrl) : this(new HttpRequestHandler(), serverUrl) { }

    public async Task Connect(string username, string password)
    {
        if (_session == null)
        {
            _session = await CreateSession(username, password);
        }
    }

    public async Task<ServerConfig?> GetServerParameters()
    {
        var response = await _httpClient.HttpGetAsync(_serverUrl + Constants.Endpoints.DescribeServer);
        var result = JsonSerializer.Deserialize<ServerConfig>(response, _defaultJsonSerializerOptions);
        return result;
    }

    public async Task<AuthenticationResponse?> GetSession()
    {
        var response = await _httpClient.HttpGetAsync(
            _serverUrl + Constants.Endpoints.GetSession,
            new Dictionary<string, string>()
            {
                {"Authorization", "Bearer "+_session.AccessJwt}
            }
        );
        var result = JsonSerializer.Deserialize<AuthenticationResponse>(response, _defaultJsonSerializerOptions);
        return result;
    }

    #region Privates
    private async Task<AuthenticationResponse?> CreateSession(string identifier, string password)
    {
        var requestPayload = new AuthenticationRequest()
        {
            identifier = identifier,
            password = password
        };

        var response = await _httpClient.HttpPostAsync(
            _serverUrl + Constants.Endpoints.CreateSession,
            new StringContent(JsonSerializer.Serialize(requestPayload), Encoding.UTF8, "application/json")
            );
        var result = JsonSerializer.Deserialize<AuthenticationResponse>(response, _defaultJsonSerializerOptions);
        return result;
    }
    #endregion
}
