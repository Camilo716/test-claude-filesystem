using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Tests.Controllers;

public class TodoItemsControllerTests : IDisposable
{
    private readonly TodoContext _context;
    private readonly TodoItemsController _controller;

    public TodoItemsControllerTests()
    {
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TodoContext(options);
        var logger = new Mock<ILogger<TodoItemsController>>();
        _controller = new TodoItemsController(_context, logger.Object);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task GetTodoItems_ReturnsAllItems()
    {
        // Arrange
        var items = new List<TodoItem>
        {
            new TodoItem { Title = "Test Item 1", Description = "Description 1" },
            new TodoItem { Title = "Test Item 2", Description = "Description 2" }
        };
        _context.TodoItems.AddRange(items);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetTodoItems();

        // Assert
        result.Value.Should().HaveCount(2);
        result.Value.Should().Contain(x => x.Title == "Test Item 1");
        result.Value.Should().Contain(x => x.Title == "Test Item 2");
    }

    [Fact]
    public async Task GetTodoItem_WithValidId_ReturnsItem()
    {
        // Arrange
        var item = new TodoItem { Title = "Test Item", Description = "Test Description" };
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetTodoItem(item.Id);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value.Title.Should().Be("Test Item");
        result.Value.Description.Should().Be("Test Description");
    }

    [Fact]
    public async Task GetTodoItem_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var result = await _controller.GetTodoItem(999);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task PostTodoItem_CreatesNewItem()
    {
        // Arrange
        var newItem = new TodoItem { Title = "New Item", Description = "New Description" };

        // Act
        var result = await _controller.PostTodoItem(newItem);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult!.Value.Should().BeEquivalentTo(newItem);
        
        var itemInDb = await _context.TodoItems.FindAsync(newItem.Id);
        itemInDb.Should().NotBeNull();
        itemInDb!.Title.Should().Be("New Item");
    }

    [Fact]
    public async Task PutTodoItem_WithValidId_UpdatesItem()
    {
        // Arrange
        var item = new TodoItem { Title = "Original Title", Description = "Original Description" };
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();

        item.Title = "Updated Title";
        item.Description = "Updated Description";

        // Act
        var result = await _controller.PutTodoItem(item.Id, item);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        
        var updatedItem = await _context.TodoItems.FindAsync(item.Id);
        updatedItem!.Title.Should().Be("Updated Title");
        updatedItem.Description.Should().Be("Updated Description");
    }

    [Fact]
    public async Task PutTodoItem_WithMismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var item = new TodoItem { Id = 1, Title = "Test", Description = "Test" };

        // Act
        var result = await _controller.PutTodoItem(2, item);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteTodoItem_WithValidId_RemovesItem()
    {
        // Arrange
        var item = new TodoItem { Title = "To Delete", Description = "Will be deleted" };
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteTodoItem(item.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        
        var deletedItem = await _context.TodoItems.FindAsync(item.Id);
        deletedItem.Should().BeNull();
    }

    [Fact]
    public async Task DeleteTodoItem_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var result = await _controller.DeleteTodoItem(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
