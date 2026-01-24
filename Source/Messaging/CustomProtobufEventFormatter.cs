namespace Fossa.Messaging;

using CloudNative.CloudEvents;
using CloudNative.CloudEvents.Protobuf;
using Google.Protobuf;

/// <summary>
/// A custom protobuf event formatter that handles IMessage data.
/// </summary>
public class CustomProtobufEventFormatter : ProtobufEventFormatter
{
    /// <inheritdoc />
    protected override void EncodeStructuredModeData(CloudEvent cloudEvent, CloudNative.CloudEvents.V1.CloudEvent proto)
    {
        ArgumentNullException.ThrowIfNull(cloudEvent);
        ArgumentNullException.ThrowIfNull(proto);

        if (cloudEvent.Data is IMessage message)
        {
            proto.BinaryData = message.ToByteString();
        }
        else
        {
            base.EncodeStructuredModeData(cloudEvent, proto);
        }
    }
}
