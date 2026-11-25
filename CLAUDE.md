# CLAUDE.md — Project Agent Guidelines

Hotel Management System (C# .NET 10 WPF MVVM + `WPF-UI` 4.3.0 + `LiveCharts2` 2.0.5 + SQL Server ADO.NET) — IT008.

## Canonical Context Pointers

- **Domain Glossary**: Read `GLOSSARY.md` before naming entities, statuses (`RoomStatus`, `BookingStatus`, `InvoiceStatus`), or implementing scheduling/billing formulas.
- **System Specification**: Read `docs/specs/hotel-management-system.md` (Issue #77) for architecture, 21 user stories, and TDD testing seams.
- **Design System**: Read `DESIGN.md` at repo root (Google `DESIGN.md` spec) for normative YAML tokens, mandatory `WPF-UI` primitives, spacing/radius rules, and the 4 mandatory UI states.
- **Task Guides & Issue Tracker**: Read `docs/tasks/<TASK-ID>.md` for ticket scope and UI Contracts; manage GitHub Issues via `gh` CLI (`docs/agents/issue-tracker.md` and `docs/agents/triage-labels.md`).

## Hard Design & Source Rules

1. **Zero Figma AI / Legacy Brief Dependency**: Never read raw designs from Figma MCP and never reference removed `design-brief` or wireframe branches/files. `DESIGN.md` + the issue's **UI Contract** (+ user-attached reference screenshot, if any) are the sole UI sources of truth.
2. **Domain Wins Over Visuals**: Whenever a visual mockup conflicts with `GLOSSARY.md` or `docs/specs/hotel-management-system.md`, enforce `GLOSSARY.md` and flag the conflict.

## Mandatory 2-Phase Execution for Every GitHub Issue

Execute every ticket strictly in two sequential phases. Never mix initial logic implementation with XAML layout tuning.

### Phase 1 — Logic, Data & State First (No Complex UI)

1. Implement or update `Models/`, `DAO/` (parameterized ADO.NET), `Services/` (pure TDD seams), and `ViewModels/` (`CommunityToolkit.Mvvm`).
2. Model all **4 Mandatory States** in the ViewModel (`IsLoading`, `IsEmpty`, `ErrorMessage` / inline validation, and populated `Success` state) plus safe delete/action lock predicates.
3. Write unit tests in `tests/HotelManagement.Tests` covering domain invariants, validation, and ViewModel state transitions.
4. **Phase 1 Gate**: Run `dotnet build` and `dotnet test` — 100% green before writing or styling complex XAML.

### Phase 2 — UI Assembly & Verification

1. Read `DESIGN.md` (and any user-approved UI screenshot attached to the issue).
2. Assemble XAML using **only** approved primitives (`ui:FluentWindow`, `ui:TitleBar`, `ui:NavigationView`, `ui:Button`, `ui:TextBox`, `ui:PasswordBox`, `ui:NumberBox`, `ComboBox`, `DatePicker`, `DataGrid`, `ui:ContentDialog`, `ui:InfoBar`, `CartesianChart`) and `{StaticResource}` tokens from `Resources/Colors.xaml` & `Resources/Styles.xaml`.
3. Enforce Anti-Slop constraints:
   - Never rewrite `<ControlTemplate>` for standard buttons, inputs, dropdowns, or modals.
   - Never hardcode hex literals (`#...`) outside `Colors.xaml` or non-4px-grid margins/paddings.
   - Maximum **1 Primary CTA** (`Appearance="Primary"`) per view or dialog.
   - Render all **4 Mandatory States** (`Loading`, `Empty`, `Error`, `Success`).
4. **Phase 2 Gate**:
   - Run static anti-slop scan on `Views/` (no inline hex colors, no custom button `ControlTemplate`, grid-aligned `Margin`/`Padding`).
   - Run `dotnet build` and `dotnet test` with zero errors.
   - On Windows (or visual verification target), verify layout at `1280×800` and `1280×720` with zero clipped controls or horizontal overflow before reporting completion.
