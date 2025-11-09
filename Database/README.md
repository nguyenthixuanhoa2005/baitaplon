# Hướng dẫn cài đặt cơ sở dữ liệu

## Thay đổi quan trọng

Dự án đã được chuyển đổi từ **Entity Framework** sang **kết nối SQL Server thuần túy** (Raw SQL) với các **Stored Procedures**.

## Các bước cài đặt

### 1. Tạo cơ sở dữ liệu

Chạy file SQL sau để tạo cơ sở dữ liệu và các bảng:

```
Database/CreateDatabase.sql
```

File này sẽ:
- Tạo database `Quanlisachcoban`
- Tạo các bảng: TaiKhoan, NhanVien, DocGia, TheLoaiSach, Sach, PhieuMuon, ChiTietPhieuMuon
- Thêm dữ liệu mẫu ban đầu

### 2. Tạo Stored Procedures

Chạy file SQL sau để tạo các stored procedures:

```
Database/StoredProcedures.sql
```

File này bao gồm các stored procedures cho:
- **Xác thực**: sp_DangNhap, sp_DangKy
- **Quản lý sách**: sp_LayDanhSachSach, sp_TimKiemSach, sp_ThemSach, sp_SuaSach, sp_XoaSach
- **Quản lý độc giả**: sp_LayDanhSachDocGia, sp_ThemDocGia, sp_SuaDocGia, sp_XoaDocGia
- **Quản lý nhân viên**: sp_LayDanhSachNhanVien, sp_ThemNhanVien, sp_SuaNhanVien, sp_XoaNhanVien
- **Mượn/Trả sách**: sp_TaoPhieuMuon, sp_TraSach, sp_LayLichSuMuon, sp_LayChiTietPhieuMuon
- **Báo cáo**: sp_ThongKe, sp_TopSachMuonNhieu
- **Thể loại**: sp_LayDanhSachTheLoai, sp_ThemTheLoai
- **Tài khoản**: sp_DoiMatKhau

### 3. Cấu hình connection string

Mở file `App.config` và chỉnh sửa connection string cho phù hợp với SQL Server của bạn:

```xml
<connectionStrings>
    <add name="QuanlisachcobanDB" 
         connectionString="Data Source=TÊN_SERVER\INSTANCE;Initial Catalog=Quanlisachcoban;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

Thay `TÊN_SERVER\INSTANCE` bằng tên SQL Server của bạn, ví dụ:
- `localhost\SQLEXPRESS`
- `DESKTOP-ABC\SQLEXPRESS`
- `.` (nếu dùng default instance)

### 4. Build và chạy ứng dụng

1. Mở solution trong Visual Studio
2. Build solution (Ctrl+Shift+B)
3. Chạy ứng dụng (F5)

## Tài khoản mặc định

Sau khi chạy script tạo database, bạn có thể đăng nhập bằng tài khoản:

- **Username**: admin
- **Password**: admin123

## Cấu trúc dự án

### DatabaseHelper.cs
Class helper chứa các phương thức để:
- Kết nối database
- Thực thi stored procedures
- Xử lý tham số SQL
- Trả về DataTable, scalar values

### Các form chính

1. **DangNhap.cs**: Đăng nhập hệ thống
2. **DangKy.cs**: Đăng ký tài khoản mới
3. **QLSach.cs**: Quản lý sách (CRUD)
4. **NhanVien.cs**: Quản lý nhân viên
5. **FormQuanLySach.cs**: Mượn/Trả sách và báo cáo
6. **TaiKhoan.cs**: Quản lý tài khoản, đổi mật khẩu

## Lưu ý

- Không còn sử dụng Entity Framework
- Tất cả thao tác database đều thông qua stored procedures
- Connection string đã được đơn giản hóa (không còn metadata)
- Đã xóa dependencies của EntityFramework khỏi packages.config

## Xử lý lỗi

Nếu gặp lỗi kết nối database:
1. Kiểm tra SQL Server có đang chạy không
2. Kiểm tra tên server trong connection string
3. Kiểm tra database đã được tạo chưa
4. Kiểm tra stored procedures đã được tạo chưa

## Các tính năng đã implement

✅ Kết nối SQL Server thuần túy  
✅ DatabaseHelper class  
✅ Tất cả stored procedures cho CRUD operations  
✅ Đăng nhập/Đăng ký  
✅ Quản lý sách  
✅ Quản lý nhân viên  
✅ Quản lý độc giả  
✅ Mượn/Trả sách  
✅ Báo cáo thống kê  
✅ Xóa dependencies Entity Framework  
