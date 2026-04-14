using System;
using System.Collections.Generic;

namespace SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

/// <summary>
/// Top-level snapshot DTO sent from the SAP add-on to the backend via HTTP POST.
/// Pure metadata — no SAP objects, no COM references.
/// </summary>
public class SnapshotDto
{
    public string SnapshotId { get; set; } = Guid.NewGuid().ToString();
    public string SchemaVersion { get; set; } = "1.0";
    public DateTime CapturedAt { get; set; } = DateTime.UtcNow;
    public string? UserName { get; set; }
    public string? MachineName { get; set; }
    public string? ClientId { get; set; }
    public FormDto Form { get; set; } = new FormDto();
    public List<ItemDto> Items { get; set; } = new List<ItemDto>();
}
