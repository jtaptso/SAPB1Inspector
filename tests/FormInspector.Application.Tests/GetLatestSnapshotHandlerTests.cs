using FormInspector.Application.DTOs;
using FormInspector.Application.Interfaces;
using FormInspector.Application.UseCases.GetLatestSnapshot;
using FormInspector.Domain.Enums;
using FormInspector.Domain.Snapshots;
using FormInspector.Domain.ValueObjects;
using Moq;

namespace FormInspector.Application.Tests;

public class GetLatestSnapshotHandlerTests
{
    private readonly Mock<ISnapshotRepository> _repositoryMock;
    private readonly GetLatestSnapshotHandler _handler;

    public GetLatestSnapshotHandlerTests()
    {
        _repositoryMock = new Mock<ISnapshotRepository>();
        _handler = new GetLatestSnapshotHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_GetLatest_FormTypeExists_ReturnsSnapshot()
    {
        // Arrange
        var snapshot = CreateTestSnapshot();
        _repositoryMock.Setup(r => r.GetLatestAsync("139")).ReturnsAsync(snapshot);

        var query = new GetLatestSnapshotQuery { FormType = "139" };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("snap-001", result.SnapshotId);
        Assert.Equal("139", result.Form.FormType);
    }

    [Fact]
    public async Task HandleAsync_GetLatest_FormTypeNotFound_ReturnsNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetLatestAsync("999")).ReturnsAsync((Snapshot?)null);

        var query = new GetLatestSnapshotQuery { FormType = "999" };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_GetAll_ReturnsAllSnapshotsAsSummaries()
    {
        // Arrange
        var snapshots = new List<Snapshot> { CreateTestSnapshot() };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(snapshots);

        var query = new GetAllSnapshotsQuery();

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.Single(result);
        Assert.Equal("139", result[0].FormType);
    }

    [Fact]
    public async Task HandleAsync_GetById_ExistingId_ReturnsSnapshot()
    {
        // Arrange
        var snapshot = CreateTestSnapshot();
        _repositoryMock.Setup(r => r.GetByIdAsync("snap-001")).ReturnsAsync(snapshot);

        var query = new GetSnapshotByIdQuery { SnapshotId = "snap-001" };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("snap-001", result.SnapshotId);
    }

    [Fact]
    public async Task HandleAsync_GetById_NonExistingId_ReturnsNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync("nonexistent")).ReturnsAsync((Snapshot?)null);

        var query = new GetSnapshotByIdQuery { SnapshotId = "nonexistent" };

        // Act
        var result = await _handler.HandleAsync(query);

        // Assert
        Assert.Null(result);
    }

    private static Snapshot CreateTestSnapshot()
    {
        return new Snapshot(
            "snap-001",
            DateTime.UtcNow,
            new SnapshotContext("manager", "WS01"),
            new FormMetadata(new FormType("139"), "1", "Sales Order", FormMode.Ok, 1),
            new List<ItemMetadata>
            {
                new("4", new ItemType("EditText"), new Layout(10, 20, 100, 30))
            });
    }
}
