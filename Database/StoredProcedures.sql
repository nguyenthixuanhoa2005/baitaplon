-- =============================================
-- Stored Procedures for Library Management System
-- Hệ thống quản lý thư viện - Các thủ tục lưu trữ
-- =============================================

USE Quanlisachcoban;
GO

-- =============================================
-- AUTHENTICATION PROCEDURES (Xác thực)
-- =============================================

-- Procedure: Login
CREATE OR ALTER PROCEDURE sp_DangNhap
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(255)
AS
BEGIN
    SELECT 
        t.MaTaiKhoan,
        t.TenDangNhap,
        t.VaiTro,
        t.Email,
        CASE 
            WHEN t.VaiTro = N'NhanVien' THEN nv.HoTen
            ELSE t.TenDangNhap
        END AS HoTen
    FROM TaiKhoan t
    LEFT JOIN NhanVien nv ON t.MaTaiKhoan = nv.MaTaiKhoan
    WHERE t.TenDangNhap = @TenDangNhap 
        AND t.MatKhau = @MatKhau 
        AND t.TrangThai = 1;
END
GO

-- Procedure: Register new account
CREATE OR ALTER PROCEDURE sp_DangKy
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(255),
    @Email NVARCHAR(100),
    @VaiTro NVARCHAR(20) = N'DocGia'
AS
BEGIN
    BEGIN TRY
        -- Check if username already exists
        IF EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap)
        BEGIN
            SELECT -1 AS Result, N'Tên đăng nhập đã tồn tại' AS Message;
            RETURN;
        END

        INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Email, VaiTro, TrangThai, NgayTao)
        VALUES (@TenDangNhap, @MatKhau, @Email, @VaiTro, 1, GETDATE());
        
        SELECT SCOPE_IDENTITY() AS Result, N'Đăng ký thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- =============================================
-- BOOK MANAGEMENT PROCEDURES (Quản lý sách)
-- =============================================

-- Procedure: Get all books
CREATE OR ALTER PROCEDURE sp_LayDanhSachSach
AS
BEGIN
    SELECT 
        s.MaSach,
        s.TenSach,
        s.TacGia,
        s.NhaXuatBan,
        s.NamXuatBan,
        tl.TenTheLoai,
        s.SoLuong,
        s.GiaSach,
        s.ViTri,
        s.TrangThai,
        s.MoTa
    FROM Sach s
    LEFT JOIN TheLoaiSach tl ON s.MaTheLoai = tl.MaTheLoai
    ORDER BY s.TenSach;
END
GO

-- Procedure: Search books
CREATE OR ALTER PROCEDURE sp_TimKiemSach
    @TuKhoa NVARCHAR(200)
AS
BEGIN
    SELECT 
        s.MaSach,
        s.TenSach,
        s.TacGia,
        s.NhaXuatBan,
        s.NamXuatBan,
        tl.TenTheLoai,
        s.SoLuong,
        s.GiaSach,
        s.ViTri,
        s.TrangThai,
        s.MoTa
    FROM Sach s
    LEFT JOIN TheLoaiSach tl ON s.MaTheLoai = tl.MaTheLoai
    WHERE s.TenSach LIKE N'%' + @TuKhoa + '%'
        OR s.TacGia LIKE N'%' + @TuKhoa + '%'
        OR s.NhaXuatBan LIKE N'%' + @TuKhoa + '%'
        OR tl.TenTheLoai LIKE N'%' + @TuKhoa + '%'
    ORDER BY s.TenSach;
END
GO

-- Procedure: Add new book
CREATE OR ALTER PROCEDURE sp_ThemSach
    @TenSach NVARCHAR(200),
    @TacGia NVARCHAR(100),
    @NhaXuatBan NVARCHAR(100),
    @NamXuatBan INT,
    @MaTheLoai INT,
    @SoLuong INT,
    @GiaSach DECIMAL(18,2),
    @ViTri NVARCHAR(50),
    @TrangThai NVARCHAR(50),
    @MoTa NVARCHAR(500) = NULL
AS
BEGIN
    BEGIN TRY
        INSERT INTO Sach (TenSach, TacGia, NhaXuatBan, NamXuatBan, MaTheLoai, SoLuong, GiaSach, ViTri, TrangThai, MoTa)
        VALUES (@TenSach, @TacGia, @NhaXuatBan, @NamXuatBan, @MaTheLoai, @SoLuong, @GiaSach, @ViTri, @TrangThai, @MoTa);
        
        SELECT SCOPE_IDENTITY() AS Result, N'Thêm sách thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- Procedure: Update book
