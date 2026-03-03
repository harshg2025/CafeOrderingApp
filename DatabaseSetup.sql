-- =============================================
-- CafeConnect Database Setup Script
-- Run this script in SQL Server Management Studio
-- Make sure to select CafeBookingApp database first
-- =============================================

USE [CafeBookingApp]
GO

-- =============================================
-- 1. Create User Table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[User]') AND type in (N'U'))
BEGIN
    CREATE TABLE [User] (
        UserId INT PRIMARY KEY IDENTITY(1,1),
        UserName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        Password NVARCHAR(100) NOT NULL,
        Latitude FLOAT DEFAULT 0,
        Longitude FLOAT DEFAULT 0
    )
    PRINT 'User table created successfully'
END
ELSE
BEGIN
    PRINT 'User table already exists'
END
GO

-- =============================================
-- 2. Create Cafe Table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Cafe]') AND type in (N'U'))
BEGIN
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
    PRINT 'Cafe table created successfully'
END
ELSE
BEGIN
    PRINT 'Cafe table already exists'
END
GO

-- =============================================
-- 3. Create Menu Table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Menu]') AND type in (N'U'))
BEGIN
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
    PRINT 'Menu table created successfully'
END
ELSE
BEGIN
    PRINT 'Menu table already exists'
END
GO

-- =============================================
-- 4. Create Order Table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Order]') AND type in (N'U'))
BEGIN
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
    PRINT 'Order table created successfully'
END
ELSE
BEGIN
    PRINT 'Order table already exists'
END
GO

-- =============================================
-- 5. Create Feedback Table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Feedback]') AND type in (N'U'))
BEGIN
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
    PRINT 'Feedback table created successfully'
END
ELSE
BEGIN
    PRINT 'Feedback table already exists'
END
GO

-- =============================================
-- 6. Insert Sample Data
-- =============================================

-- Clear existing data (optional - comment out if you want to keep existing data)
-- DELETE FROM Menu
-- DELETE FROM [Order]
-- DELETE FROM Feedback
-- DELETE FROM Cafe
-- DELETE FROM [User]
-- GO

-- Insert Sample Cafes
IF NOT EXISTS (SELECT * FROM Cafe WHERE Name = 'Cafe Mocha')
BEGIN
    INSERT INTO Cafe (Name, Description, Speciality, Ratings, Address, City, Latitude, Longitude, ImageUrl)
    VALUES 
    ('Cafe Mocha', 'Cozy coffee shop with great ambiance and free WiFi. Perfect for work or relaxation.', 'Coffee & Pastries', 4.5, '123 Main Street, Near City Mall', 'Kolhapur', 16.7049, 74.2433, '/images/cafe1.jpg'),
    ('The Green Bean', 'Organic coffee and healthy snacks. Environmentally friendly cafe with vegan options.', 'Organic Food', 4.3, '456 Park Avenue, Shivaji Park', 'Sangli', 16.8524, 74.5815, '/images/cafe2.jpg'),
    ('Sunset Cafe', 'Best place for evening hangouts. Beautiful view and amazing desserts.', 'Desserts & Beverages', 4.7, '789 Beach Road, Near Lake', 'Kolhapur', 16.7050, 74.2435, '/images/cafe3.jpg'),
    ('Brew & Bite', 'Artisan coffee and fresh sandwiches. Great for breakfast and lunch.', 'Breakfast & Lunch', 4.4, '321 Market Street', 'Sangli', 16.8530, 74.5820, '/images/cafe4.jpg'),
    ('The Corner Cafe', 'Small cozy cafe with homemade cakes and cookies. Family-friendly atmosphere.', 'Bakery Items', 4.6, '654 Station Road', 'Kolhapur', 16.7055, 74.2440, '/images/cafe5.jpg')
    PRINT 'Sample cafes inserted'
END
ELSE
BEGIN
    PRINT 'Sample cafes already exist'
