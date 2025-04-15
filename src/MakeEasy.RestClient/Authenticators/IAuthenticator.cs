namespace MakeEasy.RestClient.Authenticators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public interface IAuthenticator
{
    Task Authenticate(HttpClient client, HttpRequestMessage request);
}
