using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApplication
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            InfoUser.URL = DesktopApplication.Properties.Settings.Default.ipaddress;
            lbIP.Text = InfoUser.URL;
            lbQuay.Text = DesktopApplication.Properties.Settings.Default.quay;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "" || txtID.Text == "")
            {
                lbError.Text = "Vui lòng nhập đầy đủ tài khoản và mật khẩu !";
            }
            else
            {
                string id = txtID.Text;
                string pw = GetMD5(txtPW.Text);
                TaiKhoanUser user = new TaiKhoanUser()
                {
                    Id = id,
                    Pw = pw
                };
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(InfoUser.URL);
                    }
                    catch
                    {
                        MessageBox.Show("Lỗi địa chỉ IP không xác định. Vui lòng điều chỉnh lại !");
                    }
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response;
                    try { response = client.PostAsJsonAsync("api/ClientAPI", user).Result; }
                    catch
                    {
                        MessageBox.Show("Địa chỉ IP không hợp lệ. Vui lòng kiểm tra và thiết lập lại !", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsAsync<TaiKhoanUser>().Result;
                        if (result != null)
                        {
                            this.Hide();
                            InfoUser.Id = id;
                            InfoUser.MaCB = (int)result.MaCB;
                            InfoUser.MaBP = (int)result.MaBP;
                            InfoUser.TenBP = result.TenBP;
                            InfoUser.HoTen = result.HoTen;
                            InfoUser.HinhAnh = result.HinhAnh;
                            FormMain frmMain = new FormMain();
                            frmMain.ShowDialog();
                        }
                        else
                        {
                            lbError.Text = "Tài khoản/Mật khẩu không chính xác";
                        }
                    }
                    else
                    {
                        lbError.Text = "Tài khoản/Mật khẩu không chính xác";
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox1.Visible = !textBox1.Visible;
            label4.Visible = !label4.Visible;
            btnSetting.Visible = !btnSetting.Visible;
            lbIP.Visible = !lbIP.Visible;
            label5.Visible = !label5.Visible;
            lbQuay.Visible = !lbQuay.Visible;
            txtQuay.Visible = !txtQuay.Visible;
            btnQuay.Visible = !btnQuay.Visible;
            label6.Visible = !label6.Visible;
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            InfoUser.URL = "http://" + textBox1.Text + "/";
            DesktopApplication.Properties.Settings.Default.ipaddress = InfoUser.URL;
            DesktopApplication.Properties.Settings.Default.Save();
            lbIP.Text = InfoUser.URL;
            textBox1.Visible = !textBox1.Visible;
            label4.Visible = !label4.Visible;
            btnSetting.Visible = !btnSetting.Visible;
            lbIP.Visible = !lbIP.Visible;
            label5.Visible = !label5.Visible;
            lbQuay.Visible = !lbQuay.Visible;
            txtQuay.Visible = !txtQuay.Visible;
            btnQuay.Visible = !btnQuay.Visible;
            label6.Visible = !label6.Visible;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("Bạn chắc chắn muốn thoát?", "Thoát chương trình", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Phương thức mã hóa MD5
        /// </summary>
        /// <param name="txt">Chuỗi cần mã hóa</param>
        /// <returns></returns>
        public static String GetMD5(string txt)
        {
            String str = "";
            Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(txt);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            foreach (Byte b in buffer)
            {
                str += b.ToString("X2");
            }
            return str;
        }

        private void btnQuay_Click(object sender, EventArgs e)
        {
            DesktopApplication.Properties.Settings.Default.quay = txtQuay.Text;
            DesktopApplication.Properties.Settings.Default.Save();
            lbQuay.Text = txtQuay.Text;
            txtQuay.Text = "";
        }
    }
}