CREATE OR ALTER PROCEDURE sp_SuaSach
    @MaSach INT,
    @TenSach NVARCHAR(200),
    @TacGia NVARCHAR(100),
    @NhaXuatBan NVARCHAR(100),
    @NamXuatBan INT,
    @MaTheLoai INT,
    @SoLuong INT,
    @GiaSach DECIMAL(18,2),
    @ViTri NVARCHAR(50),
    @TrangThai NVARCHAR(50),
    @MoTa NVARCHAR(500) = NULL
AS
BEGIN
    BEGIN TRY
        UPDATE Sach
        SET TenSach = @TenSach,
            TacGia = @TacGia,
            NhaXuatBan = @NhaXuatBan,
            NamXuatBan = @NamXuatBan,
            MaTheLoai = @MaTheLoai,
            SoLuong = @SoLuong,
            GiaSach = @GiaSach,
            ViTri = @ViTri,
            TrangThai = @TrangThai,
            MoTa = @MoTa
        WHERE MaSach = @MaSach;
        
        SELECT @@ROWCOUNT AS Result, N'Cập nhật sách thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- Procedure: Delete book
CREATE OR ALTER PROCEDURE sp_XoaSach
    @MaSach INT
AS
BEGIN
    BEGIN TRY
        -- Check if book is borrowed
        IF EXISTS (SELECT 1 FROM ChiTietPhieuMuon ct
                   INNER JOIN PhieuMuon pm ON ct.MaPhieuMuon = pm.MaPhieuMuon
                   WHERE ct.MaSach = @MaSach AND pm.TrangThai = N'Đang mượn')
        BEGIN
            SELECT -1 AS Result, N'Không thể xóa sách đang được mượn' AS Message;
            RETURN;
        END

        DELETE FROM Sach WHERE MaSach = @MaSach;
        SELECT @@ROWCOUNT AS Result, N'Xóa sách thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- =============================================
-- READER MANAGEMENT PROCEDURES (Quản lý độc giả)
-- =============================================

-- Procedure: Get all readers
CREATE OR ALTER PROCEDURE sp_LayDanhSachDocGia
AS
BEGIN
    SELECT 
        MaDocGia,
        HoTen,
        NgaySinh,
        GioiTinh,
        DiaChi,
        SoDienThoai,
        Email,
        NgayDangKy,
        LoaiDocGia,
        TrangThai
    FROM DocGia
    ORDER BY HoTen;
END
GO

-- Procedure: Add new reader
CREATE OR ALTER PROCEDURE sp_ThemDocGia
    @HoTen NVARCHAR(100),
    @NgaySinh DATE,
    @GioiTinh NVARCHAR(10),
    @DiaChi NVARCHAR(200),
    @SoDienThoai NVARCHAR(15),
    @Email NVARCHAR(100),
    @LoaiDocGia NVARCHAR(50)
AS
BEGIN
    BEGIN TRY
        INSERT INTO DocGia (HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, NgayDangKy, LoaiDocGia, TrangThai)
        VALUES (@HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @Email, GETDATE(), @LoaiDocGia, 1);
        
        SELECT SCOPE_IDENTITY() AS Result, N'Thêm độc giả thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- Procedure: Update reader
CREATE OR ALTER PROCEDURE sp_SuaDocGia
    @MaDocGia INT,
    @HoTen NVARCHAR(100),
    @NgaySinh DATE,
    @GioiTinh NVARCHAR(10),
    @DiaChi NVARCHAR(200),
    @SoDienThoai NVARCHAR(15),
    @Email NVARCHAR(100),
    @LoaiDocGia NVARCHAR(50),
    @TrangThai BIT
AS
BEGIN
    BEGIN TRY
        UPDATE DocGia
        SET HoTen = @HoTen,
            NgaySinh = @NgaySinh,
            GioiTinh = @GioiTinh,
            DiaChi = @DiaChi,
            SoDienThoai = @SoDienThoai,
            Email = @Email,
            LoaiDocGia = @LoaiDocGia,
            TrangThai = @TrangThai
        WHERE MaDocGia = @MaDocGia;
        
        SELECT @@ROWCOUNT AS Result, N'Cập nhật độc giả thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- Procedure: Delete reader
