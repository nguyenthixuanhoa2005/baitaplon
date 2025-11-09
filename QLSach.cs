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
    public partial class QLSach : Form
    {
        public QLSach()
        {
            InitializeComponent();
            LoadDanhSachSach();
            LoadTheLoai();
        }

        private void panelDanhMucSach_Paint(object sender, PaintEventArgs e)
        {

        }

        // Load book list
        private void LoadDanhSachSach()
        {
            try
            {
                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_LayDanhSachSach");
                dataGridViewSach.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load categories
        private void LoadTheLoai()
        {
            try
            {
                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_LayDanhSachTheLoai");
                if (comboBoxTheLoai != null)
                {
                    comboBoxTheLoai.DataSource = dt;
                    comboBoxTheLoai.DisplayMember = "TenTheLoai";
                    comboBoxTheLoai.ValueMember = "MaTheLoai";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thể loại: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Search books
        private void buttonTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = textBoxTimKiem.Text.Trim();
                
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    LoadDanhSachSach();
                    return;
                }

                SqlParameter[] parameters = {
                    new SqlParameter("@TuKhoa", tuKhoa)
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_TimKiemSach", parameters);
                dataGridViewSach.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Add new book
        private void buttonThem_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@TenSach", textBoxTenSach.Text.Trim()),
                    new SqlParameter("@TacGia", textBoxTacGia.Text.Trim()),
                    new SqlParameter("@NhaXuatBan", textBoxNhaXuatBan.Text.Trim()),
                    new SqlParameter("@NamXuatBan", int.Parse(textBoxNamXuatBan.Text)),
                    new SqlParameter("@MaTheLoai", comboBoxTheLoai.SelectedValue),
                    new SqlParameter("@SoLuong", int.Parse(textBoxSoLuong.Text)),
                    new SqlParameter("@GiaSach", decimal.Parse(textBoxGiaSach.Text)),
                    new SqlParameter("@ViTri", textBoxViTri.Text.Trim()),
                    new SqlParameter("@TrangThai", "Có sẵn"),
                    new SqlParameter("@MoTa", textBoxMoTa.Text.Trim())
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_ThemSach", parameters);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(dt.Rows[0]["Message"].ToString(), "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachSach();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Update book
        private void buttonSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxMaSach.Text))
                {
                    MessageBox.Show("Vui lòng chọn sách cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlParameter[] parameters = {
                    new SqlParameter("@MaSach", int.Parse(textBoxMaSach.Text)),
                    new SqlParameter("@TenSach", textBoxTenSach.Text.Trim()),
                    new SqlParameter("@TacGia", textBoxTacGia.Text.Trim()),
                    new SqlParameter("@NhaXuatBan", textBoxNhaXuatBan.Text.Trim()),
                    new SqlParameter("@NamXuatBan", int.Parse(textBoxNamXuatBan.Text)),
                    new SqlParameter("@MaTheLoai", comboBoxTheLoai.SelectedValue),
                    new SqlParameter("@SoLuong", int.Parse(textBoxSoLuong.Text)),
                    new SqlParameter("@GiaSach", decimal.Parse(textBoxGiaSach.Text)),
                    new SqlParameter("@ViTri", textBoxViTri.Text.Trim()),
                    new SqlParameter("@TrangThai", comboBoxTrangThai.Text),
                    new SqlParameter("@MoTa", textBoxMoTa.Text.Trim())
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_SuaSach", parameters);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(dt.Rows[0]["Message"].ToString(), "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachSach();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Delete book
        private void buttonXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxMaSach.Text))
                {
                    MessageBox.Show("Vui lòng chọn sách cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sách này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SqlParameter[] parameters = {
                        new SqlParameter("@MaSach", int.Parse(textBoxMaSach.Text))
                    };

                    DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_XoaSach", parameters);
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show(dt.Rows[0]["Message"].ToString(), "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachSach();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            textBoxMaSach.Clear();
            textBoxTenSach.Clear();
            textBoxTacGia.Clear();
            textBoxNhaXuatBan.Clear();
            textBoxNamXuatBan.Clear();
            textBoxSoLuong.Clear();
            textBoxGiaSach.Clear();
            textBoxViTri.Clear();
            textBoxMoTa.Clear();
            if (comboBoxTheLoai != null && comboBoxTheLoai.Items.Count > 0)
                comboBoxTheLoai.SelectedIndex = 0;
        }
    }
}
