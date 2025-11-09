# 📚 HỆ THỐNG QUẢN LÝ THƯ VIỆN (WINFORM)

![GitHub stars](https://img.shields.io/github/stars/nguyenthixuanhoa2005/baitaplon)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 📖 Mục lục
- [Giới thiệu](#giới-thiệu)
- [Tính năng](#tính-năng)
- [Công nghệ](#công-nghệ)
- [Hướng dẫn sử dụng](#hướng-dẫn-sử-dụng)
- [Demo giao diện](#demo-giao-diện)

---

## 1. 👋 Giới thiệu
**Hệ thống Quản lý Thư viện** là phần mềm được xây dựng bằng Winform (C#), phù hợp cho các thư viện vừa và nhỏ tại trường học hoặc cơ quan. Ứng dụng giúp quản lý sách, độc giả, nhân viên, quá trình mượn trả và thống kê hiệu quả.

---

## 2. 🚀 Tính năng nổi bật
- **Quản lý sách:** Thêm, sửa, xóa, tìm kiếm sách theo mã, tác giả, thể loại...
- **Quản lý độc giả:** Lưu thông tin, tra cứu, xem lịch sử mượn trả.
- **Quản lý nhân viên:** Thêm, sửa, xóa, tìm kiếm nhân viên thư viện.
- **Mượn trả sách:** Tạo phiếu mượn/trả, chỉnh sửa phiếu, chi tiết mượn trả, tính phí, quản lý quá hạn.
- **Báo cáo thống kê:** Số lượng sách, số độc giả, tình trạng mượn trả, top sách được mượn nhiều nhất...
- **Phân quyền truy cập:** Phân quyền rõ ràng cho admin, nhân viên, đảm bảo an toàn dữ liệu.

---

## 3. ⚙️ Công nghệ sử dụng
- **Ngôn ngữ:** C# (.NET Framework 4.7.2)
- **Giao diện:** Winform hiện đại, dễ sử dụng
- **Cơ sở dữ liệu:** SQL Server với **Stored Procedures**
- **Kết nối database:** ADO.NET (Raw SQL - không dùng Entity Framework)

---

## 4. 🛠️ Hướng dẫn sử dụng

### Bước 1: Tải mã nguồn
```sh
git clone https://github.com/nguyenthixuanhoa2005/baitaplon.git
```

### Bước 2: Mở bằng **Visual Studio**

### Bước 3: Tạo database SQL Server

Chạy lần lượt 2 file SQL trong thư mục `Database/`:

1. **CreateDatabase.sql** - Tạo database và các bảng
2. **StoredProcedures.sql** - Tạo các stored procedures

Chi tiết xem file `Database/README.md`

### Bước 4: Sửa chuỗi kết nối database  

Mở file `App.config` và chỉnh sửa connection string:

```xml
<connectionStrings>
    <add name="QuanlisachcobanDB" 
         connectionString="Data Source=TÊN_SERVER;Initial Catalog=Quanlisachcoban;Integrated Security=True;..." 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

Thay `TÊN_SERVER` bằng tên SQL Server của bạn (ví dụ: `localhost\SQLEXPRESS`)

### Bước 5: Build và chạy chương trình  

Đăng nhập bằng tài khoản mặc định:
- **Username**: admin
- **Password**: admin123

---

## 📝 Thay đổi quan trọng

✅ **Đã chuyển từ Entity Framework sang Raw SQL**
- Sử dụng ADO.NET với SqlConnection và SqlCommand
- Tất cả thao tác database thông qua stored procedures
- Class DatabaseHelper để quản lý kết nối và thực thi SQL
- Hiệu suất tốt hơn và kiểm soát tốt hơn

📁 **Cấu trúc thư mục Database/**
- `CreateDatabase.sql` - Script tạo database và bảng
- `StoredProcedures.sql` - Tất cả stored procedures
- `README.md` - Hướng dẫn chi tiết

---

## 5. 🖼️ Demo giao diện

<p align="center">
  <img src="./img/main_screen.png" alt="Giao diện chính" width="600"/>
</p>

---

> 💡 *Chúc các bạn quản lý thư viện hiệu quả và bớt đau đầu với đống sách giấy tờ! Nếu có thắc mắc hay góp ý, hãy mở issue nhé.*
