# 📚 BookBazar Web Application

BookBazar is a web application where users can browse, buy, and manage books. Built with ASP.NET Core MVC, it features a clean, user-friendly interface, integrated shopping cart, and scalable layered architecture.

---

## 🚀 Features

- Full CRUD operations across modules  
- Browse and manage books  
- Shopping cart functionality (planned)  
- Secure authentication & authorization (planned)  
- Server-side and client-side validation  
- Responsive UI using Bootstrap  
- Entity Framework Core (Code First)  
- SQL Server integration  

---

## 🛠️ Tech Stack

- ASP.NET Core MVC (.NET 10)  
- Entity Framework Core  
- SQL Server  
- Bootstrap  
- Razor Views  

---

## 📁 Project Structure

```plaintext
BOOKBAZARASPNETMVC/
│
├── BookBazar.DataAccess/   # DbContext, Repositories, Migrations
├── BookBazar.Model/        # Entities / Domain Models
├── BookBazar.Utility/      # Helpers, Constants, Configs
├── BookBazar.Web/          # MVC Application (Startup Project)
│
├── .gitattributes
├── .gitignore
├── BookBazar.slnx
├── docker-compose.yml
├── Dockerfile
└── README.md
````

---

## ⚙️ Setup Instructions (First-Time User)

---

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/Roshanksingh/BookBazarASPNetMVC.git
cd BookBazarASPNetMVC
```

---

### 2️⃣ Restore Dependencies

```bash
dotnet restore
dotnet build
```

---

### 3️⃣ Configure Database

#### ✅ Option A: Local SQL Server (Windows)

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BookBazarDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

#### ✅ Option B: SQL Server in Docker (Recommended)

Run SQL Server:

```bash
docker run -d --name bookbazar_sql \
-e "ACCEPT_EULA=Y" \
-e "SA_PASSWORD=YourStrong!Passw0rd" \
-p 1433:1433 \
mcr.microsoft.com/mssql/server:2022-latest
```

Update connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=BookBazarDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True"
}
```

---

### 4️⃣ Run the Application (Migrations Auto-Apply)

```bash
dotnet run --project BookBazar.Web
```

👉 On first run:

* Database will be created automatically
* Migrations will be applied
* Tables will be created

---

## 🗄️ Manual Migration Commands (If Needed)

```bash
dotnet ef migrations add InitialCreate \
--project BookBazar.DataAccess \
--startup-project BookBazar.Web
```

```bash
dotnet ef database update \
--project BookBazar.DataAccess \
--startup-project BookBazar.Web
```

---

## 🐳 Run Project Using Docker (Full Setup)

```bash
docker-compose up -d --build
```

Access application:

```text
http://localhost:5000
```

---

## 🐳 Run Project Using Docker (SQL Only + Local App)

### Start SQL Server

```bash
docker start bookbazar_sql
```

### Run Application

```bash
dotnet run --project BookBazar.Web
```

---

## 🌐 Access Application

```text
http://localhost:5211
```

---

## 🛑 Docker Commands

### Stop SQL Server

```bash
docker stop bookbazar_sql
```

### Remove SQL Server

```bash
docker rm -f bookbazar_sql
```

### Clean Docker (Optional)

```bash
docker system prune -a --volumes -f
```

---

## 🧠 Key Concepts Used

* MVC Architecture
* Dependency Injection
* Entity Framework Core
* Code First Migrations
* Model Validation
* Razor Views
* Layered Architecture

---

## 🔒 Validation

* Data Annotations
* Client-side validation
* Server-side validation using `ModelState`

---

## 👨‍💻 Author

**Roshan Kumar Singh**

---

## ⭐ Future Improvements

* Authentication (Login/Register)
* Role-based authorization
* Repository & Service Layer
* API support
* Enhanced Book & Order management

---

## 📄 License

This project is for learning purposes.
