namespace Fossa.Messaging.Test;

public class ComparableTypeTests
{
    private readonly Type type1 = typeof(string);
    private readonly Type type2 = typeof(int);

    [Fact]
    public void ImplicitConversion_FromType_ShouldWork()
    {
        // Arrange
        ComparableType comparableType = this.type1;

        // Assert
        Assert.Equal(this.type1, comparableType.Value);
    }

    [Fact]
    public void ImplicitConversion_ToType_ShouldWork()
    {
        // Act
        Type type = new ComparableType(this.type1);

        // Assert
        Assert.Equal(this.type1, type);
    }

    [Fact]
    public void FromType_ShouldWork()
    {
        // Arrange
        var comparableType = ComparableType.FromType(this.type1);

        // Assert
        Assert.Equal(this.type1, comparableType.Value);
    }

    [Fact]
    public void ToType_ShouldWork()
    {
        // Arrange
        var comparableType = new ComparableType(this.type1);

        // Act
        var type = comparableType.ToType();

        // Assert
        Assert.Equal(this.type1, type);
    }

    [Fact]
    public void EqualityOperators_ShouldWork()
    {
        // Arrange
        var comparableType1 = new ComparableType(this.type1);
        var comparableType2 = new ComparableType(this.type1);
        var comparableType3 = new ComparableType(this.type2);

        // Assert
        Assert.True(comparableType1 == comparableType2);
        Assert.False(comparableType1 == comparableType3);
        Assert.False(comparableType1 != comparableType2);
        Assert.True(comparableType1 != comparableType3);
    }

    [Fact]
    public void ComparisonOperators_ShouldWork()
    {
        // Arrange
        var comparableType1 = new ComparableType(typeof(string));
        var comparableType2 = new ComparableType(typeof(int));

        // Assert
        Assert.True(comparableType1 > comparableType2);
        Assert.True(comparableType1 >= comparableType2);
        Assert.False(comparableType1 < comparableType2);
        Assert.False(comparableType1 <= comparableType2);
    }

    [Fact]
    public void Equals_ShouldWork()
    {
        // Arrange
        var comparableType1 = new ComparableType(this.type1);
        var comparableType2 = new ComparableType(this.type1);
        var comparableType3 = new ComparableType(this.type2);

        // Assert
        Assert.True(comparableType1.Equals(comparableType2));
        Assert.False(comparableType1.Equals(comparableType3));
        Assert.True(comparableType1.Equals((object)comparableType2));
        Assert.False(comparableType1.Equals((object)comparableType3));
    }

    [Fact]
    public void CompareTo_ShouldWork()
    {
        // Arrange
        var comparableType1 = new ComparableType(typeof(string));
        var comparableType2 = new ComparableType(typeof(int));
        var comparableType3 = new ComparableType(typeof(string));

        // Assert
        Assert.True(comparableType1.CompareTo(comparableType2) > 0);
        Assert.True(comparableType2.CompareTo(comparableType1) < 0);
        Assert.Equal(0, comparableType1.CompareTo(comparableType3));
    }

    [Fact]
    public void CompareTo_WithObject_ShouldWork()
    {
        // Arrange
        var comparableType1 = new ComparableType(typeof(string));
        var comparableType2 = new ComparableType(typeof(int));
        var comparableType3 = new ComparableType(typeof(string));

        // Assert
        Assert.True(comparableType1.CompareTo((object)comparableType2) > 0);
        Assert.True(comparableType2.CompareTo((object)comparableType1) < 0);
        Assert.Equal(0, comparableType1.CompareTo((object)comparableType3));
        _ = Assert.Throws<ArgumentException>(() => comparableType1.CompareTo("not a comparable type"));
    }

    [Fact]
    public void GetHashCode_ShouldWork()
    {
        // Arrange
        var comparableType1 = new ComparableType(this.type1);
        var comparableType2 = new ComparableType(this.type1);
        var comparableType3 = new ComparableType(this.type2);

        // Assert
        Assert.Equal(comparableType1.GetHashCode(), comparableType2.GetHashCode());
        Assert.NotEqual(comparableType1.GetHashCode(), comparableType3.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldWork()
    {
        // Arrange
        var comparableType = new ComparableType(this.type1);

        // Assert
        Assert.Equal(this.type1.FullName, comparableType.ToString());
    }
}
