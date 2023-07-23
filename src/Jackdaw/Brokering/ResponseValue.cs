using System.Runtime.InteropServices;

namespace Jackdaw.Brokering;

[StructLayout(LayoutKind.Explicit)]
public struct ResponseValue
{
    [FieldOffset(0)]
    public ResponseException ResponseException;

    [FieldOffset(0)]
    public ResponseData ResponseData;
}
