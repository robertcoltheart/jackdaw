namespace Jackdaw.Configuration.Evaluation;

internal readonly struct Range
{
    public Range(int start, int end)
    {
        Start = start;
        End = end;
    }

    public int Start { get; }

    public int End { get; }

    public override string ToString()
    {
        return $"{Start}..{End}";
    }
}
