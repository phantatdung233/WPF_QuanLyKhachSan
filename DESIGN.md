---
version: "alpha"
name: "Aura Hotel Desktop Design System"
description: "Windows 11 Fluent 2 & WPF-UI 4.3 light layered design system for the IT008 C# .NET 10 WPF MVVM Hotel Management application."
colors:
  primary: "#005FB8"
  primary-hover: "#00529E"
  primary-pressed: "#004382"
  primary-tint: "#EBF3FC"
  on-primary: "#FFFFFF"
  secondary: "#5C5C5C"
  tertiary: "#757575"
  neutral: "#F3F3F3"
  surface: "#F3F3F3"
  surface-layer: "#F9F9F9"
  surface-card: "#FFFFFF"
  surface-subtle: "#F3F3F3"
  surface-hover: "#EAEAEA"
  surface-flyout: "#FCFCFC"
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
  room-available-tint: "#EDFDF3"
  room-occupied: "#F56565"
  room-occupied-tint: "#FFF5F5"
  room-reserved: "#ED8936"
  room-reserved-tint: "#FFFAF0"
  room-maintenance: "#A0AEC0"
  room-maintenance-tint: "#F7FAFC"
typography:
  headline-lg:
    fontFamily: "Segoe UI Variable Display"
    fontSize: 28px
    fontWeight: 600
    lineHeight: 1.3
  headline-kpi:
    fontFamily: "Segoe UI Variable Display"
    fontSize: 24px
    fontWeight: 600
    lineHeight: 1.35
  headline-md:
    fontFamily: "Segoe UI Variable Display"
    fontSize: 20px
    fontWeight: 600
    lineHeight: 1.4
  subtitle:
    fontFamily: "Segoe UI Variable Text"
    fontSize: 16px
    fontWeight: 600
    lineHeight: 1.4
  body-strong:
    fontFamily: "Segoe UI Variable Text"
    fontSize: 14px
    fontWeight: 600
    lineHeight: 1.45
  body-md:
    fontFamily: "Segoe UI Variable Text"
    fontSize: 14px
    fontWeight: 400
    lineHeight: 1.45
  caption-strong:
    fontFamily: "Segoe UI Variable Text"
    fontSize: 12px
    fontWeight: 600
    lineHeight: 1.4
  caption:
    fontFamily: "Segoe UI Variable Text"
    fontSize: 12px
    fontWeight: 400
    lineHeight: 1.4
  monospace:
    fontFamily: "Cascadia Mono"
    fontSize: 13px
    fontWeight: 400
    lineHeight: 1.5
rounded:
  none: 0px
  xs: 2px
  sm: 4px
  md: 8px
  lg: 12px
spacing:
  xs: 4px
  sm: 8px
  md: 12px
  lg: 16px
  xl: 20px
  xxl: 24px
  xxxl: 32px
