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

    public static IEnumerable<NameValue> GetNameValues(object obj, params string[] includedProperties)
    {
        // automatically create parameters from object props
        if (obj is IDictionary dict) {
            foreach (var key in dict.Keys) {
                if (key == null) continue;
                var keyStr = key.ToString();
                if (keyStr == null) throw new Exception("Key is not permit to be null");
                yield return new NameValue(keyStr, dict[key]?.ToString());
            }
            yield break;
        }
        else {
            var type = obj.GetType();
            var props = type.GetProperties();

            foreach (var prop in props) {
                if (!IsAllowedProperty(prop.Name, includedProperties)) continue;

                var val = prop.GetValue(obj, null);
                if (val == null) continue;

                var propType = prop.PropertyType;
                if (propType.IsArray) {
                    var elementType = propType.GetElementType();
                    var array = (Array)val;

                    if (array.Length > 0 && elementType != null) {
                        // convert the array to an array of strings
                        var values = array.Cast<object>().Select(item => item.ToString());
                        yield return new NameValue(prop.Name, string.Join(",", values));

                        continue;
                    }
                }
                yield return new NameValue(prop.Name, val.ToString());
            }
        }
    }

    private static bool IsAllowedProperty(string propertyName, params string[] includedProperties)
        => includedProperties.Length == 0 || includedProperties.Length > 0 && includedProperties.Contains(propertyName);

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
