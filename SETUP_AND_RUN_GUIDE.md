# 🚀 CafeConnect Project - Setup & Run Guide

## 📋 Table of Contents
1. [Prerequisites](#prerequisites)
2. [Step 1: Install Required Software](#step-1-install-required-software)
3. [Step 2: Setup SQL Server Database](#step-2-setup-sql-server-database)
4. [Step 3: Create Database Tables](#step-3-create-database-tables)
5. [Step 4: Configure Connection String](#step-4-configure-connection-string)
6. [Step 5: Run the Project](#step-5-run-the-project)
7. [Troubleshooting](#troubleshooting)

---

## ✅ Prerequisites

Before starting, make sure you have:

- ✅ Windows 10/11 (or Windows Server)
- ✅ Administrator access
- ✅ Internet connection (for downloading software)

---

## 📦 Step 1: Install Required Software

### 1.1 Install .NET 8.0 SDK

1. **Download .NET 8.0 SDK:**
   - Visit: https://dotnet.microsoft.com/download/dotnet/8.0
   - Click "Download .NET SDK 8.0.x"
   - Choose the installer for Windows (x64)

2. **Install:**
   - Run the downloaded installer
   - Follow the installation wizard
   - Click "Install" and wait for completion

3. **Verify Installation:**
   - Open **Command Prompt** or **PowerShell**
   - Run: `dotnet --version`
   - You should see: `8.0.x` (or similar)

### 1.2 Install SQL Server

**Option A: SQL Server Express (Free, Recommended for Learning)**

1. **Download SQL Server Express:**
   - Visit: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Click "Download now" under **Express** edition
   - Choose "Express" (free version)

2. **Install SQL Server:**
   - Run the installer
   - Choose "Basic" installation type
   - Accept the license terms
   - Wait for installation to complete
   - Note your **SQL Server Instance Name** (usually: `localhost` or `localhost\SQLEXPRESS`)

3. **Install SQL Server Management Studio (SSMS):**
   - Download from: https://aka.ms/ssmsfullsetup
   - Install SSMS (this is the tool to manage your database)

**Option B: SQL Server LocalDB (Lighter Alternative)**

1. **Download SQL Server Express with LocalDB:**
   - Visit: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Download "Express" edition
   - During installation, select "LocalDB" feature

### 1.3 Install Visual Studio (Optional but Recommended)

**Option A: Visual Studio 2022 Community (Free)**

1. **Download:**
   - Visit: https://visualstudio.microsoft.com/downloads/
   - Download "Visual Studio 2022 Community" (free)

2. **Install:**
   - Run the installer
   - Select workload: **"ASP.NET and web development"**
   - Click "Install"

**Option B: Visual Studio Code (Lightweight Alternative)**

1. **Download:**
   - Visit: https://code.visualstudio.com/
   - Download and install VS Code

2. **Install Extensions:**
   - Open VS Code
   - Install: **"C#"** extension by Microsoft
   - Install: **"C# Dev Kit"** extension

---

## 🗄️ Step 2: Setup SQL Server Database

### 2.1 Open SQL Server Management Studio (SSMS)

1. Open **SQL Server Management Studio**
2. Connect to your SQL Server:
   - **Server name**: `localhost` or `localhost\SQLEXPRESS` (or your instance name)
   - **Authentication**: Windows Authentication
   - Click **Connect**

### 2.2 Create Database

1. Right-click on **"Databases"** in Object Explorer
2. Click **"New Database..."**
3. Enter database name: `CafeBookingApp`
4. Click **OK**

---

## 📊 Step 3: Create Database Tables

### 3.1 Create Tables Script

Open **SQL Server Management Studio**, connect to your server, and run this script in the `CafeBookingApp` database:

```sql
-- =============================================
-- CafeConnect Database Setup Script
-- =============================================

USE [CafeBookingApp]
GO

-- =============================================
-- 1. Create User Table
-- =============================================
CREATE TABLE [User] (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Latitude FLOAT DEFAULT 0,
    Longitude FLOAT DEFAULT 0
)
GO

-- =============================================
-- 2. Create Cafe Table
-- =============================================
CREATE TABLE Cafe (
    CafeId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Speciality NVARCHAR(200),
    Ratings FLOAT DEFAULT 0,
    Address NVARCHAR(500),
    City NVARCHAR(100),
    Latitude FLOAT,
    Longitude FLOAT,
    ImageUrl NVARCHAR(500)
)
GO

-- =============================================
-- 3. Create Menu Table
-- =============================================
CREATE TABLE Menu (
    MenuId INT PRIMARY KEY IDENTITY(1,1),
    CafeId INT NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10,2) NOT NULL,
    Category NVARCHAR(50),
    ImageUrl NVARCHAR(500),
    IsAvailable BIT DEFAULT 1,
    FOREIGN KEY (CafeId) REFERENCES Cafe(CafeId) ON DELETE CASCADE
)
GO

-- =============================================
-- 4. Create Order Table
-- =============================================
CREATE TABLE [Order] (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    CafeId INT NOT NULL,
    BookingDate DATE NOT NULL,
    BookingTime TIME NOT NULL,
    NumberOfPeople INT NOT NULL,
    SpecialRequest NVARCHAR(MAX),
    Status NVARCHAR(50) DEFAULT 'Pending',
    CreatedAt DATETIME DEFAULT GETDATE(),
    UID NVARCHAR(200),
    TID INT,
    FeedbackText NVARCHAR(MAX),
    Rating FLOAT,
    FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE,
    FOREIGN KEY (CafeId) REFERENCES Cafe(CafeId) ON DELETE CASCADE
)
GO

-- =============================================
-- 5. Create Feedback Table (Optional)
-- =============================================
CREATE TABLE Feedback (
    FeedbackId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    CafeId INT NOT NULL,
    OrderId INT NOT NULL,
    FeedbackText NVARCHAR(MAX),
    Rating FLOAT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE,
    FOREIGN KEY (CafeId) REFERENCES Cafe(CafeId) ON DELETE CASCADE,
    FOREIGN KEY (OrderId) REFERENCES [Order](OrderId) ON DELETE CASCADE
)
GO

-- =============================================
-- 6. Insert Sample Data (Optional)
-- =============================================

-- Insert Sample Cafes
INSERT INTO Cafe (Name, Description, Speciality, Ratings, Address, City, Latitude, Longitude, ImageUrl)
VALUES 
('Cafe Mocha', 'Cozy coffee shop with great ambiance', 'Coffee & Pastries', 4.5, '123 Main Street', 'Kolhapur', 16.7049, 74.2433, '/images/cafe1.jpg'),
('The Green Bean', 'Organic coffee and healthy snacks', 'Organic Food', 4.3, '456 Park Avenue', 'Sangli', 16.8524, 74.5815, '/images/cafe2.jpg'),
('Sunset Cafe', 'Best place for evening hangouts', 'Desserts', 4.7, '789 Beach Road', 'Kolhapur', 16.7050, 74.2435, '/images/cafe3.jpg')
GO

-- Insert Sample Menu Items
INSERT INTO Menu (CafeId, Name, Description, Price, Category, ImageUrl)
VALUES 
(1, 'Cappuccino', 'Rich espresso with steamed milk', 120.00, 'Beverage', '/images/menu/cappuccino.jpg'),
(1, 'Chocolate Cake', 'Decadent chocolate layer cake', 150.00, 'Dessert', '/images/menu/cake.jpg'),
(2, 'Green Salad', 'Fresh organic vegetables', 180.00, 'Snack', '/images/menu/salad.jpg'),
(3, 'Ice Cream Sundae', 'Vanilla ice cream with toppings', 100.00, 'Dessert', '/images/menu/icecream.jpg')
GO

PRINT 'Database setup completed successfully!'
GO
```

### 3.2 How to Run the Script

1. Open **SQL Server Management Studio**
2. Connect to your server
3. Click **"New Query"**
4. Make sure `CafeBookingApp` database is selected (use dropdown at top)
5. **Copy and paste** the entire script above
6. Click **"Execute"** (or press F5)
7. You should see: **"Database setup completed successfully!"**

---

## ⚙️ Step 4: Configure Connection String

### 4.1 Find Your SQL Server Instance Name

**Method 1: Check SQL Server Configuration Manager**
1. Press `Windows Key + R`
2. Type: `services.msc` and press Enter
3. Look for services starting with "SQL Server"
4. Note the instance name (e.g., `SQLEXPRESS` or `MSSQLSERVER`)

**Method 2: Check in SSMS**
- When you connect to SSMS, the server name shown is your instance name
- Common formats:
  - `localhost` or `.` (default instance)
  - `localhost\SQLEXPRESS` (named instance)
  - `LAPTOP-XXXXX` (your computer name)

### 4.2 Update Connection Strings in Code

You need to update connection strings in **3 controller files**:

**File 1: `Controllers/CafeController.cs`**
- Find line: `private readonly string _connectionString = "Server=..."`
- Replace with your server name

**File 2: `Controllers/UserController.cs`**
- Find line: `private readonly string _connectionString = "Server=..."`
- Replace with your server name

**File 3: `Controllers/OrderController.cs`**
- Find line: `private readonly string _connectionString = "Server=..."`
- Replace with your server name

**Connection String Format:**

```csharp
// For default SQL Server instance:
private readonly string _connectionString = "Server=localhost;Database=CafeBookingApp;Trusted_Connection=True;TrustServerCertificate=True";

// For named instance (like SQLEXPRESS):
private readonly string _connectionString = "Server=localhost\\SQLEXPRESS;Database=CafeBookingApp;Trusted_Connection=True;TrustServerCertificate=True";

// If using your computer name:
private readonly string _connectionString = "Server=YOUR_COMPUTER_NAME;Database=CafeBookingApp;Trusted_Connection=True;TrustServerCertificate=True";
```

**Example Update:**

```csharp
// BEFORE:
private readonly string _connectionString = "Server=LAPTOP-NEJRIJP3;Database=CafeBookingApp;Trusted_Connection=True;TrustServerCertificate=True";

// AFTER (replace LAPTOP-NEJRIJP3 with your server name):
private readonly string _connectionString = "Server=localhost;Database=CafeBookingApp;Trusted_Connection=True;TrustServerCertificate=True";
```

---

## 🏃 Step 5: Run the Project

### Method 1: Using Visual Studio

1. **Open Project:**
   - Open **Visual Studio**
   - Click **"Open a project or solution"**
   - Navigate to your project folder
   - Select `CafeOrderingApp.sln`

2. **Restore Packages:**
   - Right-click on solution in Solution Explorer
   - Click **"Restore NuGet Packages"**
   - Wait for packages to download

3. **Run Project:**
   - Press **F5** (or click green "Play" button)
   - Browser will open automatically
   - URL will be: `https://localhost:7078` or `http://localhost:5028`

### Method 2: Using Command Line (Terminal)

1. **Open Terminal/Command Prompt:**
   - Navigate to project folder:
     ```bash
     cd "C:\Users\91842\OneDrive\Desktop\Interview Project in Resume\Cafe Service System\CafeOrderingApp"
     ```

2. **Restore Packages:**
   ```bash
   dotnet restore
   ```

3. **Build Project:**
   ```bash
   dotnet build
   ```

4. **Run Project:**
   ```bash
   dotnet run
   ```

5. **Open Browser:**
   - Look for output like: `Now listening on: https://localhost:7078`
   - Open browser and go to that URL

### Method 3: Using Visual Studio Code

1. **Open Project:**
   - Open VS Code
   - Click **File → Open Folder**
   - Select your project folder

2. **Open Terminal:**
   - Press `` Ctrl + ` `` (backtick)
   - Or: **Terminal → New Terminal**

3. **Run Commands:**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

---

## 🎯 First Time Setup Checklist

- [ ] ✅ .NET 8.0 SDK installed
- [ ] ✅ SQL Server installed
- [ ] ✅ SQL Server Management Studio installed
- [ ] ✅ Database `CafeBookingApp` created
- [ ] ✅ Tables created (User, Cafe, Menu, Order, Feedback)
- [ ] ✅ Connection strings updated in all 3 controllers
- [ ] ✅ Sample data inserted (optional)
- [ ] ✅ Project builds successfully
- [ ] ✅ Project runs without errors

---

## 🔧 Troubleshooting

### Problem 1: "Cannot connect to SQL Server"

**Solutions:**
1. **Check SQL Server is running:**
   - Press `Windows Key + R`
   - Type: `services.msc`
   - Find "SQL Server (MSSQLSERVER)" or "SQL Server (SQLEXPRESS)"
   - Right-click → **Start** (if stopped)

2. **Check connection string:**
   - Verify server name is correct
   - Try `localhost` instead of computer name
   - For named instance, use: `localhost\SQLEXPRESS`

3. **Enable SQL Server Authentication (if needed):**
   - Open SSMS
   - Right-click server → Properties → Security
   - Enable "SQL Server and Windows Authentication"
   - Restart SQL Server service

### Problem 2: "Database does not exist"

**Solution:**
- Make sure you created the database `CafeBookingApp`
- Check spelling in connection string
- Verify database exists in SSMS

### Problem 3: "Table does not exist"

**Solution:**
- Run the table creation script again
- Make sure you're running it in the correct database
- Check for any errors in the script execution

### Problem 4: "Port already in use"

**Solution:**
- Change port in `Properties/launchSettings.json`
- Or stop the application using that port
- Find and kill process: `netstat -ano | findstr :5028`

### Problem 5: "Package restore failed"

**Solutions:**
1. Clear NuGet cache:
   ```bash
   dotnet nuget locals all --clear
   ```

2. Restore again:
   ```bash
   dotnet restore
   ```

3. Check internet connection

### Problem 6: "TrustServerCertificate error"

**Solution:**
- Make sure `TrustServerCertificate=True` is in connection string
- This is already included in the examples above

### Problem 7: "Session not working"

**Solution:**
- Check `Program.cs` has:
  ```csharp
  builder.Services.AddSession();
  app.UseSession();
  ```
- These should already be in your code

### Problem 8: "Images not showing"

**Solution:**
- Make sure images exist in `wwwroot/images/` folder
- Check image paths in database match actual file locations
- Verify `wwwroot` folder is in project root

---

## 📝 Quick Reference Commands

```bash
# Navigate to project folder
cd "C:\Users\91842\OneDrive\Desktop\Interview Project in Resume\Cafe Service System\CafeOrderingApp"

# Restore packages
dotnet restore

# Build project
dotnet build

# Run project
dotnet run

# Run with specific URL
dotnet run --urls "http://localhost:5000"

# Clean build
dotnet clean
dotnet build
```

---

## 🌐 Default URLs

After running the project, you can access it at:

- **HTTPS**: `https://localhost:7078`
- **HTTP**: `http://localhost:5028`

(Check your `launchSettings.json` for actual ports)

---

## ✅ Testing the Application

1. **Open Browser:**
   - Go to: `https://localhost:7078` (or the URL shown in terminal)

2. **Test Features:**
   - ✅ Homepage loads
   - ✅ Click "Update Location" button
   - ✅ Search for cafes
   - ✅ Click "Check Nearby Cafes"
   - ✅ Register a new user
   - ✅ Login with user
   - ✅ View cafe details
   - ✅ Make a booking
   - ✅ View "My Orders"

---

## 🎉 Success!

If everything works:
- ✅ You should see the CafeConnect homepage
- ✅ You can register/login
- ✅ You can search and book cafes
- ✅ Database operations work correctly

---

## 📞 Need Help?

**Common Issues:**
1. Check SQL Server is running
2. Verify connection string matches your SQL Server instance
3. Make sure all tables are created
4. Check browser console for JavaScript errors
5. Check terminal/Output window for C# errors

**Next Steps:**
- Add more sample cafes to database
- Customize the UI
- Add more features
- Deploy to cloud (Azure, AWS)

---

**Happy Coding! 🚀**
