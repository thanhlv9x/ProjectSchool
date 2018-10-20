using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinApplication
{
    public partial class MainPage : ContentPage
    {
        Color c = Color.FromHex("#89bef4");
        Color w = Color.White;
        TcpClient client;
        public static string quay;
        public static string id;
        public InfoUserMobile ium;
        public string IPaddress = "192.168.1.28";
        public int Port = 7777;

        public MainPage()
        {
            InitializeComponent();
            w = main.BackgroundColor;
            showGrid(main);
            connectServer();
            if (Application.Current.Properties.ContainsKey("Port") && Application.Current.Properties.ContainsKey("Ip"))
            {
                IPaddress = Application.Current.Properties["Ip"].ToString();
                Port = Convert.ToInt32(Application.Current.Properties["Port"]);
                lbIpaddress.Text = "Địa chỉ IP hiện tại: " + IPaddress;
                lbPort.Text = "Cổng hiện tại: " + Port.ToString();
            }
        }

        public void showGrid(Grid grid)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                error.IsVisible = false;
                info.IsVisible = false;
                thanks.IsVisible = false;
                skip.IsVisible = false;
                main.IsVisible = false;
                if (grid != null)
                    grid.IsVisible = true;
            });
        }
        public void chonMucDo(Grid grid)
        {
            gridRHL.BackgroundColor = w;
            gridHL.BackgroundColor = w;
            gridBT.BackgroundColor = w;
            gridKHL.BackgroundColor = w;
            if (grid != null)
            {
                if (grid.BackgroundColor != c)
                {
                    grid.BackgroundColor = c;
                }
                else
                {
                    grid.BackgroundColor = w;
                }

                if (grid == gridKHL)
                {
                    lbTitleGopY.Text = "Xin vui lòng góp ý (Bắt buộc)";
                    lbTitleGopY.TextColor = Color.Red;

                    lbThaiDo.Text = "Thái độ cán bộ không tốt";
                    lbThoiGian.Text = "Thời gian xử lý chậm";
                    lbGiaiQuyet.Text = "Giải quyết thủ tục không hiệu quả";
                    lbKetQua.Text = "Kết quả không như mong muốn";
                }
                else if (grid == gridBT)
                {
                    lbTitleGopY.Text = "Xin vui lòng góp ý (Không bắt buộc)";
                    lbTitleGopY.TextColor = Color.Black;

                    lbThaiDo.Text = "Thái độ cán bộ bình thường";
                    lbThoiGian.Text = "Thời gian xử lý bình thường";
                    lbGiaiQuyet.Text = "Giải quyết thủ tục khá hiệu quả";
                    lbKetQua.Text = "Kết quả tạm chấp nhận";
                }
                else
                {
                    lbTitleGopY.Text = "Xin vui lòng góp ý (Không bắt buộc)";
                    lbTitleGopY.TextColor = Color.Black;

                    lbThaiDo.Text = "Thái độ cán bộ tốt";
                    lbThoiGian.Text = "Thời gian xử lý nhanh";
                    lbGiaiQuyet.Text = "Giải quyết thủ tục hiệu quả";
                    lbKetQua.Text = "Kết quả như mong muốn";
                }
            }
        }
        public void chonGopY(Grid grid)
        {
            if (grid.BackgroundColor != c)
            {
                grid.BackgroundColor = c;
            }
            else
            {
                grid.BackgroundColor = w;
            }
        }
        public void connectServer()
        {
            try
            {
                client = new TcpClient(IPaddress, Port);
                //client.Connect(IPAddress.Parse("192.168.1.28"), 9999);
                Thread thread = new Thread(() => recData(client));
                thread.IsBackground = true;
                thread.Start();
                showGrid(info);
            }
            catch (Exception ex)
            {
                showGrid(error);
                //client.Close();
            }
        }
        public void recData(TcpClient client)
        {
            while (true)
            {
                try
                {
                    BinaryReader br = new BinaryReader(client.GetStream());
                    string str = br.ReadString();
                    if (str.Substring(0, 10).Equals("[INFO----]"))
                    {
                        string[] arr = str.Substring(10).Split('|');
                        quay = arr[0];
                        id = arr[1];
                        ium = new InfoUserMobile()
                        {
                            ID = Convert.ToInt32(arr[2]),
                            Name = arr[3],
                            BoPhan = arr[4],
                            Image = arr[5],
                        };
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            lbNumberQuay.Text = "Quầy số " + quay;
                            lbID.Text = ium.ID.ToString();
                            lbName.Text = ium.Name;
                            lbBP.Text = ium.BoPhan;
                            pbImage.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(ium.Image)));
                            showGrid(info);
                        });
                    }
                    else if (str.Substring(0, 10).Equals("[NEXT----]"))
                    {
                        showGrid(main);
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            lbNumber.Text = str.Substring(10);
                        });
                    }
                    else if (str.Substring(0, 10).Equals("[COMPLETE]"))
                    {
                        showGrid(info);
                    }
                    else if (str.Substring(0, 10).Equals("[EXIT----]"))
                    {
                        showGrid(error);
                    }
                    else if (str.Substring(0, 10).Equals("[RECEIVED]"))
                    {
                        showGrid(thanks);
                    }
                    else if (str.Substring(0, 10).Equals("[SKIP----]"))
                    {
                        showGrid(skip);
                    }
                }
                catch
                {
                    showGrid(skip);
                    break;
                }
            }
        }

        /// <summary>
        /// Sự kiện click grid Rất hài lòng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_RHL(object sender, EventArgs e)
        {
            chonMucDo(gridRHL);
        }
        /// <summary>
        /// Sự kiện click grid Hài lòng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_HL(object sender, EventArgs e)
        {
            chonMucDo(gridHL);
        }
        /// <summary>
        /// Sự kiện click grid Bình thường
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_BT(object sender, EventArgs e)
        {
            chonMucDo(gridBT);
        }
        /// <summary>
        /// Sự kiện click grid Không hài lòng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_KHL(object sender, EventArgs e)
        {
            chonMucDo(gridKHL);
        }
        /// <summary>
        /// Sự kiện click grid Thái độ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_ThaiDo(object sender, EventArgs e)
        {
            chonGopY(gridThaiDo);
        }
        /// <summary>
        /// Sự kiện click grid Thời gian
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_ThoiGian(object sender, EventArgs e)
        {
            chonGopY(gridThoiGian);
        }
        /// <summary>
        /// Sự kiện click grid Giải quyết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_GiaiQuyet(object sender, EventArgs e)
        {
            chonGopY(gridGiaiQuyet);
        }
        /// <summary>
        /// Sự kiện click grid Kết quả
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void click_KetQua(object sender, EventArgs e)
        {
            chonGopY(gridKetQua);
        }
        /// <summary>
        /// Sự kiện click button Gửi kết quả
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Clicked(object sender, EventArgs e)
        {
            int muc_do = 0;
            if (gridRHL.BackgroundColor == c) muc_do = 1;
            else if (gridHL.BackgroundColor == c) muc_do = 2;
            else if (gridBT.BackgroundColor == c) muc_do = 3;
            else if (gridKHL.BackgroundColor == c) muc_do = 4;

            if (muc_do == 0)
            {
                DisplayAlert("Chưa chọn đánh giá", "Vui lòng chọn mức đánh giá", "OK");
                return;
            }

            if (muc_do == 4 && gridThaiDo.BackgroundColor != c && gridThoiGian.BackgroundColor != c && gridGiaiQuyet.BackgroundColor != c && gridKetQua.BackgroundColor != c && txtGopY.Text.Length == 0)
            {
                DisplayAlert("Chưa góp ý", "Vui lòng góp ý", "OK");
                return;
            }

            string str = "";
            if (gridThaiDo.BackgroundColor == c) str += lbThaiDo.Text + ";";
            if (gridThoiGian.BackgroundColor == c) str += lbThoiGian.Text + ";";
            if (gridGiaiQuyet.BackgroundColor == c) str += lbGiaiQuyet.Text + ";";
            if (gridKetQua.BackgroundColor == c) str += lbKetQua.Text + ";";

            string strReport = "[REPORT]" + muc_do + "|" + str + txtGopY.Text;
            try
            {
                BinaryWriter bw = new BinaryWriter(client.GetStream());
                bw.Write(strReport);
                bw.Flush();
                gridThaiDo.BackgroundColor = w;
                gridThoiGian.BackgroundColor = w;
                gridGiaiQuyet.BackgroundColor = w;
                gridKetQua.BackgroundColor = w;
                txtGopY.Text = "";
                chonMucDo(null);
            }
            catch
            {
                showGrid(error);
            }
        }
        /// <summary>
        /// Sự kiện nút kết nối lại
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clicked(object sender, EventArgs e)
        {
            connectServer();
        }
        public void closeConnect()
        {
            try
            {
                BinaryWriter bw = new BinaryWriter(client.GetStream());
                bw.Write("[EXIT--]" + id);
                bw.Flush();
            }
            catch { }
        }
        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            closeConnect();
        }
        private void btnChangeIP_Clicked(object sender, EventArgs e)
        {
            if (txtIpaddress.Text != "" && txtPort.Text != "")
            {
                try
                {
                    Port = Convert.ToInt32(txtPort.Text);
                    lbPort.Text = "Cổng hiện tại: " + txtPort.Text;
                    IPaddress = txtIpaddress.Text;
                    lbIpaddress.Text = "Địa chỉ IP hiện tại: " + txtIpaddress.Text;
                    Application.Current.Properties["Port"] = Port;
                    Application.Current.Properties["Ip"] = IPaddress;
                    Application.Current.SavePropertiesAsync();
                }
                catch
                {
                    DisplayAlert("Địa chỉ cổng/IP không hợp lệ", "Vui lòng kiểm tra và thay đổi địa chỉ cổng/IP đã nhập", "Cancel");
                }
            }
            else
            {
                DisplayAlert("Thiếu thông tin", "Yêu cầu điền đầy đủ thông tin cổng/IP", "Cancel");
            }
        }
    }
}
