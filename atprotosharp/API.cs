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
    private Dictionary<string, string> _authorizationHeader;

    /// <summary>
    /// Instantiate the API, connected to the server of your choice. By default connects to the BlueSky server
    /// </summary>
    /// <param name="httpClient">Use your own custom HTTPRest Client... if you wish... dunno why you'd do that tho...</param>
    /// <param name="serverUrl">The base url of the server you wish to connect to. Default: https://bsky.social</param>
    public API(IHttpRequestHandler httpClient, string serverUrl = Constants.DefaultServerUrl)
    {
        _httpClient = httpClient;
        _serverUrl = serverUrl;
        _defaultJsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    /// <summary>
    /// Instantiate the API, connected to the server of your choice. By default connects to the BlueSky server
    /// </summary>
    /// <param name="serverUrl">The base url of the server you wish to connect to. Default: https://bsky.social</param>
    public API(string serverUrl = Constants.DefaultServerUrl) : this(new HttpRequestHandler(), serverUrl) { }

    /// <summary>
    /// Connects you to the server using your account details
    /// </summary>
    /// <param name="username">your username</param>
    /// <param name="password">your password</param>
    public async Task<string> ConnectAsync(string username, string password)
    {
        if (_session == null)
        {
            _session = await CreateSession(username, password);
            _authorizationHeader = new Dictionary<string, string>()
            {
                {"Authorization", "Bearer "+_session.AccessJwt}
            };
            if (!_session.success)
            {
                var message = _session.errorMessage;
                _session = null;
                return message;
            }
        }
        return null;
    }

    /// <summary>
    /// Disconnect from the current session
    /// </summary>
    /// <returns>Null if success. Errors if errors</returns>
    public async Task<string> LogOutAsync()
    {
        _session = null;
        return null;
    }

    /// <summary>
    /// Get the configuration of the server you're connecting with
    /// </summary>
    /// <returns>ServerConfig object with the information for that server</returns>
    public async Task<ServerConfig?> GetServerParameters()
    {
        var response = await _httpClient.HttpGetAsync(_serverUrl + Constants.Endpoints.DescribeServer);
        var result = JsonSerializer.Deserialize<ServerConfig>(response.responseBody, _defaultJsonSerializerOptions);
        return result;
    }

    /// <summary>
    /// Returns the session object for your provided account after authentication. If no authentication this should throw an error
    /// </summary>
    /// <returns>AuthenticationResponse object with the information for your particular session</returns>
    public async Task<AuthenticationResponse?> GetSession()
    {
        var response = await _httpClient.HttpGetAsync(_serverUrl + Constants.Endpoints.GetSession, _authorizationHeader);
        var result = JsonSerializer.Deserialize<AuthenticationResponse>(response.responseBody, _defaultJsonSerializerOptions);
        return result;
    }

    /// <summary>
    /// Get a user account by did
    /// </summary>
    /// <param name="did">the did for the profile you want to get. ex did:pfc:whatever43</param>
    /// <returns>UserProfile model containt the detials for that user</returns>
    public async Task<UserProfile> GetProfileByDid(string did)
    {
        var response = await _httpClient.HttpGetAsync(_serverUrl + Constants.Endpoints.GetProfile + "?actor=" + did, _authorizationHeader);
        var result = JsonSerializer.Deserialize<UserProfile>(response.responseBody, _defaultJsonSerializerOptions);
        return result;
    }

    /// <summary>
    /// Gets all the invite codes availabile to the signed-in account
    /// </summary>
    /// <returns>AccountInviteCodesResult mode containing all the invite codes (used, unused, and if used by which did)</returns>
    public async Task<AccountInviteCodesResult> GetAccountInviteCodes()
    {
        var response = await _httpClient.HttpGetAsync(_serverUrl + Constants.Endpoints.GetAccountInviteCodes, _authorizationHeader);
        var result = JsonSerializer.Deserialize<AccountInviteCodesResult>(response.responseBody, _defaultJsonSerializerOptions);
        return result;
    }

    public string GetMyHandle()
    {
        return _session.Handle;
    }

    #region Privates
    /// <summary>
    /// Creates a new session with the server so you can do things.
    /// </summary>
    /// <param name="identifier">your username</param>
    /// <param name="password">your password</param>
    /// <returns>The response to your authentication attempt</returns>
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
        if (!response.isSuccess)
            return new AuthenticationResponse(false, response.responseBody);
        var result = JsonSerializer.Deserialize<AuthenticationResponse>(response.responseBody, _defaultJsonSerializerOptions);
        return result;
    }
    #endregion
}

