namespace atprotosharp;

public interface IHttpRequestHandler
{
    Task<HttpResponseMessage> GetAsync(string url);
    Task<dynamic> HttpGetAsync(string url, Dictionary<string, string>? headers = null);
    Task<dynamic> HttpPostAsync(string url, HttpContent content, Dictionary<string, string>? headers = null);
}
