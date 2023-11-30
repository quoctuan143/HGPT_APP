using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using HGPT_APP.Models;
using Xamarin.Forms;
using HGPT_APP.Global;
using Xamarin.Essentials;
using Newtonsoft.Json;
using HGPT_APP.Popup;
using HGPT_APP.Models.GiamSat;
using System.Net.Http;
using System.Threading;
using NativeMedia;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Bson;
using System.Net.Http.Headers;

namespace HGPT_APP.ViewModels.GiamSat
{
  public class ThemNhanSuChoGiamSat_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"      
        
        public INavigation navigation { get; set; }       
        Employee _selectEmployee;
        public  Employee SelectEmployee { get => _selectEmployee; set => SetProperty(ref _selectEmployee, value); }
        ObservableCollection< Employee> _employeeList;
        public ObservableCollection<Employee> ListEmployee { get => _employeeList; set => SetProperty(ref _employeeList, value); }

        ObservableCollection<Employee> _nhanSuGiamSat;
        public ObservableCollection<Employee> ListNhanSuGiamSat { get => _nhanSuGiamSat; set => SetProperty(ref _nhanSuGiamSat, value); }
        #endregion

        #region "Commnad"
        public Command LoadCongDoanBaoCao { get; set; }
        public Command GuiBaoCao { get; set; }
        public Command AddNhanSu { get; set; }
        public Command DeleteNhanSu { get; set; }

        #endregion

        public ThemNhanSuChoGiamSat_ViewModel()
        {
            try
            {                   
                Title = "Quản lý nhân sự";
                ListEmployee = new ObservableCollection<Employee >();
                ListNhanSuGiamSat = new ObservableCollection<Employee>();
                LoadCongDoanBaoCao = new Command(async () => await LoadCongDoanBaoCaoExcute());
                AddNhanSu = new Command(async () => await AddNhanSuClick());
                DeleteNhanSu = new Command(DeleteNhanSuClick); 
                GuiBaoCao = new Command(async () => await GuiBaoCaoClick());   
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }
          
        }

        private async  void DeleteNhanSuClick(object obj)
        {
            try
            {
                var ok = await new MessageYesNo("Thông báo", "Bạn có muốn xóa nhân sự này không?").Show();
                if (ok == DialogReturn.OK)
                {
                    Employee e = obj as Employee;
                    ListNhanSuGiamSat.Remove(e);
                }
                   
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
                return;
            }
        }

        private async Task AddNhanSuClick()
        {
            try
            {
                if (_selectEmployee == null )
                {
                    await new MessageBox("Thông báo", "Vui lòng chọn nhân sự muốn thêm").Show();
                    return;
                }    
                if (ListNhanSuGiamSat.Contains(_selectEmployee ))
                {
                    await new MessageBox("Thông báo", "Nhân sự này đã có trong danh sách rồi. không tạo thêm được nữa").Show();
                    return;
                }
                ListNhanSuGiamSat.Add(_selectEmployee);
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message ).Show();
                return;
            }
        }

        private async Task GuiBaoCaoClick()
        {
            try
            {    
                var ok = await new MessageYesNo("Thông báo", "Bạn có muốn cập nhật nhân sự để quản lý không?").Show();
                if (ok == DialogReturn.OK )
                {                  

                    ShowLoading("Đang xử lý vui lòng đợi");
                    await Task.Delay(1000);
                    HttpResponseMessage ok1 = null;
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        var content = new StringContent(JsonConvert.SerializeObject(ListNhanSuGiamSat), Encoding.UTF8, "application/json");                       
                        ok1 = await client.PostAsync($"CaphatNhanSuGiamSat?NguoiGiamSat={Preferences.Get(Config.User, "")}", content);
                        if (ok1.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            HideLoading();
                            ShortAlert("Đã cập nhật nhân sự thành công");
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
     
        private async Task LoadCongDoanBaoCaoExcute()
        {
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                //get danh sách công trình
               
                var _json = Config.client.GetStringAsync(Config.URL + $"XemToanBoNhanVien?giamsat={Preferences.Get(Config.User ,"")}").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "[]")
                {              
                    string result = _json.Substring(1, _json.Length - 2);
                    ListEmployee = JsonConvert.DeserializeObject<ObservableCollection< Employee  >>(result);                    
                }
                 _json = Config.client.GetStringAsync(Config.URL + $"XemNhanSuTheoGiamSat?giamsat={Preferences.Get(Config.User ,"")}").Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json != "[]")
                {
                    string result = _json.Substring(1, _json.Length - 2);
                    ListNhanSuGiamSat = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(result);
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
}