CREATE OR ALTER PROCEDURE sp_XoaDocGia
    @MaDocGia INT
AS
BEGIN
    BEGIN TRY
        -- Check if reader has active borrows
        IF EXISTS (SELECT 1 FROM PhieuMuon WHERE MaDocGia = @MaDocGia AND TrangThai = N'Đang mượn')
        BEGIN
            SELECT -1 AS Result, N'Không thể xóa độc giả đang mượn sách' AS Message;
            RETURN;
        END

        DELETE FROM DocGia WHERE MaDocGia = @MaDocGia;
        SELECT @@ROWCOUNT AS Result, N'Xóa độc giả thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- =============================================
-- EMPLOYEE MANAGEMENT PROCEDURES (Quản lý nhân viên)
-- =============================================

-- Procedure: Get all employees
CREATE OR ALTER PROCEDURE sp_LayDanhSachNhanVien
AS
BEGIN
    SELECT 
        nv.MaNhanVien,
        nv.HoTen,
        nv.NgaySinh,
        nv.GioiTinh,
        nv.DiaChi,
        nv.SoDienThoai,
        nv.Email,
        nv.ChucVu,
        nv.NgayVaoLam,
        nv.TrangThai,
        tk.TenDangNhap,
        tk.VaiTro
    FROM NhanVien nv
    LEFT JOIN TaiKhoan tk ON nv.MaTaiKhoan = tk.MaTaiKhoan
    ORDER BY nv.HoTen;
END
GO

-- Procedure: Add new employee
CREATE OR ALTER PROCEDURE sp_ThemNhanVien
    @HoTen NVARCHAR(100),
    @NgaySinh DATE,
    @GioiTinh NVARCHAR(10),
    @DiaChi NVARCHAR(200),
    @SoDienThoai NVARCHAR(15),
    @Email NVARCHAR(100),
    @ChucVu NVARCHAR(50),
    @NgayVaoLam DATE,
    @TenDangNhap NVARCHAR(50) = NULL,
    @MatKhau NVARCHAR(255) = NULL
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @MaTaiKhoan INT = NULL;
        
        -- Create account if username provided
        IF @TenDangNhap IS NOT NULL
        BEGIN
            IF EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap)
            BEGIN
                SELECT -1 AS Result, N'Tên đăng nhập đã tồn tại' AS Message;
                ROLLBACK TRANSACTION;
                RETURN;
            END
            
            INSERT INTO TaiKhoan (TenDangNhap, MatKhau, Email, VaiTro, TrangThai, NgayTao)
            VALUES (@TenDangNhap, @MatKhau, @Email, N'NhanVien', 1, GETDATE());
            
            SET @MaTaiKhoan = SCOPE_IDENTITY();
        END
        
        INSERT INTO NhanVien (HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, ChucVu, MaTaiKhoan, NgayVaoLam, TrangThai)
        VALUES (@HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @Email, @ChucVu, @MaTaiKhoan, @NgayVaoLam, 1);
        
        COMMIT TRANSACTION;
        SELECT SCOPE_IDENTITY() AS Result, N'Thêm nhân viên thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- Procedure: Update employee
CREATE OR ALTER PROCEDURE sp_SuaNhanVien
    @MaNhanVien INT,
    @HoTen NVARCHAR(100),
    @NgaySinh DATE,
    @GioiTinh NVARCHAR(10),
    @DiaChi NVARCHAR(200),
    @SoDienThoai NVARCHAR(15),
    @Email NVARCHAR(100),
    @ChucVu NVARCHAR(50),
    @NgayVaoLam DATE,
    @TrangThai BIT
AS
BEGIN
    BEGIN TRY
        UPDATE NhanVien
        SET HoTen = @HoTen,
            NgaySinh = @NgaySinh,
            GioiTinh = @GioiTinh,
            DiaChi = @DiaChi,
            SoDienThoai = @SoDienThoai,
            Email = @Email,
            ChucVu = @ChucVu,
            NgayVaoLam = @NgayVaoLam,
            TrangThai = @TrangThai
        WHERE MaNhanVien = @MaNhanVien;
        
        SELECT @@ROWCOUNT AS Result, N'Cập nhật nhân viên thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- Procedure: Delete employee
