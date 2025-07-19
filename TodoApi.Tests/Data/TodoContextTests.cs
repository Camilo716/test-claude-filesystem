using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Tests.Data;

public class TodoContextTests : IDisposable
{
    private TodoContext _context;

    public TodoContextTests()
    {
        var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TodoContext(options);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task TodoContext_CanAddAndRetrieveTodoItem()
    {
        // Arrange
        var todoItem = new TodoItem
        {
            Title = "Test Todo",
            Description = "Test Description"
        };

        // Act
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        // Assert
        var savedItem = await _context.TodoItems.FirstOrDefaultAsync(t => t.Title == "Test Todo");
        savedItem.Should().NotBeNull();
        savedItem!.Description.Should().Be("Test Description");
    }

    [Fact]
    public async Task TodoContext_SetsTimestampsOnAdd()
    {
        // Arrange
        var todoItem = new TodoItem
        {
            Title = "Timestamp Test",
            Description = "Testing timestamps"
        };

        // Act
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        // Assert
        todoItem.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        todoItem.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        todoItem.CreatedAt.Should().BeCloseTo(todoItem.UpdatedAt, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task TodoContext_UpdatesTimestampOnModify()
    {
        // Arrange
        var todoItem = new TodoItem
        {
            Title = "Update Test",
            Description = "Initial Description"
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        var initialCreatedAt = todoItem.CreatedAt;
        var initialUpdatedAt = todoItem.UpdatedAt;

        // Wait a bit to ensure timestamp difference
        await Task.Delay(100);

        // Act
        todoItem.Description = "Updated Description";
        await _context.SaveChangesAsync();

        // Assert
        todoItem.CreatedAt.Should().Be(initialCreatedAt);
        todoItem.UpdatedAt.Should().BeAfter(initialUpdatedAt);
    }

    [Fact]
    public async Task TodoContext_CanDeleteTodoItem()
    {
        // Arrange
        var todoItem = new TodoItem
        {
            Title = "Delete Test",
            Description = "To be deleted"
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        // Act
        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        // Assert
        var deletedItem = await _context.TodoItems.FindAsync(todoItem.Id);
        deletedItem.Should().BeNull();
    }

    [Fact]
    public async Task TodoContext_CanHandleMultipleTodoItems()
    {
        // Arrange
        var items = new List<TodoItem>
        {
            new TodoItem { Title = "Item 1", Description = "Description 1" },
            new TodoItem { Title = "Item 2", Description = "Description 2" },
            new TodoItem { Title = "Item 3", Description = "Description 3" }
        };

        // Act
        _context.TodoItems.AddRange(items);
        await _context.SaveChangesAsync();

        // Assert
        var count = await _context.TodoItems.CountAsync();
        count.Should().Be(3);

        var allItems = await _context.TodoItems.ToListAsync();
        allItems.Should().HaveCount(3);
        allItems.Should().Contain(i => i.Title == "Item 1");
        allItems.Should().Contain(i => i.Title == "Item 2");
        allItems.Should().Contain(i => i.Title == "Item 3");
    }
}
