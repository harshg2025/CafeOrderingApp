# 💻 Real Code Examples from CafeConnect Project

## 📋 Table of Contents
1. [C# Examples](#c-examples)
2. [SQL Examples](#sql-examples)
3. [HTML Examples](#html-examples)
4. [CSS Examples](#css-examples)
5. [JavaScript Examples](#javascript-examples)
6. [Razor Examples](#razor-examples)

---

## 🔵 C# Examples

### Example 1: Controller Method - Finding Nearby Cafes
**File**: `Controllers/CafeController.cs`

```csharp
public IActionResult Nearby()
{
    // Get user location from session (C# Session concept)
    var latStr = HttpContext.Session.GetString("Latitude");
    var lngStr = HttpContext.Session.GetString("Longitude");

    // Check if location exists (C# Conditional)
    if (string.IsNullOrEmpty(latStr) || string.IsNullOrEmpty(lngStr))
    {
        ViewBag.Message = "Location not set. Please update your location.";
        return View(new List<Cafe>());
    }

    // Convert string to double (C# Type Conversion)
    double userLat = Convert.ToDouble(latStr);
    double userLng = Convert.ToDouble(lngStr);

    // Create list to store cafes (C# Collections)
    List<Cafe> cafes = new List<Cafe>();

    // Connect to database (C# Using Statement)
    using (SqlConnection con = new SqlConnection(_connectionString))
    {
        // SQL query with Haversine formula
        string query = @"
            SELECT TOP 9 *, 
                   (6371 * ACOS(COS(RADIANS(@UserLat)) * COS(RADIANS(Latitude)) *
                   COS(RADIANS(Longitude) - RADIANS(@UserLng)) +
                   SIN(RADIANS(@UserLat)) * SIN(RADIANS(Latitude)))) AS Distance
            FROM Cafe
            ORDER BY Distance";

        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@UserLat", userLat);
        cmd.Parameters.AddWithValue("@UserLng", userLng);

        con.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        // Loop through results (C# While Loop)
        while (reader.Read())
        {
            // Create Cafe object (C# Object Creation)
            cafes.Add(new Cafe
            {
                CafeId = Convert.ToInt32(reader["CafeId"]),
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                Speciality = reader["Speciality"].ToString(),
                Ratings = Convert.ToDouble(reader["Ratings"]),
                Address = reader["Address"].ToString(),
                City = reader["City"].ToString(),
                Latitude = Convert.ToDouble(reader["Latitude"]),
                Longitude = Convert.ToDouble(reader["Longitude"]),
                ImageUrl = reader["ImageUrl"].ToString()
            });
        }
    }

    // Return view with data (C# Return Statement)
    return View(cafes);
}
```

**Concepts Used:**
- ✅ Classes and Objects (`Cafe` class)
- ✅ Methods (`Nearby()`)
- ✅ Sessions (`HttpContext.Session`)
- ✅ Collections (`List<Cafe>`)
- ✅ Conditional Statements (`if`)
- ✅ Loops (`while`)
- ✅ Type Conversion (`Convert.ToDouble()`)
- ✅ Using Statement (automatic cleanup)

---

### Example 2: User Login Method
**File**: `Controllers/UserController.cs`

```csharp
[HttpPost]  // HTTP Method Attribute
public IActionResult Login(string email, string password, double latitude, double longitude)
{
    User user = null;  // C# Nullable variable
    
    // Get location from session
    latitude = Convert.ToDouble(HttpContext.Session.GetString("Latitude"));
    longitude = Convert.ToDouble(HttpContext.Session.GetString("Longitude"));

    // Database connection
    using (SqlConnection con = new SqlConnection(_connectionString))
    {
        // SQL SELECT query
        string query = "SELECT * FROM [User] WHERE Email = @Email AND Password = @Password";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@Email", email);
        cmd.Parameters.AddWithValue("@Password", password);

        con.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        
        // Check if user exists
        if (reader.Read())  // C# Conditional
        {
            // Create User object
            user = new User
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                UserName = reader["UserName"].ToString(),
                Email = reader["Email"].ToString()
            };
        }
        reader.Close();

        // Update user location if login successful
        if (user != null)  // C# Null Check
        {
            string updateQuery = "UPDATE [User] SET Latitude = @Latitude, Longitude = @Longitude WHERE UserId = @UserId";
            SqlCommand updateCmd = new SqlCommand(updateQuery, con);
            updateCmd.Parameters.AddWithValue("@Latitude", latitude);
            updateCmd.Parameters.AddWithValue("@Longitude", longitude);
            updateCmd.Parameters.AddWithValue("@UserId", user.UserId);
            updateCmd.ExecuteNonQuery();
        }
    }

    // Set session if user found
    if (user != null)
    {
        HttpContext.Session.SetString("UserName", user.UserName);
        HttpContext.Session.SetInt32("UserId", user.UserId);
        return RedirectToAction("Index", "Home");
    }
    
    // Redirect to register if login fails
    return RedirectToAction("Register", "User");
}
```

**Concepts Used:**
- ✅ HTTP POST Method
- ✅ Method Parameters
- ✅ Nullable Types
- ✅ Conditional Statements
- ✅ Object Creation
- ✅ Session Management
- ✅ Redirect Actions

---

### Example 3: Model Class - Cafe
**File**: `Models/Cafe.cs`

```csharp
namespace CafeOrderingApp.Models
{
    public class Cafe  // C# Class
    {
        // Properties with getters and setters
        public int CafeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Speciality { get; set; }
        public double Ratings { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageUrl { get; set; }
    }
}
```

**Concepts Used:**
- ✅ Class Definition
- ✅ Properties (`get; set;`)
- ✅ Data Types (`int`, `string`, `double`)
- ✅ Namespace

---

## 🗄️ SQL Examples

### Example 1: Finding Nearby Cafes (Haversine Formula)
**Used in**: `CafeController.Nearby()`

```sql
SELECT TOP 9 *, 
       (6371 * ACOS(
           COS(RADIANS(@UserLat)) * COS(RADIANS(Latitude)) *
           COS(RADIANS(Longitude) - RADIANS(@UserLng)) +
           SIN(RADIANS(@UserLat)) * SIN(RADIANS(Latitude))
       )) AS Distance
FROM Cafe
ORDER BY Distance
```

**Concepts:**
- ✅ SELECT statement
- ✅ TOP clause (limit results)
- ✅ Mathematical functions (ACOS, COS, SIN, RADIANS)
- ✅ Parameters (@UserLat, @UserLng)
- ✅ ORDER BY clause
- ✅ Calculated column (Distance)

---

### Example 2: Joining Tables
**Used in**: `OrderController.MyBookings()`

```sql
SELECT o.*, c.Name AS CafeName 
FROM [Order] o
INNER JOIN Cafe c ON o.CafeId = c.CafeId
WHERE o.UserId = @UserId
ORDER BY o.BookingDate DESC
```

**Concepts:**
- ✅ SELECT with specific columns
- ✅ INNER JOIN (combine tables)
- ✅ Table aliases (o, c)
- ✅ WHERE clause (filtering)
- ✅ Parameters (@UserId)
- ✅ ORDER BY DESC (sorting)

---

### Example 3: Inserting New Booking
**Used in**: `CafeController.Book()`

```sql
INSERT INTO [Order] 
    (UserId, CafeId, BookingDate, BookingTime, NumberOfPeople, 
     SpecialRequest, Status, CreatedAt, UID, TID)
VALUES 
    (@UserId, @CafeId, @BookingDate, @BookingTime, @NumberOfPeople, 
     @SpecialRequest, @Status, @CreatedAt, @UID, @TID)
```

**Concepts:**
- ✅ INSERT statement
- ✅ Column list
- ✅ VALUES clause
- ✅ Parameters (all @ variables)

---

### Example 4: Updating Order Status
**Used in**: `OrderController.UpdateOrderStatus()`

```sql
UPDATE [Order] 
SET Status = @Status 
WHERE OrderId = @OrderId
```

**Concepts:**
- ✅ UPDATE statement
- ✅ SET clause
- ✅ WHERE clause
- ✅ Parameters

---

### Example 5: Searching Cafes
**Used in**: `CafeController.Search()`

```sql
SELECT TOP 20 * 
FROM Cafe
WHERE Name LIKE @term 
   OR City LIKE @term 
   OR Address LIKE @term
```

**Concepts:**
- ✅ SELECT statement
- ✅ WHERE clause
- ✅ LIKE operator (pattern matching)
- ✅ OR operator (multiple conditions)
- ✅ Parameters with wildcards (`%term%`)

---

## 🌐 HTML Examples

### Example 1: Login Form
**File**: `Views/Home/Index.cshtml`

```html
<!-- Modal for Login -->
<div class="modal fade" id="loginModal" tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Login</h5>
                <button type="button" class="close" data-dismiss="modal">
                    <span>&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <form asp-controller="User" asp-action="Login" method="post">
                    <div class="form-group">
                        <input type="email" 
                               name="email" 
                               class="form-control" 
                               placeholder="Email" 
                               required />
                    </div>
                    <div class="form-group">
                        <input type="password" 
                               name="password" 
                               class="form-control" 
                               placeholder="Password" 
                               required />
                    </div>
                    <input type="hidden" id="latitude" name="latitude" />
                    <input type="hidden" id="longitude" name="longitude" />
                    <button type="submit" class="btn btn-primary btn-block">
                        Login
                    </button>
                </form>
            </div>
        </div>
    </div>
</div>
```

**Concepts:**
- ✅ Modal structure
- ✅ Form element
- ✅ Input types (email, password, hidden)
- ✅ Required attribute
- ✅ Button element
- ✅ Razor tag helpers (asp-controller, asp-action)

---

### Example 2: Booking Form
**File**: `Views/Cafe/Details.cshtml`

```html
<form asp-action="Book" asp-controller="Cafe" method="post">
    <input type="hidden" name="CafeId" value="@Model.CafeId" />
    
    <div class="mb-3">
        <label class="form-label">Booking Date</label>
        <input type="date" 
               class="form-control" 
               name="BookingDate" 
               required />
    </div>
    
    <div class="mb-3">
        <label class="form-label">Booking Time</label>
        <input type="time" 
               class="form-control" 
               name="BookingTime" 
               required />
    </div>
    
    <div class="mb-3">
        <label class="form-label">Number of People</label>
        <input type="number" 
               class="form-control" 
               name="NumberOfPeople" 
               min="1" 
               required />
    </div>
    
    <div class="mb-3">
        <label class="form-label">Special Request</label>
        <textarea class="form-control" 
                  name="SpecialRequest" 
                  rows="3" 
                  placeholder="Any preferences or notes...">
        </textarea>
    </div>
    
    <button type="submit" class="btn btn-primary">Submit Booking</button>
</form>
```

**Concepts:**
- ✅ Form with POST method
- ✅ Hidden input (CafeId)
- ✅ Date input type
- ✅ Time input type
- ✅ Number input with min attribute
- ✅ Textarea element
- ✅ Razor syntax (@Model.CafeId)

---

### Example 3: Navigation Menu
**File**: `Views/Home/Index.cshtml`

```html
<nav class="navbar navbar-expand-lg">
    <a class="navbar-brand" href="/">
        <span>CafeConnect</span>
    </a>
    <div class="User_option">
        <button onclick="updateLocation()" class="btn btn-primary">
            <i class="fa fa-map-marker"></i> Update Location
        </button>
        
        <form class="form-inline" asp-controller="Cafe" asp-action="Search" method="get">
            <input type="search" 
                   name="term" 
                   placeholder="Search" 
                   class="form-control" />
            <button class="btn" type="submit">
                <i class="fa fa-search"></i>
            </button>
        </form>
        
        @if (!string.IsNullOrEmpty(userName))
        {
            <a href="#" class="pt-3">
                <p> 👤 @userName</p>
            </a>
            <a href="/User/Logout" class="btn btn-outline-danger btn-sm">
                <i class="fa fa-sign-out"></i> Logout
            </a>
        }
        else
        {
            <a href="#" class="pt-3">
                <p> 👤 Guest</p>
            </a>
        }
    </div>
</nav>
```

**Concepts:**
- ✅ Navigation structure
- ✅ Links (`<a>` tags)
- ✅ Forms
- ✅ Conditional Razor syntax (@if)
- ✅ Razor variables (@userName)

---

## 🎨 CSS Examples

### Example 1: Custom Modal Styling
**File**: `Views/Home/Index.cshtml` (inline styles)

```css
.auth-modal {
    border-radius: 10px;
    box-shadow: 0 10px 30px rgba(0,0,0,0.1);
}

.auth-modal .modal-header {
    border-bottom: none;
    background-color: #4855fe;
    border-top-left-radius: 10px;
    border-top-right-radius: 10px;
}

.auth-modal .modal-title {
    font-weight: 600;
    font-size: 20px;
}

.auth-modal .form-control {
    border-radius: 6px;
    font-size: 14px;
    padding: 10px;
}

.auth-modal .btn-block {
    padding: 10px;
    font-weight: 600;
}

.auth-modal a {
    color: #ffe537;
    text-decoration: none;
    font-weight: 500;
}
```

**Concepts:**
- ✅ Class selectors (`.auth-modal`)
- ✅ Nested selectors
- ✅ Properties (border-radius, box-shadow, background-color)
- ✅ Color values (hex codes)
- ✅ Typography (font-weight, font-size)

---

### Example 2: Bootstrap Classes Used
**Throughout the project**

```html
<!-- Container and Grid System -->
<div class="container">
    <div class="row">
        <div class="col-md-6">Content</div>
        <div class="col-md-6">Content</div>
    </div>
</div>

<!-- Buttons -->
<button class="btn btn-primary">Primary Button</button>
<button class="btn btn-outline-danger">Danger Outline</button>

<!-- Cards -->
<div class="card">
    <div class="card-body">
        <h5 class="card-title">Title</h5>
        <p class="card-text">Text</p>
    </div>
</div>

<!-- Forms -->
<div class="form-group">
    <label class="form-label">Label</label>
    <input class="form-control" />
</div>
```

**Concepts:**
- ✅ Bootstrap grid system
- ✅ Bootstrap components
- ✅ Utility classes

---

## ⚡ JavaScript Examples

### Example 1: Geolocation Function
**File**: `Views/Home/Index.cshtml`

```javascript
function updateLocation() {
    // Check if browser supports geolocation
    if (navigator.geolocation) {
        // Get current position
        navigator.geolocation.getCurrentPosition(
            function (position) {  // Success callback
                // Create data object
                const data = {
                    latitude: position.coords.latitude,
                    longitude: position.coords.longitude
                };

                // Send to server using Fetch API
                fetch('/User/UpdateLocation', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data)
                })
                .then(response => {
                    if (response.ok) {
                        alert('Location updated!');
                    } else {
                        alert('Failed to update location.');
                    }
                })
                .catch(error => {
                    console.error('Error updating location:', error);
                });
            },
            function (error) {  // Error callback
                alert("Geolocation error: " + error.message);
            }
        );
    } else {
        alert("Geolocation is not supported by this browser.");
    }
}
```

**Concepts:**
- ✅ Function definition
- ✅ Conditional statements
- ✅ API calls (navigator.geolocation)
- ✅ Callback functions
- ✅ Fetch API
- ✅ JSON.stringify()
- ✅ Promise handling (.then, .catch)
- ✅ Error handling

---

### Example 2: Search Toggle Functionality
**File**: `Views/Home/Index.cshtml`

```javascript
const toggleSearch = document.getElementById('toggleSearch');
const searchInput = document.getElementById('searchInput');
const searchForm = document.getElementById('searchForm');

let searchVisible = false;  // Variable to track state

// Event listener for button click
toggleSearch.addEventListener('click', function () {
    if (!searchVisible) {
        // Show the search input
        searchInput.style.display = 'inline-block';
        searchInput.focus();  // Focus on input
        searchVisible = true;
    } else {
        // Submit the form if input has value
        if (searchInput.value.trim() !== "") {
            searchForm.submit();
        } else {
            alert("Please enter a search term.");
        }
    }
});
```

**Concepts:**
- ✅ DOM selection (getElementById)
- ✅ Variables (const, let)
- ✅ Event listeners (addEventListener)
- ✅ Conditional statements
- ✅ DOM manipulation (style.display)
- ✅ Form submission
- ✅ String methods (trim())

---

## 🔷 Razor Examples

### Example 1: Displaying Cafe List
**File**: `Views/Cafe/Nearby.cshtml` (conceptual)

```razor
@model List<Cafe>  <!-- Model binding -->

@if (Model != null && Model.Count > 0)
{
    <div class="row">
        @foreach (var cafe in Model)  <!-- Loop through cafes -->
        {
            <div class="col-md-4 mb-4">
                <div class="card">
                    <img src="@cafe.ImageUrl" class="card-img-top" alt="@cafe.Name" />
                    <div class="card-body">
                        <h5 class="card-title">@cafe.Name</h5>
                        <p class="card-text">@cafe.Description</p>
                        <p><strong>Rating:</strong> @cafe.Ratings ★</p>
                        <p><strong>Address:</strong> @cafe.Address, @cafe.City</p>
                        <a href="/Cafe/Details/@cafe.CafeId" class="btn btn-primary">
                            View Details
                        </a>
                    </div>
                </div>
            </div>
        }
    </div>
}
else
{
    <p>No cafes found.</p>
}
```

**Concepts:**
- ✅ @model directive
- ✅ @if conditional
- ✅ @foreach loop
- ✅ @Model property access
- ✅ Razor expressions (@cafe.Name)

---

### Example 2: Conditional Display Based on Login
**File**: `Views/Home/Index.cshtml`

```razor
@{
    var userName = Context.Session.GetString("UserName");
}

@if (!string.IsNullOrEmpty(userName))
{
    <!-- Show for logged-in users -->
    <a href="#" class="pt-3">
        <p> 👤 @userName</p>
    </a>
    <a href="/User/Logout" class="btn btn-outline-danger btn-sm">
        <i class="fa fa-sign-out"></i> Logout
    </a>
    <a href="/Order/MyBookings" class="btn btn-outline-primary">
        <i class="fa fa-calendar-check"></i> My Orders
    </a>
}
else
{
    <!-- Show for guests -->
    <a href="#" class="pt-3">
        <p> 👤 Guest</p>
    </a>
    <a href="#" data-toggle="modal" data-target="#loginModal">
        <i class="fa fa-user"></i> <span>Login</span>
    </a>
    <a href="#" data-toggle="modal" data-target="#registerModal">
        <i class="fa fa-user-plus"></i> <span>Register</span>
    </a>
}
```

**Concepts:**
- ✅ Razor code block (@{ })
- ✅ Session access
- ✅ Conditional rendering (@if/@else)
- ✅ String methods (IsNullOrEmpty)
- ✅ Razor variables (@userName)

---

### Example 3: Displaying Menu Items
**File**: `Views/Cafe/Details.cshtml`

```razor
@model Cafe
@{
    var menuItems = ViewBag.MenuItems as List<Menu>;
}

<section class="py-5 bg-light">
    <div class="container">
        <h3>Menu Highlights</h3>
        <div class="row">
            @foreach (var item in menuItems)  <!-- Loop through menu -->
            {
                <div class="col-md-4 mb-4">
                    <div class="card h-100">
                        <img src="@item.ImageUrl" 
                             class="card-img-top" 
                             alt="@item.Name" />
                        <div class="card-body">
                            <h5 class="card-title">@item.Name</h5>
                            <p class="card-text">@item.Description</p>
                            <div class="d-flex justify-content-between">
                                <span class="badge bg-primary">₹@item.Price</span>
                                <small class="text-muted">@item.Category</small>
                            </div>
                        </div>
                    </div>
                </div>
            }
        </div>
    </div>
</section>
```

**Concepts:**
- ✅ ViewBag usage
- ✅ Type casting (as List<Menu>)
- ✅ @foreach loop
- ✅ Property access (@item.Name, @item.Price)

---

## 🔄 Complete Flow Example

### User Books a Cafe - All Languages Together

**1. HTML Form (User Input)**
```html
<form asp-action="Book" asp-controller="Cafe" method="post">
    <input type="date" name="BookingDate" required />
    <input type="time" name="BookingTime" required />
    <button type="submit">Book</button>
</form>
```

**2. C# Controller (Process Request)**
```csharp
[HttpPost]
public IActionResult Book(Order order)
{
    order.UserId = HttpContext.Session.GetInt32("UserId").Value;
    order.Status = "Pending";
    
    // SQL INSERT
    using (SqlConnection conn = new SqlConnection(_connectionString))
    {
        string query = @"INSERT INTO [Order] (...) VALUES (...)";
        // Execute query...
    }
    
    TempData["Success"] = "Booking submitted!";
    return RedirectToAction("Details", new { id = order.CafeId });
}
```

**3. SQL Database (Store Data)**
```sql
INSERT INTO [Order] 
    (UserId, CafeId, BookingDate, BookingTime, NumberOfPeople, Status)
VALUES 
    (@UserId, @CafeId, @BookingDate, @BookingTime, @NumberOfPeople, @Status)
```

**4. Razor View (Display Result)**
```razor
@if (TempData["Success"] != null)
{
    <div class="alert alert-success">
        @TempData["Success"]
    </div>
}
```

**5. CSS (Style the Message)**
```css
.alert-success {
    background-color: #d4edda;
    color: #155724;
    padding: 15px;
    border-radius: 5px;
}
```

---

## 📚 Summary

All these code examples show how different languages work together:
- **C#** handles logic and data processing
- **SQL** stores and retrieves data
- **HTML** creates structure
- **CSS** makes it beautiful
- **JavaScript** adds interactivity
- **Razor** connects C# with HTML

They all work as a team! 🎯
