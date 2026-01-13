namespace Fossa.Messaging.Test;

using System.Collections.Generic;

public class MessageMapTests
{

    [Fact]
    public void GetMessageType_WithRegisteredId_ShouldReturnCorrectType()
    {
        // Arrange
        MessageMap messageMap = new();
        var expectedType = typeof(CompanyChanged);
        const int messageId = 64169988;

        // Act
        var actualType = messageMap.GetMessageType(messageId);

        // Assert
        Assert.Equal(expectedType, actualType);
    }

    [Fact]
    public void GetMessageType_WithUnregisteredId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        MessageMap messageMap = new();
        const int unregisteredId = 999;

        // Act & Assert
        _ = Assert.Throws<KeyNotFoundException>(() => messageMap.GetMessageType(unregisteredId));
    }

    [Fact]
    public void GetMessageTypeID_WithRegisteredType_ShouldReturnCorrectId()
    {
        // Arrange
        MessageMap messageMap = new();
        var messageType = typeof(CompanyChanged);
        const int expectedId = 64169988;

        // Act
        var actualId = messageMap.GetMessageTypeID(messageType);

        // Assert
        Assert.Equal(expectedId, actualId);
    }

    [Fact]
    public void GetMessageTypeID_WithUnregisteredType_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        MessageMap messageMap = new();
        var unregisteredType = typeof(string);

        // Act & Assert
        _ = Assert.Throws<KeyNotFoundException>(() => messageMap.GetMessageTypeID(unregisteredType));
    }

    [Theory]
    [InlineData(typeof(CompanyChanged), 64169988)]
    [InlineData(typeof(CompanyDeleted), 64169993)]
    [InlineData(typeof(EmployeeChanged), 64171400)]
    [InlineData(typeof(EmployeeDeleted), 64171404)]
    [InlineData(typeof(BranchChanged), 64171407)]
    [InlineData(typeof(BranchDeleted), 64171411)]
    [InlineData(typeof(DepartmentChanged), 64171414)]
    [InlineData(typeof(DepartmentDeleted), 64171418)]
    public void RegisterMessageTypes_ShouldRegisterAllTypes(Type messageType, int expectedId)
    {
        // Arrange
        MessageMap messageMap = new();

        // Act & Assert
        Assert.Equal(messageType, messageMap.GetMessageType(expectedId));
        Assert.Equal(expectedId, messageMap.GetMessageTypeID(messageType));
    }
}