components:
  button-primary:
    backgroundColor: "{colors.primary}"
    textColor: "{colors.on-primary}"
    typography: "{typography.body-strong}"
    rounded: "{rounded.sm}"
    padding: "{spacing.lg}"
    height: 36px
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
    padding: "{spacing.lg}"
    height: 36px
  button-disabled:
    backgroundColor: "{colors.neutral}"
    textColor: "{colors.disabled-text}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.lg}"
    height: 36px
  button-danger:
    backgroundColor: "{colors.error}"
    textColor: "{colors.on-primary}"
    typography: "{typography.body-strong}"
    rounded: "{rounded.sm}"
    padding: "{spacing.lg}"
    height: 36px
  input-default:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.on-surface}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
    height: 36px
  helper-caption:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.secondary}"
    typography: "{typography.caption}"
  role-badge:
    backgroundColor: "{colors.surface-subtle}"
    textColor: "{colors.on-surface}"
    typography: "{typography.caption-strong}"
    rounded: "{rounded.sm}"
    padding: "{spacing.sm}"
  nav-item:
    backgroundColor: "{colors.surface}"
    textColor: "{colors.on-surface}"
    typography: "{typography.body-md}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
    height: 40px
  nav-item-active:
    backgroundColor: "{colors.surface-hover}"
    textColor: "{colors.on-surface}"
    typography: "{typography.body-strong}"
    rounded: "{rounded.sm}"
    padding: "{spacing.md}"
    height: 40px
  workspace-sheet:
    backgroundColor: "{colors.surface-layer}"
    textColor: "{colors.on-surface}"
    rounded: "{rounded.md}"
    padding: "{spacing.xxxl}"
  card-surface:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.on-surface}"
    rounded: "{rounded.md}"
    padding: "{spacing.xxl}"
  toolbar-card:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.on-surface}"
    rounded: "{rounded.md}"
    padding: "{spacing.lg}"
  empty-state-card:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.on-surface}"
    rounded: "{rounded.md}"
    padding: "{spacing.xxxl}"
    width: 440px
  table-row:
    backgroundColor: "{colors.surface-card}"
    textColor: "{colors.on-surface}"
    typography: "{typography.body-md}"
    padding: "{spacing.md}"
    height: 40px
  divider-hairline:
    backgroundColor: "{colors.border}"
    height: 1px
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

# Aura Hotel — Desktop Design System (Windows 11 Fluent 2 & `WPF-UI` 4.3)

## Overview

Calm, high-density Windows 11 Fluent 2 desktop utility built for front-desk hotel receptionists working continuous shifts under the **AURA HOTEL** brand. Built on C# `.NET 10` (`net10.0-windows`), `CommunityToolkit.Mvvm` 8.4.2, `WPF-UI` 4.3.0, and `LiveChartsCore.SkiaSharpView.WPF` 2.0.5, modeled after flagship `WPF-UI` / Fluent 2 applications (**WPF-UI Gallery**, **WinUI 3 Gallery**, **Microsoft PowerToys Settings**, and **Lenovo Legion Toolkit**; see [`docs/research/wpf-ui-fluent-design.md`](docs/research/wpf-ui-fluent-design.md)).

Visual hierarchy relies on **Windows 11 three-tier tonal surface layering** (`Mica` / `#F3F3F3` base rail → `#F9F9F9` workspace sheet → `#FFFFFF` elevated cards with `1px` `#E5E5E5` hairline borders), scannable `40px` tabular rows, diacritic-safe Vietnamese typography, and restrained `#005FB8` accent highlights. Domain terms strictly follow `@GLOSSARY.md`.

## Colors

All XAML `Color` and `SolidColorBrush` definitions live exclusively in `src/AuraHotel/Resources/Colors.xaml` (aliasing `WPF-UI` theme keys after `<ui:ThemesDictionary Theme="Light" />`) and are consumed via `{StaticResource}`. Inline hex literals in Views or ViewModels are forbidden.

- **Three-Tier Surface Layering (`#F3F3F3` → `#F9F9F9` → `#FFFFFF`):**
  - **Layer 0 — Window & Rail Base (`AppSurfaceBrush` `{colors.surface}` `#F3F3F3` + `Mica`):** Anchors `ui:FluentWindow` (`WindowBackdropType="Mica"` with `#F3F3F3` fallback), `ui:TitleBar`, and the left `NavigationView` rail.
  - **Layer 1 — Workspace Content Sheet (`WorkspaceSurfaceBrush` `{colors.surface-layer}` `#F9F9F9`):** Backs the main content area (matching `WPF-UI` `SolidBackgroundFillColorTertiary` / `NavigationViewContentBackground`). Never make the workspace sheet `#FFFFFF`—keeping Layer 1 at `#F9F9F9` ensures every `#FFFFFF` card inside it achieves immediate tonal contrast.
  - **Layer 2 — Elevated Cards & Tables (`CardSurfaceBrush` `{colors.surface-card}` `#FFFFFF`):** Elevates filter/toolbar strips, `DataGrid` containers, KPI cards, room cards, empty-state panels, and modal form cards, bounded by `1px` `CardStrokeBrush` (`{colors.border}` `#E5E5E5`).
  - **Subtle & Interactive Fills (`SubtleSurfaceBrush` `#F3F3F3`, `HoverSurfaceBrush` `#EAEAEA`, `AccentTintBrush` `#EBF3FC`, `FlyoutSurfaceBrush` `#FCFCFC`):** Used for icon badges, pill tags, active navigation highlights, and transient popups.
