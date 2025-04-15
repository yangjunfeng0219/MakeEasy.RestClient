namespace MakeEasy.RestClient.Authenticators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class OAuth2UriAuthenticator : IAuthenticator
{
    public string Token { get; }
    public string Scheme { get; }

    public OAuth2UriAuthenticator(string token, string scheme = "oauth_token")
    {
        Token = token;
        Scheme = scheme;
    }

    public Task Authenticate(HttpClient client, HttpRequestMessage request)
    {
        var uri = request.RequestUri;
        if (uri == null) throw new ArgumentNullException(nameof(request.RequestUri));
        var newUrl = RestUtils.BuildQueryUrl(request.RequestUri!.ToString(), Scheme, Token);
        request.RequestUri = new Uri(newUrl);
        return Task.FromResult(0);

        //var uri = request.RequestUri;
        //if (uri == null) throw new ArgumentNullException(nameof(request.RequestUri));
        //var sb = new StringBuilder();
        //sb.Append(uri.GetLeftPart(UriPartial.Path));

        //var query = uri.Query;
        //if (string.IsNullOrEmpty(query)) {
        //    sb.Append($"?");
        //} else {
        //    sb.Append(query).Append("&");
        //}
        //sb.Append(RestUtils.BuildQueryUrl(Scheme, Token));
        //request.RequestUri = new Uri(sb.ToString());
        //return Task.FromResult(0);
    }
}
