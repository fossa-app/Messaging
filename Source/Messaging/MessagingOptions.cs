namespace Fossa.Messaging;

/// <summary>
/// Represents the options for messaging.
/// </summary>
public class MessagingOptions
{
    /// <summary>
    /// Gets the actor.
    /// </summary>
    public Dictionary<string, string>? Actor { get; init; }

    /// <summary>
    /// Gets the topic.
    /// </summary>
    public string? Topic { get; init; }
}
