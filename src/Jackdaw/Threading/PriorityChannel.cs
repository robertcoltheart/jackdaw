using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;

namespace Jackdaw.Threading;

internal class PriorityChannel<T> : Channel<T>
    where T : IPrioritized
{
    private readonly Channel<bool> channel = Channel.CreateUnbounded<bool>(new UnboundedChannelOptions
    {
        AllowSynchronousContinuations = true,
        SingleReader = true,
        SingleWriter = false
    });

    private readonly ConcurrentQueue<T>[] queues;

    public PriorityChannel()
    {
        queues = new ConcurrentQueue<T>[Enum.GetNames(typeof(Priority)).Length];

        for (var i = 0; i < queues.Length; i++)
        {
            queues[i] = new ConcurrentQueue<T>();
        }

        Reader = new PriorityChannelReader(this);
        Writer = new PriorityChannelWriter(this);
    }

    private class PriorityChannelReader : ChannelReader<T>
    {
        private readonly PriorityChannel<T> parent;

        public PriorityChannelReader(PriorityChannel<T> parent)
        {
            this.parent = parent;
        }

        public override Task Completion => parent.channel.Reader.Completion;

        public override bool TryRead([MaybeNullWhen(false)] out T item)
        {
            if (parent.channel.Reader.TryRead(out _))
            {
                foreach (var queue in parent.queues)
                {
                    if (queue.TryDequeue(out item))
                    {
                        return true;
                    }
                }
            }

            item = default;

            return false;
        }

        public override ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken = default)
        {
            return parent.channel.Reader.WaitToReadAsync(cancellationToken);
        }
    }

    private class PriorityChannelWriter : ChannelWriter<T>
    {
        private readonly PriorityChannel<T> parent;

        public PriorityChannelWriter(PriorityChannel<T> parent)
        {
            this.parent = parent;
        }

        public override bool TryWrite(T item)
        {
            var result = parent.channel.Writer.TryWrite(true);

            if (result)
            {
                parent.queues[(int) item.Priority].Enqueue(item);
            }

            return result;
        }

        public override ValueTask<bool> WaitToWriteAsync(CancellationToken cancellationToken = default)
        {
            return parent.channel.Writer.WaitToWriteAsync(cancellationToken);
        }
    }
}
