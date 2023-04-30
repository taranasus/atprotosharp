namespace atprotosharp;

public interface IHttpRequestHandler
{
    Task<(bool isSuccess, string responseBody)> HttpGetAsync(string url, Dictionary<string, string>? headers = null);
    Task<(bool isSuccess, string responseBody)> HttpPostAsync(string url, HttpContent content, Dictionary<string, string>? headers = null);
}
