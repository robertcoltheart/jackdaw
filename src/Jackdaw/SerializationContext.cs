namespace Jackdaw;

public struct SerializationContext
{
    public static SerializationContext Empty { get; } = new();

    public SerializationContext(MessageComponentType component, string topic, Headers? headers = null)
    {
        Component = component;
        Topic = topic;
        Headers = headers;
    }

    public MessageComponentType Component { get; }

    public string Topic { get; }

    public Headers? Headers { get; }
}
