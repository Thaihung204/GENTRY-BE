-- Restaurant Management System Database
-- SQL Server Script for Inserting Sample Data with Audit Fields

--Create database RestX_RestaurantManagement

-- Use the existing database
USE RestX_RestaurantManagement;
GO

-- Insert into [File]
INSERT INTO [File] (Id, Name, Url, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
('550e8400-e29b-41d4-a716-446655440000', 'restaurant_logo.jpg', '/Uploads/restaurant_logo.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440001', 'staff_avatar_1.jpg', '/Uploads/staff_avatar_1.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440002', 'staff_avatar_2.jpg', '/Uploads/staff_avatar_2.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440003', 'dish_com_tam_suon.jpg', '/Uploads/dish_com_tam_suon.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440004', 'dish_com_ga_nuong.jpg', '/Uploads/dish_com_ga_nuong.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440005', 'dish_thit_nuong.jpg', '/Uploads/dish_thit_nuong.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440006', 'dish_ca_kho_to.jpg', '/Uploads/dish_ca_kho_to.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440007', 'dish_suon_xao_chua_ngot.jpg', '/Uploads/dish_suon_xao_chua_ngot.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440008', 'dish_ga_chien_nuoc_mam.jpg', '/Uploads/dish_ga_chien_nuoc_mam.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440009', 'dish_bo_luc_lac.jpg', '/Uploads/dish_bo_luc_lac.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440010', 'dish_tom_rang_me.jpg', '/Uploads/dish_tom_rang_me.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440011', 'dish_muc_chien_gion.jpg', '/Uploads/dish_muc_chien_gion.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440012', 'dish_canh_chua_ca.jpg', '/Uploads/dish_canh_chua_ca.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440013', 'dish_banh_xeo.jpg', '/Uploads/dish_banh_xeo.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440014', 'dish_bun_thit_nuong.jpg', '/Uploads/dish_bun_thit_nuong.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440015', 'dish_goi_cuon_tom_thit.jpg', '/Uploads/dish_goi_cuon_tom_thit.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440016', 'dish_nem_nuong.jpg', '/Uploads/dish_nem_nuong.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440017', 'dish_banh_khot.jpg', '/Uploads/dish_banh_khot.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440018', 'dish_chao_tom.jpg', '/Uploads/dish_chao_tom.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440019', 'dish_goi_ngo_sen.jpg', '/Uploads/dish_goi_ngo_sen.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440020', 'dish_banh_trang_nuong.jpg', '/Uploads/dish_banh_trang_nuong.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440021', 'dish_ca_phe_sua_da.jpg', '/Uploads/dish_ca_phe_sua_da.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440022', 'dish_tra_da.jpg', '/Uploads/dish_tra_da.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440023', 'dish_nuoc_mia.jpg', '/Uploads/dish_nuoc_mia.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440024', 'dish_sinh_to_bo.jpg', '/Uploads/dish_sinh_to_bo.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440025', 'dish_nuoc_dua.jpg', '/Uploads/dish_nuoc_dua.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440026', 'dish_tra_sua_tran_chau.jpg', '/Uploads/dish_tra_sua_tran_chau.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440027', 'dish_che_ba_mau.jpg', '/Uploads/dish_che_ba_mau.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440028', 'dish_banh_flan.jpg', '/Uploads/dish_banh_flan.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440029', 'dish_che_dau_xanh.jpg', '/Uploads/dish_che_dau_xanh.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440030', 'dish_kem_xoi.jpg', '/Uploads/dish_kem_xoi.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440031', 'dish_banh_chuoi.jpg', '/Uploads/dish_banh_chuoi.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440032', 'dish_che_thai.jpg', '/Uploads/dish_che_thai.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440033', 'dish_pho_bo.jpg', '/Uploads/dish_pho_bo.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440034', 'dish_pho_ga.jpg', '/Uploads/dish_pho_ga.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440035', 'dish_bun_bo_hue.jpg', '/Uploads/dish_bun_bo_hue.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440036', 'dish_mi_quang.jpg', '/Uploads/dish_mi_quang.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440037', 'dish_hu_tieu.jpg', '/Uploads/dish_hu_tieu.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440038', 'dish_banh_canh_cua.jpg', '/Uploads/dish_banh_canh_cua.jpg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');

-- Insert into Admin
INSERT INTO Admin (Id, Name, Email, Phone, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
('550e8400-e29b-41d4-a716-446655440039', 'System Admin', 'admin@restaurant.com', '0123456789', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');

-- Insert into Owner
INSERT INTO Owner (Id, FileId, Name, Address, Information, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
('550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440000', 'Nguyen Van A', '123 Le Loi, District 1, Ho Chi Minh City', 'Traditional Vietnamese Restaurant', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');

-- Insert into Staff
INSERT INTO Staff (Id, OwnerId, FileId, Name, Email, Phone, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
('550e8400-e29b-41d4-a716-446655440041', '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440001', 'Tran Thi B', 'staff1@restaurant.com', '0987654321', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440042', '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440002', 'Le Van C', 'staff2@restaurant.com', '0912345678', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');

-- Insert into Account
INSERT INTO Account (Id, AdminId, OwnerId, StaffId, Username, Password, Role, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
('550e8400-e29b-41d4-a716-446655440043', '550e8400-e29b-41d4-a716-446655440039', NULL, NULL, 'admin', 'hashed_password_admin', 'Admin', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440044', NULL, '550e8400-e29b-41d4-a716-446655440040', NULL, 'owner1', 'hashed_password_owner', 'Owner', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440045', NULL, NULL, '550e8400-e29b-41d4-a716-446655440041', 'staff1', 'hashed_password_staff1', 'Staff', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440046', NULL, NULL, '550e8400-e29b-41d4-a716-446655440042', 'staff2', 'hashed_password_staff2', 'Staff', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');

-- Insert into Customer
INSERT INTO Customer (Id, OwnerId, Name, Phone, Point, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
('550e8400-e29b-41d4-a716-446655440047', '550e8400-e29b-41d4-a716-446655440040', 'Pham Van D', '0901234567', 100, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440048', '550e8400-e29b-41d4-a716-446655440040', 'Hoang Thi E', '0898765432', 50, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440049', '550e8400-e29b-41d4-a716-446655440040', 'Vo Van F', '0876543210', 75, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');

-- Insert into TableStatus
SET IDENTITY_INSERT TableStatus ON;
INSERT INTO TableStatus (Id, Name, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
(1, 'Available', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(2, 'Occupied', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(3, 'Reserved', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(4, 'Cleaning', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');
SET IDENTITY_INSERT TableStatus OFF;

-- Insert into [Table]
SET IDENTITY_INSERT [Table] ON;
INSERT INTO [Table] (Id, OwnerId, TableStatusId, TableNumber, QRCode, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
(1, '550e8400-e29b-41d4-a716-446655440040', 1, 1, 'QR_TABLE_001', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(2, '550e8400-e29b-41d4-a716-446655440040', 1, 2, 'QR_TABLE_002', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(3, '550e8400-e29b-41d4-a716-446655440040', 2, 3, 'QR_TABLE_003', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(4, '550e8400-e29b-41d4-a716-446655440040', 1, 4, 'QR_TABLE_004', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(5, '550e8400-e29b-41d4-a716-446655440040', 3, 5, 'QR_TABLE_005', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');
SET IDENTITY_INSERT [Table] OFF;

-- Insert into Category
SET IDENTITY_INSERT Category ON;
INSERT INTO Category (Id, Name, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
(1, 'Main Dishes', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(2, 'Appetizers', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(3, 'Beverages', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(4, 'Desserts', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(5, 'Soup & Noodles', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');
SET IDENTITY_INSERT Category OFF;

-- Insert into Dish
SET IDENTITY_INSERT Dish ON;
INSERT INTO Dish (Id, OwnerId, FileId, CategoryId, Name, Description, Price, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
(1, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440003', 1, 'Com Tam Suon', 'Broken rice with grilled pork ribs', 55000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(2, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440004', 1, 'Com Ga Nuong', 'Grilled chicken rice', 50000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(3, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440005', 1, 'Thit Nuong', 'Grilled pork with rice paper', 60000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(4, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440006', 1, 'Ca Kho To', 'Braised fish in clay pot', 70000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(5, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440007', 1, 'Suon Xao Chua Ngot', 'Sweet and sour pork ribs', 65000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(6, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440008', 1, 'Ga Chien Nuoc Mam', 'Fish sauce fried chicken', 55000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(7, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440009', 1, 'Bo Luc Lac', 'Shaking beef', 85000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(8, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440010', 1, 'Tom Rang Me', 'Tamarind prawns', 80000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(9, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440011', 1, 'Muc Chien Gion', 'Crispy fried squid', 70000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(10, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440012', 1, 'Canh Chua Ca', 'Sour fish soup with rice', 65000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(11, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440013', 1, 'Banh Xeo', 'Vietnamese crepe', 45000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(12, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440014', 1, 'Bun Thit Nuong', 'Grilled pork vermicelli', 50000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(13, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440015', 2, 'Goi Cuon Tom Thit', 'Fresh spring rolls with shrimp and pork', 40000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(14, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440016', 2, 'Nem Nuong', 'Grilled pork balls', 45000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(15, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440017', 2, 'Banh Khot', 'Mini pancakes with shrimp', 35000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(16, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440018', 2, 'Chao Tom', 'Sugarcane shrimp', 50000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(17, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440019', 2, 'Goi Ngo Sen', 'Lotus root salad', 30000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(18, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440020', 2, 'Banh Trang Nuong', 'Grilled rice paper with egg', 25000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(19, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440021', 3, 'Ca Phe Sua Da', 'Iced coffee with condensed milk', 20000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(20, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440022', 3, 'Tra Da', 'Iced tea', 10000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(21, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440023', 3, 'Nuoc Mia', 'Fresh sugarcane juice', 15000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(22, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440024', 3, 'Sinh To Bo', 'Avocado smoothie', 25000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(23, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440025', 3, 'Nuoc Dua', 'Fresh coconut water', 18000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(24, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440026', 3, 'Tra Sua Tran Chau', 'Bubble milk tea', 30000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(25, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440027', 4, 'Che Ba Mau', 'Three-color dessert', 20000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(26, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440028', 4, 'Banh Flan', 'Vietnamese creme caramel', 18000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(27, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440029', 4, 'Che Dau Xanh', 'Mung bean dessert', 15000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(28, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440030', 4, 'Kem Xoi', 'Sticky rice ice cream', 22000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(29, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440031', 4, 'Banh Chuoi', 'Banana cake', 16000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(30, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440032', 4, 'Che Thai', 'Thai-style mixed dessert', 25000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(31, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440033', 5, 'Pho Bo', 'Traditional beef noodle soup', 65000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(32, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440034', 5, 'Pho Ga', 'Chicken noodle soup', 60000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(33, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440035', 5, 'Bun Bo Hue', 'Spicy beef noodle soup', 70000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(34, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440036', 5, 'Mi Quang', 'Quang-style noodles', 55000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(35, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440037', 5, 'Hu Tieu', 'Clear noodle soup', 50000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(36, '550e8400-e29b-41d4-a716-446655440040', '550e8400-e29b-41d4-a716-446655440038', 5, 'Banh Canh Cua', 'Crab thick noodle soup', 60000, 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');
SET IDENTITY_INSERT Dish OFF;

-- Insert into OrderStatus (Check for existing data to avoid duplicates)
IF NOT EXISTS (SELECT 1 FROM OrderStatus WHERE Id IN (1, 2, 3, 4, 5, 6))
BEGIN
    SET IDENTITY_INSERT OrderStatus ON;
    INSERT INTO OrderStatus (Id, Name) VALUES
    (1, 'Pending'),
    (2, 'Confirmed'),
    (3, 'Preparing'),
    (4, 'Ready'),
    (5, 'Completed'),
    (6, 'Cancelled');
    SET IDENTITY_INSERT OrderStatus OFF;
END
GO

-- Insert into [Order]
INSERT INTO [Order] (Id, CustomerId, TableId, OwnerId, OrderStatusId, Time, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
('550e8400-e29b-41d4-a716-446655440106', '550e8400-e29b-41d4-a716-446655440047', 1, '550e8400-e29b-41d4-a716-446655440040', 5, '2025-06-18 12:30:00', 1, '2025-06-18 12:30:00', '2025-06-18 12:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440107', '550e8400-e29b-41d4-a716-446655440048', 3, '550e8400-e29b-41d4-a716-446655440040', 5, '2025-06-18 13:15:00', 1, '2025-06-18 13:15:00', '2025-06-18 13:15:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440108', '550e8400-e29b-41d4-a716-446655440049', 2, '550e8400-e29b-41d4-a716-446655440040', 5, '2025-06-18 14:00:00', 1, '2025-06-18 14:00:00', '2025-06-18 14:00:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440109', '550e8400-e29b-41d4-a716-446655440047', 4, '550e8400-e29b-41d4-a716-446655440040', 5, '2025-06-18 11:45:00', 1, '2025-06-18 11:45:00', '2025-06-18 11:45:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440110', '550e8400-e29b-41d4-a716-446655440048', 1, '550e8400-e29b-41d4-a716-446655440040', 5, '2025-06-18 15:30:00', 1, '2025-06-18 15:30:00', '2025-06-18 15:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440111', '550e8400-e29b-41d4-a716-446655440049', 5, '550e8400-e29b-41d4-a716-446655440040', 3, '2025-06-18 16:00:00', 1, '2025-06-18 16:00:00', '2025-06-18 16:00:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440112', '550e8400-e29b-41d4-a716-446655440047', 2, '550e8400-e29b-41d4-a716-446655440040', 2, '2025-06-18 16:30:00', 1, '2025-06-18 16:30:00', '2025-06-18 16:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440113', '550e8400-e29b-41d4-a716-446655440048', 3, '550e8400-e29b-41d4-a716-446655440040', 1, '2025-06-18 17:00:00', 1, '2025-06-18 17:00:00', '2025-06-18 17:00:00', 'staff2', 'staff2');

-- Insert into OrderDetail
INSERT INTO OrderDetail (Id, OrderId, DishId, Quantity, Price, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
('550e8400-e29b-41d4-a716-446655440114', '550e8400-e29b-41d4-a716-446655440106', 31, 2, 65000, 1, '2025-06-18 12:30:00', '2025-06-18 12:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440115', '550e8400-e29b-41d4-a716-446655440106', 21, 2, 20000, 1, '2025-06-18 12:30:00', '2025-06-18 12:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440116', '550e8400-e29b-41d4-a716-446655440106', 13, 1, 40000, 1, '2025-06-18 12:30:00', '2025-06-18 12:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440117', '550e8400-e29b-41d4-a716-446655440107', 1, 2, 55000, 1, '2025-06-18 13:15:00', '2025-06-18 13:15:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440118', '550e8400-e29b-41d4-a716-446655440107', 7, 1, 85000, 1, '2025-06-18 13:15:00', '2025-06-18 13:15:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440119', '550e8400-e29b-41d4-a716-446655440107', 8, 1, 80000, 1, '2025-06-18 13:15:00', '2025-06-18 13:15:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440120', '550e8400-e29b-41d4-a716-446655440107', 22, 3, 10000, 1, '2025-06-18 13:15:00', '2025-06-18 13:15:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440121', '550e8400-e29b-41d4-a716-446655440107', 27, 2, 20000, 1, '2025-06-18 13:15:00', '2025-06-18 13:15:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440122', '550e8400-e29b-41d4-a716-446655440108', 12, 1, 50000, 1, '2025-06-18 14:00:00', '2025-06-18 14:00:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440123', '550e8400-e29b-41d4-a716-446655440108', 24, 1, 25000, 1, '2025-06-18 14:00:00', '2025-06-18 14:00:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440124', '550e8400-e29b-41d4-a716-446655440108', 15, 2, 35000, 1, '2025-06-18 14:00:00', '2025-06-18 14:00:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440125', '550e8400-e29b-41d4-a716-446655440109', 2, 1, 50000, 1, '2025-06-18 11:45:00', '2025-06-18 11:45:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440126', '550e8400-e29b-41d4-a716-446655440109', 21, 1, 20000, 1, '2025-06-18 11:45:00', '2025-06-18 11:45:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440127', '550e8400-e29b-41d4-a716-446655440110', 16, 3, 25000, 1, '2025-06-18 15:30:00', '2025-06-18 15:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440128', '550e8400-e29b-41d4-a716-446655440110', 26, 2, 30000, 1, '2025-06-18 15:30:00', '2025-06-18 15:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440129', '550e8400-e29b-41d4-a716-446655440110', 31, 1, 16000, 1, '2025-06-18 15:30:00', '2025-06-18 15:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440130', '550e8400-e29b-41d4-a716-446655440111', 33, 1, 70000, 1, '2025-06-18 16:00:00', '2025-06-18 16:00:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440131', '550e8400-e29b-41d4-a716-446655440111', 14, 2, 45000, 1, '2025-06-18 16:00:00', '2025-06-18 16:00:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440132', '550e8400-e29b-41d4-a716-446655440112', 4, 1, 70000, 1, '2025-06-18 16:30:00', '2025-06-18 16:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440133', '550e8400-e29b-41d4-a716-446655440112', 23, 1, 15000, 1, '2025-06-18 16:30:00', '2025-06-18 16:30:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440134', '550e8400-e29b-41d4-a716-446655440113', 30, 2, 60000, 1, '2025-06-18 17:00:00', '2025-06-18 17:00:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440135', '550e8400-e29b-41d4-a716-446655440113', 15, 1, 30000, 1, '2025-06-18 17:00:00', '2025-06-18 17:00:00', 'staff2', 'staff2');

-- Insert into PaymentMethod (Check for existing data to avoid duplicates)
IF NOT EXISTS (SELECT 1 FROM PaymentMethod WHERE Id IN ('550e8400-e29b-41d4-a716-446655440136', '550e8400-e29b-41d4-a716-446655440137', '550e8400-e29b-41d4-a716-446655440138', '550e8400-e29b-41d4-a716-446655440139'))
BEGIN
    INSERT INTO PaymentMethod (Id, Name) VALUES
    ('550e8400-e29b-41d4-a716-446655440136', 'Cash'),
    ('550e8400-e29b-41d4-a716-446655440137', 'Credit Card'),
    ('550e8400-e29b-41d4-a716-446655440138', 'QR Payment'),
    ('550e8400-e29b-41d4-a716-446655440139', 'Bank Transfer');
END
GO

-- Insert into Payment
INSERT INTO Payment (Id, OrderId, PaymentMethodId, Time, Cost, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
('550e8400-e29b-41d4-a716-446655440140', '550e8400-e29b-41d4-a716-446655440106', '550e8400-e29b-41d4-a716-446655440136', '2025-06-18 13:00:00', 170000, 1, '2025-06-18 13:00:00', '2025-06-18 13:00:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440141', '550e8400-e29b-41d4-a716-446655440107', '550e8400-e29b-41d4-a716-446655440138', '2025-06-18 14:30:00', 295000, 1, '2025-06-18 14:30:00', '2025-06-18 14:30:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440142', '550e8400-e29b-41d4-a716-446655440108', '550e8400-e29b-41d4-a716-446655440137', '2025-06-18 14:45:00', 120000, 1, '2025-06-18 14:45:00', '2025-06-18 14:45:00', 'staff1', 'staff1'),
('550e8400-e29b-41d4-a716-446655440143', '550e8400-e29b-41d4-a716-446655440109', '550e8400-e29b-41d4-a716-446655440136', '2025-06-18 12:15:00', 70000, 1, '2025-06-18 12:15:00', '2025-06-18 12:15:00', 'staff2', 'staff2'),
('550e8400-e29b-41d4-a716-446655440144', '550e8400-e29b-41d4-a716-446655440110', '550e8400-e29b-41d4-a716-446655440138', '2025-06-18 16:00:00', 131000, 1, '2025-06-18 16:00:00', '2025-06-18 16:00:00', 'staff1', 'staff1');

-- Insert into Supplier
INSERT INTO Supplier (Id, OwnerId, Name, Email, Phone, Address, IsActive, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
('550e8400-e29b-41d4-a716-446655440145', '550e8400-e29b-41d4-a716-446655440040', 'Fresh Meat Supplier Co.', 'meat@supplier.com', '0123111111', '456 Nguyen Trai, District 5', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440146', '550e8400-e29b-41d4-a716-446655440040', 'Green Vegetable Farm', 'veg@farm.com', '0123222222', '789 Vo Van Kiet, District 6', 0, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440147', '550e8400-e29b-41d4-a716-446655440040', 'Saigon Beverage Co.', 'drinks@dist.com', '0123333333', '321 Cach Mang Thang 8', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440148', '550e8400-e29b-41d4-a716-446655440040', 'Ocean Fresh Seafood', 'seafood@ocean.com', '0123444444', '654 Le Lai, District 1', 0, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440149', '550e8400-e29b-41d4-a716-446655440040', 'Golden Rice Trading', 'rice@golden.com', '01234567890', '987 Tran Hung Dao, District 1', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
('550e8400-e29b-41d4-a716-446655440150', '550e8400-e29b-41d4-a716-446655440040', 'Spice & Herb Center', 'spice@center.com', '0123456789', '159 Hai Ba Trung, District 3', 1, '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');

-- Insert into Ingredient (Enable IDENTITY_INSERT for explicit Id values)
SET IDENTITY_INSERT Ingredient ON;
INSERT INTO Ingredient (Id, OwnerId, Name, CurrentQuantity, Unit, CreatedDate, ModifiedDate, CreatedBy, ModifiedBy) VALUES
(1, '550e8400-e29b-41d4-a716-446655440040', 'Beef', 80.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(2, '550e8400-e29b-41d4-a716-446655440040', 'Pork', 60.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(3, '550e8400-e29b-41d4-a716-446655440040', 'Chicken', 50.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(4, '550e8400-e29b-41d4-a716-446655440040', 'Shrimp', 25.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(5, '550e8400-e29b-41d4-a716-446655440040', 'Fish', 30.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(6, '550e8400-e29b-41d4-a716-446655440040', 'Squid', 20.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(7, '550e8400-e29b-41d4-a716-446655440040', 'Crab', 15.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(8, '550e8400-e29b-41d4-a716-446655440040', 'Rice', 45.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(9, '550e8400-e29b-41d4-a716-446655440040', 'Rice Noodles', 50.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(10, '550e8400-e29b-41d4-a716-446655440040', 'Vermicelli', 40.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(11, '550e8400-e29b-41d4-a716-446655440040', 'Rice Paper', 45.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(12, '550e8400-e29b-41d4-a716-446655440040', 'Bread', 100.0, 'pieces', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(13, '550e8400-e29b-41d4-a716-446655440040', 'Lettuce', 25.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(14, '550e8400-e29b-41d4-a716-446655440040', 'Bean Sprouts', 40.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(15, '550e8400-e29b-41d4-a716-446655440040', 'Cucumber', 15.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(16, '550e8400-e29b-41d4-a716-446655440040', 'Carrot', 20.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(17, '550e8400-e29b-41d4-a716-446655440040', 'Onion', 30.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(18, '550e8400-e29b-41d4-a716-446655440040', 'Garlic', 10.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(19, '550e8400-e29b-41d4-a716-446655440040', 'Ginger', 8.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(20, '550e8400-e29b-41d4-a716-446655440040', 'Chili', 5.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(21, '550e8400-e29b-41d4-a716-446655440040', 'Tomato', 25.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(22, '550e8400-e29b-41d4-a716-446655440040', 'Lotus Root', 12.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(23, '550e8400-e29b-41d4-a716-446655440040', 'Fish Sauce', 20.0, 'liters', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(24, '550e8400-e29b-41d4-a716-446655440040', 'Soy Sauce', 15.0, 'liters', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(25, '550e8400-e29b-41d4-a716-446655440040', 'Sugar', 50.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(26, '550e8400-e29b-41d4-a716-446655440040', 'Salt', 20.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(27, '550e8400-e29b-41d4-a716-446655440040', 'Pepper', 2.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(28, '550e8400-e29b-41d4-a716-446655440040', 'Lemongrass', 8.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(29, '550e8400-e29b-41d4-a716-446655440040', 'Tamarind', 10.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(30, '550e8400-e29b-41d4-a716-446655440040', 'Coffee Beans', 15.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(31, '550e8400-e29b-41d4-a716-446655440040', 'Tea Leaves', 10.0, 'kg', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin'),
(32, '550e8400-e29b-41d4-a716-446655440040', 'Condensed Milk', 30.0, 'liters', '2025-06-18 09:00:00', '2025-06-18 09:00:00', 'admin', 'admin');
SET IDENTITY_INSERT Ingredient OFF;