# 📘 BookBazar ASP.NET MVC Application

BookBazar is an ASP.NET Core MVC web application using Entity Framework Core, SQL Server, and ASP.NET Identity.

---

## 🛠 Prerequisites

Ensure the following are installed:

- .NET SDK (same version as project)
- Docker (for containerized setup)
- SQL Server (only if running locally without Docker)

---

## 🚀 Running the Application

You can run the application in two ways:

---

# 1️⃣ Run Locally (Using .NET CLI + User Secrets)

### Step 1: Configure User Secrets

Navigate to the Web project:

```bash
cd BookBazar.Web
```

Set required secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=BookBazarDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True"

dotnet user-secrets set "Stripe:PublishableKey" "pk_test_xxx"
dotnet user-secrets set "Stripe:SecretKey" "sk_test_xxx"

dotnet user-secrets set "SeedAdmin:Email" "admin@example.com"
dotnet user-secrets set "SeedAdmin:Password" "StrongPassword!"
```

---

### Step 2: Start SQL Server

Using Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrongPassword123!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

---

### Step 3: Apply Migrations

From solution root:

```bash
dotnet ef database update --project BookBazar.DataAccess --startup-project BookBazar.Web
```

---

### Step 4: Run the Application

```bash
dotnet run --project BookBazar.Web
```

Open in browser:

```
http://localhost:5211
```

---

# 2️⃣ Run Using Docker

### Step 1: Create `.env`

Update `.env`:

```env
SA_PASSWORD=YourStrongPassword123!

APP_DB_CONNECTION=Server=sqlserver,1433;Database=BookBazarDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True

STRIPE_PUBLISHABLE_KEY=pk_test_xxx
STRIPE_SECRET_KEY=sk_test_xxx

SEED_ADMIN_EMAIL=admin@example.com
SEED_ADMIN_PASSWORD=StrongPassword!
```

---

### Step 2: Run Containers

```bash
docker compose up --build
```

---

### Step 3: Access Application

```
http://localhost:8080
```

---

## ⚙️ Behavior

- Database is created automatically on startup
- EF Core migrations run automatically (Docker mode)
- Admin user is seeded using provided credentials
- Email sending is disabled (no-op implementation)

---

## 🛑 Troubleshooting

### `dotnet ef` not found

```bash
dotnet tool install --global dotnet-ef
```

---

### SQL connection issues

- Ensure SQL Server is running
- Verify password consistency between:
  - `SA_PASSWORD`
  - connection string

---

### Port already in use

```bash
dotnet run --project BookBazar.Web --urls "http://localhost:5005"
```

---

## 📌 Notes

- Do not commit `.env` file
- Use strong passwords for SQL and admin user
- For production, use environment variables or a secure secret manager

---

## ✅ Quick Commands

| Mode   | Command                              |
| ------ | ------------------------------------ |
| Local  | `dotnet run --project BookBazar.Web` |
| Docker | `docker compose up --build`          |
