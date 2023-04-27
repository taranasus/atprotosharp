namespace atprotosharp.tests;

public class APITests
{
    [Fact]
    public async Task GetServerParameters_Returns_Deserialized_ServerConfig()
    {
        // Arrange
        var jsonResponse = "{\"availableUserDomains\": [\".bsky.social\"], \"inviteCodeRequired\": true, \"links\": {\"privacyPolicy\": \"https://bsky.app/support/privacy\", \"termsOfService\": \"https://bsky.app/support/tos\"}}";

        var httpClientMock = new Mock<IHttpRequestHandler>();
        httpClientMock.Setup(client => client.HttpGetAsync("https://bsky.social/xrpc/com.atproto.server.describeServer", null)).ReturnsAsync(jsonResponse);

        var api = new API(httpClientMock.Object);

        // Act
        var serverConfig = await api.GetServerParameters();

        // Assert
        Assert.NotNull(serverConfig);
        Assert.True(serverConfig.InviteCodeRequired);
        Assert.Equal(".bsky.social", serverConfig!.AvailableUserDomains![0]);
        Assert.Equal("https://bsky.app/support/privacy", serverConfig!.Links!.PrivacyPolicy);
        Assert.Equal("https://bsky.app/support/tos", serverConfig.Links.TermsOfService);

        httpClientMock.Verify(client => client.HttpGetAsync("https://bsky.social/xrpc/com.atproto.server.describeServer", null), Times.Once);
    }
}
