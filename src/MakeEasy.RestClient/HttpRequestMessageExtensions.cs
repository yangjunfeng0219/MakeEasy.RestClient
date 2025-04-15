namespace MakeEasy.RestClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static MakeEasy.RestClient.RestUtils;

public static class HttpRequestMessageExtensions
{
    public static void AddOrUpdateHeaders(this HttpRequestMessage request, object? headers)
    {
        if (headers == null) return;

        var props = GetNameValues(headers);
        foreach (var nameValue in props) {
            request.AddOrUpdateHeader(nameValue.Name, nameValue.Value);
        }
    }

    public static void AddOrUpdateHeader(this HttpRequestMessage request, string name, string? value)
    {
        if (!request.Headers.TryAddWithoutValidation(name, value)) {
            request.Headers.Remove(name);
            request.Headers.Add(name, value);
        }
    }
}
