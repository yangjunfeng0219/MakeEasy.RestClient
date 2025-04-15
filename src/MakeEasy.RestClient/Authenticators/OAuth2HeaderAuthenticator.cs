namespace MakeEasy.RestClient.Authenticators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class OAuth2HeaderAuthenticator : IAuthenticator
{
    public string Token { get; }
    public string Scheme { get; }

    public OAuth2HeaderAuthenticator(string token, string scheme = "OAuth")
    {
        Token = token;
        Scheme = scheme;
    }

    public Task Authenticate(HttpClient client, HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue(Scheme, Token);
        return Task.FromResult(0);
    }
}
