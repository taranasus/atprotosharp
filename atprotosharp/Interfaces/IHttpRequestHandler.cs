namespace atprotosharp;

public interface IHttpRequestHandler
{
    Task<string> HttpGetAsync(string url);
    Task<string> HttpPostAsync(string url, HttpContent content);
}
