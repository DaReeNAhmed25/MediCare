PROJECT CONTEXT: MediCare (read this before every task)

PROJECT
MediCare is a web-based clinic management and appointment platform built with ASP.NET Core MVC (latest LTS .NET installed), Entity Framework Core, ASP.NET Identity, SQL Server locally and Azure SQL in production, in an N-Tier architecture. Roles: Patient, Doctor, Admin.

SOURCES OF TRUTH
1. docs/SPEC.md: functional requirements (FR), non-functional requirements (NFR), business rules (BR), use cases and the database schema. If a task prompt conflicts with SPEC.md, SPEC.md wins; tell me about the conflict instead of guessing.
2. The UI/ folder at the repository root: design images of the project's screens. See the UI RULE below.

UI RULE (CRITICAL, APPLIES TO EVERY VIEW, LAYOUT AND PARTIAL)
- The UI/ folder contains design images for MOST of the pages, but NOT ALL of them.
- These images are the BASIS of the design. Before you build or change any view, layout, partial or CSS, open and study every image in UI/ that relates to that page. Treat them as the visual foundation: layout and page structure, color palette, typography, spacing, component styles (navbar or sidebar, buttons, cards, tables, forms, badges, calendars, charts), icons, and the wording of labels and headings.
- Reproduce the images as closely as possible using Bootstrap 5 plus custom CSS in wwwroot/css/site.css. Extract the colors, fonts and spacing from the images into CSS variables (for example --mc-primary) and reuse them everywhere. Do not invent a different visual style.
- For a page that has NO image: design it in the same visual language as the images, reusing the same components, colors and fonts. Do not introduce new colors, fonts or component styles. List these pages in your final report.
- If an image shows a feature, field or button that is out of scope or contradicts SPEC.md (for example payment, SMS, ratings, video call, walk-in registration, record signing), build the page layout but do NOT implement that functionality: omit the element or render it disabled, and mention it in your report.
- At the start of your first task, list the files in UI/ and write docs/UI-NOTES.md with: the design tokens (colors, fonts, radius, spacing), and a table mapping each image file to the page it represents. Keep this file updated in later tasks and read it before every UI task.
- Every page must be responsive (phone and desktop), keyboard accessible, with labels on all form fields and a validation message next to each failing field.

ARCHITECTURE RULES
- Solution MediCare.sln with three projects: MediCare.Web (MVC controllers, Razor views, view models), MediCare.Services (business logic and service interfaces), MediCare.Data (entities, ApplicationDbContext, repositories, Unit of Work, migrations).
- Dependencies flow one way only: Web -> Services -> Data. Controllers contain no business rules. Services depend on repository and Unit of Work interfaces. Use Dependency Injection everywhere. Use the Factory pattern to create notifications (email types).
- Use async/await for all I/O. Put [Authorize(Roles=...)] on every action and enforce ownership on the server (a patient sees only their own data; a doctor sees only their own appointments and patients). Anti-forgery tokens on every POST.
- Dates and times follow the schema: AppointmentDate (date) plus StartTime/EndTime (time) are clinic local time; CreatedAt and similar audit timestamps are UTC.
- Secrets (SendGrid key, connection strings) come from user-secrets, environment variables or Azure App Settings, never from committed files. appsettings.json holds placeholders only.
- Use sample data only. No real patient data.

OUT OF SCOPE (do NOT build)
SignalR or any real-time alerts, SMS, online payment, billing, insurance, walk-in patients, medical record signing/locking/addenda, record-view audit logging, doctor licence verification, AI diagnosis, mobile apps, ratings, video consultation.

KEY DECISIONS ALREADY MADE
- Email provider is SendGrid behind an IEmailSender abstraction; settings from configuration (SendGrid:ApiKey, SendGrid:FromEmail, SendGrid:FromName). Every email is logged in the Notifications table.
- The Admin can view, confirm, reject and cancel any appointment. There are no walk-in patients: Patients.UserId is required.
- Saving a medical record marks the appointment Completed in the same transaction. No signing, locking or addenda.
- Soft delete (IsDeleted) for Users, Patients, Doctors and Specializations. A deleted or suspended user cannot log in.
- A doctor cannot change working hours, slot length or vacation days if that conflicts with future Pending or Confirmed appointments; show the list of conflicting appointments instead.
- Patients and Admin can cancel, and patients can reschedule, only at least 2 hours before the start (configurable key Booking:CancelLeadHours). A rescheduled appointment returns to Pending.
- Double booking is prevented by a unique filtered index on Appointments (DoctorId, AppointmentDate, StartTime) where Status is not Cancelled or Rejected, plus RowVersion, plus checking and saving inside one Unit of Work transaction.
- AuditLogs record only Confirm, Reject and Cancel actions.

WORKFLOW RULES
- Work on a feature branch named feature/<short-name>. Make small commits with meaningful messages. Do not touch code unrelated to the task.
- Comment services and non-trivial logic clearly.
- The solution must build (dotnet build) and run (dotnet run) before you finish. Run all existing tests.
- Do not silently change decisions, delete earlier work or rewrite earlier modules; ask first.
- Finish every task with a short report: what you did, files changed, how to run and verify it, deviations from SPEC.md or the UI images, pages built without an image, and open questions.