using System.Runtime.Serialization;

namespace Jackdaw;

public enum SecurityProtocol
{
    [EnumMember(Value = "plaintext")]
    Plaintext,

    [EnumMember(Value = "ssl")]
    Ssl,

    [EnumMember(Value = "sasl_plaintext")]
    SaslPlaintext,

    [EnumMember(Value = "sasl_ssl")]
    SaslSsl
}
