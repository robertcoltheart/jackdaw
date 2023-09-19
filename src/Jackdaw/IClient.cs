namespace Jackdaw;

public interface IClient : IDisposable
{
    string Name { get; }
}
