namespace atprotosharp;

public static class Constants
{
    public const string DefaultServerUrl = "https://bsky.social";
    public class Endpoints
    {
        public const string DescribeServer = "/xrpc/com.atproto.server.describeServer";
        public const string CreateSession = "/xrpc/com.atproto.server.createSession";
        public const string GetSession = "/xrpc/com.atproto.server.getSession";
    }
}
