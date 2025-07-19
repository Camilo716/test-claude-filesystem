using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Tests.Integration;

public class TodoItemsIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TodoItemsIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTodoItems_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/todoitems");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostTodoItem_CreatesTodoItem()
    {
        // Arrange
        var newItem = new TodoItem
        {
            Title = "Integration Test Item",
            Description = "This is created by integration test"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/todoitems", newItem);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdItem = await response.Content.ReadFromJsonAsync<TodoItem>();
        createdItem.Should().NotBeNull();
        createdItem!.Title.Should().Be(newItem.Title);
        createdItem.Description.Should().Be(newItem.Description);
        createdItem.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetTodoItem_WithValidId_ReturnsTodoItem()
    {
        // Arrange
        var newItem = new TodoItem
        {
            Title = "Item to Get",
            Description = "Description for get test"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/todoitems", newItem);
        var createdItem = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        // Act
        var response = await _client.GetAsync($"/api/todoitems/{createdItem!.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var retrievedItem = await response.Content.ReadFromJsonAsync<TodoItem>();
        retrievedItem.Should().NotBeNull();
        retrievedItem!.Id.Should().Be(createdItem.Id);
        retrievedItem.Title.Should().Be(createdItem.Title);
    }

    [Fact]
    public async Task GetTodoItem_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/todoitems/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutTodoItem_UpdatesTodoItem()
    {
        // Arrange
        var newItem = new TodoItem
        {
            Title = "Original Title",
            Description = "Original Description"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/todoitems", newItem);
        var createdItem = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        createdItem!.Title = "Updated Title";
        createdItem.Description = "Updated Description";

        // Act
        var response = await _client.PutAsJsonAsync($"/api/todoitems/{createdItem.Id}", createdItem);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the update
        var getResponse = await _client.GetAsync($"/api/todoitems/{createdItem.Id}");
        var updatedItem = await getResponse.Content.ReadFromJsonAsync<TodoItem>();
        updatedItem!.Title.Should().Be("Updated Title");
        updatedItem.Description.Should().Be("Updated Description");
    }

    [Fact]
    public async Task DeleteTodoItem_RemovesTodoItem()
    {
        // Arrange
        var newItem = new TodoItem
        {
            Title = "Item to Delete",
            Description = "This will be deleted"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/todoitems", newItem);
        var createdItem = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/todoitems/{createdItem!.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/todoitems/{createdItem.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
