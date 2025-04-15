namespace MakeEasy.RestClient.Authenticators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class HttpBasicAuthenticator : IAuthenticator
{
    public string UserName { get; }
    public string Password { get; }
    public Encoding Encoding { get; }

    public HttpBasicAuthenticator(string username, string password, Encoding encoding)
    {
        UserName = username;
        Password = password;
        Encoding = encoding;
    }

    public HttpBasicAuthenticator(string username, string password)
        : this(username, password, Encoding.UTF8)
    {
    }

    public Task Authenticate(HttpClient client, HttpRequestMessage request)
    {
        client.DefaultRequestHeaders.Authorization = new  AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.GetBytes($"{UserName}:{Password}")));
        return Task.FromResult(0);
    }
}
