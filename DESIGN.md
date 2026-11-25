---
version: "alpha"
name: "Aura Hotel Desktop Design System"
description: "Fluent Windows UI Kit light design system for the IT008 C# .NET 10 WPF MVVM Hotel Management desktop application."
colors:
  primary: "#005FB8"
  primary-hover: "#00529E"
  primary-pressed: "#004382"
  on-primary: "#FFFFFF"
  secondary: "#5C5C5C"
  neutral: "#F3F3F3"
  surface: "#F3F3F3"
  surface-card: "#FFFFFF"
  surface-subtle: "#F6F6F6"
  surface-flyout: "#F9F9F9"
  on-surface: "#1A1A1A"
  disabled-text: "#595959"
  border: "#E5E5E5"
  success: "#0F7B0F"
  success-surface: "#DFF6DD"
  caution: "#8A5000"
  caution-surface: "#FFF4CE"
  error: "#C42B1C"
  error-text: "#A42618"
  error-surface: "#FDE7E9"
  room-available: "#48BB78"
  room-occupied: "#F56565"
  room-reserved: "#ED8936"
  room-maintenance: "#A0AEC0"
typography:
  headline-lg:
    fontFamily: "Segoe UI Variable Display"
    fontSize: 28px
    fontWeight: 600
    lineHeight: 36px
  headline-kpi:
    fontFamily: "Segoe UI Variable Display"
    fontSize: 24px
    fontWeight: 600
    lineHeight: 32px
  headline-md:
    fontFamily: "Segoe UI Variable Display"
    fontSize: 20px
    fontWeight: 600
    lineHeight: 28px
  body-strong:
    fontFamily: "Segoe UI Variable Text"
    fontSize: 14px
    fontWeight: 600
    lineHeight: 20px
  body-md:
    fontFamily: "Segoe UI Variable Text"
    fontSize: 14px
    fontWeight: 400
    lineHeight: 20px
  caption:
    fontFamily: "Segoe UI Variable Text"
    fontSize: 12px
    fontWeight: 400
    lineHeight: 16px
  monospace:
    fontFamily: "Cascadia Mono"
    fontSize: 13px
    fontWeight: 400
    lineHeight: 20px
rounded:
  none: 0px
  xs: 2px
  sm: 4px
  md: 8px
spacing:
  xs: 4px
  sm: 8px
  md: 12px
  lg: 16px
  xl: 24px
  xxl: 32px
components:
  button-primary:
    backgroundColor: "{colors.primary}"
    textColor: "{colors.on-primary}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
    height: 32px
  button-primary-hover:
    backgroundColor: "{colors.primary-hover}"
    textColor: "{colors.on-primary}"
  button-primary-active:
    backgroundColor: "{colors.primary-pressed}"
    textColor: "{colors.on-primary}"
  button-secondary:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.on-surface}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
    height: 32px
  button-disabled:
    backgroundColor: "{colors.neutral}"
    textColor: "{colors.disabled-text}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
    height: 32px
  button-danger:
    backgroundColor: "{colors.error}"
    textColor: "{colors.on-primary}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
    height: 32px
  input-default:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.on-surface}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.sm}"
    height: 32px
  helper-caption:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.secondary}"
    typography: "{typography.caption}"
  nav-item:
    backgroundColor: "{colors.surface}"
    textColor: "{colors.on-surface}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
    height: 40px
  nav-item-active:
    backgroundColor: "{colors.surface-subtle}"
    textColor: "{colors.on-surface}"
    typography: "{typography.body-strong}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
    height: 40px
  table-row:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.on-surface}"
    typography: "{typography.body-md}"
    padding: "{spacing.md}"
    height: 40px
  divider-hairline:
    backgroundColor: "{colors.border}"
    height: 1px
  card-surface:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.on-surface}"
    rounded: "{rounded.md}"
    padding: "{spacing.xl}"
  flyout-menu:
    backgroundColor: "{colors.surface-flyout}"
    textColor: "{colors.on-surface}"
    rounded: "{rounded.md}"
    padding: "{spacing.xs}"
  status-bar-available:
    backgroundColor: "{colors.room-available}"
    rounded: "{rounded.xs}"
    width: 4px
  status-bar-occupied:
    backgroundColor: "{colors.room-occupied}"
    rounded: "{rounded.xs}"
    width: 4px
  status-bar-reserved:
    backgroundColor: "{colors.room-reserved}"
    rounded: "{rounded.xs}"
    width: 4px
  status-bar-maintenance:
    backgroundColor: "{colors.room-maintenance}"
    rounded: "{rounded.xs}"
    width: 4px
  infobar-error:
    backgroundColor: "{colors.error-surface}"
    textColor: "{colors.error-text}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
  infobar-caution:
    backgroundColor: "{colors.caution-surface}"
    textColor: "{colors.caution}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
  infobar-success:
    backgroundColor: "{colors.success-surface}"
    textColor: "{colors.success}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
