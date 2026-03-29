# 📚 BookBazar Web Application

BookBazar is a web application where users can browse, buy, and manage books. Built with ASP.NET Core MVC, it features a clean, user-friendly interface, integrated shopping cart, and secure authentication. The app demonstrates modern web development best practices and scalable architecture for online marketplaces.

---

## 🚀 Features

- Full CRUD operations across modules
- Browse and manage books
- Shopping cart functionality (planned / in progress)
- Secure authentication & authorization (planned)
- Server-side and client-side validation
- Clean and responsive UI using Bootstrap
- Entity Framework Core (Code First)
- SQL

---

## 🛠️ Tech Stack

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core
- SQL Server
- Bootstrap (UI)
- Razor Views

---

## 📁 Project Structure

```
BookBazar.Web/
│
├── Controllers/
├── Models/
├── Data/
├── Views/
│   ├── Shared/
│   └── (Feature-based folders)
├── Migrations/
└── wwwroot/
```

---

## ⚙️ Setup Instructions

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/Roshanksingh/BookBazarASPNetMVC.git
cd BookBazarASPNetMVC
```

---

### 2️⃣ Configure Database

#### ✅ Option A: Local SQL Server (Windows)

Update `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BookBazarDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

#### ✅ Option B: Docker SQL Server (Mac / Cross-platform)

Run SQL Server in Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" \
-p 1433:1433 --name sqlserver \
-d mcr.microsoft.com/mssql/server:2022-latest
```

Update `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=BookBazarDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True"
}
```

---

### 3️⃣ Apply Migrations

```bash
dotnet ef database update
```

---

### 4️⃣ Run the Application

```bash
dotnet run
```

Then open:

```
https://localhost:xxxx
```
---

# 📦 Run Project Using Docker

## 🚀 Start the Application

From the project root (where `docker-compose.yml` exists):

```bash id="docker1"
docker-compose up -d --build
```

---

## 🌐 Access the Application

Open your browser:

```text id="docker2"
http://localhost:5000
```

---

## 🗄️ Run Database Migrations (First Time Only)

```bash id="docker3"
docker run -it --rm \
  --network bookbazaraspnetmvc_default \
  -v $(pwd):/app \
  -w /app/BookBazar.Web \
  mcr.microsoft.com/dotnet/sdk:10.0 \
  bash -c "dotnet restore && \
           dotnet tool install --global dotnet-ef && \
           export PATH=$PATH:/root/.dotnet/tools && \
           dotnet ef database update"
```

---

## 🔄 Restart the Application

```bash id="docker4"
docker-compose up -d
```

---

## 🛑 Stop the Application

```bash id="docker5"
docker-compose down
```

---

## 🧹 Clean Everything (Reset Docker)

```bash id="docker6"
docker system prune -a --volumes -f
```

---

## ⚠️ Notes

* Make sure Docker Desktop is running before executing commands
* If using Apple Silicon (M1/M2/M3), you may see a platform warning — this is normal
* Database runs inside Docker (`bookbazar_db` container)

---

# 📦 Run Project Using Docker (SQL Server Only) + Local App

## 🐳 Run SQL Server in Docker

```bash
docker run -d \
  --name bookbazar_sql \
  -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=YourStrong!Passw0rd" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

---

## ⚙️ Update Connection String

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=BookBazarDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True"
}
```

---

## ▶️ Run Application Locally

```bash
dotnet run
```

---

## 🗄️ Run Database Migrations (First Time Only)

```bash
dotnet ef database update
```

---

## 🔄 Start SQL Server Container

```bash
docker start bookbazar_sql
```

---

## 🛑 Stop SQL Server Container

```bash
docker stop bookbazar_sql
```

---

## ❌ Remove SQL Server Container

```bash
docker rm -f bookbazar_sql
```

---

## 🌐 Access Application

```text
http://localhost:5000
```

## 🔒 Validation

- Data annotations for model validation
- Client-side validation using unobtrusive validation
- Server-side validation using ModelState

---

## 🧠 Key Concepts Used

- MVC Architecture (Model-View-Controller)
- Dependency Injection
- Entity Framework Core (DbContext, Migrations)
- Model Validation with Data Annotations
- Razor Tag Helpers (`asp-for`, `asp-action`, etc.)

---

## 👨‍💻 Author

**Roshan Kumar Singh**

---

## ⭐ Future Improvements

- Add authentication (Login/Register)
- Add Book management (not just categories)
- Use Repository Pattern / Service Layer
- Add API version of the project

---

## 📄 License

This project is for learning purposes.
