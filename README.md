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
