namespace MakeEasy.RestClient.Serializers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class JsonResponseContentDeserializer : IResponseContentDeserializer
{
    /// <inheritdoc/>
    public T? Deserialize<T>(string? content, HttpResponseMessage response)
    {
        if (response == null) throw new ArgumentNullException(nameof(response));
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent) {
            return default;
        }
        if (string.IsNullOrEmpty(content)) {
            return default;
        }

        return Deserialize<T>(content!);
    }

#if NET5_0_OR_GREATER
    private T? Deserialize<T>(string content)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(content, OptionsOfCaseInsensitive);
    }

    private static readonly System.Text.Json.JsonSerializerOptions OptionsOfCaseInsensitive =
        new System.Text.Json.JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
#else
    private T? Deserialize<T>(string content)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
    }
#endif
}
