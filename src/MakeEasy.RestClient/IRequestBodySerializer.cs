namespace MakeEasy.RestClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public interface IRequestBodySerializer
{
    HttpContent? Serialize<T>(T? body);
}
