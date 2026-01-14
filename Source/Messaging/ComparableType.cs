namespace Fossa.Messaging;

/// <summary>
/// A wrapper around <see cref="Type"/> that provides comparability.
/// </summary>
/// <param name="value">The <see cref="Type"/> to wrap.</param>
public readonly struct ComparableType(Type value) : IComparable<ComparableType>, IEquatable<ComparableType>, IComparable
{
    /// <summary>
    /// Gets the underlying <see cref="Type"/>.
    /// </summary>
    public readonly Type Value { get; } = value;

    /// <summary>
    /// Implicitly converts a <see cref="Type"/> to a <see cref="ComparableType"/>.
    /// </summary>
    /// <param name="t">The <see cref="Type"/>.</param>
    public static implicit operator ComparableType(Type t) => new(t);

    /// <summary>
    /// Implicitly converts a <see cref="ComparableType"/> to a <see cref="Type"/>.
    /// </summary>
    /// <param name="c">The <see cref="ComparableType"/>.</param>
    public static implicit operator Type(ComparableType c) => c.Value;

    /// <summary>
    /// Compares two <see cref="ComparableType"/> values for equality.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableType"/>.</param>
    /// <param name="right">The right <see cref="ComparableType"/>.</param>
    /// <returns><c>true</c> if the values are equal, <c>false</c> otherwise.</returns>
    public static bool operator ==(ComparableType left, ComparableType right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="ComparableType"/> values for inequality.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableType"/>.</param>
    /// <param name="right">The right <see cref="ComparableType"/>.</param>
    /// <returns><c>true</c> if the values are not equal, <c>false</c> otherwise.</returns>
    public static bool operator !=(ComparableType left, ComparableType right) => !(left == right);

    /// <summary>
    /// Compares two <see cref="ComparableType"/> values to determine which is less.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableType"/>.</param>
    /// <param name="right">The right <see cref="ComparableType"/>.</param>
    /// <returns><c>true</c> if the left value is less than the right value, <c>false</c> otherwise.</returns>
    public static bool operator <(ComparableType left, ComparableType right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Compares two <see cref="ComparableType"/> values to determine if the left is less than or equal to the right.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableType"/>.</param>
    /// <param name="right">The right <see cref="ComparableType"/>.</param>
    /// <returns><c>true</c> if the left value is less than or equal to the right value, <c>false</c> otherwise.</returns>
    public static bool operator <=(ComparableType left, ComparableType right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Compares two <see cref="ComparableType"/> values to determine which is greater.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableType"/>.</param>
    /// <param name="right">The right <see cref="ComparableType"/>.</param>
    /// <returns><c>true</c> if the left value is greater than the right value, <c>false</c> otherwise.</returns>
    public static bool operator >(ComparableType left, ComparableType right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Compares two <see cref="ComparableType"/> values to determine if the left is greater than or equal to the right.
    /// </summary>
    /// <param name="left">The left <see cref="ComparableType"/>.</param>
    /// <param name="right">The right <see cref="ComparableType"/>.</param>
    /// <returns><c>true</c> if the left value is greater than or equal to the right value, <c>false</c> otherwise.</returns>
    public static bool operator >=(ComparableType left, ComparableType right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Converts a <see cref="Type"/> to a <see cref="ComparableType"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to convert.</param>
    /// <returns>A <see cref="ComparableType"/>.</returns>
    public static ComparableType FromType(Type type) => new(type);

    /// <inheritdoc/>
    public int CompareTo(ComparableType other) =>
        string.Compare(this.Value.FullName, other.Value.FullName, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public int CompareTo(object? obj) =>
        obj switch
        {
            null => 1,
            ComparableType other => this.CompareTo(other),
            _ => throw new ArgumentException($"Object is not a {nameof(ComparableType)}", nameof(obj)),
        };

    /// <inheritdoc/>
    public bool Equals(ComparableType other) =>
            this.Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is ComparableType other && this.Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() =>
        this.Value?.GetHashCode() ?? 0;

    /// <inheritdoc/>
    public override string ToString() => this.Value.FullName ?? string.Empty;

    /// <summary>
    /// Converts a <see cref="ComparableType"/> to a <see cref="Type"/>.
    /// </summary>
    /// <returns>The underlying <see cref="Type"/>.</returns>
    public Type ToType() => this.Value;
}
