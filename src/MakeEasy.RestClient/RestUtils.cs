namespace MakeEasy.RestClient;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public static class RestUtils
{
    public static string DecodeUrl(string input)
        => HttpUtility.UrlDecode(input);

    public static string EncodeUrl(string input, Encoding? encoding = null)
    {
        if (encoding == null) encoding = Encoding.UTF8;
        var encoded = HttpUtility.UrlEncode(input, encoding);
        return encoded.Replace("+", "%20");
    }

    public static string BuildQueryUrl(string name, string? value)
    {
        if (string.IsNullOrEmpty(name)) return string.Empty;
        var nameStr = EncodeUrl(name);
        var valueStr = value != null ? EncodeUrl(value) : value;
        return valueStr == null ? nameStr : $"{nameStr}={valueStr}";
    }

    public static string BuildQueryUrl(string url, string name, string? value)
    {
        var query = BuildQueryUrl(name, value);
        return url.IndexOf('?') > 0 ? $"{url}&{query}" : $"{url}?{query}";
    }

    public static string BuildQueryUrl(object? queryParameters)
    {
        if (queryParameters == null) return string.Empty;
        var nvList = GetNameValues(queryParameters)
            .Select(e => BuildQueryUrl(e.Name, e.Value));
        return string.Join("&", nvList);
    }

    public static string BuildQueryUrl(string url, object? queryParameters)
    {
        if (queryParameters == null) return url;
        var query = BuildQueryUrl(queryParameters);
        return url.IndexOf('?') > 0 ? $"{url}&{query}" : $"{url}?{query}";
    }

    public static string BuildSegmentUrl(string url, object? segmentParameters)
    {
        if (segmentParameters == null) return url;

        var nvList = GetNameValues(segmentParameters);
        foreach (var nv in nvList) {
            if (nv.Value == null) throw new Exception("value in segment can't be null");
            url = url.Replace($"{{{nv.Name}}}", EncodeUrl(nv.Value));
        }
        return url;
    }

    public static IEnumerable<NameValue> GetNameValues(object obj)
    {
        // automatically create parameters from object props
        if (obj is IDictionary dict) {
            foreach (var key in dict.Keys) {
                var keyStr = key?.ToString();
                if (keyStr == null) continue;

                var val = dict[key!];
                if (val == null) continue;
                yield return new NameValue(keyStr, GetValueString(val));
            }
            yield break;
        }
        else {
            foreach (var prop in obj.GetType().GetProperties()) {
                var val = prop.GetValue(obj, null);
                if (val == null) continue;
                yield return new NameValue(prop.Name, GetValueString(val));
            }
        }
    }

    private static string GetValueString(object value)
    {
        if (value is string str) return str;

        if (value is Array arr) {
            var sb = new StringBuilder();
            foreach (var item in arr) {
                if (sb.Length > 0) sb.Append(",");
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }
        return value.ToString()!;
    }

    public struct NameValue
    {
        public string Name { get; set; }
        public string? Value { get; set; }

        public NameValue(string name, string? value)
        {
            Name = name;
            Value = value;
        }
    }
}
