namespace Jackdaw;

public class ConsumerConfig
{
    public string BootstrapServers { get; set; }

    public AutoOffsetReset AutoOffsetReset { get; set; }

    public string GroupId { get; set; }
}
