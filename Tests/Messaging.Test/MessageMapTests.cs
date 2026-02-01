namespace Fossa.Messaging.Test;

using System.Collections.Generic;
using Fossa.Messaging.Messages.Events;

public class MessageMapTests
{

    [Fact]
    public void GetMessageType_WithRegisteredId_ShouldReturnCorrectType()
    {
        // Arrange
        MessageMap messageMap = new();
        var expectedType = typeof(CompanyChangedProtoEvent);
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
        var messageType = typeof(CompanyChangedProtoEvent);
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
    [InlineData(typeof(CompanyChangedProtoEvent), 64169988)]
    [InlineData(typeof(CompanyDeletedProtoEvent), 64169993)]
    [InlineData(typeof(EmployeeChangedProtoEvent), 64171400)]
    [InlineData(typeof(EmployeeDeletedProtoEvent), 64171404)]
    [InlineData(typeof(BranchChangedProtoEvent), 64171407)]
    [InlineData(typeof(BranchDeletedProtoEvent), 64171411)]
    [InlineData(typeof(DepartmentChangedProtoEvent), 64171414)]
    [InlineData(typeof(DepartmentDeletedProtoEvent), 64171418)]
    public void RegisterMessageTypes_ShouldRegisterAllTypes(Type messageType, int expectedId)
    {
        // Arrange
        MessageMap messageMap = new();

        // Act & Assert
        Assert.Equal(messageType, messageMap.GetMessageType(expectedId));
        Assert.Equal(expectedId, messageMap.GetMessageTypeID(messageType));
    }
}