---

# Aura Hotel — Desktop Design System (WPF .NET 10)

## Overview

Calm, high-density Windows 11 Fluent desktop utility built for front-desk hotel receptionists working continuous shifts under the **AURA HOTEL** brand. Built on C# `.NET 10` (`net10.0-windows`), `CommunityToolkit.Mvvm` 8.4.2, `WPF-UI` 4.3.0, and `LiveChartsCore.SkiaSharpView.WPF` 2.0.5.

Visual hierarchy relies on crisp light neutral surfaces, tabular alignment, scannable `40px` data rows, and restrained Windows accent highlights rather than marketing whitespace, dark navy chrome, or saturated dashboard blocks. Domain terms strictly follow `@GLOSSARY.md`.

## Colors

All XAML color and `SolidColorBrush` definitions live exclusively in `src/HotelManagement/Resources/Colors.xaml` and are consumed via `{StaticResource}`. Inline hex literals in Views or ViewModels are forbidden.

- **Surfaces (`#F3F3F3` / `#FFFFFF` / `#F6F6F6` / `#F9F9F9`):** `AppSurfaceBrush` (`{colors.surface}`) anchors the window frame and left navigation pane; `CardSurfaceBrush` (`{colors.surface-card}`) elevates content containers, tables, dialogs, KPI cards, and room cards; `FlyoutSurfaceBrush` (`{colors.surface-flyout}`) backs context menus.
- **Typography & Borders (`#1A1A1A` / `#5C5C5C` / `#E5E5E5`):** `TextPrimaryBrush` (`{colors.on-surface}`) for primary content, `TextSecondaryBrush` (`{colors.secondary}`) for labels and metadata, and `CardStrokeBrush` (`{colors.border}`) for `1px` hairline dividers.
- **Primary Accent (`#005FB8`):** `AccentBrush` (`{colors.primary}`) is reserved strictly for the single primary call-to-action per screen or dialog, active navigation selection indicators, keyboard focus rings, and occupancy progress fills.
- **Semantic Feedback:** `SystemSuccessBrush` (`#0F7B0F` on `#DFF6DD`), `SystemCautionBrush` (`#8A5000` on `#FFF4CE`), and `SystemCriticalBrush` (`#C42B1C` / `#A42618` on `#FDE7E9`) drive validation messages, destructive actions, and status banners.
- **Fixed Domain `RoomStatus` Indicators (Immutable):**
  - `RoomAvailableBrush` (`#48BB78`): `Trống` (`Available`)
  - `RoomOccupiedBrush` (`#F56565`): `Đang ở` (`Occupied`)
  - `RoomReservedBrush` (`#ED8936`): `Đã đặt` (`Reserved`)
  - `RoomMaintenanceBrush` (`#A0AEC0`): `Bảo trì` (`Maintenance`)
  - Apply these four brushes **only** to status indicator bars (`4px` width), legend swatches, status dots, or compact badges accompanied by Vietnamese text labels. Keep room card and table row backgrounds neutral (`CardSurfaceBrush`).

## Typography

Use **Segoe UI Variable** (`Segoe UI` fallback) across the application, **Segoe Fluent Icons** (via `WPF-UI` `SymbolIcon`) for iconography, and **Cascadia Mono** (`Consolas` fallback) exclusively for monospace `.txt` invoice previews.

- **`PageTitleTextStyle` (`28px / 36px` SemiBold):** Rendered once in the shell header for the active view; never duplicated inside child `UserControl` content.
- **`KpiValueTextStyle` (`24px / 32px` SemiBold):** Numeric KPI figures on `ReportView`, paired with `CaptionTextStyle` labels.
- **`SectionTitleTextStyle` (`20px / 28px` SemiBold):** Modal dialog titles and major card headers.
- **`BodyStrongTextStyle` (`14px / 20px` SemiBold):** Table column headers, room numbers, and emphasized field values.
- **`BodyTextStyle` (`14px / 20px` Regular):** Standard table cells, form inputs, buttons, and body copy.
- **`CaptionTextStyle` (`12px / 16px` Regular):** Helper hints, timestamps, filter counters, and `InlineErrorTextStyle` (`{colors.error-text}`) directly beneath invalid inputs.
- **`MonospaceReceiptTextStyle` (`13px / 20px` Regular):** Read-only UTF-8 receipt preview matching exported `.txt` alignment.

