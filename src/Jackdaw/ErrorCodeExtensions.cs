namespace Jackdaw;

public static class ErrorCodeExtensions
{
    private static readonly Dictionary<ErrorCode, string> Reasons = new()
    {
        {ErrorCode.Local_BadMsg, "Local: Bad message format"}
    };

    public static string GetReason(this ErrorCode code)
    {
        return Reasons[code];
    }
}
