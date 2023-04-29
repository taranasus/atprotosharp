using System.Dynamic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace atprotosharp;
public class API
{
    private string _serverUrl;
    private readonly IHttpRequestHandler _httpClient;
    private dynamic _session;
    private Dictionary<string, string> _authorizationHeader;

    #region Constructor

    /// <summary>
    /// Instantiate the API, connected to the server of your choice. By default connects to the BlueSky server
    /// </summary>
    /// <param name="httpClient">Use your own custom HTTPRest Client... if you wish... dunno why you'd do that tho...</param>
    /// <param name="serverUrl">The base url of the server you wish to connect to. Default: https://bsky.social</param>
    public API(IHttpRequestHandler httpClient, string serverUrl = Constants.DefaultServerUrl)
    {
        _httpClient = httpClient;
        _serverUrl = serverUrl;
    }

    /// <summary>
    /// Instantiate the API, connected to the server of your choice. By default connects to the BlueSky server
    /// </summary>
    /// <param name="serverUrl">The base url of the server you wish to connect to. Default: https://bsky.social</param>
    public API(string serverUrl = Constants.DefaultServerUrl) : this(new HttpRequestHandler(), serverUrl) { }

    #endregion

    /// <summary>
    /// (Unauthenticated) Get the configuration of the server you're connecting with
    /// </summary>
    /// <returns>ServerConfig object with the information for that server.</returns>
    public async Task<dynamic> GetServerParameters()
    {
        var response = await _httpClient.HttpGetAsync(_serverUrl + Constants.Endpoints.DescribeServer, _authorizationHeader);

        dynamic result = new ExpandoObject();
        if (!response.isSuccess)
        {
            result.success = false;
            result.error = response.responseBody;
            return result;
        }

        try
        {
            result = JObject.Parse(response.responseBody);
            result.serverUrl = _serverUrl;
            result.success = true;
        }
        catch
        {
            result.success = false;
            result.error = "Invalid Server";
            return result;
        }
        return result;
    }

    /// <summary>
    /// Connects you to the server using your account details
    /// </summary>
    /// <param name="username">your username</param>
    /// <param name="password">your password</param>
    public async Task<string> LoginAsync(string username, string password)
    {
        if (_session == null)
        {
            _session = await CreateSession(username, password);
            if (!(bool)_session.success)
            {
                var message = _session.error;
                _session = null;
                return message;
            }
            _authorizationHeader = new Dictionary<string, string>()
            {
                {"Authorization", "Bearer "+_session.accessJwt}
            };
        }
        return null;
    }

    /// <summary>
    /// Disconnect from the current session
    /// </summary>
    /// <returns>Null if success. Errors if errors</returns>
    public async Task LogoutAsync()
    {
        _session = null;
    }



    /// <summary>
    /// Returns the session object for your provided account after authentication. If no authentication this should throw an error
    /// </summary>
    /// <returns>AuthenticationResponse object with the information for your particular session</returns>
    public async Task<AuthenticationResponse?> GetSession()
    {
        var response = await _httpClient.HttpGetAsync(_serverUrl + Constants.Endpoints.GetSession, _authorizationHeader);
        var result = JsonConvert.DeserializeObject<AuthenticationResponse>(response.responseBody);
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
        var result = JsonConvert.DeserializeObject<UserProfile>(response.responseBody);
        return result;
    }

    /// <summary>
    /// Gets the timeline of the logged in user.
    /// </summary>
    /// <param name="algorithm">The order of the timeline. Possible: reverse-chronological</param>
    /// <param name="limit">How many posts to get. Default limit is 30</param>
    /// <returns>A dynamic ExpandoObject with whatever the server gives us.</returns>
    public async Task<dynamic> GetTimeline(string algorithm, int limit = 30)
    {
        dynamic result = new ExpandoObject();
        result.success = false;

        if (_authorizationHeader == null)
        {
            result.error = "You need to be logged in in order to conduct this operation. Type \"login\" in the terminal";
            return result;
        }

        var requestUrl = _serverUrl + Constants.Endpoints.GetTimeline + "?";
        if (!string.IsNullOrWhiteSpace(algorithm))
            requestUrl += "algorithm=" + algorithm + "&";
        requestUrl += "limit=" + limit.ToString();

        var response = await _httpClient.HttpGetAsync(requestUrl, _authorizationHeader);


        if (!response.isSuccess)
        {
            result.error = response.responseBody;
            return result;
        }
        result = JObject.Parse(response.responseBody);
        result.success = true;
        return result;
    }

    /// <summary>
    /// Gets all the invite codes availabile to the signed-in account
    /// </summary>
    /// <returns>AccountInviteCodesResult mode containing all the invite codes (used, unused, and if used by which did)</returns>
    public async Task<AccountInviteCodesResult> GetAccountInviteCodes()
    {
        var response = await _httpClient.HttpGetAsync(_serverUrl + Constants.Endpoints.GetAccountInviteCodes, _authorizationHeader);
        var result = JsonConvert.DeserializeObject<AccountInviteCodesResult>(response.responseBody);
        return result;
    }

    public string GetMyHandle()
    {
        return _session.handle;
    }

    public async Task<(bool success, string error)> SwitchServer(string url)
    {
        await LogoutAsync();
        if (url.StartsWith("http://"))
            return (false, "Only https:// is allowed. http:// not permitted");
        _serverUrl = url;
        return (true, null);
    }

    #region Privates
    /// <summary>
    /// Creates a new session with the server so you can do things.
    /// </summary>
    /// <param name="identifier">your username</param>
    /// <param name="password">your password</param>
    /// <returns>The response to your authentication attempt</returns>
    private async Task<dynamic> CreateSession(string identifier, string password)
    {
        var requestPayload = new AuthenticationRequest()
        {
            identifier = identifier,
            password = password
        };

        dynamic result = new ExpandoObject();
        result.success = false;
        var response = await _httpClient.HttpPostAsync(
            _serverUrl + Constants.Endpoints.CreateSession,
            new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json")
            );
        if (!response.isSuccess)
        {
            result.error = response.responseBody;
            return result;
        }
        result = JObject.Parse(response.responseBody);
        result.success = true;
        return result;
    }
    #endregion
}

