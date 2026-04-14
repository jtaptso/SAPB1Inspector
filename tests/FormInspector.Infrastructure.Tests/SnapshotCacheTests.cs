using FormInspector.Domain.Enums;
using FormInspector.Domain.Snapshots;
using FormInspector.Domain.ValueObjects;
using FormInspector.Infrastructure.Persistence;

namespace FormInspector.Infrastructure.Tests;

public class SnapshotCacheTests
{
    private readonly SnapshotCache _cache;

    public SnapshotCacheTests()
    {
        _cache = new SnapshotCache();
    }

    [Fact]
    public void Update_StoresSnapshot()
    {
        // Arrange
        var snapshot = CreateTestSnapshot("139");

        // Act
        _cache.Update(snapshot);

        // Assert
        var result = _cache.TryGetLatest();
        Assert.NotNull(result);
        Assert.Equal("139", result.Form.FormType.Value);
    }

    [Fact]
    public void TryGetLatest_ByFormType_Matching_ReturnsSnapshot()
    {
        // Arrange
        var snapshot = CreateTestSnapshot("139");
        _cache.Update(snapshot);

        // Act
        var result = _cache.TryGetLatest("139");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void TryGetLatest_ByFormType_NonMatching_ReturnsNull()
    {
        // Arrange
        var snapshot = CreateTestSnapshot("139");
        _cache.Update(snapshot);

        // Act
        var result = _cache.TryGetLatest("142");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Clear_RemovesCachedSnapshot()
    {
        // Arrange
        var snapshot = CreateTestSnapshot("139");
        _cache.Update(snapshot);

        // Act
        _cache.Clear();

        // Assert
        Assert.Null(_cache.TryGetLatest());
    }

    [Fact]
    public void Update_ReplacesPreviousSnapshot()
    {
        // Arrange
        var first = CreateTestSnapshot("139", "snap-001");
        var second = CreateTestSnapshot("142", "snap-002");

        _cache.Update(first);
        _cache.Update(second);

        // Act
        var result = _cache.TryGetLatest();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("snap-002", result.SnapshotId);
    }

    private static Snapshot CreateTestSnapshot(string formType, string? snapshotId = null)
    {
        return new Snapshot(
            snapshotId ?? $"snap-{formType}",
            DateTime.UtcNow,
            new SnapshotContext("manager", "WS01"),
            new FormMetadata(new FormType(formType), "1", $"Form {formType}", FormMode.Ok, 1));
    }
}
