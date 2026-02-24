namespace Fossa.Messaging;

/// <summary>
/// Represents the options for a messaging actor entry.
/// </summary>
public class MessagingActorEntryOptions
{
    /// <summary>
    /// Gets or sets the key.
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets the plain text value.
    /// </summary>
    public string? PlainTextValue { get; set; }

    /// <summary>
    /// Gets or sets the Base64 value.
    /// </summary>
    public string? Base64Value { get; set; }
}
