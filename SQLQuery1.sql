SELECT name FROM sys.tables;
SELECT * FROM Services
SELECT * FROM Users

-- ==========================================
-- 1. CLEAN THE SLATE (Prevents Duplicate Errors)
-- ==========================================
DELETE FROM Bookings;
DELETE FROM Barbers;
DELETE FROM Services;
DELETE FROM Shops;
DELETE FROM Users;

-- Optional: Reset the ID counters back to 1
DBCC CHECKIDENT ('Barbers', RESEED, 0);
DBCC CHECKIDENT ('Services', RESEED, 0);
DBCC CHECKIDENT ('Shops', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);

-- ==========================================
-- 2. CREATE THE SHOPS
-- ==========================================
DECLARE @Shop1Id INT, @Shop2Id INT;

INSERT INTO Shops (ShopName, UniqueUrlSlug, OwnerEmail, CreatedAt)
VALUES ('Fade Cave', 'fade-cave', 'admin@fadecave.com', GETUTCDATE());
SET @Shop1Id = SCOPE_IDENTITY(); -- Grabs Fade Cave's new ID

INSERT INTO Shops (ShopName, UniqueUrlSlug, OwnerEmail, CreatedAt)
VALUES ('City Clippers', 'city-clippers', 'admin@cityclippers.com', GETUTCDATE());
SET @Shop2Id = SCOPE_IDENTITY(); -- Grabs City Clippers' new ID

-- ==========================================
-- 3. CREATE THE SERVICES FOR EACH SHOP
-- ==========================================
INSERT INTO Services (Name, Price, Duration, ShopId) VALUES
('Premium Haircut', 300.00, 30, @Shop1Id),
('Luxury Shave', 150.00, 15, @Shop1Id),
('Facial Treatment', 700.00, 45, @Shop1Id),
('City Clippers Signature Cut', 500.00, 40, @Shop2Id);

-- ==========================================
-- 4. CREATE THE USER LOGIN ACCOUNTS
-- ==========================================
DECLARE @RahulUserId INT, @AmitUserId INT, @SureshUserId INT;

-- Global Admin & Customer
INSERT INTO Users (Name, Email, Password, IsAdmin) VALUES ('System Admin', 'admin@barbershop.com', 'admin123', 1);
INSERT INTO Users (Name, Email, Password, IsAdmin) VALUES ('Test Customer', 'customer@test.com', 'password123', 0);

-- Barber Logins (Capturing their IDs to link them later)
INSERT INTO Users (Name, Email, Password, IsAdmin) VALUES ('Rahul Sharma', 'barber1@barbershop.com', 'barber123', 0);
SET @RahulUserId = SCOPE_IDENTITY();

INSERT INTO Users (Name, Email, Password, IsAdmin) VALUES ('Amit Patil', 'barber2@barbershop.com', 'barber123', 0);
SET @AmitUserId = SCOPE_IDENTITY();

INSERT INTO Users (Name, Email, Password, IsAdmin) VALUES ('Suresh Kumar', 'barber3@barbershop.com', 'barber123', 0);
SET @SureshUserId = SCOPE_IDENTITY();

-- ==========================================
-- 5. LINK USERS TO BARBER PROFILES AND SHOPS
-- ==========================================
INSERT INTO Barbers (Name, Specialization, UserId, ShopId) VALUES
('Rahul Sharma', 'Haircut & Styling', @RahulUserId, @Shop1Id),
('Amit Patil', 'Beard Trim & Styling', @AmitUserId, @Shop1Id),
('Suresh Kumar', 'Hair Spa & Color', @SureshUserId, @Shop2Id);

PRINT '✅ SAAS DATABASE SEEDED SUCCESSFULLY VIA SQL!';

