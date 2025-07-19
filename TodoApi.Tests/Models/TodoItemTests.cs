using FluentAssertions;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Tests.Models;

public class TodoItemTests
{
    [Fact]
    public void TodoItem_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var todoItem = new TodoItem();

        // Assert
        todoItem.Id.Should().Be(0);
        todoItem.Title.Should().Be(string.Empty);
        todoItem.Description.Should().Be(string.Empty);
        todoItem.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void TodoItem_SetProperties_WorksCorrectly()
    {
        // Arrange
        var todoItem = new TodoItem();
        var testDate = DateTime.UtcNow;

        // Act
        todoItem.Id = 1;
        todoItem.Title = "Test Title";
        todoItem.Description = "Test Description";
        todoItem.IsCompleted = true;
        todoItem.CreatedAt = testDate;
        todoItem.UpdatedAt = testDate;

        // Assert
        todoItem.Id.Should().Be(1);
        todoItem.Title.Should().Be("Test Title");
        todoItem.Description.Should().Be("Test Description");
        todoItem.IsCompleted.Should().BeTrue();
        todoItem.CreatedAt.Should().Be(testDate);
        todoItem.UpdatedAt.Should().Be(testDate);
    }

    [Theory]
    [InlineData("Short title", "Short description")]
    [InlineData("A very long title that contains many characters to test the limits", "A very long description that contains many characters and should be handled properly by the model")]
    [InlineData("", "")]
    public void TodoItem_WithVariousLengths_HandlesCorrectly(string title, string description)
    {
        // Arrange & Act
        var todoItem = new TodoItem
        {
            Title = title,
            Description = description
        };

        // Assert
        todoItem.Title.Should().Be(title);
        todoItem.Description.Should().Be(description);
    }
}
