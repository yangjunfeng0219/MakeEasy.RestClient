namespace MakeEasy.RestClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class SendOptions
{
    public object? QueryParameters { get; set; } = null;
    public object? SegmentParameters { get; set; } = null;
    public object? Headers { get; set; } = null;
    public IRequestBodySerializer? RequestBodySerializer { get; set; } = null;
    public IResponseContentDeserializer? ResponseContentDeserializer { get; set; } = null;
    public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
}
