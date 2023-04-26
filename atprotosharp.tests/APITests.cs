namespace atprotosharp.tests;

public class APITests
{
    [Fact]
    public async Task GetServerParameters_Returns_Deserialized_ServerConfig()
    {
        // Arrange
        var jsonResponse = "{\"availableUserDomains\": [\".bsky.social\"], \"inviteCodeRequired\": true, \"links\": {\"privacyPolicy\": \"https://bsky.app/support/privacy\", \"termsOfService\": \"https://bsky.app/support/tos\"}}";

        var httpClientMock = new Mock<IHttpRequestHandler>();
        httpClientMock.Setup(client => client.HttpGetAsync("https://bsky.social/xrpc/com.atproto.server.describeServer")).ReturnsAsync(jsonResponse);

        var api = new API(httpClientMock.Object);
        
        // Act
        var serverConfig = await api.GetServerParameters();

        // Assert
        Assert.NotNull(serverConfig);
        Assert.True(serverConfig.InviteCodeRequired);
        Assert.Equal(".bsky.social", serverConfig.AvailableUserDomains[0]);
        Assert.Equal("https://bsky.app/support/privacy", serverConfig.Links.PrivacyPolicy);
        Assert.Equal("https://bsky.app/support/tos", serverConfig.Links.TermsOfService);

        httpClientMock.Verify(client => client.HttpGetAsync("https://bsky.social/xrpc/com.atproto.server.describeServer"), Times.Once);
    }

    [Fact]
    public async Task Authenticate_Returns_AuthenticationResponse()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpRequestHandler>();
        var yourClassInstance = new API(httpClientMock.Object);

        var jsonResponse = "{\"did\": \"did:plc:asdf\", \"handle\": \"taranasus.xyz\", \"email\": \"email@gmail.com\", \"accessJwt\": \"accessJWTString\", \"refreshJwt\": \"refreshJWTString\"}";
        httpClientMock.Setup(client => client.HttpPostAsync(Constants.AuthenticationEndpoint, It.IsAny<StringContent>())).ReturnsAsync(jsonResponse);

        // Act
        var authenticationResponse = await yourClassInstance.Authenticate("test@example.com", "testPassword");

        // Assert
        Assert.NotNull(authenticationResponse);
        Assert.Equal("did:plc:asdf", authenticationResponse.Did);
        Assert.Equal("taranasus.xyz", authenticationResponse.Handle);
        Assert.Equal("email@gmail.com", authenticationResponse.Email);
        Assert.Equal("accessJWTString", authenticationResponse.AccessJwt);
        Assert.Equal("refreshJWTString", authenticationResponse.RefreshJwt);

        httpClientMock.Verify(client => client.HttpPostAsync(Constants.AuthenticationEndpoint, It.IsAny<StringContent>()), Times.Once);
    }
}
