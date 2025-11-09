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
    public partial class DangKy : Form
    {
        public DangKy()
        {
            InitializeComponent();
        }

        private void buttonDangKy_Click(object sender, EventArgs e)
        {
            try
            {
                string tenDangNhap = textBoxTaiKhoan.Text.Trim();
                string matKhau = textBoxMatKhau.Text.Trim();
                string xacNhanMatKhau = textBoxXacNhanMatKhau.Text.Trim();
                string email = textBoxEmail.Text.Trim();

                // Validate input
                if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (matKhau != xacNhanMatKhau)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (matKhau.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Call stored procedure for registration
                SqlParameter[] parameters = {
                    new SqlParameter("@TenDangNhap", tenDangNhap),
                    new SqlParameter("@MatKhau", matKhau),
                    new SqlParameter("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email),
                    new SqlParameter("@VaiTro", "DocGia")
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_DangKy", parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    int result = Convert.ToInt32(row["Result"]);
                    string message = row["Message"].ToString();

                    if (result > 0)
                    {
                        MessageBox.Show(message, "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