CREATE OR ALTER PROCEDURE sp_XoaNhanVien
    @MaNhanVien INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @MaTaiKhoan INT;
        SELECT @MaTaiKhoan = MaTaiKhoan FROM NhanVien WHERE MaNhanVien = @MaNhanVien;
        
        DELETE FROM NhanVien WHERE MaNhanVien = @MaNhanVien;
        
        IF @MaTaiKhoan IS NOT NULL
        BEGIN
            DELETE FROM TaiKhoan WHERE MaTaiKhoan = @MaTaiKhoan;
        END
        
        COMMIT TRANSACTION;
        SELECT 1 AS Result, N'Xóa nhân viên thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- =============================================
-- BORROW/RETURN PROCEDURES (Mượn/Trả sách)
-- =============================================

-- Procedure: Create borrow slip
CREATE OR ALTER PROCEDURE sp_TaoPhieuMuon
    @MaDocGia INT,
    @MaNhanVien INT,
    @NgayHenTra DATE,
    @DanhSachMaSach NVARCHAR(MAX) -- Comma-separated book IDs
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Create borrow slip
        INSERT INTO PhieuMuon (MaDocGia, MaNhanVien, NgayMuon, NgayHenTra, TrangThai)
        VALUES (@MaDocGia, @MaNhanVien, GETDATE(), @NgayHenTra, N'Đang mượn');
        
        DECLARE @MaPhieuMuon INT = SCOPE_IDENTITY();
        
        -- Insert borrow details and update book quantity
        DECLARE @MaSach INT;
        DECLARE @Pos INT;
        DECLARE @List NVARCHAR(MAX) = @DanhSachMaSach + ',';
        
        WHILE CHARINDEX(',', @List) > 0
        BEGIN
            SET @Pos = CHARINDEX(',', @List);
            SET @MaSach = CAST(LEFT(@List, @Pos - 1) AS INT);
            
            -- Check book availability
            IF NOT EXISTS (SELECT 1 FROM Sach WHERE MaSach = @MaSach AND SoLuong > 0)
            BEGIN
                ROLLBACK TRANSACTION;
                SELECT -1 AS Result, N'Sách không còn trong kho' AS Message;
                RETURN;
            END
            
            -- Insert detail
            INSERT INTO ChiTietPhieuMuon (MaPhieuMuon, MaSach, TinhTrangSach)
            VALUES (@MaPhieuMuon, @MaSach, N'Tốt');
            
            -- Update book quantity
            UPDATE Sach SET SoLuong = SoLuong - 1 WHERE MaSach = @MaSach;
            
            SET @List = RIGHT(@List, LEN(@List) - @Pos);
        END
        
        COMMIT TRANSACTION;
        SELECT @MaPhieuMuon AS Result, N'Tạo phiếu mượn thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- Procedure: Return books
CREATE OR ALTER PROCEDURE sp_TraSach
    @MaPhieuMuon INT,
    @MaSach INT,
    @TinhTrangSach NVARCHAR(50),
    @PhiPhat DECIMAL(18,2) = 0
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Update borrow detail
        UPDATE ChiTietPhieuMuon
        SET NgayTra = GETDATE(),
            TinhTrangSach = @TinhTrangSach,
            PhiPhat = @PhiPhat
        WHERE MaPhieuMuon = @MaPhieuMuon AND MaSach = @MaSach;
        
        -- Update book quantity
        UPDATE Sach SET SoLuong = SoLuong + 1 WHERE MaSach = @MaSach;
        
        -- Check if all books returned
        IF NOT EXISTS (SELECT 1 FROM ChiTietPhieuMuon 
                      WHERE MaPhieuMuon = @MaPhieuMuon AND NgayTra IS NULL)
        BEGIN
            UPDATE PhieuMuon SET TrangThai = N'Đã trả' WHERE MaPhieuMuon = @MaPhieuMuon;
        END
        
        COMMIT TRANSACTION;
        SELECT 1 AS Result, N'Trả sách thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- Procedure: Get borrow history
CREATE OR ALTER PROCEDURE sp_LayLichSuMuon
    @MaDocGia INT = NULL
