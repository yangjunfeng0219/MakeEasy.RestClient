namespace MakeEasy.RestClient.Serializers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public interface IResponseContentDeserializer
{
    T? Deserialize<T>(string? content, HttpResponseMessage response);
}
