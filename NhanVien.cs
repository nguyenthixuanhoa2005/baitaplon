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
            LoadDanhSachNhanVien();
        }

        private void buttonThem_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@HoTen", textBoxHoTen.Text.Trim()),
                    new SqlParameter("@NgaySinh", dateTimePickerNgaySinh.Value),
                    new SqlParameter("@GioiTinh", comboBoxGioiTinh.Text),
                    new SqlParameter("@DiaChi", textBoxDiaChi.Text.Trim()),
                    new SqlParameter("@SoDienThoai", textBoxSoDienThoai.Text.Trim()),
                    new SqlParameter("@Email", textBoxEmail.Text.Trim()),
                    new SqlParameter("@ChucVu", textBoxChucVu.Text.Trim()),
                    new SqlParameter("@NgayVaoLam", dateTimePickerNgayVaoLam.Value),
                    new SqlParameter("@TenDangNhap", string.IsNullOrEmpty(textBoxTenDangNhap.Text) ? (object)DBNull.Value : textBoxTenDangNhap.Text.Trim()),
                    new SqlParameter("@MatKhau", string.IsNullOrEmpty(textBoxMatKhau.Text) ? (object)DBNull.Value : textBoxMatKhau.Text.Trim())
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_ThemNhanVien", parameters);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(dt.Rows[0]["Message"].ToString(), "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachNhanVien();
                    ClearFields();
                }
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
                dataGridViewNhanVien.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxMaNhanVien.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlParameter[] parameters = {
                    new SqlParameter("@MaNhanVien", int.Parse(textBoxMaNhanVien.Text)),
                    new SqlParameter("@HoTen", textBoxHoTen.Text.Trim()),
                    new SqlParameter("@NgaySinh", dateTimePickerNgaySinh.Value),
                    new SqlParameter("@GioiTinh", comboBoxGioiTinh.Text),
                    new SqlParameter("@DiaChi", textBoxDiaChi.Text.Trim()),
                    new SqlParameter("@SoDienThoai", textBoxSoDienThoai.Text.Trim()),
                    new SqlParameter("@Email", textBoxEmail.Text.Trim()),
                    new SqlParameter("@ChucVu", textBoxChucVu.Text.Trim()),
                    new SqlParameter("@NgayVaoLam", dateTimePickerNgayVaoLam.Value),
                    new SqlParameter("@TrangThai", checkBoxTrangThai.Checked ? 1 : 0)
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_SuaNhanVien", parameters);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(dt.Rows[0]["Message"].ToString(), "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachNhanVien();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxMaNhanVien.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SqlParameter[] parameters = {
                        new SqlParameter("@MaNhanVien", int.Parse(textBoxMaNhanVien.Text))
                    };

                    DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_XoaNhanVien", parameters);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show(dt.Rows[0]["Message"].ToString(), "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachNhanVien();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            textBoxMaNhanVien.Clear();
            textBoxHoTen.Clear();
            textBoxDiaChi.Clear();
            textBoxSoDienThoai.Clear();
            textBoxEmail.Clear();
            textBoxChucVu.Clear();
            textBoxTenDangNhap.Clear();
            textBoxMatKhau.Clear();
            checkBoxTrangThai.Checked = true;
        }
    }
}
