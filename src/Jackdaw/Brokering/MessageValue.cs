using System.Runtime.InteropServices;
using Jackdaw.Routing;

namespace Jackdaw.Brokering;

[StructLayout(LayoutKind.Explicit)]
public struct MessageValue
{
    [FieldOffset(0)]
    public TaskCompletionSource<RoutingTable> Promise;
}
