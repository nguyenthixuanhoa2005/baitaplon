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
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }

        private void textBoxMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonDangNhap_Click(object sender, EventArgs e)
        {
            try
            {
                string tenDangNhap = textBoxTaiKhoan.Text.Trim();
                string matKhau = textBoxMatKhau.Text.Trim();

                if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Call stored procedure for login
                SqlParameter[] parameters = {
                    new SqlParameter("@TenDangNhap", tenDangNhap),
                    new SqlParameter("@MatKhau", matKhau)
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_DangNhap", parameters);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string hoTen = row["HoTen"].ToString();
                    string vaiTro = row["VaiTro"].ToString();

                    MessageBox.Show($"Đăng nhập thành công!\nXin chào {hoTen} - {vaiTro}", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open main form (Form2 or appropriate main form)
                    // Form2 mainForm = new Form2();
                    // mainForm.Show();
                    // this.Hide();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDangKy_Click(object sender, EventArgs e)
        {
            DangKy formDangKy = new DangKy();
            formDangKy.Show();
        }
    }
}
