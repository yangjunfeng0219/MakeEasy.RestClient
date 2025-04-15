namespace MakeEasy.RestClient.Authenticators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class JwtAuthenticator : IAuthenticator
{
    public string Token { get; }
    public string Scheme { get; }

    public JwtAuthenticator(string token, string scheme = "Bearer")
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
