 MediCare — Clinic Management & Appointment System

 Project Overview
MediCare is a clinic management and appointment booking system built with ASP.NET Core MVC. It aims to simplify clinic operations, manage patient and doctor information, and make appointment scheduling easier.

 Main Objectives
- Organize clinic operations and patient records.
- Allow patients to book and manage appointments.
- Help doctors manage their schedules and profiles.
- Provide administrators with tools to manage the system.

 User Roles
- Patient: Register, manage their profile, and book appointments.
- Doctor:Manage their profile and appointments.
- Admin: Manage users and oversee system operations.

 Key Features
- User authentication and authorization.
- Patient and doctor profile management.
- Appointment booking and scheduling.
- Email notifications using Gmail SMTP.
- Real-time in-app notifications using SignalR.
- Dashboard and calendar views for appointment management.

 Technologies Used
- C#
- ASP.NET Core MVC
- Razor Views
- Entity Framework Core
- ASP.NET Core Identity
- SQL Server
- Bootstrap 5
- SignalR
- FullCalendar.js
- Chart.js
- Gmail SMTP

 Architecture & Design Patterns
The project follows an N-Tier Architecture to separate responsibilities and improve maintainability.

- MediCare.Web:Presentation layer and MVC controllers/views.
- MediCare.Services:Business logic.
- MediCare.Data: Data access and database operations.

Design patterns and concepts:
- Repository Pattern
- Unit of Work Pattern
- Dependency Injection

 Getting Started

 Prerequisites
- Visual Studio
- .NET SDK compatible with the project
- SQL Server
- Git

 Installation
1. Clone the repository:

   ```bash
   git clone https://github.com/DaReeNAhmed25/MediCare.git
   ```

2. Open the solution in Visual Studio.

3. Configure the database connection in `appsettings.json`.

4. Apply the Entity Framework Core migrations:

   ```bash
   dotnet ef database update
   ```

5. Run the application from Visual Studio or using:

   ```bash
   dotnet run
   ```

 Configuration & Security
- Configure your database connection string locally.
- Configure Gmail SMTP settings for email notifications.
- Never commit passwords, API keys, or other secrets to GitHub.

 Team Members
- Dareen Ahmed Abdelfatah Yahia — Project Manager/Frontend Developer
- Haneen Alaa Moustafa Mohamed— Frontend Developer
- Philopatir Philip Raouf Farah — Backend Developer
- Ahmed Ali Mohamed — Data and Integrations Developer
- Youssef Ali Farghaly Mohamed— QA and DevOps Engineer

 Future Enhancements
- Doctor license verification.
- Appointment waiting lists.
- Online payment integration.
- Patient reviews and ratings.
- Bilingual interface.
- Video consultations.

 Project Status
This project is being developed as a clinic management and appointment scheduling system.

 License
No license has been specified yet.
