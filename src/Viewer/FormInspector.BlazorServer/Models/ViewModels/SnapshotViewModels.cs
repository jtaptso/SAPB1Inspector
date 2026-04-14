using FormInspector.Application.DTOs;

namespace FormInspector.BlazorServer.Models.ViewModels;

/// <summary>
/// View model for the snapshot viewer page.
/// Holds the currently selected snapshot and the list of available snapshots.
/// </summary>
public class SnapshotViewerViewModel
{
    /// <summary>List of available snapshot summaries.</summary>
    public IReadOnlyList<SnapshotSummaryDto> AvailableSnapshots { get; set; } = [];

    /// <summary>The currently selected snapshot with full details.</summary>
    public SnapshotOutputDto? SelectedSnapshot { get; set; }

    /// <summary>The currently selected item UID for detail viewing.</summary>
    public string? SelectedItemUid { get; set; }

    /// <summary>Whether the component is loading data.</summary>
    public bool IsLoading { get; set; }

    /// <summary>Last update timestamp received from SignalR.</summary>
    public DateTime? LastUpdatedAt { get; set; }

    /// <summary>Whether connected to SignalR.</summary>
    public bool IsConnected { get; set; }
}

/// <summary>
/// View model for a single item in the item tree.
/// </summary>
public class ItemTreeNode
{
    public string ItemUid { get; set; } = string.Empty;
    public string ItemType { get; set; } = string.Empty;
    public bool Visible { get; set; }
    public bool Enabled { get; set; }
    public string? Description { get; set; }
    public bool HasDataBinding { get; set; }
    public bool IsMatrix { get; set; }
    public List<ItemTreeNode> Children { get; set; } = [];
}
