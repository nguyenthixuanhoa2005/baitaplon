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
            // Uncomment when form is fully designed
            // LoadDanhSachSach();
            // LoadTheLoai();
        }

        private void panelDanhMucSach_Paint(object sender, PaintEventArgs e)
        {

        }

        // Load book list - call this method when dataGridView1 is ready
        private void LoadDanhSachSach()
        {
            try
            {
                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_LayDanhSachSach");
                if (dataGridView1 != null)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load categories - call this when comboBox for categories is ready
        private void LoadTheLoai()
        {
            try
            {
                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_LayDanhSachTheLoai");
                // Assign to appropriate comboBox when controls are defined
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thể loại: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Example: Search books by keyword from textBoxTimKiem
        private void TimKiemSach()
        {
            try
            {
                string tuKhoa = textBoxTimKiem != null ? textBoxTimKiem.Text.Trim() : "";
                
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    LoadDanhSachSach();
                    return;
                }

                SqlParameter[] parameters = {
                    new SqlParameter("@TuKhoa", tuKhoa)
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_TimKiemSach", parameters);
                if (dataGridView1 != null)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Example method for adding a book
        // Wire this to buttonThem.Click event handler
        private void ThemSach()
        {
            try
            {
                // Get values from form controls
                // Example structure - modify based on actual controls:
                /*
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
                }
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
