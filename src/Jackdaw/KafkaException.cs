namespace Jackdaw;

public class KafkaException : Exception
{
    public KafkaException(ErrorCode code)
        : base(code.GetReason())
    {
        Error = new Error(code);
    }

    public KafkaException(Error error)
        : base(error.ToString())
    {
        Error = error;
    }

    public KafkaException(Error error, Exception innerException)
        : base(error.Reason, innerException)
    {
        Error = error;
    }

    public Error Error { get; }
}
