# SaaS.LicenseManager

Nagadan ERP License Manager – a modular SaaS-based solution for managing software licenses, subscriptions, and user access across ERP deployments.

## 🚀 Overview
SaaS.LicenseManager is designed to provide **secure, scalable, and flexible license management** for ERP systems.  
It helps administrators and developers to:
- Generate and validate licenses
- Manage subscription lifecycles
- Control user access and permissions
- Integrate license checks into ERP modules

This project is built with **ASP.NET Core MVC** and follows a clean architecture with Controllers, Models, Services, and Views.

---

## 📂 Project Structure
- **Controllers/** – Handles API and UI requests  
- **Filter/** – Custom filters for authentication and license validation  
- **Helpers/** – Utility classes for common tasks  
- **Migrations/** – Database migration scripts  
- **Models/** – Entity and data models  
- **Services/** – Business logic and license management services  
- **Views/** – Razor views for UI  
- **wwwroot/** – Static assets (CSS, JS, images)  
- **Program.cs** – Application entry point  
- **appsettings.json** – Configuration file  

---

## 🛠️ Technologies Used
- **C# / ASP.NET Core MVC** (Backend)
- **Entity Framework Core** (Database ORM)
- **HTML, CSS, JavaScript** (Frontend)
- **SQL Server** (Default database, configurable)

---

## 🔑 Admin Credentials & Access

To manage the system, use the following admin credentials:

- **Admin Login URL:** `/Admin/Login`
- **Default Username:** `admin`
- **Default Password:** `admin123`

### 🛠️ Admin Routing
The admin panel has a dedicated route configured in `Program.cs`:
- **Pattern:** `Admin/{action=Dashboard}/{id?}`
- **Primary Dashboard:** After login, you will be redirected to the Customer Management page (`/Customer/Index`).

## ⚙️ Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/basithalam/SaaS.LicenseManager.git
   cd SaaS.LicenseManager


1. Configure database connection
Update appsettings.json with your SQL Server connection string:"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=LicenseManagerDB;Trusted_Connection=True;"
}

2. Apply migrationsdotnet ef database update

3. Run the applicationdotnet run

4. Access the app at: http://localhost:5000


---

🔑 Features

• License generation and validation
• Subscription management (start, renew, expire)
• Role-based access control
• ERP integration-ready APIs
• Admin dashboard for monitoring licenses


---

📖 Usage

• Admins can create and assign licenses to ERP modules.
• Users authenticate and validate their license before accessing ERP features.
• Developers can extend services to integrate with other SaaS applications.


---

🤝 Contributing

Contributions are welcome!

1. Fork the repo
2. Create a feature branch (git checkout -b feature-name)
3. Commit changes (git commit -m "Add feature")
4. Push to branch (git push origin feature-name)
5. Open a Pull Request


---

📜 License

This project is licensed under the MIT License – feel free to use and modify with attribution.

---

👨‍💻 Author

Developed by Md. Sah Alam Basith

• Microsoft .NET Developer & ERP Architect
• Passionate about building scalable SaaS solutions


---
