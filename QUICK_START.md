# ⚡ Quick Start Guide - CafeConnect Project

## 🎯 Fast Setup (5 Steps)

### Step 1: Install Software (15 minutes)
- ✅ Download & Install [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ Download & Install [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- ✅ Download & Install [SQL Server Management Studio](https://aka.ms/ssmsfullsetup)

### Step 2: Create Database (2 minutes)
1. Open **SQL Server Management Studio**
2. Connect to: `localhost` (or `localhost\SQLEXPRESS`)
3. Right-click **Databases** → **New Database**
4. Name: `CafeBookingApp` → Click **OK**

### Step 3: Run SQL Script (1 minute)
1. In SSMS, select `CafeBookingApp` database
2. Click **File → Open → File**
3. Open `DatabaseSetup.sql` from project folder
4. Click **Execute** (F5)
5. Wait for "Database setup completed successfully!"

### Step 4: Update Connection String (2 minutes)
Open these 3 files and update connection string:

**Files to edit:**
- `Controllers/CafeController.cs` (line 12)
- `Controllers/UserController.cs` (line 9)
- `Controllers/OrderController.cs` (line 10)

**Change this:**
```csharp
// FROM:
Server=LAPTOP-NEJRIJP3

// TO (choose one):
Server=localhost                    // Default instance
Server=localhost\SQLEXPRESS        // Named instance
Server=YOUR_COMPUTER_NAME           // Your computer name
```

### Step 5: Run Project (1 minute)
```bash
# Open terminal in project folder
cd "C:\Users\91842\OneDrive\Desktop\Interview Project in Resume\Cafe Service System\CafeOrderingApp"

# Run these commands:
dotnet restore
dotnet build
dotnet run
```

**Open browser:** `https://localhost:7078`

---

## ✅ Done! 

You should now see the CafeConnect homepage.

**Test it:**
1. Click "Update Location" button
2. Register a new user
3. Search for cafes
4. Make a booking

---

## 🆘 Quick Troubleshooting

| Problem | Solution |
|---------|----------|
| Can't connect to SQL Server | Start SQL Server service in Services (services.msc) |
| Database doesn't exist | Create it in SSMS (Step 2) |
| Tables don't exist | Run DatabaseSetup.sql script (Step 3) |
| Connection string error | Update all 3 controller files (Step 4) |
| Port already in use | Change port in `Properties/launchSettings.json` |

---

## 📚 Full Guide

For detailed instructions, see: **SETUP_AND_RUN_GUIDE.md**

---

**Total Setup Time: ~20 minutes** ⏱️
