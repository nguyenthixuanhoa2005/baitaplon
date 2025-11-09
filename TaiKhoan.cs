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
    public partial class TaiKhoan : Form
    {
        private int maTaiKhoan;

        public TaiKhoan(int maTaiKhoan)
        {
            InitializeComponent();
            this.maTaiKhoan = maTaiKhoan;
        }

        public TaiKhoan()
        {
            InitializeComponent();
        }

        // Example method for changing password
        // Wire this to the appropriate button click event
        private void DoiMatKhau()
        {
            try
            {
                // Example - modify based on actual controls
                /*
                string matKhauCu = textBoxMatKhauCu.Text.Trim();
                string matKhauMoi = textBoxMatKhauMoi.Text.Trim();
                string xacNhanMatKhau = textBoxXacNhanMatKhau.Text.Trim();

                if (string.IsNullOrEmpty(matKhauCu) || string.IsNullOrEmpty(matKhauMoi))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (matKhauMoi != xacNhanMatKhau)
                {
                    MessageBox.Show("Mật khẩu mới không khớp!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (matKhauMoi.Length < 6)
                {
                    MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlParameter[] parameters = {
                    new SqlParameter("@MaTaiKhoan", maTaiKhoan),
                    new SqlParameter("@MatKhauCu", matKhauCu),
                    new SqlParameter("@MatKhauMoi", matKhauMoi)
                };

                DataTable dt = DatabaseHelper.ExecuteStoredProcedure("sp_DoiMatKhau", parameters);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    int result = Convert.ToInt32(row["Result"]);
                    string message = row["Message"].ToString();

                    if (result > 0)
                    {
                        MessageBox.Show(message, "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đổi mật khẩu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
