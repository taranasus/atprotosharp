namespace atprotosharp;

public class HttpRequestHandler : IHttpRequestHandler
{
    private readonly HttpClient _httpClient;

    public HttpRequestHandler()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> HttpGetAsync(string url, Dictionary<string, string>? headers = null)
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
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"HttpGet request failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Error executing HttpGet request.", ex);
        }
    }


    public async Task<string> HttpPostAsync(string url, HttpContent content)
    {
        try
        {
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"HttpPost request failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Error executing HttpPost request.", ex);
        }
    }
}
