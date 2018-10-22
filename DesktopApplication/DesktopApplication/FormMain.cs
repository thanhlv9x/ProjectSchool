using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApplication
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.Text = "Cán bộ: " + InfoUser.HoTen + " - Mã số: " + InfoUser.MaCB + " - Bộ phận: " + InfoUser.TenBP;
            DateTime dt = DateTime.Now;
            lbDate.Text = string.Format("Ngày {0} - Tháng {1} - Năm {2}", dt.Day, dt.Month, dt.Year);
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Add("Số thứ tự");
            listView1.Columns[0].Width = 60;
            listView1.Columns.Add("Thời gian");
            listView1.Columns[1].Width = 1000;
            lbStatus.Text = "Trạng thái: Chưa kết nối";
            lbStatus.ForeColor = Color.Red;
        }

        TcpListener server;
        List<InfoClient> listIC = new List<InfoClient>();
        public string IPaddress;
        public int Port;

        private void FormMain_Load(object sender, EventArgs e)
        {
            openServer();
            lbQuay.Text = DesktopApplication.Properties.Settings.Default.quay;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!lbStatus.Text.Equals("Trạng thái: Đã kết nối"))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(InfoUser.URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("api/ClientAPI/?_MaCB=" + InfoUser.MaCB + "&_MaBP=" + InfoUser.MaBP).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsAsync<SoThuTuUser>().Result;
                        if (result != null)
                        {
                            InfoUser.MaSTT = result.MaSTT;
                            InfoUser.STT = (int)result.STT;
                            int stt = (int)result.STT;
                            lbNumber.Text = stt.ToString();
                            btnSkip.Enabled = true;
                            btnNext.Enabled = false;
                            sendMessage("[NEXT----]" + stt);
                        }
                        else
                        {
                            MessageBox.Show("Hết số thứ tự.", "Lỗi lấy số thứ tự", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hết số thứ tự.", "Lỗi lấy số thứ tự", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Chưa có kết nối với máy đánh giá.", "Lỗi kết nối với máy đánh giá", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            KetQuaDanhGiaUser md = new KetQuaDanhGiaUser()
            {
                MaSTT = InfoUser.MaSTT,
                MaCB = InfoUser.MaCB,
                GopY = InfoUser.GopY,
                MucDo = InfoUser.MucDo
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(InfoUser.URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync("api/ClientAPI/?_Success=1", md).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsAsync<DateTime>().Result;
                    if (result != null)
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Text = InfoUser.STT.ToString();
                        item1.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = result.Hour + " : " + result.Minute + " : " + result.Second });
                        listView1.Items.Add(item1);
                        btnComplete.Enabled = false;
                        btnSkip.Enabled = false;
                        btnNext.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Gặp sự cố trong quá trình gửi nhận dữ liệu.", "Lỗi gửi nhận dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Gặp sự cố trong quá trình gửi nhận dữ liệu.", "Lỗi gửi nhận dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            sendMessage("[COMPLETE]");
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            btnSkip.Enabled = false;
            btnComplete.Enabled = false;
            btnNext.Enabled = true;
            sendMessage("[SKIP----]");
        }

        private void btnChangePw_Click(object sender, EventArgs e)
        {
            gbPw.Enabled = true;
            gbPw.Visible = true;
            lbErrorChangePw.Text = String.Empty;
        }

        private void btnSubmitChange_Click(object sender, EventArgs e)
        {
            string opw = FormLogin.GetMD5(txtOldPw.Text);
            string pw1 = FormLogin.GetMD5(txtNewPw1.Text);
            string pw2 = FormLogin.GetMD5(txtNewPw2.Text);
            if (pw1.Equals(pw2))
            {
                TaiKhoanUserNew md = new TaiKhoanUserNew()
                {
                    Id = InfoUser.Id,
                    MaCB = InfoUser.MaCB,
                    OldPw = opw,
                    NewPw = pw1
                };
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(InfoUser.URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.PutAsJsonAsync("api/ClientAPI/?_MaCB=" + InfoUser.MaCB, md).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsAsync<bool>().Result;
                        if (result)
                        {
                            txtNewPw1.Text = String.Empty;
                            txtNewPw2.Text = String.Empty;
                            txtOldPw.Text = String.Empty;
                            lbErrorChangePw.Text = "Thay đổi thành công";
                            lbErrorChangePw.ForeColor = Color.Green;
                        }
                        else
                        {
                            lbErrorChangePw.Text = "Thay đổi không thành công";
                            lbErrorChangePw.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        lbErrorChangePw.Text = "Mật khẩu cũ không chính xác";
                        lbErrorChangePw.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                lbErrorChangePw.Text = "Nhập lại mật khẩu mới không chính xác";
                lbErrorChangePw.ForeColor = Color.Red;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            gbPw.Enabled = false;
            gbPw.Visible = false;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn muốn đăng xuất?", "Đăng xuất", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeServer();
            FormLogin frmLogin = new FormLogin();
            frmLogin.Show();
        }

        /// <summary>
        /// Hàm mở máy chủ.
        /// </summary>
        void openServer()
        {
            try
            {
                string host = Dns.GetHostName();
                IPHostEntry myIP = Dns.GetHostByName(host);
                IPaddress = myIP.AddressList[0].ToString();
                Port = 7777;
                server = new TcpListener(IPAddress.Parse(IPaddress), Port);
                server.Start();

                //string myIP = Dns.GetHostByName(Dns.GetHostName()).AddressList[2].ToString();
                //server = new TcpListener(IPAddress.Parse(myIP), 9999); // Tạo địa chỉ IP và cổng kết nối cho server.
                //server.Start(); // Khởi chạy server.

                //server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
                //server.Start();
                //waitForConnect();
                Thread thread = new Thread(waitForConnect); // Tạo luồng chờ Client kết nối.
                thread.IsBackground = true; // Thuộc tính BackGround: khi luồng chính dừng thì luồng BackGround dừng theo.
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Lỗi mở máy chủ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hàm đợi kết nối từ Client
        /// </summary>
        public void waitForConnect()
        {
            while (true)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient(); // Tạo đối tượng TcpClient để kết nối với Client.
                    BinaryWriter bw = new BinaryWriter(client.GetStream()); // Tạo bộ đọc ghi cho từng đối tượng Client.
                    string id = client.Client.RemoteEndPoint.ToString(); // Tạo id cho Client đã kết nối.
                    string quay = DesktopApplication.Properties.Settings.Default.quay;
                    InfoClient ic = new InfoClient() // Tạo đối tượng thông tin của Client.
                    {
                        Client = client,
                        Bw = bw
                    };
                    listIC.Add(ic); // Thêm đối tượng thông tin của Client vào danh sách để quản lý.
                    InfoUserMobile ium = new InfoUserMobile() // Tạo đối tượng giữ thông tin gửi đến mobile
                    {
                        ID = InfoUser.MaCB,
                        Name = InfoUser.HoTen,
                        BoPhan = InfoUser.TenBP,
                        Image = InfoUser.HinhAnh
                    };
                    //MemoryStream ms = new MemoryStream();
                    //Image img = Image.FromFile(ium.Image);
                    //img.Save(ms, img.RawFormat);
                    //byte[] data = ms.ToArray();
                    //string strImg = Convert.ToBase64String(data);
                    bw.Write("[INFO----]" + quay + "|" + id + "|" + ium.ID.ToString() + "|" + ium.Name.ToString() + "|" + ium.BoPhan.ToString() + "|" + ium.Image.ToString()); // Gửi thông tin cán bộ tới Client.
                    lbStatus.BeginInvoke((MethodInvoker)delegate // Thay đổi trạng thái kết nối.
                    {
                        lbStatus.Text = "Trạng thái: Đã kết nối";
                        lbStatus.ForeColor = Color.Green;
                    });
                    Thread thread = new Thread(() => recData(client)); // Tạo luồng nhận dữ liệu từ Client.
                    thread.IsBackground = true;
                    thread.Start(); // Bắt đầu chạy luồng.
                }
                catch
                {
                    //MessageBox.Show(ex.ToString(), "Lỗi nhận kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
        }

        /// <summary>
        /// Hàm nhận dữ liệu từ Client.
        /// </summary>
        /// <param name="client">Client thực hiện gửi</param>
        public void recData(TcpClient client)
        {
            try
            {
                while (true)
                {
                    BinaryReader br = new BinaryReader(client.GetStream()); // Tạo bộ đọc dữ liệu được gửi từ client.
                    string str = br.ReadString(); // Đọc chuỗi dữ liệu được gửi từ client.
                    if (str.Substring(0, 8).Equals("[REPORT]"))
                    {
                        // Sự kiện tạo bộ kết quả đánh giá gửi về server.
                        string[] arr = str.Substring(8).Split('|');
                        InfoUser.MucDo = Convert.ToInt32(arr[0]);
                        InfoUser.GopY = arr[1].ToString();
                        btnComplete.BeginInvoke((MethodInvoker)delegate
                        {
                            btnComplete.Enabled = true;
                        });
                        sendMessage("[RECEIVED]");
                    }
                    else if (str.Substring(0, 8).Equals("[EXIT--]"))
                    {
                        bool breakClient = false;
                        // Sự kiện ngắt kết nối của Client.
                        foreach (var item in listIC)
                        {
                            if (item.Client.Client.RemoteEndPoint.ToString().Equals(str.Substring(8)))
                            {
                                item.Client.Close(); // Hủy bỏ Client.
                                item.Bw.Close(); // Hủy bỏ bộ ghi của Client.
                                listIC.Remove(item); // Xóa bỏ Client ra khỏi listClient.
                                lbStatus.BeginInvoke((MethodInvoker)delegate
                                {
                                    lbStatus.Text = "Trạng thái: Chưa kết nối";
                                    lbStatus.ForeColor = Color.Red;
                                });
                                breakClient = true;
                                break;
                            }
                        }
                        if (breakClient) break;
                    }
                }
            }
            catch
            {
                // Sự kiện ngắt kết nối của Client.
                foreach (var item in listIC)
                {
                    if (item.Client.Client.RemoteEndPoint.ToString().Equals(client.Client.RemoteEndPoint.ToString()))
                    {
                        item.Client.Close(); // Hủy bỏ Client.
                        item.Bw.Close(); // Hủy bỏ bộ ghi của Client.
                        listIC.Remove(item); // Xóa bỏ Client ra khỏi listClient.
                        lbStatus.BeginInvoke((MethodInvoker)delegate
                        {
                            lbStatus.Text = "Trạng thái: Chưa kết nối";
                            lbStatus.ForeColor = Color.Red;
                        });
                        break;
                    }
                }
                //MessageBox.Show(ex.ToString(), "Lỗi gửi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Hàm gửi dữ liệu tới Client.
        /// </summary>
        /// <param name="str">Chuỗi dữ liệu cần gửi</param>
        public void sendMessage(string str)
        {
            foreach (var item in listIC)
            {
                try
                {
                    item.Bw.Write(str);
                    item.Bw.Flush();
                }
                catch
                {
                    item.Client.Close();
                    item.Bw.Close();
                    listIC.Remove(item);
                    continue;
                }
            }
        }

        /// <summary>
        /// Hàm đóng máy chủ.
        /// </summary>
        public void closeServer()
        {
            sendMessage("[EXIT----]"); // Gửi tín hiệu đóng máy chủ đến các Client.
            listIC.Clear(); // Xóa hết listClient đang kết nối.
            server.Stop();
        }

        private void pbSetting_Click(object sender, EventArgs e)
        {
            MessageBox.Show("IP hiện tại: " + IPaddress + " - Cổng: " + Port, "Thông tin địa chỉ IP/cổng", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
