# Tóm tắt chuyển đổi từ Entity Framework sang Raw SQL

## Thay đổi chính

### 1. Đã xóa Entity Framework
- ❌ Xóa EntityFramework package từ packages.config
- ❌ Xóa các reference Entity Framework từ .csproj
- ❌ Xóa Entity Framework configuration từ App.config
- ✅ Thêm System.Configuration reference cho connection strings

### 2. Tạo DatabaseHelper.cs
Class helper mới để quản lý kết nối SQL Server:

**Các phương thức:**
- `GetConnection()` - Lấy SqlConnection
- `ExecuteStoredProcedure()` - Thực thi stored procedure, trả về DataTable
- `ExecuteNonQuery()` - Thực thi INSERT/UPDATE/DELETE
- `ExecuteScalar()` - Thực thi query trả về giá trị đơn
- `ExecuteQuery()` - Thực thi SQL query thuần túy

### 3. Cập nhật App.config
```xml
<!-- Cũ: Entity Framework connection string -->
<add name="QuanlisachcobanEntities" 
     connectionString="metadata=res://*/Model1.csdl|..." 
     providerName="System.Data.EntityClient" />

<!-- Mới: Raw SQL connection string -->
<add name="QuanlisachcobanDB" 
     connectionString="Data Source=localhost\SQLEXPRESS;Initial Catalog=Quanlisachcoban;Integrated Security=True;..." 
     providerName="System.Data.SqlClient" />
```

### 4. Tạo Database Schema (Database/CreateDatabase.sql)

**Các bảng:**
- TaiKhoan (Accounts)
- NhanVien (Employees)
- DocGia (Readers)
- TheLoaiSach (Book Categories)
- Sach (Books)
- PhieuMuon (Borrow Slips)
- ChiTietPhieuMuon (Borrow Details)

### 5. Tạo Stored Procedures (Database/StoredProcedures.sql)

**Xác thực:**
- sp_DangNhap - Login
- sp_DangKy - Register

**Quản lý sách:**
- sp_LayDanhSachSach - Get all books
- sp_TimKiemSach - Search books
- sp_ThemSach - Add book
- sp_SuaSach - Update book
- sp_XoaSach - Delete book

**Quản lý độc giả:**
- sp_LayDanhSachDocGia - Get all readers
- sp_ThemDocGia - Add reader
- sp_SuaDocGia - Update reader
- sp_XoaDocGia - Delete reader

**Quản lý nhân viên:**
- sp_LayDanhSachNhanVien - Get all employees
- sp_ThemNhanVien - Add employee
- sp_SuaNhanVien - Update employee
- sp_XoaNhanVien - Delete employee

**Mượn/Trả sách:**
- sp_TaoPhieuMuon - Create borrow slip
- sp_TraSach - Return book
- sp_LayLichSuMuon - Get borrow history
- sp_LayChiTietPhieuMuon - Get borrow details

**Báo cáo:**
- sp_ThongKe - Get statistics
- sp_TopSachMuonNhieu - Top borrowed books

**Khác:**
- sp_LayDanhSachTheLoai - Get categories
- sp_ThemTheLoai - Add category
- sp_DoiMatKhau - Change password

### 6. Cập nhật Form Code

**DangNhap.cs:**
- Thêm phương thức `buttonDangNhap_Click()` - gọi sp_DangNhap
- Thêm phương thức `buttonDangKy_Click()` - mở form đăng ký
- Sử dụng controls: textBoxTaiKhoan, textBoxMatKhau

**DangKy.cs:**
- Thêm phương thức `buttonTaoTaiKhoan_Click()` - gọi sp_DangKy
- Validate mật khẩu và xác nhận mật khẩu
- Sử dụng controls: textBoxTaiKhoan, textBoxMatKhau, textBox1

**QLSach.cs:**
- Thêm các phương thức mẫu cho quản lý sách
- LoadDanhSachSach(), LoadTheLoai(), TimKiemSach(), ThemSach()
- Template code để tích hợp với UI controls

**NhanVien.cs:**
- Thêm phương thức mẫu cho quản lý nhân viên
- LoadDanhSachNhanVien()
- Template code cho thêm/sửa/xóa nhân viên

**TaiKhoan.cs:**
- Thêm phương thức DoiMatKhau()
- Template code cho đổi mật khẩu

**FormQuanLySach.cs:**
- Thêm các phương thức mẫu cho mượn/trả sách
- TaoPhieuMuon(), TraSach(), XemLichSuMuon(), ThongKe()

## Lợi ích của việc chuyển sang Raw SQL

✅ **Hiệu suất tốt hơn**: Stored procedures được biên dịch sẵn  
✅ **Bảo mật tốt hơn**: Sử dụng tham số hóa, tránh SQL injection  
✅ **Kiểm soát tốt hơn**: Quản lý logic database tập trung  
✅ **Dễ maintain**: Database logic tách biệt khỏi application code  
✅ **Không phụ thuộc ORM**: Giảm dependencies và kích thước ứng dụng  

## Hướng dẫn sử dụng

### Bước 1: Cài đặt database
```sql
-- Chạy file này trước
Database/CreateDatabase.sql

-- Sau đó chạy file này
Database/StoredProcedures.sql
```

### Bước 2: Cấu hình connection string
Mở `App.config` và điều chỉnh `Data Source` cho phù hợp với SQL Server của bạn.

### Bước 3: Build và chạy
```
F5 trong Visual Studio
```

### Đăng nhập mặc định
- Username: **admin**
- Password: **admin123**

## Tài liệu tham khảo

- `Database/README.md` - Hướng dẫn chi tiết về database
- `DatabaseHelper.cs` - Class helper cho SQL connections
- `Database/CreateDatabase.sql` - Script tạo database
- `Database/StoredProcedures.sql` - Tất cả stored procedures

## Ghi chú

Code trong các form (.cs files) đã được cập nhật với các phương thức template và ví dụ. Một số phương thức được comment để tránh lỗi biên dịch do controls chưa được thiết kế đầy đủ trong Designer. Khi hoàn thành thiết kế form, có thể uncomment và kết nối với các controls thực tế.

## Tác giả

Chuyển đổi bởi: GitHub Copilot  
Ngày: 2025-11-09  
Dự án: Hệ thống Quản lý Thư viện (Library Management System)
