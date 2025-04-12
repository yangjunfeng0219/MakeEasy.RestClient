namespace MakeEasy.RestClient;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class RestClient : IDisposable
{
    private readonly string? baseUrl;
    private readonly HttpClient client;

    public string? BaseUrl => baseUrl;
    public IRequestBodySerializer RequestBodySerializer { get; set; } = new JsonRequestBodySerializer();
    public IResponseContentDeserializer ResponseContentDeserializer { get; set; } = new JsonResponseContentDeserializer();

    public RestClient(string baseUrl)
    {
        this.baseUrl = baseUrl;
        client = new HttpClient();
    }

    public RestClient()
    {
        baseUrl = null;
        client = new HttpClient();
    }

    // GET
    public Task<HttpResponseMessage> GetAsync(string url, SendOptions? options = null)
        => SendWithoutBodyAsync(HttpMethod.Get, url, null, options);

    public Task<T?> GetAsync<T>(string url, SendOptions? options = null)
        => SendWithoutBodyAsync<T>(HttpMethod.Get, url, null, options);

    public Task<HttpResponseMessage> GetAsync(string url, object? queryParams, SendOptions? options = null)
        => SendWithoutBodyAsync(HttpMethod.Get, url, queryParams, options);

    public Task<T?> GetAsync<T>(string url, object? queryParams, SendOptions? options = null)
        => SendWithoutBodyAsync<T>(HttpMethod.Get, url, queryParams, options);

    // DELETE
    public Task<HttpResponseMessage> DeleteAsync(string url, SendOptions? options = null)
        => SendWithoutBodyAsync(HttpMethod.Delete, url, null, options);

    public Task<T?> DeleteAsync<T>(string url, SendOptions? options = null)
        => SendWithoutBodyAsync<T>(HttpMethod.Delete, url, null, options);

    public Task<HttpResponseMessage> DeleteAsync(string url, object? queryParams, SendOptions? options = null)
        => SendWithoutBodyAsync(HttpMethod.Delete, url, queryParams, options);

    public Task<T?> DeleteAsync<T>(string url, object? queryParams, SendOptions? options = null)
        => SendWithoutBodyAsync<T>(HttpMethod.Delete, url, queryParams, options);

    // HEAD
    public Task<HttpResponseMessage> HeadAsync(string url, SendOptions? options = null)
        => SendWithoutBodyAsync(HttpMethod.Head, url, null, options);

    public Task<T?> HeadAsync<T>(string url, SendOptions? options = null)
        => SendWithoutBodyAsync<T>(HttpMethod.Head, url, null, options);

    public Task<HttpResponseMessage> HeadAsync(string url, object? queryParams, SendOptions? options = null)
        => SendWithoutBodyAsync(HttpMethod.Head, url, queryParams, options);

    public Task<T?> HeadAsync<T>(string url, object? queryParams, SendOptions? options = null)
        => SendWithoutBodyAsync<T>(HttpMethod.Head, url, queryParams, options);

    // OPTIONS
    public Task<HttpResponseMessage> OptionsAsync(string url, SendOptions? options = null)
        => SendWithoutBodyAsync(HttpMethod.Options, url, null, options);

    public Task<T?> OptionsAsync<T>(string url, SendOptions? options = null)
        => SendWithoutBodyAsync<T>(HttpMethod.Options, url, null, options);

    public Task<HttpResponseMessage> OptionsAsync(string url, object? queryParams, SendOptions? options = null)
        => SendWithoutBodyAsync(HttpMethod.Options, url, queryParams, options);

    public Task<T?> OptionsAsync<T>(string url, object? queryParams, SendOptions? options = null)
        => SendWithoutBodyAsync<T>(HttpMethod.Options, url, queryParams, options);

    // POST
    public Task<HttpResponseMessage> PostAsync(string url, object? body, SendOptions? options = null)
        => SendAsync(HttpMethod.Post, url, body, options);

    public Task<T?> PostAsync<T>(string url, object? body, SendOptions? options = null)
        => SendAsync<T>(HttpMethod.Post, url, body, options);

    public Task<HttpResponseMessage> PostMultipartAsync(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendMultipartAsync(HttpMethod.Post, url, body, options);

    public Task<T?> PostMultipartAsync<T>(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendMultipartAsync<T>(HttpMethod.Post, url, body, options);

    public Task<HttpResponseMessage> PostStringAsync(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendStringAsync(HttpMethod.Post, url, body, contentType, options);

    public Task<T?> PostStringAsync<T>(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendStringAsync<T>(HttpMethod.Post, url, body, contentType, options);

    // PUT
    public Task<HttpResponseMessage> PutAsync(string url, object? body, SendOptions? options = null)
        => SendAsync(HttpMethod.Put, url, body, options);

    public Task<T?> PutAsync<T>(string url, object? body, SendOptions? options = null)
        => SendAsync<T>(HttpMethod.Put, url, body, options);

    public Task<HttpResponseMessage> PutMultipartAsync(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendMultipartAsync(HttpMethod.Put, url, body, options);

    public Task<T?> PutMultipartAsync<T>(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendMultipartAsync<T>(HttpMethod.Put, url, body, options);

    public Task<HttpResponseMessage> PutStringAsync(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendStringAsync(HttpMethod.Put, url, body, contentType, options);

    public Task<T?> PutStringAsync<T>(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendStringAsync<T>(HttpMethod.Put, url, body, contentType, options);

    // PATCH
#if NET5_0_OR_GREATER
    private static readonly HttpMethod PatchMethod = HttpMethod.Patch;
#else
    private static readonly HttpMethod PatchMethod = new("PATCH");
#endif
    public Task<HttpResponseMessage> PatchAsync(string url, object? body, SendOptions? options = null)
        => SendAsync(PatchMethod, url, body, options);

    public Task<T?> PatchAsync<T>(string url, object? body, SendOptions? options = null)
        => SendAsync<T>(PatchMethod, url, body, options);

    public Task<HttpResponseMessage> PatchMultipartAsync(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendMultipartAsync(PatchMethod, url, body, options);

    public Task<T?> PatchMultipartAsync<T>(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendMultipartAsync<T>(PatchMethod, url, body, options);

    public Task<HttpResponseMessage> PatchStringAsync(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendStringAsync(PatchMethod, url, body, contentType, options);

    public Task<T?> PatchStringAsync<T>(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendStringAsync<T>(PatchMethod, url, body, contentType, options);

    /******* private methods **********/

    private void AddOrUpdateHeaders(HttpRequestMessage request, object? headers)
    {
        if (headers == null) return;

        var props = RestUtils.GetNameValues(headers);
        foreach (var nameValue in props) {
            request.Headers.Remove(nameValue.Name);
            request.Headers.Add(nameValue.Name, nameValue.Value);
        }
    }

    private async Task<T?> FromResponse<T>(HttpResponseMessage response, SendOptions? options)
    {
        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Status code: {response.StatusCode} Description:{response.ReasonPhrase}");
        }
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var deserializer = options?.ResponseContentDeserializer ?? ResponseContentDeserializer;
        return deserializer.Deserialize<T>(content, response);
    }

    private string BuildUrl(string url, SendOptions? options)
    {
        var absoluteUrl = string.IsNullOrEmpty(BaseUrl) ? url : BaseUrl + url;

        var queryParams = options?.QueryParameters;
        if (queryParams != null) absoluteUrl = RestUtils.BuildQueryUrl(absoluteUrl, queryParams);

        var segmentParams = options?.SegmentParameters;
        if (segmentParams != null) absoluteUrl = RestUtils.BuildSegmentUrl(absoluteUrl, segmentParams);

        return absoluteUrl;
    }

    private Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, object? body, SendOptions? options = null)
    {
        var absoluteUrl = BuildUrl(url, options);
        var request = new HttpRequestMessage(method, absoluteUrl);
        var serializer = options?.RequestBodySerializer ?? RequestBodySerializer;
        request.Content = serializer.Serialize(body);
        AddOrUpdateHeaders(request, options?.Headers);

        return client.SendAsync(request,
            HttpCompletionOption.ResponseContentRead,
            options?.CancellationToken ?? CancellationToken.None);
    }

    private async Task<T?> SendAsync<T>(HttpMethod method, string url, object? body, SendOptions? options = null)
    {
        var response = await SendAsync(method, url, body, options).ConfigureAwait(false);
        return await FromResponse<T>(response, options).ConfigureAwait(false);
    }

    private Task<HttpResponseMessage> SendMultipartAsync(HttpMethod method, string url, MultipartFormDataContent body, SendOptions? options = null)
    {
        var absoluteUrl = BuildUrl(url, options);
        var request = new HttpRequestMessage(method, absoluteUrl);
        request.Content = body;
        AddOrUpdateHeaders(request, options?.Headers);

        return client.SendAsync(request,
            HttpCompletionOption.ResponseContentRead,
            options?.CancellationToken ?? CancellationToken.None);
    }

    private async Task<T?> SendMultipartAsync<T>(HttpMethod method, string url, MultipartFormDataContent body, SendOptions? options = null)
    {
        var response = await SendAsync(method, url, body, options).ConfigureAwait(false);
        return await FromResponse<T>(response, options).ConfigureAwait(false);
    }

    private Task<HttpResponseMessage> SendStringAsync(HttpMethod method, string url, string body, string? contentType, SendOptions? options = null)
    {
        var absoluteUrl = BuildUrl(url, options);
        var request = new HttpRequestMessage(method, absoluteUrl);
        request.Content = new StringContent(body, null, contentType);
        AddOrUpdateHeaders(request, options?.Headers);

        return client.SendAsync(request,
            HttpCompletionOption.ResponseContentRead,
            options?.CancellationToken ?? CancellationToken.None);
    }

    private async Task<T?> SendStringAsync<T>(HttpMethod method, string url, string body, string? contentType, SendOptions? options = null)
    {
        var response = await SendStringAsync(method, url, body, contentType, options).ConfigureAwait(false);
        return await FromResponse<T>(response, options).ConfigureAwait(false);
    }

    private Task<HttpResponseMessage> SendWithoutBodyAsync(HttpMethod method, string url, object? queryParams = null, SendOptions? options = null)
    {
        var absoluteUrl = BuildUrl(url, options);
        if (queryParams != null) absoluteUrl = RestUtils.BuildQueryUrl(absoluteUrl, queryParams);
        var request = new HttpRequestMessage(method, absoluteUrl);
        AddOrUpdateHeaders(request, options?.Headers);

        return client.SendAsync(request,
            HttpCompletionOption.ResponseContentRead,
            options?.CancellationToken ?? CancellationToken.None);
    }

    private async Task<T?> SendWithoutBodyAsync<T>(HttpMethod method, string url, object? queryParams = null, SendOptions? options = null)
    {
        var response = await SendWithoutBodyAsync(method, url, queryParams, options).ConfigureAwait(false);
        return await FromResponse<T>(response, options).ConfigureAwait(false);
    }

    public void Dispose()
    {
        client.Dispose();
        GC.SuppressFinalize(this);
    }
}
