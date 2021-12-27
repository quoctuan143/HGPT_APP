using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HGPT_APP.ViewModels
{
  public class Tao_Cong_Viec_Cho_Cong_Nhan_ViewModel :BaseViewModel
    {
        #region "Khai báo biến"
        public DateTime NgayLamViec { get; set; }
        public string   LenhSanXuat { get; set; }
        ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN> Cong_Doan_Cong_Nhans;
        public ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN> ListCong_Doan_Cong_Nhans
        {
            get => Cong_Doan_Cong_Nhans;
            set
            {
                if (value != null)
                {
                    Cong_Doan_Cong_Nhans = value;
                    OnPropertyChanged("ListCong_Doan_Cong_Nhans");
                }
            }
        }
        LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN _selectCongDoan;
        public LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN SelectCongDoan
        {
            get => _selectCongDoan;
            set
            {
                if (value != null)
                {
                    _selectCongDoan = value;
                    OnPropertyChanged("SelectCongDoan");
                }
            }
        }
        ObservableCollection<DM_TO_SAN_XUAT> _listToSanXuat;
        public ObservableCollection<DM_TO_SAN_XUAT> ListToSanXuat
        {
            get => _listToSanXuat;
            set
            {
                if (value != null)
                {
                    _listToSanXuat = value;
                    OnPropertyChanged("ListToSanXuat");
                }
            }
        }
   
        DM_TO_SAN_XUAT _selectToSanXuat;
        public DM_TO_SAN_XUAT SelectToSanXuat
        {
            get => _selectToSanXuat;
            set
            {
                _selectToSanXuat = value;
                LoadCongDoanCongNhan.Execute(_selectToSanXuat.Code);
                OnPropertyChanged("SelectToSanXuat");
            }
        }
        
        #endregion

        #region "Commnad"
        public Command<string> LoadCongDoanCongNhan { get; set; }
        public Command StartAndBeginAllCommand { get; set; }
        public Command StartAllCommand { get; set; }
        public Command LoadToSanXuatCommand { get; set; }      
        #endregion

        public Tao_Cong_Viec_Cho_Cong_Nhan_ViewModel(string _LenhSanXuat)
        {
            LenhSanXuat = _LenhSanXuat;
            ListCong_Doan_Cong_Nhans = new ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN>();
            ListToSanXuat = new ObservableCollection<DM_TO_SAN_XUAT>();          
           
            LoadCongDoanCongNhan = new Command<string>(async (p) => await LoadCongDoanCongNhanExcute(p));
            StartAndBeginAllCommand = new Command(async () => await StartAndBeginAllExcute());
            StartAllCommand = new Command(async () => await StartAllExcute());
            LoadToSanXuatCommand = new Command(async () => await LoadToSanXuatExcute());
            ListDMCongDoan = new ObservableCollection<Work_Type_Header>();

            ListDMCongDoan.Clear();
            var _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_Cong_Doan").Result;
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
            {
                Int32 from = _json.IndexOf("[");
                Int32 to = _json.IndexOf("]");
                string result = _json.Substring(from, to - from + 1);
                ListDMCongDoan = JsonConvert.DeserializeObject<ObservableCollection<Work_Type_Header>>(result);
            }
        }

        

        private async Task LoadToSanXuatExcute()
        {
            try
            {                             
                ListToSanXuat.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_To_San_Xuat?nhamay=" + Preferences.Get (Config.NhaMay ,"")).Result;
              
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListToSanXuat = JsonConvert.DeserializeObject<ObservableCollection<DM_TO_SAN_XUAT>>(result);
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
            
        }

        private async Task StartAllExcute()
        {
            try
            {
                if (ListCong_Doan_Cong_Nhans.Count > 0)
                {

                    if (IsBusy == true) return;
                    ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN> listOK = new ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN>();
                    foreach (LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN cn in ListCong_Doan_Cong_Nhans )
                    {
                        if (cn.MA_CONG_DOAN != null)
                        {
                            cn.LENH_SAN_XUAT = LenhSanXuat;
                            cn.TRANG_THAI = 0;
                            cn.NGUOI_TAO = Preferences.Get(Config.User, "");
                            cn.NGAY_TAO = DateTime.Now.Date;
                            cn.MA_NHA_MAY = Preferences.Get(Config.NhaMay, "");
                            cn.MA_XUONG = Preferences.Get(Config.MaXuong, "");
                            listOK.Add(cn);
                        }                                              
                    }
                   if (listOK.Count > 0)
                    {
                        var OK = await new MessageYesNo("Thông báo", "Bạn có muốn phân chia công việc này không?").Show();
                        if (OK == DialogReturn.OK)
                        {
                            IsBusy = true;
                            using (HttpClient client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(Config.URL);
                                var ok = client.PostAsJsonAsync("api/hgpt/post_Khoi_Tao_Cong_Viec?nguoitao=" + Preferences.Get(Config.User, ""), listOK);
                                if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                                {
                                    IsBusy = false;
                                    LoadCongDoanCongNhan.Execute(_selectToSanXuat.Code);
                                    DependencyService.Get<IMessage>().ShortAlert("Đã phân chia công việc");
                                }
                                else
                                {
                                    await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                                }
                                client.Dispose();
                            }    
                               

                        }
                    }    
                   

                }

            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task StartAndBeginAllExcute()
        {
            try
            {
                if (ListCong_Doan_Cong_Nhans.Count > 0)
                {

                    if (IsBusy == true) return;
                    ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN> listOK = new ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN>();
                    foreach (LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN cn in ListCong_Doan_Cong_Nhans)
                    {
                        if (cn.MA_CONG_DOAN != null)
                        {
                            cn.LENH_SAN_XUAT = LenhSanXuat;
                            cn.TRANG_THAI = 1;
                            cn.NGUOI_TAO = Preferences.Get(Config.User, "");
                            cn.NGAY_TAO = DateTime.Now.Date;
                            cn.GIO_BAT_DAU = DateTime.Now;
                            cn.MA_NHA_MAY = Preferences.Get(Config.NhaMay, "");
                            cn.MA_XUONG = Preferences.Get(Config.MaXuong, "");
                            listOK.Add(cn);
                        }

                    }
                    if (listOK.Count > 0)
                    {
                        var OK = await new MessageYesNo("Thông báo", "Bạn có muốn phân chia công việc này không?").Show();
                        if (OK == DialogReturn.OK)
                        {
                            IsBusy = true;
                            using (HttpClient client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(Config.URL);
                                var ok = client.PostAsJsonAsync("api/hgpt/post_Khoi_Tao_Cong_Viec?nguoitao=" + Preferences.Get(Config.User, ""), listOK);
                                if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                                {
                                    IsBusy = false;
                                    LoadCongDoanCongNhan.Execute(_selectToSanXuat.Code);
                                    DependencyService.Get<IMessage>().ShortAlert("Đã phân chia công việc");
                                }
                                else
                                {
                                    await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                                }
                                client.Dispose();
                            }    
                                

                        }
                    }    
                    

                }

            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadCongDoanCongNhanExcute(string tosanxuat)
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ListCong_Doan_Cong_Nhans.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_CongNhan_Theo_To_De_Phan_Chia_Cong_Viec?tosanxuat=" + tosanxuat).Result;
                await Task.Delay(1000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListCong_Doan_Cong_Nhans = JsonConvert.DeserializeObject<ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN>>(result);
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

        ObservableCollection<Work_Type_Header> _listDmCongDoan;
        public ObservableCollection<Work_Type_Header> ListDMCongDoan
        {
            get => _listDmCongDoan;
            set
            {
                if (value != null)
                {
                    _listDmCongDoan = value;
                    OnPropertyChanged("ListDMCongDoan");
                }
            }
        }
    }    
}
