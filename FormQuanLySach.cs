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
            // Uncomment when controls are ready
            // LoadDanhSachDocGia();
            // LoadDanhSachSach();
        }

        private void LoadDanhSachDocGia()
        {
            try
            {
                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_LayDanhSachDocGia");
                // Bind to combobox when available
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
                // Bind to DataGridView when available
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Example method for creating borrow slip
        // Wire this to appropriate button click event
        private void TaoPhieuMuon()
        {
            try
            {
                /* Example implementation - modify based on actual controls
                if (comboBoxDocGia.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get selected book IDs
                string danhSachMaSach = "1,2,3"; // Get from form

                SqlParameter[] parameters = {
                    new SqlParameter("@MaDocGia", comboBoxDocGia.SelectedValue),
                    new SqlParameter("@MaNhanVien", 1),
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
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo phiếu mượn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Example method for returning books
        private void TraSach()
        {
            try
            {
                /* Example implementation
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
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi trả sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Example method for viewing borrow history
        private void XemLichSuMuon()
        {
            try
            {
                /* Example implementation
                SqlParameter[] parameters = {
                    new SqlParameter("@MaDocGia", comboBoxDocGia.SelectedValue)
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_LayLichSuMuon", parameters);
                // Bind to DataGridView
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xem lịch sử: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Example method for statistics
        private void ThongKe()
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
