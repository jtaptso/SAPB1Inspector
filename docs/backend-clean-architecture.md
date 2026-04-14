# Backend — Clean Architecture

## Domain Layer

**Zero framework dependencies.** Defines what exists, not how it is delivered or stored.

### Entities
- `Snapshot` — Aggregate root representing one captured SAP UI state
- `FormMetadata` — Form-level metadata (type, title, mode, pane)
- `ItemMetadata` — Item-level metadata (UID, type, layout, visibility, binding)
- `MatrixMetadata` / `ColumnMetadata` — Line-level UI metadata

### Value Objects
- `FormType` — SAP form type identifier
- `ItemType` — UI item type
- `DataBinding` — Table/column binding
- `Layout` — Position and size

### Enums
- `FormMode` — ADD, OK, FIND, UPDATE
- `VisibilityState` — Visible, Hidden, Collapsed

## Application Layer

**Depends only on Domain.** Defines use cases and ports (interfaces).

### Use Cases
- **ReceiveSnapshot** — Validates schema, persists, triggers notification
- **GetLatestSnapshot** — Retrieves snapshot by form type

### Ports (Interfaces)
- `ISnapshotRepository` — Persistence abstraction
- `ISnapshotNotifier` — Notification abstraction
- `ITimeProvider` — Testable time access

## Infrastructure Layer

**Implements Application ports.** Technical details hidden behind interfaces.

- `InMemorySnapshotRepository` — MVP in-memory storage
- `SignalRSnapshotNotifier` — SignalR broadcast adapter
- `SystemTimeProvider` — Real UTC time
- `SnapshotJsonMapper` — JSON serialization settings

## Presentation Layer

**Thin delivery mechanism.** No business logic.

- `SnapshotController` — REST API (POST/GET)
- `SnapshotHub` — SignalR hub
- `ApiExceptionMiddleware` — Global error handling
