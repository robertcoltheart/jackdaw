using Jackdaw.IO;

namespace Jackdaw.Brokering;

public struct ResponseData
{
    public ReusableMemoryStream Data;

    public int CorrelationId;
}
