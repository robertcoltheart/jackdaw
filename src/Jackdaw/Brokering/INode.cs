using Jackdaw.Protocol;

namespace Jackdaw.Brokering;

public interface INode
{
    Task<MetadataResponse> FetchMetadata();
}
