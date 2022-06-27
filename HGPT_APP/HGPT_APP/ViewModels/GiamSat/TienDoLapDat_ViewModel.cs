using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using HGPT_APP.Global;
using Xamarin.Essentials;
using Newtonsoft.Json;
using HGPT_APP.Popup;
using HGPT_APP.Models.GiamSat;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;

namespace HGPT_APP.ViewModels.GiamSat
{
  public class TienDoLapDat_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"              
        public INavigation navigation { get; set; }
        DateTime _ngaylamviec;
        public DateTime NgayLamViec { get => _ngaylamviec; set => SetProperty(ref _ngaylamviec, value); }             
        DanhSachCongTrinh _selectCongTrinh;
        public DanhSachCongTrinh SelectCongTrinh { get => _selectCongTrinh; 
            set 
            { 
                SetProperty(ref _selectCongTrinh, value);
                HienThiThongTin();
            } 
        }      
       ObservableCollection < TienDoLapDat> _tiendolapdat;
        public ObservableCollection<TienDoLapDat> ListTienDoLapDat
        {
            get => _tiendolapdat;
            set
            {
                if (value != null)
                {
                    _tiendolapdat = value;
                    OnPropertyChanged("ListTienDoLapDat");
                }
            }
        }       

        ObservableCollection<NhomBaoCaoTienDo > _nhomBaoCao;
        public ObservableCollection <NhomBaoCaoTienDo > ListNhomBaoCao { get => _nhomBaoCao; set => SetProperty(ref _nhomBaoCao, value); }

        NhomBaoCaoTienDo _selectBaoCao;
        public NhomBaoCaoTienDo SelectNhomBaoCao { get => _selectBaoCao; set
            {
                SetProperty(ref _selectBaoCao, value);
                Title = value.Name;
                HienThiThongTin();
            }
            }
        #endregion

        #region "Commnad"
        public Command GuiBaoCao { get; set; }    
        public Command SelectAllCommand { get; set; }
        #endregion

        public TienDoLapDat_ViewModel()
        {
            try
            {
                NgayLamViec = DateTime.Now.Date;                
                Title = "Báo cáo tiến độ lắp đặt";
                ListTienDoLapDat = new ObservableCollection<TienDoLapDat >();
                ListNhomBaoCao = new ObservableCollection<NhomBaoCaoTienDo>
                {
                    new NhomBaoCaoTienDo {Value="HaHang", Name ="Hạ Hàng"},
                    new NhomBaoCaoTienDo {Value="NghiemThuCongTrinh", Name ="Nghiệm thu tại công trình"},
                    new NhomBaoCaoTienDo {Value="LapKetCau", Name ="Lắp kết cấu"},
                    new NhomBaoCaoTienDo {Value="HoanThien", Name ="Hoàn thiện"},
                    new NhomBaoCaoTienDo {Value="NghiemThuBanGiao", Name ="Nghiệm thu bàn giao"}
                };
                GuiBaoCao = new Command(async () => await GuiBaoCaoClick());
                SelectAllCommand = new Command(ChonTatCaClick);
               
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }
          
        }

        private void ChonTatCaClick(object obj)
        {
            try
            {
                TienDoLapDat item = obj as TienDoLapDat;

                foreach (TienDoLapDat td in ListTienDoLapDat)
                {
                    if (td.OrderCode.StartsWith(item.OrderCode))
                        if (item.ValueCha == true)
                        {
                            td.SoLuongCanLap = td.Quantity - td.SoLuongLapDat;
                        }
                        else
                        {
                                td.SoLuongCanLap = 0;
                        }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task GuiBaoCaoClick()
        {
            try
            {
                if (_selectCongTrinh == null )
                {
                    await new MessageBox("Thông báo", "Vui lòng chọn công trình").Show();
                    return;
                }
                if (_selectBaoCao == null)
                {
                    await new MessageBox("Thông báo", "Vui lòng chọn nhóm báo cáo").Show();
                    return;
                }
                if (ListTienDoLapDat.Where(x=> x.SoLuongCanLap > 0).Count() == 0)
                {
                    await new MessageBox("Thông báo", "Vui lòng nhập số lượng để báo cáo").Show();
                    return;
                }
                
                var ok = await new MessageYesNo("Thông báo", $"Bạn có muốn cập nhật {_selectBaoCao.Name} không?").Show();
                if (ok == DialogReturn.OK )
                {                
                    ShowLoading("Đang xử lý vui lòng đợi");
                    await Task.Delay(1000);
                    HttpResponseMessage ok1 = null;
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);   
                        ok1 = await client.PostAsJsonAsync($"CaphatTienDoLapDatCongTrinh?NguoiGiamSat={Preferences.Get(Config.User ,"")}&MaCongTrinh={_selectCongTrinh.Code}&ngaylapdat={ string.Format("{0:yyyy-MM-dd}",NgayLamViec )}&loaitiendo={_selectBaoCao.Value }", ListTienDoLapDat);
                        if (ok1.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            HideLoading();
                            ShortAlert("Đã cập nhật báo cáo thành công");
                            await navigation.PopAsync();
                        }
                        else
                        {
                            HideLoading();
                            await new MessageBox("Thông báo", ok1.Content.ReadAsStringAsync().Result).Show();
                            return;
                        }

                    }

                }
            }
            catch (Exception ex)            
            {
                HideLoading();
                await new MessageBox("Thông báo",ex.Message ).Show();
                return;
            }           
        }

        public override async Task LoadData(object ojb)
        {
           
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                //get danh sách công trình
                GetCongTrinh("1");                
                HideLoading();
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

        public async void HienThiThongTin ()
        {
            try
            {
                if (_selectCongTrinh == null  )
                {
                    await new MessageBox("Thông báo", "Vui lòng chọn công trình báo cáo").Show();
                    return;
                }
                if (_selectBaoCao == null)
                {
                    await new MessageBox("Thông báo", "Vui lòng chọn công đoạn báo cáo").Show();
                    return;
                }
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                //get danh sách công trình
                var _json = Config.client.GetStringAsync(Config.URL + $"BaoCaoTienDoLapDatTaiCongTrinh?macongtrinh={(_selectCongTrinh == null ? "abc" : _selectCongTrinh.Code)}&loaitiendo={ (_selectBaoCao == null ? "abc" : _selectBaoCao.Value  ) }").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "[]")
                {
                    string result = _json.Substring(1, _json.Length - 2);
                    ListTienDoLapDat = JsonConvert.DeserializeObject<ObservableCollection<TienDoLapDat>>(result);
                }
                HideLoading();
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }
    }

    public class NhomBaoCaoTienDo
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
