# API Tester — How To Use

## Setup
1. Run the project (`F5` in Visual Studio or `dotnet run`)
2. Check the port in `launchSettings.json` and update `@baseUrl` in each `.http` file if needed
3. In each `.http` file, update the `@...Token` and `@...Id` variables at the top after you get them from responses

---

## Recommended Testing Order

Follow this exact flow — each step depends on the previous one.

### Step 1 — Auth (AuthAPITester/Auth.http)
1. Register an **Owner** account
2. Register a **Customer** account
3. Register an **Engineer** account
4. Verify email for each (paste OTP from email inbox)
5. Login with each account — copy the `accessToken` from each response

### Step 2 — Company (CompanyAPITester/Company.http)
1. Use Owner token → Create a Company → copy `companyId` from response
2. Add the Engineer as a member → copy Engineer's `userId` from DB or response
3. Set Engineer rank to `Leader`
4. Get company members to verify

### Step 3 — Department (DepartmentAPITester/Department.http)
1. Owner creates a Department inside the company → copy `departmentId`
2. Get department teams (should be empty for now)

### Step 4 — Team (TeamAPITester/Team.http)
1. Owner creates a Team inside the department with the Engineer as leader → copy `teamId`
2. Get team members to verify

### Step 5 — Project (ProjectAPITester/Project.http)
1. Customer creates a Project → copy `projectId`
2. Owner browses pending projects
3. Owner submits an offer on the project (via Company.http → Add Offer)
4. Customer views offers on their project
5. Customer accepts the offer → project becomes `InProgress`
6. Owner assigns the project to the team
7. Verify with Get Accepted Projects

### Step 6 — Task (TaskAPITester/Task.http)
1. Owner or Leader creates a Task assigned to the Engineer
2. Engineer gets their tasks
3. Engineer updates task status → `InProgress`
4. Leader (Engineer with Leader rank) marks task as `Done`

---

## Token Variables Reference

| Variable | Where to get it |
|---|---|
| `@ownerToken` | Login response → `accessToken` |
| `@customerToken` | Login response → `accessToken` |
| `@engineerToken` | Login response → `accessToken` |
| `@companyId` | Create Company response → `companyId` |
| `@departmentId` | Create Department response → `departmentId` |
| `@teamId` | Create Team response → `teamId` |
| `@projectId` | Create Project response → `projectId` |
| `@taskId` | Create Task response → `taskId` |
