namespace Jackdaw;

public class LogMessage
{
    public LogMessage(string name, SyslogLevel level, string facility, string message)
    {
        Name = name;
        Level = level;
        Facility = facility;
        Message = message;
    }

    public string Name { get; }

    public SyslogLevel Level { get; }

    public string Facility { get; }

    public string Message { get; }
}