## Layout

All `Margin`, `Padding`, and grid gaps must use strict multiples of `4px` (`0`, `4`, `8`, `12`, `16`, `24`, `32`). Arbitrary pixel offsets (`5`, `6`, `14`, `18`, `22`, `26`) are forbidden.

- **Application Shell (`MainWindow`):** Design target `1280×800`, hard minimum `MinWidth="1280" MinHeight="720"` for projector compatibility.
  - Top system `ui:TitleBar`: `48px` height.
  - Left `ui:NavigationView` pane: `240px` fixed open width (`Administrator`: 8 items; `Staff`: 6 items, hiding `Loại phòng` and `Báo cáo`).
  - Content header bar: single page title on the left, compact account menu (`FullName`, `RoleDisplay`, `Đăng xuất`) on the right.
  - Main workspace container: `<ContentControl Content="{Binding CurrentView}" />` wrapped in a neutral card container with `24px` outer right/bottom gutter and `24px` inner padding.
- **Catalog Views (`Room`, `RoomType`, `Customer`, `Service`, `Booking`):** Shared 3-row vertical Grid:
  1. Toolbar row (`Auto`, bottom margin `16px`): left search/filter controls (`8px` gap), right single Primary CTA button.
  2. Data region (`*`): `DataGrid` (`40px` row height) or state overlay (`Loading` / `Empty`).
  3. Footer summary (`Auto`, top margin `12px`): item count and contextual lock hint.
- **Room Matrix (`RoomMapView`):** Toolbar + 4-color legend above a `ScrollViewer` hosting an `ItemsControl` with `WrapPanel` so room cards wrap cleanly down to `1280×720` without horizontal scrollbars.

## Elevation & Depth

Hierarchy is achieved through surface contrast (`#F3F3F3` base vs `#FFFFFF` layer) and `1px` neutral borders (`CardStrokeBrush` `#E5E5E5`), never heavy drop shadows, blur effects, or colored panels.

- Base window uses `AppSurfaceBrush`.
- Workspace panels, KPI cards, room cards, and tables sit on `CardSurfaceBrush` with `BorderThickness="1"` and `BorderBrush="{StaticResource CardStrokeBrush}"`.
- Modal dialogs (`Window.ShowDialog()` or `ui:ContentDialog`) use solid `CardSurfaceBrush` with standard OS/WPF-UI backdrop dimming.

## Shapes

Follow Windows 11 corner geometry consistently through shared `CornerRadius` resources in `Styles.xaml`:

- **Interactive Controls (`{rounded.sm}` = `4px`):** Buttons, text inputs, password boxes, combo boxes, date pickers, search boxes, badges, and navigation items.
- **Containers & Overlays (`{rounded.md}` = `8px`):** Dialogs, flyouts, workspace containers, room cards, and KPI cards.
- **Adjoining Edges (`{rounded.none}` = `0px`):** Table row dividers and flush structural splits. Pill-shaped (`999px`) buttons or badges are not used.

## Motion & Micro-interaction

Front-desk reception workflows prioritize instant responsiveness (`0ms` view switching, immediate hover/press visual feedback built into native `WPF-UI` controls, and `ui:ProgressRing` only while awaiting database operations). Custom page-transition storyboards, parallax effects, and chart entrance animations are omitted.

## Voice & Content Strategy

All user-facing UI text must be **100% Vietnamese with full diacritics**, sentence case (`Thêm khách hàng`, never `THÊM KHÁCH HÀNG` except receipt headers), concise, and operational.

### Data Formatting Rules

- **Currency (VNĐ):** Dot thousands separator with `đ` suffix: `#,##0 đ` (e.g., `500.000 đ`, `1.070.000 đ`) across both XAML views and exported `.txt` receipts.
- **Dates & Stay Duration:** `dd/MM/yyyy` (e.g., `14/10/2026`); stay unit is always `đêm` (minimum `1 đêm` per `@GLOSSARY.md`).
- **Identifiers:** Phone `10` digits starting with `0` (`0901234567`); Citizen ID `9` or `12` digits (`079201001234`).

### 4 Mandatory UI States (Required on Every Data Screen & Form)

Every ViewModel and View handling data or mutations must explicitly model and render all four states:

1. **Loading (`IsLoading`):** Display `ui:ProgressRing` centered in the data region and disable submit/mutation buttons to prevent double-submission.
2. **Empty (`IsEmpty`):** Never render a blank `DataGrid` or empty panel. Distinguish between:
   - *Zero records in database:* Clear message (e.g., `"Chưa có khách hàng nào"`) + action prompt.
   - *Zero filter/search matches:* Specific message (e.g., `"Không tìm thấy kết quả phù hợp"`) + `"Xóa bộ lọc"` action.
