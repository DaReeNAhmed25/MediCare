# MediCare — UI Notes

> Reference document for all future UI tasks. Read this before building or modifying any view, layout, or partial.

---

## Design Tokens

Extracted from `UI/MediCare interface system.png` (the official interface reference sheet).

### Colors

| Token | Hex | Usage |
|-------|-----|-------|
| `--mc-primary` | `#90D0F2` | Baby blue — primary brand, buttons, active nav, focus rings |
| `--mc-primary-light` | `#EAF4FA` | Light blue — hover states, card highlights, backgrounds |
| `--mc-sage` | `#B9CDB F` | Sage green — secondary accent, available badges |
| `--mc-ink` | `#253747` | Ink / near-black — headings, body text |
| `--mc-canvas` | `#F7F9FA` | Canvas — page background, sidebar background |
| `--mc-secondary-text` | `#607280` | Muted secondary text, metadata labels |
| `--mc-muted` | `#B4919B` | Muted pink-grey — lower-priority labels |
| `--mc-hairline` | `#E2EBEC` | Hairlines, dividers, table borders |
| `--mc-surface` | `#FFFFFF` | Card and panel surfaces |

### Status Badge Colors

| Status | Background | Text / Border |
|--------|-----------|---------------|
| `Confirmed` | `#EAF4FA` | `#2D7DD2` (blue) |
| `Pending` | `#FFF8E1` | `#E6A817` (amber) |
| `Completed` | `#E8F5E9` | `#388E3C` (green) |
| `Cancelled` | `#FFEBEE` | `#C62828` (red) |
| `Rejected` | `#F3E5F5` | `#6A1B9A` (purple) |
| `Available` | `#E8F5E9` | `#388E3C` (green) |
| `Off duty` | `#F5F5F5` | `#757575` (grey) |

### Typography

| Role | Font | Weight | Size |
|------|------|--------|------|
| All text | **Inter** | — | — |
| Page titles (h1) | Inter | 600 | 28px |
| Section headings (h2) | Inter | 600 | 17px |
| Body / table content | Inter | 400 | 14px |
| Labels / metadata | Inter | 400 | 12px |
| Compact labels | Inter | 400 | 11px |

### Spacing & Layout

| Token | Value | Usage |
|-------|-------|-------|
| `--mc-radius` | `8px` | Cards, inputs, buttons |
| `--mc-radius-sm` | `4px` | Badges, chips |
| Spacing unit | 4px | Base grid unit; use multiples |
| Control height | 36px (`6×4px`) | Buttons, inputs |
| Panel padding | 32px (`8×4px`) | Card/section inner padding |
| Table row | 66px min | Table data rows |
| Table header | 39px | Table header rows |
| Sidebar width | 160px | Fixed left nav |

### Shadows

| Level | CSS |
|-------|-----|
| Cards | `0 1px 3px rgba(37,55,71,0.08)` |
| Elevated panels | `0 2px 8px rgba(37,55,71,0.12)` |
| No default elevation | Cards use border instead of shadow by default |

---

## Image-to-Page Map

| Image File | Page / View |
|-----------|-------------|
| `MediCare interface system.png` | UI design tokens reference sheet (not a page) |
| `Admin overview.png` | Admin Dashboard — `GET /Admin` |
| `Admin overview — tablet.png` | Admin Dashboard (tablet breakpoint) |
| `Admin overview — mobile.png` | Admin Dashboard (mobile breakpoint) |
| `Doctors directory.png` | Doctors list / search — `GET /Doctors` |
| `Doctor profile.png` | Doctor detail / booking page — `GET /Doctors/{id}` |
| `Doctor dashboard.png` | Doctor Dashboard (day schedule) — `GET /Doctor` |
| `Doctor dashboard — mobile.png` | Doctor Dashboard (mobile breakpoint) |
| `Appointment management — day.png` | Appointment day view / Admin appointment list — `GET /Admin/Appointments?view=day` |
| `Appointment management — week.png` | Appointment week view — `GET /Admin/Appointments?view=week` |
| `Patient dashboard.png` | Patient Dashboard — `GET /Patient` |
| `Patient dashboard — mobile.png` | Patient Dashboard (mobile breakpoint) |
| `Patient management.png` | Admin Patient directory — `GET /Admin/Patients` |
| `Patient profile.png` | Admin Patient detail — `GET /Admin/Patients/{id}` |
| `Reports and analytics.png` | Admin Reports — `GET /Admin/Reports` |

---

## Pages Without a UI Image

The following pages exist in SPEC.md but have **no corresponding image in `UI/`**. They must be designed in the same visual language as the images above.

| Page | Route (planned) | Notes |
|------|----------------|-------|
| Login page | `GET /Account/Login` | Use mc-primary for the submit button |
| Registration (Patient) | `GET /Account/Register` | Multi-step form with OTP confirmation |
| Registration (Doctor) | `GET /Account/Register/Doctor` | Specialization, fee, photo upload |
| OTP verification | `GET /Account/VerifyOtp` | 6-digit code entry with countdown |
| Access denied | `GET /Account/AccessDenied` | Simple error card |
| Home / landing | `GET /` | Placeholder for Task 1; full landing in later task |
| Doctor — My Schedule | `GET /Doctor/Schedule` | Week view with FullCalendar |
| Doctor — My Patients | `GET /Doctor/Patients` | Table of patients with appointments |
| Doctor — Medical record form | `GET /Doctor/Records/Create/{appointmentId}` | Vitals + symptoms + prescription |
| Doctor — Prescription print | `GET /Doctor/Records/{id}/Prescription` | Print-optimised view |
| Doctor — Profile edit | `GET /Doctor/Profile` | Edit photo, fee, location |
| Patient — My Appointments | `GET /Patient/Appointments` | List/calendar of own appointments |
| Patient — Book appointment | `GET /Patient/Book/{doctorId}` | Date/slot picker |
| Patient — My Health | `GET /Patient/Health` | Medical records list |
| Patient — My Profile | `GET /Patient/Profile` | Edit personal info |
| Admin — Doctors list / management | `GET /Admin/Doctors` | Different from public directory |
| Admin — Specializations | `GET /Admin/Specializations` | CRUD list |
| Admin — Notifications log | `GET /Admin/Notifications` | Email log table |

---

## Component Conventions

- **Sidebar**: fixed 160 px, `--mc-canvas` background, active item highlighted with `--mc-primary-light` and left border in `--mc-primary`.
- **Topbar**: white background, search input, bell icon, user avatar+name dropdown.
- **Stat cards (KPI cards)**: white surface, 8 px radius, 1 px `--mc-hairline` border, 32 px padding; large numeric value in `--mc-ink`.
- **Tables**: no outer border, `--mc-hairline` row separators, 39 px header rows, 66 px data rows.
- **Status badges**: `border-radius: 4px`, `font-size: 12px`, `font-weight: 500`, color from status table above.
- **Buttons (primary)**: `background: --mc-primary`, `color: --mc-ink`, `border-radius: 8px`, `height: 36px`.
- **Buttons (secondary/ghost)**: `border: 1px solid --mc-hairline`, `background: white`, `color: --mc-ink`.
- **Forms**: `border: 1px solid --mc-hairline`, `border-radius: 8px`, `height: 36px` for inputs.
- **Clinic branding**: "MediCare" wordmark with a `+` icon in `--mc-primary`.
