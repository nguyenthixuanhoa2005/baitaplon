-- =============================================
-- Database Creation Script for Library Management System
-- Hệ thống quản lý thư viện
-- =============================================

USE master;
GO

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'Quanlisachcoban')
BEGIN
    ALTER DATABASE Quanlisachcoban SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Quanlisachcoban;
END
GO

-- Create database
CREATE DATABASE Quanlisachcoban;
GO

USE Quanlisachcoban;
GO

-- =============================================
-- Table: TaiKhoan (User Accounts)
-- =============================================
CREATE TABLE TaiKhoan
(
    MaTaiKhoan INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    MatKhau NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100),
    VaiTro NVARCHAR(20) NOT NULL CHECK (VaiTro IN (N'Admin', N'NhanVien', N'DocGia')),
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);
GO

-- =============================================
-- Table: NhanVien (Employees)
-- =============================================
CREATE TABLE NhanVien
(
    MaNhanVien INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(200),
    SoDienThoai NVARCHAR(15),
    Email NVARCHAR(100),
    ChucVu NVARCHAR(50),
    MaTaiKhoan INT,
    NgayVaoLam DATE,
    TrangThai BIT DEFAULT 1,
    FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan)
);
GO

-- =============================================
-- Table: DocGia (Readers)
-- =============================================
CREATE TABLE DocGia
(
    MaDocGia INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(200),
    SoDienThoai NVARCHAR(15),
    Email NVARCHAR(100),
    NgayDangKy DATE DEFAULT GETDATE(),
    LoaiDocGia NVARCHAR(50),
    TrangThai BIT DEFAULT 1
);
GO

-- =============================================
-- Table: TheLoaiSach (Book Categories)
-- =============================================
CREATE TABLE TheLoaiSach
(
    MaTheLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenTheLoai NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255)
);
GO

-- =============================================
-- Table: Sach (Books)
-- =============================================
CREATE TABLE Sach
(
    MaSach INT IDENTITY(1,1) PRIMARY KEY,
    TenSach NVARCHAR(200) NOT NULL,
    TacGia NVARCHAR(100),
    NhaXuatBan NVARCHAR(100),
    NamXuatBan INT,
    MaTheLoai INT,
    SoLuong INT DEFAULT 0,
    GiaSach DECIMAL(18,2),
    ViTri NVARCHAR(50),
    TrangThai NVARCHAR(50),
    MoTa NVARCHAR(500),
    FOREIGN KEY (MaTheLoai) REFERENCES TheLoaiSach(MaTheLoai)
);
GO

-- =============================================
-- Table: PhieuMuon (Borrow Slips)
-- =============================================
CREATE TABLE PhieuMuon
(
    MaPhieuMuon INT IDENTITY(1,1) PRIMARY KEY,
    MaDocGia INT NOT NULL,
    MaNhanVien INT,
    NgayMuon DATE NOT NULL DEFAULT GETDATE(),
    NgayHenTra DATE NOT NULL,
    TrangThai NVARCHAR(50) DEFAULT N'Đang mượn',
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaDocGia) REFERENCES DocGia(MaDocGia),
    FOREIGN KEY (MaNhanVien) REFERENCES NhanVien(MaNhanVien)
);
GO

-- =============================================
-- Table: ChiTietPhieuMuon (Borrow Details)
-- =============================================
CREATE TABLE ChiTietPhieuMuon
(
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuMuon INT NOT NULL,
    MaSach INT NOT NULL,
    NgayTra DATE,
    TinhTrangSach NVARCHAR(50),
    PhiPhat DECIMAL(18,2) DEFAULT 0,
    FOREIGN KEY (MaPhieuMuon) REFERENCES PhieuMuon(MaPhieuMuon),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);
GO

-- =============================================
-- Insert sample data
-- =============================================

-- Admin account (password: admin123)
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Email, VaiTro) 
VALUES (N'admin', N'admin123', N'admin@library.com', N'Admin');
GO

-- Sample categories
INSERT INTO TheLoaiSach (TenTheLoai, MoTa) 
VALUES 
    (N'Văn học', N'Sách văn học trong nước và ngoại quốc'),
    (N'Khoa học', N'Sách về khoa học tự nhiên'),
    (N'Công nghệ', N'Sách về công nghệ thông tin'),
    (N'Lịch sử', N'Sách về lịch sử'),
    (N'Kinh tế', N'Sách về kinh tế và quản trị');
GO

-- Sample books
INSERT INTO Sach (TenSach, TacGia, NhaXuatBan, NamXuatBan, MaTheLoai, SoLuong, GiaSach, ViTri, TrangThai)
VALUES 
    (N'Lập trình C# cơ bản', N'Nguyễn Văn A', N'NXB Giáo dục', 2023, 3, 10, 150000, N'Kệ A1', N'Có sẵn'),
    (N'Cơ sở dữ liệu SQL Server', N'Trần Thị B', N'NXB Lao Động', 2022, 3, 8, 180000, N'Kệ A2', N'Có sẵn'),
    (N'Lịch sử Việt Nam', N'Lê Văn C', N'NXB Văn hóa', 2021, 4, 15, 120000, N'Kệ B1', N'Có sẵn');
GO
