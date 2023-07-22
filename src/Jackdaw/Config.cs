using System.Collections;

namespace Jackdaw;

public class Config : IEnumerable<KeyValuePair<string, string>>
{
    private readonly Dictionary<string, string> properties = new();

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return properties.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
