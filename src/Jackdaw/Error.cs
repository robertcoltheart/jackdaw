namespace Jackdaw;

public class Error
{
    private readonly string reason;

    public Error(ErrorCode code)
    {

    }

    public ErrorCode Code { get; }

    public bool IsFatal { get; }

    public string Reason => ToString();

    public override string ToString()
    {
        return !string.IsNullOrEmpty(reason)
            ? reason
            : Code.GetReason();
    }
}
