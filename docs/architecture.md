# SAP B1 Form Inspector — Architecture

## Overview

The SAP Business One Form Metadata Inspector is a semi-live inspection system that captures SAP B1 UI form metadata at runtime and exposes it through a web-based Blazor viewer.

## System Architecture

```
┌─────────────────────────┐
│  SAP Business One Client │
│  SwissAddonFramework     │
│  - UI Event Listener     │
│  - Form Inspector        │
│  - JSON Snapshot Builder │
│  - REST Publisher        │
└───────────┬─────────────┘
            │ HTTP/JSON
            ▼
┌─────────────────────────┐
│  Backend (Clean Arch.)   │
│  Presentation (REST/SigR)│
│  Application (Use Cases) │
│  Domain (Entities)       │
│  Infrastructure (Adapters│
└───────────┬─────────────┘
            │ SignalR
            ▼
┌─────────────────────────┐
│  Blazor Server Viewer    │
│  - Live Form Tree        │
│  - Field Details         │
│  - Semi-Live Updates     │
└─────────────────────────┘
```

## Clean Architecture Dependency Rule

```
Presentation → Application → Domain
Infrastructure → Application → Domain
```

**Forbidden:** Domain → Application, Application → Infrastructure, Domain → ASP.NET/SignalR

## Projects

| Project | Type | Role |
|---------|------|------|
| `FormInspector.Domain` | Class Library | Pure domain model, zero dependencies |
| `FormInspector.Application` | Class Library | Use cases, ports, DTOs |
| `FormInspector.Infrastructure` | Class Library | Adapters (persistence, SignalR, time) |
| `FormInspector.Presentation` | Web API | REST controllers, SignalR hub |
| `FormInspector.BlazorServer` | Blazor Server | Read-only reactive viewer |
| `SapB1.Addon.FormInspector` | Class Library | SAP add-on (external producer) |

## Key Design Decisions

- **Read-only**: The system never modifies SAP UI or data
- **Semi-live**: Updates driven by structural UI events, not polling
- **Event throttling**: Prevents UI freezes in SAP client
- **Schema versioning**: Snapshots carry a version for forward compatibility
- **In-memory storage**: MVP uses in-memory repository; swap to DB later