- **Typography & Borders (`#1A1A1A` / `#5C5C5C` / `#757575` / `#E5E5E5`):** `TextPrimaryBrush` (`{colors.on-surface}`) for primary values, `TextSecondaryBrush` (`{colors.secondary}`) for labels/subtitles, `TextTertiaryBrush` (`{colors.tertiary}`) for hints/placeholders, and `CardStrokeBrush` (`{colors.border}`) for `1px` dividers.
- **Primary Accent (`#005FB8` / `#00529E` / `#004382`):** `AccentBrush` (`{colors.primary}`) is wired 1-to-1 into `WPF-UI`'s `AccentFillColorDefaultBrush` and reserved strictly for the single Primary CTA per screen/dialog, `3px × 16px` active navigation indicators, focus rings, and occupancy progress fills.
- **Semantic Feedback:** `SystemSuccessBrush` (`#0F7B0F` on `#DFF6DD`), `SystemCautionBrush` (`#8A5000` on `#FFF4CE`), and `SystemCriticalBrush` (`#C42B1C` / `#A42618` on `#FDE7E9`).
- **Fixed Domain `RoomStatus` Indicators (Immutable):**
  - `RoomAvailableBrush` (`#48BB78`) + `RoomAvailableTintBrush` (`#EDFDF3`): `Trống` (`Available`)
  - `RoomOccupiedBrush` (`#F56565`) + `RoomOccupiedTintBrush` (`#FFF5F5`): `Đang ở` (`Occupied`)
  - `RoomReservedBrush` (`#ED8936`) + `RoomReservedTintBrush` (`#FFFAF0`): `Đã đặt` (`Reserved`)
  - `RoomMaintenanceBrush` (`#A0AEC0`) + `RoomMaintenanceTintBrush` (`#F7FAFC`): `Bảo trì` (`Maintenance`)
  - Apply saturated status brushes **only** to `4px` left/top indicator bars, `8px` legend dots, or status text/strokes paired with soft tint pill backgrounds (`Room*TintBrush`). Room card and table row base surfaces remain `CardSurfaceBrush` (`#FFFFFF`).

## Typography

Use **Segoe UI Variable Display** / **Segoe UI Variable Text** (`Segoe UI` fallback) across the UI, **Segoe Fluent Icons** (via `WPF-UI` `ui:SymbolIcon`) for iconography, and **Cascadia Mono** (`Consolas` fallback) exclusively for monospace `.txt` invoice previews.

