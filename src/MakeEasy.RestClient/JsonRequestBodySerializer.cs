namespace MakeEasy.RestClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class JsonRequestBodySerializer : IRequestBodySerializer
{
    /// <inheritdoc/>
    public HttpContent? Serialize<T>(T? body)
    {
        string jsonStr;
#if NET5_0_OR_GREATER
        jsonStr = System.Text.Json.JsonSerializer.Serialize(body);
#else
        jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(body);
#endif

        return new StringContent(jsonStr, Encoding.UTF8, RestContentTypes.Json);
    }
}
