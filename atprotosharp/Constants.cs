namespace atprotosharp;

public static class Constants
{
    public const string DefaultServerUrl = "https://bsky.social";
    public class Endpoints
    {
        public const string DescribeServer = "/xrpc/com.atproto.server.describeServer";
        public const string CreateSession = "/xrpc/com.atproto.server.createSession";
        public const string GetSession = "/xrpc/com.atproto.server.getSession";
        public const string GetProfile = "/xrpc/app.bsky.actor.getProfile";
        public const string GetTimeline = "/xrpc/app.bsky.feed.getTimeline";
        public const string GetAccountInviteCodes = "/xrpc/com.atproto.server.getAccountInviteCodes";
        public const string UploadBlob = "/xrpc/com.atproto.repo.uploadBlob";
        public const string CreateRecord = "/xrpc/com.atproto.repo.createRecord";
    }
}
