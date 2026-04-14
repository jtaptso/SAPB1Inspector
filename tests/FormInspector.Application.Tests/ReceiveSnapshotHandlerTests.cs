using FormInspector.Application.DTOs;
using FormInspector.Application.Interfaces;
using FormInspector.Application.UseCases.ReceiveSnapshot;
using Moq;

namespace FormInspector.Application.Tests;

public class ReceiveSnapshotHandlerTests
{
    private readonly Mock<ISnapshotRepository> _repositoryMock;
    private readonly Mock<ISnapshotNotifier> _notifierMock;
    private readonly ReceiveSnapshotHandler _handler;

    public ReceiveSnapshotHandlerTests()
    {
        _repositoryMock = new Mock<ISnapshotRepository>();
        _notifierMock = new Mock<ISnapshotNotifier>();

        _handler = new ReceiveSnapshotHandler(
            _repositoryMock.Object,
            _notifierMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidSnapshot_SavesAndNotifies()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        _repositoryMock.Verify(r => r.SaveAsync(It.IsAny<Domain.Snapshots.Snapshot>()), Times.Once);
        _notifierMock.Verify(n => n.NotifyUpdatedAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task HandleAsync_ValidSnapshot_ReturnsSnapshotId()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.Equal(command.Snapshot.SnapshotId, result);
    }

    [Fact]
    public async Task HandleAsync_UnsupportedSchemaVersion_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = CreateValidCommand() with
        {
            Snapshot = CreateValidCommand().Snapshot with { SchemaVersion = "99.0" }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_NullSchemaVersion_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = CreateValidCommand() with
        {
            Snapshot = CreateValidCommand().Snapshot with { SchemaVersion = null! }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_EmptySchemaVersion_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = CreateValidCommand() with
        {
            Snapshot = CreateValidCommand().Snapshot with { SchemaVersion = "" }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_SupportedMinorVersion_Succeeds()
    {
        // Arrange
        var command = CreateValidCommand() with
        {
            Snapshot = CreateValidCommand().Snapshot with { SchemaVersion = "1.1" }
        };

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
    }

    private static ReceiveSnapshotCommand CreateValidCommand()
    {
        return new ReceiveSnapshotCommand
        {
            Snapshot = new SnapshotInputDto
            {
                SnapshotId = "test-snap-001",
                SchemaVersion = "1.0",
                CapturedAt = DateTime.UtcNow,
                Context = new SnapshotContextDto { UserName = "manager", MachineName = "WS01" },
                Form = new FormMetadataDto
                {
                    FormType = "139",
                    UniqueId = "1",
                    Title = "Sales Order",
                    Mode = "OK",
                    PaneLevel = 1
                },
                Items =
                [
                    new ItemMetadataDto
                    {
                        ItemUid = "4",
                        ItemType = "EditText",
                        Layout = new LayoutDto { Top = 10, Left = 20, Width = 100, Height = 30 },
                        Visible = true,
                        Enabled = true,
                        DataBinding = new DataBindingDto { TableName = "ORDR", ColumnName = "DocNum" }
                    }
                ]
            }
        };
    }
}
