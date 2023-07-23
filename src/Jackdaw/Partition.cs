using Jackdaw.Brokering;

namespace Jackdaw;

public struct Partition : IEquatable<Partition>
{
    public static readonly Partition Any = new(-1);

    public Partition(int value)
    {
        Value = value;
    }

    public int Value { get; }

    internal INode Leader { get; }

    internal int Isr { get; }

    public override bool Equals(object obj)
    {
        if (obj is Partition partition)
        {
            return Equals(partition);
        }

        return false;
    }

    public bool Equals(Partition other)
    {
        return other.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        if (Value == -1)
        {
            return "[Any]";
        }

        return $"[{Value}]";
    }
}
