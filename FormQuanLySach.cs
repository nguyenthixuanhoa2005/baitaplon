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
    public partial class FormQuanLySach : Form
    {
        public FormQuanLySach()
        {
            InitializeComponent();
            LoadDanhSachDocGia();
            LoadDanhSachSach();
        }

        private void LoadDanhSachDocGia()
        {
            try
            {
                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_LayDanhSachDocGia");
                comboBoxDocGia.DataSource = dt;
                comboBoxDocGia.DisplayMember = "HoTen";
                comboBoxDocGia.ValueMember = "MaDocGia";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách độc giả: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        private void buttonTaoPhieuMuon_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxDocGia.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get selected book IDs from DataGridView
                string danhSachMaSach = "";
                foreach (DataGridViewRow row in dataGridViewSachMuon.Rows)
                {
                    if (row.Cells["MaSach"].Value != null)
                    {
                        danhSachMaSach += row.Cells["MaSach"].Value.ToString() + ",";
                    }
                }

                if (string.IsNullOrEmpty(danhSachMaSach))
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một cuốn sách!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                danhSachMaSach = danhSachMaSach.TrimEnd(',');

                SqlParameter[] parameters = {
                    new SqlParameter("@MaDocGia", comboBoxDocGia.SelectedValue),
                    new SqlParameter("@MaNhanVien", 1), // Replace with actual logged-in employee ID
                    new SqlParameter("@NgayHenTra", dateTimePickerNgayHenTra.Value),
                    new SqlParameter("@DanhSachMaSach", danhSachMaSach)
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_TaoPhieuMuon", parameters);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(dt.Rows[0]["Message"].ToString(), "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachSach();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo phiếu mượn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonTraSach_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxMaPhieuMuon.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã phiếu mượn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textBoxMaSach.Text))
                {
                    MessageBox.Show("Vui lòng chọn sách cần trả!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlParameter[] parameters = {
                    new SqlParameter("@MaPhieuMuon", int.Parse(textBoxMaPhieuMuon.Text)),
                    new SqlParameter("@MaSach", int.Parse(textBoxMaSach.Text)),
                    new SqlParameter("@TinhTrangSach", comboBoxTinhTrangSach.Text),
                    new SqlParameter("@PhiPhat", decimal.Parse(textBoxPhiPhat.Text))
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_TraSach", parameters);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(dt.Rows[0]["Message"].ToString(), "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachSach();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi trả sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonXemLichSu_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] parameters = null;
                
                if (comboBoxDocGia.SelectedValue != null)
                {
                    parameters = new SqlParameter[] {
                        new SqlParameter("@MaDocGia", comboBoxDocGia.SelectedValue)
                    };
                }

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_LayLichSuMuon", parameters);
                dataGridViewLichSuMuon.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xem lịch sử: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonThongKe_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_ThongKe");
                
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string thongKe = $"Tổng số sách: {row["TongSoSach"]}\n";
                    thongKe += $"Sách còn lại: {row["SachConLai"]}\n";
                    thongKe += $"Tổng độc giả: {row["TongDocGia"]}\n";
                    thongKe += $"Phiếu mượn đang mượn: {row["PhieuMuonDangMuon"]}\n";
                    thongKe += $"Phiếu quá hạn: {row["PhieuQuaHan"]}";

                    MessageBox.Show(thongKe, "Thống kê thư viện",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thống kê: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