- **Vietnamese Diacritic Safety Rule (Mandatory in WPF):** Following `WPF-UI`'s `Typography.xaml`, XAML `TextBlock` styles **must omit explicit `LineHeight` setters** so DirectWrite's native font metrics render stacked Vietnamese diacritics (`ấ`, `ệ`, `ử`, `ỗ`, `Đặt phòng`) without top/bottom clipping across 100%–150% Windows DPI scales.
- **`PageTitleTextStyle` (`28px` SemiBold, `Segoe UI Variable Display`):** Rendered once in the workspace header row; never duplicated inside child `UserControl` content.
- **`KpiValueTextStyle` (`24px` SemiBold, `Segoe UI Variable Display`):** Numeric KPI figures on `ReportView`, paired with `CaptionTextStyle` labels.
- **`SectionTitleTextStyle` (`20px` SemiBold, `Segoe UI Variable Display`):** Modal dialog titles, empty-state titles, and major card headers.
- **`SubtitleTextStyle` (`16px` SemiBold, `Segoe UI Variable Text`):** Room card numbers (`P.101`) and drawer/panel subheaders.
- **`BodyStrongTextStyle` (`14px` SemiBold, `Segoe UI Variable Text`):** Table column headers, active nav labels, user full name, and emphasized values.
- **`BodyTextStyle` (`14px` Regular, `Segoe UI Variable Text`):** Standard table cells, form inputs, secondary buttons, and descriptions.
- **`CaptionStrongTextStyle` / `CaptionTextStyle` (`12px` SemiBold / Regular, `Segoe UI Variable Text`):** Role/status badges, section group headers, helper hints, footer counters, and `InlineErrorTextStyle` (`{colors.error-text}` `#A42618`) directly beneath invalid inputs.
- **`MonospaceReceiptTextStyle` (`13px` Regular, `Cascadia Mono`):** Read-only UTF-8 receipt preview matching exported `.txt` alignment.

## Iconography

`WPF-UI` 4.3.0's `SymbolExtensions.GetString()` decodes `SymbolRegular` values via 16-bit UTF-16LE bytes; any Plane 15 codepoint `>= 0xF0000` (such as `Calendar24 = 0xF0285` or `Door24 = 0xF09BA`) truncates into corrupted or blank glyphs. **All `ui:SymbolIcon` usages MUST use verified 16-bit BMP (`<= 0xFFFF`) `SymbolRegular` members:**

- **Navigation Rail (8 Canonical Modules):**
  - `Sơ đồ phòng`: `GridDots24` (`0xE6C5`)
  - `Đặt phòng`: `CalendarLtr24` (`0xE24F`)
  - `Phòng`: `Bed24` (`0xF1DA`)
  - `Loại phòng`: `Tag24` (`0xF77D`)
  - `Khách hàng`: `People24` (`0xF5A9`)
  - `Dịch vụ`: `DrinkCoffee24` (`0xF3C5`)
  - `Hóa đơn`: `ReceiptMoney24` (`0xEA03`)
  - `Báo cáo`: `DataTrending24` (`0xE45E`)
- **Shell & Standard Actions:** Brand `Building24` (`0xF205`), Account `PersonAccounts24` (`0xF5C1`), Logout `SignOut24` (`0xF6DA`), Search `Search24` (`0xF690`), Create `Add24` (`0xF10A`), Edit `Edit24` (`0xF3DE`), Delete `Delete24` (`0xF34D`), Export `.txt` `ArrowDownload24` (`0xF151`), Filter reset `ArrowCounterclockwise24` (`0xF14E`).

## Layout

All `Margin`, `Padding`, and grid gaps must use strict multiples of `4px` (`0`, `4`, `8`, `12`, `16`, `20`, `24`, `32`). Arbitrary pixel offsets (`5`, `6`, `14`, `18`, `22`, `26`) are forbidden.

