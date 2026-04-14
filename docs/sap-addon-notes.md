# SAP Add-On Development Notes

## SwissAddonFramework Integration

The SAP add-on project (`SapB1.Addon.FormInspector`) uses SwissAddonFramework to connect to the SAP Business One UI API.

### Prerequisites
- SAP Business One client installed
- SwissAddonFramework SDK referenced
- SAPbouiCOM interop available

### Event Subscription

Only structural events are subscribed to avoid UI freezes:

| Event | Reason | Setting |
|-------|--------|---------|
| FormLoad | Initial structure | `TrackFormLoad` (default: true) |
| FormActivate | Context switch | `TrackFormActivate` (default: true) |
| FormVisible | Late-loaded items | `TrackFormVisible` (default: true) |
| FormModeChange | Mode-based UI changes | `TrackFormModeChange` (default: true) |
| PaneChange | Pane-dependent items | `TrackPaneChanges` (default: false) |

### Throttling

The `Throttler` class prevents excessive snapshot creation for the same form type. Default interval: 1000ms.

### Configuration

Settings are managed via `InspectorSettings`:
- `BackendUrl` — Backend API URL (default: http://localhost:5000)
- `ThrottleIntervalMs` — Min interval between snapshots per form type
- `IncludedFormTypes` — Whitelist of form types (empty = all)
- `ExcludedFormTypes` — Blacklist of form types

### Implementation TODOs

All SAP-specific code has TODO comments where SwissAddonFramework/SAPbouiCOM API calls need to be implemented:
- `AddonStartup.cs` — Framework initialization
- `ConnectionBootstrap.cs` — Connection lifecycle
- `FormEventDispatcher.cs` — Event handler registration
- `FormInspector.cs` — Form metadata extraction
- `ItemInspector.cs` — Item metadata extraction
- `MatrixInspector.cs` — Matrix/grid inspection
- `DataSourceInspector.cs` — Data source binding extraction
- `SapHelpers.cs` — User/machine/client context

### Safety Principles

1. **Read-only** — Never modify SAP UI or data
2. **No DI API** — Only UI API for metadata extraction
3. **Exception swallowing** — Prevent SAP client crashes
4. **No persistence** — Snapshots sent to backend immediately
