namespace atprotosharp;

public class HttpRequestHandler : IHttpRequestHandler
{
    private readonly HttpClient _httpClient;

    public HttpRequestHandler()
    {
        _httpClient = new HttpClient();
    }

    public async Task<(bool isSuccess, string responseBody)> HttpGetAsync(string url, Dictionary<string, string>? headers = null)
    {
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
                return (true, await response.Content.ReadAsStringAsync());
            else
                return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }


    public async Task<(bool isSuccess, string responseBody)> HttpPostAsync(string url, HttpContent content, Dictionary<string, string>? headers = null)
    {
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
                return (true, await response.Content.ReadAsStringAsync());
            else
                return (false, await response.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