- **Application Shell (`MainWindow` — Unified Fluent Silhouette):** Design target `1280×800`, hard minimum `MinWidth="1280" MinHeight="720"` for classroom projector compatibility.
  - **Root Window:** `<ui:FluentWindow ExtendsContentIntoTitleBar="True" WindowBackdropType="Mica" WindowCornerPreference="Default">` (`AppSurfaceBrush` `#F3F3F3` fallback).
  - **Top `ui:TitleBar` (`40px`–`48px`):** Lightweight draggable system caption bar (`AURA HOTEL — Hệ thống quản lý khách sạn`) with native Windows 11 Snap Layouts.
  - **Left Navigation Rail (`248px` width on Layer 0 `#F3F3F3` / `Mica`):**
    1. *Top Brand Header (`PaneHeader`):* `36×36` rounded accent-tint icon badge (`Building24`) + `AURA HOTEL` (`BodyStrongTextStyle`) + `Quản lý lưu trú` (`CaptionTextStyle`).
    2. *Middle Module List (`MenuItems`):* `40px`-tall `NavigationViewItem` rows (`Administrator`: 8 items; `Staff`: 6 items, hiding `Loại phòng` and `Báo cáo`).
    3. *Bottom Pinned Session Footer (`PaneFooter`):* Compact elevated `CardSurfaceBrush` (`#FFFFFF`) account card showing `PersonAccounts24`, `FullName`, `RoleDisplay` pill (`Quản trị` / `Lễ tân`), and a clean `Đăng xuất` (`SignOut24`) action button. Keeping session controls in `PaneFooter` balances vertical rail weight and frees the top-right workspace header for module actions.
  - **Right Workspace Sheet (Layer 1 `WorkspaceSurfaceBrush` `#F9F9F9`):** Flush right/bottom sheet with `CornerRadius="8,0,0,0"` and `BorderThickness="1,1,0,0"` (`CardStrokeBrush`), inner `Padding="32,24,32,24"`:
    1. *Workspace Header Row (`Auto`, bottom margin `20px`):* Left `PageTitle` (`28px` SemiBold) + optional contextual subtitle; right slot reserved for module-level actions.
    2. *Module Content Host (`*`):* `<ContentControl Content="{Binding CurrentView}" />` rendered **directly on the `#F9F9F9` workspace sheet** (never wrapped inside an extra outer `#FFFFFF` box).
- **Modal Windows & Auth Dialogs (`LoginWindow`, `RolePreviewWindow`, Form Modals):**
  - Fixed `Width` (`420px`–`520px`), **`SizeToContent="Height"`**, `ResizeMode="NoResize"`, `WindowStartupLocation="CenterScreen"` or `"CenterOwner"`. Never hardcode tight fixed `Height` on form/preview dialogs so wrapped Vietnamese text and validation banners never clip.
- **Catalog Views (`Room`, `RoomType`, `Customer`, `Service`, `Booking`, `Invoice`):** 3-row vertical `Grid` on the `#F9F9F9` sheet:
  1. *Command & Filter Card (`Auto`, bottom margin `12px`):* `CardSurfaceBrush` (`#FFFFFF`, `Padding="16,12"`, `CornerRadius="8"`, `1px` border) with left search/filter inputs (`8px` gap) and right single Primary CTA (`Appearance="Primary"`).
  2. *Data Card (`*`):* `CardSurfaceBrush` (`#FFFFFF`, `CornerRadius="8"`, `1px` border) hosting `DataGrid` (`40px` row height) or centered state card (`Loading` / `Empty`).
  3. *Footer Summary (`Auto`, top margin `12px`):* Record count and safe-delete lock hint in `CaptionTextStyle`.
- **Room Matrix (`RoomMapView`):** Elevated filter + 4-status legend toolbar card (`Margin="0,0,0,16"`) above a `ScrollViewer` hosting an `ItemsControl` + `WrapPanel` (`12px` card gap) directly over the `#F9F9F9` sheet so `#FFFFFF` room cards stand out with crisp contrast down to `1280×720`.

## Elevation & Depth

Hierarchy is achieved strictly through **three-step tonal contrast** (`#F3F3F3` Mica base → `#F9F9F9` workspace sheet → `#FFFFFF` cards) paired with `1px` `CardStrokeBrush` (`#E5E5E5`) borders—never heavy drop shadows, glassmorphism, or saturated panel fills.

- Window & navigation rail sit at Layer 0 (`Mica` / `AppSurfaceBrush` `#F3F3F3`).
- Workspace content sheet sits at Layer 1 (`WorkspaceSurfaceBrush` `#F9F9F9`) with a `1px` top-and-left seam (`CardStrokeBrush`).
- Cards, toolbars, KPI tiles, room tiles, empty-state panels, and tables sit at Layer 2 (`CardSurfaceBrush` `#FFFFFF`, `BorderThickness="1"`, `BorderBrush="{StaticResource CardStrokeBrush}"`).

