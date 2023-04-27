namespace atprotosharp;

public interface IHttpRequestHandler
{
    Task<string> HttpGetAsync(string url, Dictionary<string, string>? headers = null);
    Task<string> HttpPostAsync(string url, HttpContent content);
}