END
GO

-- Insert Sample Menu Items
IF NOT EXISTS (SELECT * FROM Menu WHERE Name = 'Cappuccino')
BEGIN
    INSERT INTO Menu (CafeId, Name, Description, Price, Category, ImageUrl)
    VALUES 
    -- Cafe Mocha (CafeId = 1)
    (1, 'Cappuccino', 'Rich espresso with steamed milk and foam', 120.00, 'Beverage', '/images/menu/cappuccino.jpg'),
    (1, 'Chocolate Cake', 'Decadent chocolate layer cake with cream', 150.00, 'Dessert', '/images/menu/cake.jpg'),
    (1, 'Veg Sandwich', 'Fresh vegetables with special sauce', 80.00, 'Snack', '/images/menu/sandwich.jpg'),
    (1, 'French Fries', 'Crispy golden fries with ketchup', 90.00, 'Snack', '/images/menu/fries.jpg'),
    
    -- The Green Bean (CafeId = 2)
    (2, 'Green Salad', 'Fresh organic vegetables with dressing', 180.00, 'Snack', '/images/menu/salad.jpg'),
    (2, 'Smoothie Bowl', 'Mixed fruits with yogurt and granola', 200.00, 'Snack', '/images/menu/smoothie.jpg'),
    (2, 'Organic Tea', 'Herbal tea with natural ingredients', 100.00, 'Beverage', '/images/menu/tea.jpg'),
    
    -- Sunset Cafe (CafeId = 3)
    (3, 'Ice Cream Sundae', 'Vanilla ice cream with chocolate sauce and nuts', 100.00, 'Dessert', '/images/menu/icecream.jpg'),
    (3, 'Cold Coffee', 'Iced coffee with cream and ice', 110.00, 'Beverage', '/images/menu/coldcoffee.jpg'),
    (3, 'Brownie', 'Chocolate brownie with vanilla ice cream', 130.00, 'Dessert', '/images/menu/brownie.jpg'),
    
    -- Brew & Bite (CafeId = 4)
    (4, 'Egg Sandwich', 'Scrambled eggs with cheese and vegetables', 120.00, 'Snack', '/images/menu/eggsandwich.jpg'),
    (4, 'Pancakes', 'Fluffy pancakes with maple syrup', 140.00, 'Snack', '/images/menu/pancakes.jpg'),
    
    -- The Corner Cafe (CafeId = 5)
    (5, 'Chocolate Chip Cookies', 'Homemade cookies with chocolate chips', 60.00, 'Dessert', '/images/menu/cookies.jpg'),
    (5, 'Muffin', 'Blueberry muffin fresh from oven', 70.00, 'Dessert', '/images/menu/muffin.jpg')
    
    PRINT 'Sample menu items inserted'
END
ELSE
BEGIN
    PRINT 'Sample menu items already exist'
END
GO

-- =============================================
-- 7. Create Indexes for Better Performance
-- =============================================

-- Index on Cafe table for location-based searches
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Cafe_Location')
BEGIN
    CREATE INDEX IX_Cafe_Location ON Cafe(Latitude, Longitude)
    PRINT 'Index on Cafe location created'
END
GO

-- Index on Order table for user queries
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Order_UserId')
BEGIN
    CREATE INDEX IX_Order_UserId ON [Order](UserId)
    PRINT 'Index on Order UserId created'
END
GO

-- Index on Order table for cafe queries
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Order_CafeId')
BEGIN
    CREATE INDEX IX_Order_CafeId ON [Order](CafeId)
    PRINT 'Index on Order CafeId created'
END
GO

PRINT '============================================='
PRINT 'Database setup completed successfully!'
PRINT '============================================='
PRINT 'Tables created: User, Cafe, Menu, Order, Feedback'
PRINT 'Sample data inserted: 5 cafes, 14 menu items'
PRINT 'Indexes created for better performance'
PRINT '============================================='
GO