## Shapes

Follow Windows 11 corner geometry consistently through shared `CornerRadius` resources in `Styles.xaml`:

- **Interactive Controls & Badges (`{rounded.sm}` = `4px`):** Buttons, text inputs, password boxes, combo boxes, date pickers, status pills, and navigation items.
- **Cards, Sheets & Dialogs (`{rounded.md}` = `8px`):** Workspace sheet (`8,0,0,0`), filter cards, table cards, room cards, KPI cards, empty-state cards, flyouts, and dialogs.
- **Status Indicator Bars (`{rounded.xs}` = `2px`) & Dividers (`{rounded.none}` = `0px`):** `4px`-wide status bars and `1px` table row dividers. Pill-shaped (`999px`) buttons are forbidden.

## Motion & Micro-interaction

Front-desk reception workflows prioritize instant responsiveness (`0ms` view switching, native `WPF-UI` pointer-over/pressed brush transitions, and `ui:ProgressRing` only during async I/O). Decorative entrance storyboards, bounce animations, and chart transitions are omitted.

## Voice & Content Strategy

All user-facing UI text must be **100% Vietnamese with full diacritics**, sentence case (`Thêm khách hàng`, never `THÊM KHÁCH HÀNG` except receipt headers), concise, and operational, strictly respecting `@GLOSSARY.md` (`_Avoid_` list enforced).

### Data Formatting Rules

- **Currency (VNĐ):** Dot thousands separator with `đ` suffix: `#,##0 đ` (e.g., `500.000 đ`, `1.070.000 đ`) across XAML views and exported `.txt` receipts.
- **Dates & Stay Duration:** `dd/MM/yyyy` (e.g., `14/10/2026`); stay unit is always `đêm` (minimum `1 đêm` per `@GLOSSARY.md`).
- **Identifiers:** Phone `10` digits starting with `0` (`0901234567`); Citizen ID `9` or `12` digits (`079201001234`).

### 4 Mandatory UI States (Mutually Exclusive & Required on Every Data Screen)

Every ViewModel and View handling data or mutations must explicitly model and render all four states without visual overlap (`IsEmptyStateVisible => IsEmpty && !IsLoading`):

1. **Loading (`IsLoading`):** Display `ui:ProgressRing` (`32×32`) + Vietnamese loading caption inside a centered surface container and disable submit/mutation buttons to prevent double-submission.
2. **Empty (`IsEmptyStateVisible`):** Never render a blank `DataGrid` or tiny floating text lost in an empty window. Render a centered `EmptyStateCard` (`MaxWidth="440"`, `CardSurfaceBrush` `#FFFFFF`, `1px` border, `8px` radius, `Padding="32"`) containing a `48×48` subtle icon badge, `SectionTitleTextStyle` heading, `BodyTextStyle` guidance, and optional action CTA (`"Thêm ..."` or `"Xóa bộ lọc"`).
3. **Error (`ErrorMessage` / Field Validation):**
   - Field input errors render inline directly beneath the offending control using `InlineErrorTextStyle` (`12px`, `#A42618`).
   - Business rule blocks (booking overlap conflict, invalid date range, failed login, blocked deletion) render via `ui:InfoBar` (`Severity="Error"` or `"Warning"`) without raw `MessageBox.Show` spam.
4. **Success (Populated Data & Feedback):** Render aligned card/tabular data with safe action locks (disable `Xóa` + explanatory `ToolTip` when referenced or room is `Đang ở`), and confirm completed workflows via `ui:InfoBar` (`Severity="Success"`).

## Components

Build interfaces exclusively from **`WPF-UI` (`Wpf.Ui.Controls`, `xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"`), standard WPF data controls, and `LiveCharts2`**. Rewriting `<ControlTemplate>` for buttons, inputs, modals, or dropdowns is strictly prohibited.

