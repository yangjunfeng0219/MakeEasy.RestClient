namespace MakeEasy.RestClient;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

    public static string BuildQueryUrl(object? queryParameters)
    {
        if (queryParameters == null) return string.Empty;
        var nvList = GetNameValues(queryParameters)
            .Select(e => {
                var name = e.Name != null ? EncodeUrl(e.Name) : e.Name;
                var val = e.Value != null ? EncodeUrl(e.Value) : e.Value;
                return val == null ? name : $"{name}={val}";
            });
        return string.Join("&", nvList);
    }

    public static string BuildQueryUrl(string url, object? queryParameters)
    {
        if (queryParameters == null) return url;
        return $"{url}?{BuildQueryUrl(queryParameters)}";
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
