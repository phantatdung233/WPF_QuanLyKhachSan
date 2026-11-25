# Project Agent Guidelines

Hotel Management System (C# .NET 10 WPF MVVM + `WPF-UI` 4.3.0 + `LiveCharts2` 2.0.5 + SQL Server ADO.NET) — IT008.

## Agent skills

### Issue tracker

GitHub Issues via `gh` CLI. See `docs/agents/issue-tracker.md`.

### Triage labels

Default canonical roles (`needs-triage`, `needs-info`, `ready-for-agent`, `ready-for-human`, `wontfix`). See `docs/agents/triage-labels.md`.

### Domain docs

Single-context (`GLOSSARY.md` and `docs/adr/` at repo root). See `docs/agents/domain.md`.

## Context Pointers

### Domain glossary
Before designing entities or naming concepts, read `GLOSSARY.md` at root for canonical terms (`RoomStatus`, `BookingStatus`, `InvoiceStatus`, billing formulas).

### Specification
Before implementing features or answering architecture questions, read `docs/specs/hotel-management-system.md` (Issue #77) for system requirements, 21 user stories, testing seams, and the 18/11–30/12 delivery plan.

### Design system (Google `DESIGN.md`)
Before building or styling any XAML window, dialog, card, table, or chart, read `DESIGN.md` at repo root for normative YAML design tokens, mandatory `WPF-UI` primitives, anti-slop rules, and the 4 mandatory UI states (`Loading`, `Empty`, `Error`, `Success`). Never read raw designs from Figma MCP or legacy `design-brief` files.

### Task implementation guides
When working on any ticket, read its corresponding technical guide and UI Contract under `docs/tasks/<TASK-ID>.md` (e.g. `docs/tasks/TASK-01.md`).

## Mandatory 2-Phase Execution Workflow

Every GitHub Issue MUST be executed in two sequential phases (full rules in `CLAUDE.md`):

1. **Phase 1 — Logic & State First**: Complete `Models/` / `DAO/` / `Services/` (TDD seams) / `ViewModels/` (including `Loading`, `Empty`, `Error`, `Success` states) and pass `dotnet build` + `dotnet test` 100% before complex XAML layout work.
2. **Phase 2 — UI Assembly & Verification**: Read `DESIGN.md` (+ attached approved screenshot if provided), assemble XAML exclusively from `WPF-UI` primitives + `{StaticResource}` tokens (max 1 Primary CTA, 4px/8px spacing grid, zero inline hex or custom button `ControlTemplate`), and verify build/layout cleanly before completion.
