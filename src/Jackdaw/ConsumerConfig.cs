namespace Jackdaw;

public class ConsumerConfig : ClientConfig
{
    public AutoOffsetReset AutoOffsetReset { get; set; }

    public string GroupId { get; set; }
}
