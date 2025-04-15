namespace MakeEasy.RestClient;

using MakeEasy.RestClient.Authenticators;
using MakeEasy.RestClient.Serializers;
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
    public HttpClient HttpClient => client;
    public IRequestBodySerializer RequestBodySerializer { get; set; } = new JsonRequestBodySerializer();
    public IResponseContentDeserializer ResponseContentDeserializer { get; set; } = new JsonResponseContentDeserializer();
    public IAuthenticator? Authenticator { get; set; } = null;

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
        => SendContentAsync(HttpMethod.Get, url, null, options);

    public Task<T?> GetAsync<T>(string url, SendOptions? options = null)
        => SendContentAsync<T>(HttpMethod.Get, url, null, options);

    public Task<HttpResponseMessage> GetAsync(string url, object? queryParams, SendOptions? options = null)
    {
        if (queryParams != null) url = RestUtils.BuildQueryUrl(url, queryParams);
        return SendContentAsync(HttpMethod.Get, url, null, options);
    }

    public Task<T?> GetAsync<T>(string url, object? queryParams, SendOptions? options = null)
    {
        if (queryParams != null) url = RestUtils.BuildQueryUrl(url, queryParams);
        return SendContentAsync<T>(HttpMethod.Get, url, null, options);
    }

    // DELETE
    public Task<HttpResponseMessage> DeleteAsync(string url, SendOptions? options = null)
        => SendContentAsync(HttpMethod.Delete, url, null, options);

    public Task<T?> DeleteAsync<T>(string url, SendOptions? options = null)
        => SendContentAsync<T>(HttpMethod.Delete, url, null, options);

    public Task<HttpResponseMessage> DeleteAsync(string url, object? queryParams, SendOptions? options = null)
    {
        if (queryParams != null) url = RestUtils.BuildQueryUrl(url, queryParams);
        return SendContentAsync(HttpMethod.Delete, url, null, options);
    }

    public Task<T?> DeleteAsync<T>(string url, object? queryParams, SendOptions? options = null)
    {
        if (queryParams != null) url = RestUtils.BuildQueryUrl(url, queryParams);
        return SendContentAsync<T>(HttpMethod.Delete, url, null, options);
    }

    // HEAD
    public Task<HttpResponseMessage> HeadAsync(string url, SendOptions? options = null)
        => SendContentAsync(HttpMethod.Head, url, null, options);

    public Task<T?> HeadAsync<T>(string url, SendOptions? options = null)
        => SendContentAsync<T>(HttpMethod.Head, url, null, options);

    public Task<HttpResponseMessage> HeadAsync(string url, object? queryParams, SendOptions? options = null)
    {
        if (queryParams != null) url = RestUtils.BuildQueryUrl(url, queryParams);
        return SendContentAsync(HttpMethod.Head, url, null, options);
    }

    public Task<T?> HeadAsync<T>(string url, object? queryParams, SendOptions? options = null)
    {
        if (queryParams != null) url = RestUtils.BuildQueryUrl(url, queryParams);
        return SendContentAsync<T>(HttpMethod.Head, url, null, options);
    }

    // OPTIONS
    public Task<HttpResponseMessage> OptionsAsync(string url, SendOptions? options = null)
        => SendContentAsync(HttpMethod.Options, url, null, options);

    public Task<T?> OptionsAsync<T>(string url, SendOptions? options = null)
        => SendContentAsync<T>(HttpMethod.Options, url, null, options);

    public Task<HttpResponseMessage> OptionsAsync(string url, object? queryParams, SendOptions? options = null)
    {
        if (queryParams != null) url = RestUtils.BuildQueryUrl(url, queryParams);
        return SendContentAsync(HttpMethod.Options, url, null, options);
    }

    public Task<T?> OptionsAsync<T>(string url, object? queryParams, SendOptions? options = null)
    {
        if (queryParams != null) url = RestUtils.BuildQueryUrl(url, queryParams);
        return SendContentAsync<T>(HttpMethod.Options, url, null, options);
    }

    // POST
    public Task<HttpResponseMessage> PostAsync(string url, object? body, SendOptions? options = null)
    {
        var serializer = options?.RequestBodySerializer ?? RequestBodySerializer;
        return SendContentAsync(HttpMethod.Post, url, serializer.Serialize(body), options);
    }

    public Task<T?> PostAsync<T>(string url, object? body, SendOptions? options = null)
    {
        var serializer = options?.RequestBodySerializer ?? RequestBodySerializer;
        return SendContentAsync<T>(HttpMethod.Post, url, serializer.Serialize(body), options);
    }

    public Task<HttpResponseMessage> PostMultipartAsync(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendContentAsync(HttpMethod.Post, url, body, options);

    public Task<T?> PostMultipartAsync<T>(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendContentAsync<T>(HttpMethod.Post, url, body, options);

    public Task<HttpResponseMessage> PostStringAsync(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendContentAsync(HttpMethod.Post, url, new StringContent(body, null, contentType), options);

    public Task<T?> PostStringAsync<T>(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendContentAsync<T>(HttpMethod.Post, url, new StringContent(body, null, contentType), options);

    // PUT
    public Task<HttpResponseMessage> PutAsync(string url, object? body, SendOptions? options = null)
    {
        var serializer = options?.RequestBodySerializer ?? RequestBodySerializer;
        return SendContentAsync(HttpMethod.Put, url, serializer.Serialize(body), options);
    }

    public Task<T?> PutAsync<T>(string url, object? body, SendOptions? options = null)
    {
        var serializer = options?.RequestBodySerializer ?? RequestBodySerializer;
        return SendContentAsync<T>(HttpMethod.Put, url, serializer.Serialize(body), options);
    }

    public Task<HttpResponseMessage> PutMultipartAsync(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendContentAsync(HttpMethod.Put, url, body, options);

    public Task<T?> PutMultipartAsync<T>(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendContentAsync<T>(HttpMethod.Put, url, body, options);

    public Task<HttpResponseMessage> PutStringAsync(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendContentAsync(HttpMethod.Put, url, new StringContent(body, null, contentType), options);

    public Task<T?> PutStringAsync<T>(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendContentAsync<T>(HttpMethod.Put, url, new StringContent(body, null, contentType), options);
 
    // PATCH
#if NET5_0_OR_GREATER
    private static readonly HttpMethod PatchMethod = HttpMethod.Patch;
#else
    private static readonly HttpMethod PatchMethod = new("PATCH");
#endif
    public Task<HttpResponseMessage> PatchAsync(string url, object? body, SendOptions? options = null)
    {
        var serializer = options?.RequestBodySerializer ?? RequestBodySerializer;
        return SendContentAsync(PatchMethod, url, serializer.Serialize(body), options);
    }

    public Task<T?> PatchAsync<T>(string url, object? body, SendOptions? options = null)
    {
        var serializer = options?.RequestBodySerializer ?? RequestBodySerializer;
        return SendContentAsync<T>(PatchMethod, url, serializer.Serialize(body), options);
    }

    public Task<HttpResponseMessage> PatchMultipartAsync(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendContentAsync(PatchMethod, url, body, options);

    public Task<T?> PatchMultipartAsync<T>(string url, MultipartFormDataContent body, SendOptions? options = null)
        => SendContentAsync<T>(PatchMethod, url, body, options);

    public Task<HttpResponseMessage> PatchStringAsync(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendContentAsync(PatchMethod, url, new StringContent(body, null, contentType), options);

    public Task<T?> PatchStringAsync<T>(string url, string body, string contentType = RestContentTypes.PlainText, SendOptions? options = null)
        => SendContentAsync<T>(PatchMethod, url, new StringContent(body, null, contentType), options);
    
    /******* private methods **********/

    private async Task<T?> FromResponse<T>(HttpResponseMessage response, SendOptions? options)
    {
        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Status code: {response.StatusCode} Description:{response.ReasonPhrase}");
        }
        if (response.StatusCode == HttpStatusCode.NoContent) {
            return default;
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

    private Task<HttpResponseMessage> SendContentAsync(HttpMethod method, string url, HttpContent? content, SendOptions? options = null)
    {
        var absoluteUrl = BuildUrl(url, options);
        var request = new HttpRequestMessage(method, absoluteUrl);
        request.Content = content;
        request.AddOrUpdateHeaders(options?.Headers);

        var authenticator = options?.Authenticator ?? Authenticator;
        authenticator?.Authenticate(client, request);

        return client.SendAsync(request,
            HttpCompletionOption.ResponseContentRead,
            options?.CancellationToken ?? CancellationToken.None);
    }

    private async Task<T?> SendContentAsync<T>(HttpMethod method, string url, HttpContent? content, SendOptions? options = null)
    {
        var response = await SendContentAsync(method, url, content, options).ConfigureAwait(false);
        return await FromResponse<T>(response, options).ConfigureAwait(false);
    }

    public void Dispose()
    {
        client.Dispose();
        GC.SuppressFinalize(this);
    }
}
