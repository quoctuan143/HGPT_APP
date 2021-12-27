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
    public class Phan_Chia_Cong_Viec_ViewModel :BaseViewModel
    {
        #region "Khai báo biến"
        public DateTime NgayLamViec { get; set; }
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
        public  LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN SelectCongDoan { get => _selectCongDoan;
        set
            {
                if (value !=  null )
                {
                    _selectCongDoan = value;
                    OnPropertyChanged("SelectCongDoan");
                }    
            }
        }
        #endregion

        #region "Commnad"
        public Command LoadCongDoanCongNhan { get; set; }
        public Command StopAllCommand { get; set; }
        public Command StartAllCommand { get; set; }
        public Command DieuChuyenCommand { get; set; }
        #endregion

        public Phan_Chia_Cong_Viec_ViewModel()
        {
            try
            {
                NgayLamViec = DateTime.Now.Date;
                ListCong_Doan_Cong_Nhans = new ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN>();
                Title = "Công việc đang thực hiện";
                LoadCongDoanCongNhan = new Command(async () => await LoadCongDoanCongNhanExcute());
                StopAllCommand = new Command(async () => await StopAllExcute());
                StartAllCommand = new Command(async () => await StartAllExcute());
                DieuChuyenCommand = new Command(async () => await DieuChuyenExcute());
            }
            catch (Exception ex)
            {

                 new MessageBox("Thông báo", ex.ToString()).Show();
            }
           
        }

        private async  Task DieuChuyenExcute()
        {
            try
            {
                if (_selectCongDoan != null)
                {
                    if (_selectCongDoan.TRANG_THAI < 2) //trạng thái mới hoặc đang kích hoạt
                    {
                        var result = await new Dieu_Chuyen_Cong_Viec(_selectCongDoan).Show();
                        if (result != null) // dòng đc thêm vào
                        {
                            //kết thúc dòng hiện tại
                            using (HttpClient client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(Config.URL);
                                ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN> _DieuChuyens = new ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN>();
                                _selectCongDoan.TRANG_THAI = 2;
                                _DieuChuyens.Add(_selectCongDoan);
                                _DieuChuyens.Add(result);
                                var ok = client.PostAsJsonAsync("api/hgpt/put_Dieu_Chuyen_Cong_Nhan?nguoitao=" + Preferences.Get(Config.User, ""), _DieuChuyens);
                                if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                                {
                                    LoadCongDoanCongNhan.Execute(null);
                                    DependencyService.Get<IMessage>().ShortAlert("điều chuyển thành công");
                                }
                                else
                                {
                                    _selectCongDoan.TRANG_THAI = 1;
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
              
        }

        private async  Task StartAllExcute()
        {
            try
            {
                if (ListCong_Doan_Cong_Nhans.Count > 0)
                {                    
                    bool cothaydoi = false;
                    foreach (LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN item in ListCong_Doan_Cong_Nhans)
                    {
                        if (item.TRANG_THAI == 0)
                        {
                            cothaydoi = true;
                            break;
                        }
                    }
                    if (cothaydoi == true )
                    {
                        var OK = await new MessageYesNo("Thông báo", "Bạn có muốn bắt đầu tất cả công việc không?").Show();
                        if (OK == DialogReturn.OK)
                        {                           
                            using (HttpClient client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(Config.URL);
                                var ok = client.PostAsJsonAsync("api/hgpt/put_Tat_Ca_Bat_Dau_Cong_Viec?nguoitao=" + Preferences.Get(Config.User, ""), ListCong_Doan_Cong_Nhans);
                                if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                                {                                   
                                    LoadCongDoanCongNhan.Execute(null);
                                    DependencyService.Get<IMessage>().ShortAlert("kích hoạt thành công");
                                }
                                else
                                {
                                    await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                                }
                                client.Dispose();
                            }    
                         
                            

                        }
                    }   
                    else
                    {
                        await new MessageBox("Thông báo", "Bạn đã bắt đầu công việc hết rồi.!").Show();
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

        private async Task StopAllExcute()
        {
            try
            {
                if (ListCong_Doan_Cong_Nhans.Count > 0)
                {                  
                    ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN> listKetthuc = new ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN>();
                    foreach (LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN item in ListCong_Doan_Cong_Nhans )
                    {
                        if (item.TRANG_THAI == 1 && item.UnCheck != 1 )
                        {
                            listKetthuc.Add(item);                            
                        }
                    }    
                    if (listKetthuc.Count  > 0)
                    {
                        var OK = await new MessageYesNo("Thông báo", "Bạn có muốn kết thúc tất cả công việc không?").Show();
                        if (OK == DialogReturn.OK)
                        {
                           
                            using (HttpClient client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(Config.URL);
                                var ok = client.PostAsJsonAsync("api/hgpt/put_Ket_Thuc_Tat_Ca_Cong_Viec?nguoitao=" + Preferences.Get(Config.User, ""), listKetthuc);
                                if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                                {                                  
                                    LoadCongDoanCongNhan.Execute(null);
                                    DependencyService.Get<IMessage>().ShortAlert("kết thúc thành công");
                                }
                                else
                                {
                                    await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                                }
                                client.Dispose();
                            }    
                               

                        }
                    }
                    else
                    {
                        await new MessageBox("Thông báo", "Bạn đã kết thúc công việc hết rồi.!").Show();
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

        private async  Task LoadCongDoanCongNhanExcute()
        {
            try
            {              
                IsRunning = true;
                ListCong_Doan_Cong_Nhans.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/hgpt/get_CongNhan_Theo_Cong_Doan_Theo_NHA_MAY?nhamay=" + Preferences.Get(Config.NhaMay,"")).Result;
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
                IsRunning = false;
            }
        }
    }
}
