# Project Agent Guidelines

Hotel Management System (WPF .NET 8 MVVM) - IT008.

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
Before implementing features or answering architecture questions, read `docs/specs/hotel-management-system.md` (Issue #77) for system requirements, 24 user stories, and architectural decisions.

### Task implementation guides
When working on any ticket, read its corresponding technical guide under `docs/tasks/<TASK-ID>.md` (e.g. `docs/tasks/TASK-01.md`). It contains architecture scope, classes, validation rules, edge cases, and testing strategies.
