using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlisachcoban
{
    public partial class NhanVien : Form
    {
        public NhanVien()
        {
            InitializeComponent();
            // Uncomment when form controls are ready
            // LoadDanhSachNhanVien();
        }

        private void buttonThem_Click(object sender, EventArgs e)
        {
            // Example method for adding employee
            // Implement based on actual form controls
            try
            {
                /*
                SqlParameter[] parameters = {
                    new SqlParameter("@HoTen", textBoxHoTen.Text.Trim()),
                    new SqlParameter("@NgaySinh", dateTimePickerNgaySinh.Value),
                    new SqlParameter("@GioiTinh", comboBoxGioiTinh.Text),
                    new SqlParameter("@DiaChi", textBoxDiaChi.Text.Trim()),
                    new SqlParameter("@SoDienThoai", textBoxSoDienThoai.Text.Trim()),
                    new SqlParameter("@Email", textBoxEmail.Text.Trim()),
                    new SqlParameter("@ChucVu", textBoxChucVu.Text.Trim()),
                    new SqlParameter("@NgayVaoLam", dateTimePickerNgayVaoLam.Value),
                    new SqlParameter("@TenDangNhap", DBNull.Value),
                    new SqlParameter("@MatKhau", DBNull.Value)
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_ThemNhanVien", parameters);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(dt.Rows[0]["Message"].ToString(), "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachNhanVien();
                }
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachNhanVien()
        {
            try
            {
                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_LayDanhSachNhanVien");
                // Assign to DataGridView when control is available
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
