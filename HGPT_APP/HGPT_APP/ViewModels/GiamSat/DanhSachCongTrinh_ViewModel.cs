using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using HGPT_APP.Global;
using Newtonsoft.Json;
using HGPT_APP.Popup;
using HGPT_APP.Models.GiamSat;
using HGPT_APP.Views.GiamSat;
using System.Linq;

namespace HGPT_APP.ViewModels.GiamSat
{
  public class DanhSachCongTrinh_ViewModel : BaseViewModel
    {
        public Command StopCongTrinh { get; set; }        
        public DanhSachCongTrinh SelectCongTrinh { get; set; }  
        public DanhSachCongTrinh_ViewModel()  
        {
            try
            {                                    
                Title = "Danh sách công trình";
                StopCongTrinh = new Command(() => StopCongTrinhClicked());
                LoadData(null);
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }
          
        }

        private async void StopCongTrinhClicked()
        {
            try
            {
                if (SelectCongTrinh == null)
                {
                    await new MessageBox("Thông báo", "Vui lòng chọn công trình cần đóng").Show();
                    return;
                }
                var ask = await new MessageYesNo("Thông báo", "Bạn có muốn đóng công trình này không?").Show();
                if (ask == DialogReturn.OK)
                {
                    var result = await RunHttpClientPost("KetThucCongTrinh", SelectCongTrinh.Code);

                    if (result.IsSuccessStatusCode)
                    {
                        CongTrinhList.Remove(CongTrinhList.FirstOrDefault(x => x.Code == SelectCongTrinh.Code));
                    }
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.ToString()).Show();
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

       
    }
}
