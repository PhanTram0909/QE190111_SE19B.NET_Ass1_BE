Assignment 1 – Task & Team Management App: Project Setup & Public CRUD
Objective
Set up the foundation for a Task Management web application. This assignment focuses on:
-	Project scaffolding and folder structure
-	Database setup with Entity Framework Core on PostgreSQL
-	Building a public (no login required) CRUD interface for Departments, Projects, and Tasks
-	Deploying a live application: backend to Render.com, frontend to Vercel

Subject Code	PRN232
Assignment	Assignment 1 of 2
Duration	180 minutes (3 hours)
Total Score	10 points
Tech Stack	ASP.NET Core Web API (.NET 8)  |  PostgreSQL  |  Next.js (TypeScript)
Deployment	Backend → Render.com  |  Frontend → Vercel

1. Project Initialization
-	Backend: Create an ASP.NET Core Web API solution named StudentID_ClassCode_Ass1_BE.sln with three projects:
-	TaskTrack.API — Controllers, Program.cs, appsettings.json
-	TaskTrack.Repo — EF Core entities, DbContext, Repositories
-	TaskTrack.Service — Service interfaces and implementations
-	Frontend: Create a Next.js project (App Router, TypeScript) named StudentID_ClassCode_Ass1_FE. Organize with app/, components/, lib/ folders.
-	Environment: Add a .env.example file listing all required variables (DATABASE_URL, NEXT_PUBLIC_API_URL) without real secrets.
-	Version control: Create a public GitHub repository. Make an initial commit and continue committing regularly (at least 5 meaningful commits).

2. Database Setup
A PostgreSQL database and seed data are provided in TaskManagementDB_Postgres.sql. Run this script on your Render PostgreSQL instance to create all tables and insert sample data. Do not modify the schema.

Tables included:
-	Department (DepartmentID, DepartmentName, DepartmentDescription, IsActive)
-	Project (ProjectID, ProjectName, Description, StartDate, EndDate, Status, DepartmentID, IsActive, CreatedDate)
-	Project Status: 0 = Not Started, 1 = In Progress, 2 = Completed, 3 = On Hold
-	Task (TaskID, Title, Description, Status, Priority, DueDate, ProjectID, IsActive, CreatedDate, ModifiedDate)
-	Task Status: 0 = To Do, 1 = In Progress, 2 = Done, 3 = Cancelled
-	Task Priority: 0 = Low, 1 = Medium, 2 = High, 3 = Critical
-	Tag (TagID, TagName, Color) and TaskTag join table (TaskID, TagID)

-	Use EF Core Database-First to scaffold entities from the database:
-	dotnet ef dbcontext scaffold "<connection>" Npgsql.EntityFrameworkCore.PostgreSQL -o Models
-	Store the connection string in appsettings.json and read it from there — no hard-coded strings.
-	All database access must go through the Repository layer. Direct DB calls from Controllers are not allowed.

3. API Endpoints (No Authentication Required)
All endpoints in this assignment are public — no login or token is required. Implement the following using ASP.NET Core Web API backed by EF Core:

Departments
-	GET  /api/departments  — List all active departments
-	GET  /api/departments/{id}  — Get one department and its projects
-	POST /api/departments  — Create a new department
-	PUT  /api/departments/{id}  — Update a department
-	DELETE /api/departments/{id}  — Delete only if no projects are linked; otherwise return HTTP 400
-	GET  /api/departments/search?name=  — Search by name (partial match)

Projects
-	GET  /api/projects  — List all active projects (include department name)
-	GET  /api/projects/{id}  — Get one project and its tasks
-	GET  /api/projects/department/{departmentId}  — Get projects by department
-	POST /api/projects  — Create a new project
-	PUT  /api/projects/{id}  — Update a project
-	DELETE /api/projects/{id}  — Delete only if no tasks are linked; otherwise return HTTP 400
-	GET  /api/projects/search?name=&status=&departmentId=  — Filter projects (all params optional)

Tasks
-	GET  /api/tasks  — List all active tasks
-	GET  /api/tasks/{id}  — Get one task including its tags
-	GET  /api/tasks/project/{projectId}  — Get tasks by project
-	POST /api/tasks  — Create a task (accept optional TagIDs array)
-	PUT  /api/tasks/{id}  — Update a task; replace its tags; set ModifiedDate to now
-	DELETE /api/tasks/{id}  — Soft-delete: set IsActive = false, never hard-delete
-	GET  /api/tasks/search?title=&status=&priority=&projectId=&tagId=  — Filter tasks (all params optional)

Tags
-	GET  /api/tags  — List all tags
-	POST /api/tags  — Create a tag
-	PUT  /api/tags/{id}  — Update a tag
-	DELETE /api/tags/{id}  — Delete only if not used by any task; otherwise return HTTP 400

General rules: Validate all required fields and return HTTP 400 with field-level errors on failure. Enable Swagger in development mode. Configure CORS to allow the Vercel frontend domain.

4. Frontend Pages (Next.js)
All pages are public — no login required. The frontend must call the backend API for all data.

Public Pages
-	Home (/): Welcome banner, summary counts (departments, projects, tasks), list of active projects as cards.
-	Departments (/departments): List of all active departments. Click a department to see its projects.
-	Department Detail (/departments/[id]): Department info and its list of projects.
-	Project Detail (/projects/[id]): Project details and task list with status badge, priority badge, tags, and due date.
-	Task Detail (/tasks/[id]): All fields of a single task including tags.
-	Search (/search): Filter tasks by title, status, priority, project, and/or tag. Results update as filters change.

Management Pages (No Login — Public CRUD)
-	Department Management (/departments/manage): Table with Create, Edit, Delete (confirmation required). Create and Edit via modal.
-	Project Management (/projects/manage): Table with Create, Edit, Delete (confirmation required). Create and Edit via modal.
-	Task Management (/tasks/manage): Table with Create, Edit, soft-Delete (confirmation required). Tag selection via multi-select. Create and Edit via modal.
-	Tag Management (/tags/manage): Table with Create, Edit, Delete (confirmation required).

UI Requirements
-	Use a modern UI library: Tailwind CSS, shadcn/ui, MUI, Ant Design, or similar.
-	Status and priority must be displayed as colored badges, not raw numbers.
-	Show loading indicators during API calls and toast notifications after operations.
-	All delete actions require a confirmation dialog. All forms have client-side validation.
-	Responsive layout — works on desktop and mobile.

5. Deployment
-	Database: Create a free PostgreSQL database on Render.com. Run TaskManagementDB_Postgres.sql to initialize tables and seed data.
-	Backend: Push to a public GitHub repo. Create a Web Service on Render.com. Set environment variables: DATABASE_URL, ASPNETCORE_ENVIRONMENT=Production.
-	Frontend: Push to a separate public GitHub repo. Import into Vercel. Set NEXT_PUBLIC_API_URL to your Render backend URL.
-	Verify the API loads at the Render URL (Swagger) and the frontend loads at the Vercel URL.
-	Update CORS in the backend to allow the Vercel domain.

Optional Features (Bonus)
-	Add a status filter on the task list page (e.g. show only To Do or Done).
-	Add a GitHub Actions CI check that runs build/lint on every push.
-	Add an ERD diagram image to the repository README.

Deliverables
Submit a document named StudentID_ClassCode_Ass1.docx containing:
-	Link to your public GitHub repository (backend) with full source code and commit history.
-	Link to your public GitHub repository (frontend) with full source code and commit history.
-	Link to the live Render backend URL (Swagger should be accessible).
-	Link to the live Vercel frontend URL, where all public pages load real data.
-	Any known issues or incomplete features.
