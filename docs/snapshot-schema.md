# Snapshot Schema

## Version 1.0

The snapshot schema defines the wire format between the SAP add-on and the backend.

### Top-Level Structure

```json
{
  "snapshotId": "guid",
  "schemaVersion": "1.0",
  "capturedAt": "ISO-8601",
  "context": {
    "userName": "string?",
    "machineName": "string?",
    "clientId": "string?"
  },
  "form": { "...formMetadata..." },
  "items": [ "...itemMetadata..." ]
}
```

### Form Metadata

| Field | Type | Description |
|-------|------|-------------|
| formType | string | SAP form type ID (e.g., "139" for Sales Order) |
| uniqueId | string | Form instance unique ID |
| title | string | Form caption |
| mode | string | "ADD", "OK", "FIND", "UPDATE" |
| paneLevel | int | Current pane level |
| sapVersion | string? | SAP client version |

### Item Metadata

| Field | Type | Description |
|-------|------|-------------|
| itemUid | string | Item UID within the form |
| itemType | string | "EditText", "Button", "CheckBox", "Matrix", etc. |
| layout | object | { top, left, width, height } |
| visible | bool | Whether item is visible |
| enabled | bool | Whether item is interactive |
| dataBinding | object? | { tableName, columnName } |
| fromPane | int | Start pane visibility |
| toPane | int | End pane visibility |
| description | string? | Item description/label |
| matrixMetadata | object? | For matrix/grid items |

### Matrix Metadata

| Field | Type | Description |
|-------|------|-------------|
| matrixUid | string | Matrix UID |
| rowCount | int | Visible row count |
| editable | bool | Whether matrix is editable |
| columns | array | Column definitions |

### Column Metadata

| Field | Type | Description |
|-------|------|-------------|
| columnUid | string | Column UID |
| columnType | string | Column type |
| dataBinding | object? | { tableName, columnName } |
| editable | bool | Whether column is editable |
| caption | string? | Column header text |
| width | int | Column width |
