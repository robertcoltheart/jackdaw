namespace Jackdaw;

public class ConsumeException : KafkaException
{
    public ConsumeException(ConsumeResult<byte[], byte[]> consumerRecord, Error error)
        : base(error)
    {
        this.ConsumerRecord = consumerRecord;
    }

    public ConsumeResult<byte[], byte[]> ConsumerRecord { get; }
}
