namespace atprotosharp;

public class ServerConfig
{
    public List<string>? AvailableUserDomains { get; set; }
    public bool InviteCodeRequired { get; set; }
    public ServerConfigLinks? Links { get; set; }
}
