namespace Fossa.Messaging;

/// <summary>
/// Provides a mapping between message types and their corresponding integer identifiers for message serialization and
/// deserialization.
/// </summary>
/// <remarks>Use this class to resolve message type IDs to .NET types and vice versa when processing messages. The
/// mapping is fixed at construction and includes predefined message types such as CompanyChanged, EmployeeChanged, and
/// others. This class is typically used in scenarios where messages are identified by integer IDs and need to be
/// associated with their .NET type for further processing.</remarks>
public class MessageMap
{
    private readonly BiMap<ComparableType, int> messageTypeBiMap;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMap"/> class.
    /// </summary>
    public MessageMap()
    {
        BiMap<ComparableType, int> xMessageTypeBiMap = [];

        RegisterMessageTypes(ref xMessageTypeBiMap);
        this.messageTypeBiMap = xMessageTypeBiMap;
    }

    /// <summary>
    /// Gets the message type associated with the specified message type ID.
    /// </summary>
    /// <param name="messageTypeID">The ID of the message for which to obtain the identifier.</param>
    /// <returns>The <see cref="Type"/> corresponding to the given message type ID.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the specified message type ID does not have an associated type.</exception>
    public Type GetMessageType(int messageTypeID) =>
        this.messageTypeBiMap.Find(messageTypeID)
            .IfNone(() => throw new KeyNotFoundException($"Message type not found for ID {messageTypeID}"));

    /// <summary>
    /// Retrieves the unique identifier associated with the specified message type.
    /// </summary>
    /// <param name="messageType">The type of the message for which to obtain the identifier. Cannot be null.</param>
    /// <returns>The identifier corresponding to the specified message type.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the specified message type does not have an associated identifier.</exception>
    public int GetMessageTypeID(Type messageType) =>
        this.messageTypeBiMap.Find(messageType)
            .IfNone(() => throw new KeyNotFoundException($"Message type ID not found for type {messageType.FullName}"));

    private static void RegisterMessageType<TKey>(int value, ref BiMap<ComparableType, int> messageTypeBiMap) =>
        messageTypeBiMap = messageTypeBiMap.Add(typeof(TKey), value);

    private static void RegisterMessageTypes(ref BiMap<ComparableType, int> messageTypeBiMap)
    {
        RegisterMessageType<CompanyChanged>(64169988, ref messageTypeBiMap);
        RegisterMessageType<CompanyDeleted>(64169993, ref messageTypeBiMap);
        RegisterMessageType<EmployeeChanged>(64171400, ref messageTypeBiMap);
        RegisterMessageType<EmployeeDeleted>(64171404, ref messageTypeBiMap);
        RegisterMessageType<BranchChanged>(64171407, ref messageTypeBiMap);
        RegisterMessageType<BranchDeleted>(64171411, ref messageTypeBiMap);
        RegisterMessageType<DepartmentChanged>(64171414, ref messageTypeBiMap);
        RegisterMessageType<DepartmentDeleted>(64171418, ref messageTypeBiMap);
    }
}
