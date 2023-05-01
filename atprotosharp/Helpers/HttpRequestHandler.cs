using System.Dynamic;

namespace atprotosharp;

public class HttpRequestHandler : IHttpRequestHandler
{
    private readonly HttpClient _httpClient;

    public HttpRequestHandler()
    {
        _httpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        return await _httpClient.GetAsync(url);
    }

    public async Task<dynamic> HttpGetAsync(string url, Dictionary<string, string>? headers = null)
    {
        dynamic result = GenerateResult(false);
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                result.success = true;
                result.responseBody = await response.Content.ReadAsStringAsync();
            }
            else
            {
                result.error = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            result.error = ex.Message;
        }
        return result;
    }


    public async Task<dynamic> HttpPostAsync(string url, HttpContent content, Dictionary<string, string>? headers = null)
    {
        dynamic result = GenerateResult(false);
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                result.success = true;
                result.responseBody = await response.Content.ReadAsStringAsync();
            }
            else
            {
                result.error = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            result.error = ex.Message;
        }
        return result;
    }

    /// <summary>
    /// Create a dynamic result object to provide cleaner code in the methods
    /// </summary>
    private dynamic GenerateResult(bool success, string? error = null)
    {
        dynamic result = new ExpandoObject();
        result.success = success;
        if (error != null)
            result.error = error;
        return result;
    }
}