3. **Error (`ErrorMessage` / Field Validation):**
   - Field input errors render inline directly beneath the offending control using `InlineErrorTextStyle` (`12px`, `{colors.error-text}`).
   - Business rule blocks (booking overlap conflict, invalid report date range, failed login, blocked deletion) render via `ui:InfoBar` (`Severity="Error"` or `"Warning"`) without spamming raw `MessageBox.Show`.
4. **Success (Populated Data & Feedback):** Render aligned tabular/card data with safe action locks (disable `Xóa` button + explanatory `ToolTip` when a record is in use or room is `Đang ở`), and confirm completed workflows via `ui:InfoBar` (`Severity="Success"`).

## Components

Build interfaces exclusively from **`WPF-UI` (`Wpf.Ui.Controls`, `xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"`), standard WPF data controls, and `LiveCharts2`**. Rewriting `ControlTemplate` from scratch for standard buttons, inputs, modals, or dropdowns is strictly prohibited.

- **Window & Shell:** `ui:FluentWindow` + `ui:TitleBar` + `ui:NavigationView` (`NavigationViewItem` with `ui:SymbolIcon`).
- **Buttons:** `ui:Button` using `Appearance="Primary"` (maps to `button-primary`), `Appearance="Secondary"` (`button-secondary`), or `Appearance="Danger"` (`button-danger` for destructive confirmations).
- **Inputs & Pickers:** `ui:TextBox` (with `PlaceholderText` and `Icon` for search), `ui:PasswordBox`, `ui:NumberBox`, standard `ComboBox`, `DatePicker`, and `RadioButton` (for `Tiền mặt` / `Chuyển khoản`).
- **Tables:** Standard WPF `DataGrid` (`AutoGenerateColumns="False"`, `IsReadOnly="True"`, `RowHeight="40"`, `GridLinesVisibility="Horizontal"`), styled via shared `Styles.xaml` tokens.
- **Room Card (`RoomMapView`):** `Border` using `CardSurfaceBorderStyle` inside `ItemsControl` + `WrapPanel`, with XAML `DataTrigger` binding `Status` (`Trống`, `Đang ở`, `Đã đặt`, `Bảo trì`) to the status indicator bar/dot/badge brush (`RoomAvailableBrush`, `RoomOccupiedBrush`, `RoomReservedBrush`, `RoomMaintenanceBrush`).
- **KPI & Occupancy (`ReportView`):** Three neutral `CardSurfaceBorderStyle` KPI cards (hierarchy via `KpiValueTextStyle`, never 3 colored backgrounds), standard `ProgressBar` (`AccentBrush`), and `LiveChartsCore.SkiaSharpView.WPF` `CartesianChart` (`LineSeries`).
- **Dialogs & Banners:** Modal `ui:FluentWindow` (`ShowDialog()`, `ResizeMode="NoResize"`, `WindowStartupLocation="CenterOwner"`) or `ui:ContentDialog` for forms/confirmations; `ui:InfoBar` (`IsOpen`, `Severity`, `Message`) for inline banners.

## Do's and Don'ts

- **Do** use `WPF-UI` primitives (`ui:Button`, `ui:TextBox`, `ui:PasswordBox`, `ui:InfoBar`, `ui:NavigationView`) directly with shared `StaticResource` brushes and styles.
- **Don't** write custom `<ControlTemplate TargetType="Button">` or recreate inputs, dropdowns, or modals from raw shapes.
- **Do** enforce at most **one Primary CTA** (`Appearance="Primary"`) per screen or modal dialog; all sibling actions must use `Secondary` or `Transparent`.
- **Don't** hardcode hex colors (`#...`) or non-grid pixel margins/paddings (`5`, `6`, `14`, `18`, `22`) inside View XAML or C# code-behind.
- **Do** keep room cards and KPI cards on neutral `CardSurfaceBrush` (`#FFFFFF`) and express `RoomStatus` through `DataTrigger` indicator bars, dots, and labeled badges.
- **Don't** fill entire room cards, sidebar panels, headers, or KPI cards with saturated colors, gradients, glassmorphism, heavy drop shadows, or decorative animations.
- **Do** implement all **4 Mandatory States** (`Loading`, `Empty`, `Error`, `Success`) in the ViewModel first before assembling XAML.
- **Don't** expose English enum names, raw SQL exceptions, or blank empty tables to the user.