AS
BEGIN
    SELECT 
        pm.MaPhieuMuon,
        dg.HoTen AS TenDocGia,
        nv.HoTen AS TenNhanVien,
        pm.NgayMuon,
        pm.NgayHenTra,
        pm.TrangThai,
        pm.GhiChu
    FROM PhieuMuon pm
    INNER JOIN DocGia dg ON pm.MaDocGia = dg.MaDocGia
    LEFT JOIN NhanVien nv ON pm.MaNhanVien = nv.MaNhanVien
    WHERE (@MaDocGia IS NULL OR pm.MaDocGia = @MaDocGia)
    ORDER BY pm.NgayMuon DESC;
END
GO

-- Procedure: Get borrow details
CREATE OR ALTER PROCEDURE sp_LayChiTietPhieuMuon
    @MaPhieuMuon INT
AS
BEGIN
    SELECT 
        ct.MaChiTiet,
        s.TenSach,
        s.TacGia,
        ct.NgayTra,
        ct.TinhTrangSach,
        ct.PhiPhat
    FROM ChiTietPhieuMuon ct
    INNER JOIN Sach s ON ct.MaSach = s.MaSach
    WHERE ct.MaPhieuMuon = @MaPhieuMuon;
END
GO

-- =============================================
-- REPORT PROCEDURES (Báo cáo thống kê)
-- =============================================

-- Procedure: Get statistics
CREATE OR ALTER PROCEDURE sp_ThongKe
AS
BEGIN
    -- Total books
    SELECT COUNT(*) AS TongSoSach FROM Sach;
    
    -- Available books
    SELECT COUNT(*) AS SachConLai FROM Sach WHERE SoLuong > 0;
    
    -- Total readers
    SELECT COUNT(*) AS TongDocGia FROM DocGia WHERE TrangThai = 1;
    
    -- Active borrows
    SELECT COUNT(*) AS PhieuMuonDangMuon FROM PhieuMuon WHERE TrangThai = N'Đang mượn';
    
    -- Overdue borrows
    SELECT COUNT(*) AS PhieuQuaHan 
    FROM PhieuMuon 
    WHERE TrangThai = N'Đang mượn' AND NgayHenTra < GETDATE();
END
GO

-- Procedure: Top borrowed books
CREATE OR ALTER PROCEDURE sp_TopSachMuonNhieu
    @Top INT = 10
AS
BEGIN
    SELECT TOP (@Top)
        s.MaSach,
        s.TenSach,
        s.TacGia,
        COUNT(ct.MaChiTiet) AS SoLanMuon
    FROM Sach s
    INNER JOIN ChiTietPhieuMuon ct ON s.MaSach = ct.MaSach
    GROUP BY s.MaSach, s.TenSach, s.TacGia
    ORDER BY SoLanMuon DESC;
END
GO

-- =============================================
-- CATEGORY PROCEDURES (Quản lý thể loại)
-- =============================================

-- Procedure: Get all categories
CREATE OR ALTER PROCEDURE sp_LayDanhSachTheLoai
AS
BEGIN
    SELECT MaTheLoai, TenTheLoai, MoTa
    FROM TheLoaiSach
    ORDER BY TenTheLoai;
END
GO

-- Procedure: Add category
CREATE OR ALTER PROCEDURE sp_ThemTheLoai
    @TenTheLoai NVARCHAR(100),
    @MoTa NVARCHAR(255) = NULL
AS
BEGIN
    BEGIN TRY
        INSERT INTO TheLoaiSach (TenTheLoai, MoTa)
        VALUES (@TenTheLoai, @MoTa);
        
        SELECT SCOPE_IDENTITY() AS Result, N'Thêm thể loại thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- =============================================
-- ACCOUNT MANAGEMENT PROCEDURES
-- =============================================

-- Procedure: Change password
CREATE OR ALTER PROCEDURE sp_DoiMatKhau
    @MaTaiKhoan INT,
    @MatKhauCu NVARCHAR(255),
    @MatKhauMoi NVARCHAR(255)
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE MaTaiKhoan = @MaTaiKhoan AND MatKhau = @MatKhauCu)
        BEGIN
            SELECT -1 AS Result, N'Mật khẩu cũ không đúng' AS Message;
            RETURN;
        END
        
        UPDATE TaiKhoan
        SET MatKhau = @MatKhauMoi,
            NgayCapNhat = GETDATE()
        WHERE MaTaiKhoan = @MaTaiKhoan;
        
        SELECT 1 AS Result, N'Đổi mật khẩu thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT -1 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

PRINT 'All stored procedures created successfully!';
GO
