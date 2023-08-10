namespace Jackdaw.Messages;

internal interface IMessage
{
    ReadOnlyMemory<byte> Serialize(int correlationId);
}
