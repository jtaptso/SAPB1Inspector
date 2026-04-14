using FormInspector.Domain.Enums;
using FormInspector.Domain.Snapshots;
using FormInspector.Domain.ValueObjects;
using FormInspector.Infrastructure.Persistence;

namespace FormInspector.Infrastructure.Tests;

public class InMemorySnapshotRepositoryTests
{
    private readonly InMemorySnapshotRepository _repository;

    public InMemorySnapshotRepositoryTests()
    {
        _repository = new InMemorySnapshotRepository();
    }

    [Fact]
    public async Task SaveAsync_ValidSnapshot_StoresSnapshot()
    {
        // Arrange
        var snapshot = CreateTestSnapshot("139", "snap-001");

        // Act
        await _repository.SaveAsync(snapshot);

        // Assert
        var result = await _repository.GetLatestAsync("139");
        Assert.NotNull(result);
        Assert.Equal("snap-001", result.SnapshotId);
    }

    [Fact]
    public async Task SaveAsync_SameFormType_ReplacesPrevious()
    {
        // Arrange
        var snapshot1 = CreateTestSnapshot("139", "snap-001");
        var snapshot2 = CreateTestSnapshot("139", "snap-002");

        // Act
        await _repository.SaveAsync(snapshot1);
        await _repository.SaveAsync(snapshot2);

        // Assert
        var result = await _repository.GetLatestAsync("139");
        Assert.NotNull(result);
        Assert.Equal("snap-002", result.SnapshotId);
    }

    [Fact]
    public async Task GetLatestAsync_NonExistingFormType_ReturnsNull()
    {
        // Arrange & Act
        var result = await _repository.GetLatestAsync("999");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_NoSnapshots_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_MultipleSnapshots_ReturnsAllOrderedByDate()
    {
        // Arrange
        var older = CreateTestSnapshot("139", "snap-001", DateTime.UtcNow.AddMinutes(-5));
        var newer = CreateTestSnapshot("142", "snap-002", DateTime.UtcNow);

        await _repository.SaveAsync(older);
        await _repository.SaveAsync(newer);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("snap-002", result[0].SnapshotId); // Newest first
        Assert.Equal("snap-001", result[1].SnapshotId);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsSnapshot()
    {
        // Arrange
        var snapshot = CreateTestSnapshot("139", "snap-001");
        await _repository.SaveAsync(snapshot);

        // Act
        var result = await _repository.GetByIdAsync("snap-001");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("snap-001", result.SnapshotId);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByIdAsync("nonexistent");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SaveAsync_NullSnapshot_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.SaveAsync(null!));
    }

    private static Snapshot CreateTestSnapshot(string formType, string snapshotId, DateTime? capturedAt = null)
    {
        return new Snapshot(
            snapshotId,
            capturedAt ?? DateTime.UtcNow,
            new SnapshotContext("manager", "WS01"),
            new FormMetadata(new FormType(formType), "1", $"Form {formType}", FormMode.Ok, 1));
    }
}