- **Window & Shell:** `ui:FluentWindow` (`WindowBackdropType="Mica"`) + `ui:TitleBar` + `ui:NavigationView` (`PaneHeader`, `MenuItems`, `PaneFooter`) + `#F9F9F9` workspace sheet.
- **Buttons:** `ui:Button` with `MinHeight="36"` and `Padding="16,6"` (`Appearance="Primary"`, `Appearance="Secondary"`, or `Appearance="Danger"`). Never force rigid `Height="32"` on Vietnamese buttons.
- **Inputs & Pickers:** `ui:TextBox` (`PlaceholderText`, `Icon`), `ui:PasswordBox`, `ui:NumberBox`, `ComboBox`, `DatePicker` (`MinHeight="36"`), and `RadioButton` (`Tiền mặt` / `Chuyển khoản`).
- **Cards & Containers:** `ui:Card` / `ui:CardControl` or `Border` with `CardSurfaceBorderStyle` (`#FFFFFF`, `1px` `#E5E5E5`, `8px` radius) placed over the `#F9F9F9` workspace sheet.
- **Tables:** WPF `DataGrid` or `ui:DataGrid` (`AutoGenerateColumns="False"`, `IsReadOnly="True"`, `RowHeight="40"`, `GridLinesVisibility="Horizontal"`) hosted inside a `CardSurfaceBrush` card.
- **Room Card (`RoomMapView`):** `#FFFFFF` `CardSurfaceBorderStyle` tile (`Width="220"`) on the `#F9F9F9` sheet with XAML `DataTrigger` binding `Status` (`Trống`, `Đang ở`, `Đã đặt`, `Bảo trì`) to a `4px` status bar (`Room*Brush`) and soft tint badge (`Room*TintBrush` + Vietnamese text).
- **KPI & Occupancy (`ReportView`):** Three equal-width `#FFFFFF` `CardSurfaceBorderStyle` KPI cards (`12px` gap) over `#F9F9F9`, `ProgressBar` (`AccentBrush`), and `LiveCharts2` `CartesianChart` (`LineSeries`) in its own `#FFFFFF` card.
- **Dialogs & Banners:** Modal `ui:FluentWindow` (`SizeToContent="Height"`, `ResizeMode="NoResize"`) or `ui:ContentDialog`; `ui:InfoBar` (`StatusInfoBarStyle`) for inline banners.

## Do's and Don'ts

- **Do** preserve the **3-tier surface ramp** (`#F3F3F3` Mica rail → `#F9F9F9` workspace sheet → `#FFFFFF` cards) so cards and tables have natural elevation contrast.
- **Don't** wrap `<ContentControl Content="{Binding CurrentView}" />` inside a giant `#FFFFFF` outer card (which flattens inner `#FFFFFF` cards to zero contrast).
- **Do** pin the user session card (`FullName`, `RoleDisplay`, `Đăng xuất`) in `NavigationView.PaneFooter` at the bottom-left of the navigation rail.
- **Don't** use Plane 15 `SymbolRegular` icons (`>= 0xF0000` like `Calendar24` or `Door24`); restrict `ui:SymbolIcon` to verified 16-bit BMP icons (`<= 0xFFFF`).
- **Do** omit explicit `LineHeight` on WPF `TextBlock` styles, use `MinHeight="36"` on buttons/inputs, and set `SizeToContent="Height"` on modal dialogs so Vietnamese diacritics and wrapped lines never clip.
- **Don't** write custom `<ControlTemplate TargetType="Button">`, hardcode inline `#...` hex outside `Colors.xaml`, or use non-4px margins/paddings.
- **Do** enforce at most **one Primary CTA** (`Appearance="Primary"`) per screen or modal dialog and keep `Loading` vs `Empty` states mutually exclusive.
