using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using HGPT_APP.Global;
using Newtonsoft.Json;
using HGPT_APP.Popup;
using HGPT_APP.Models.GiamSat;
using HGPT_APP.Views.GiamSat;

namespace HGPT_APP.ViewModels.GiamSat
{
  public class DanhSachCongTrinhDuocLapDatTrongNgay_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"              
        public INavigation navigation { get; set; }       
             
       ObservableCollection <DanhSachCongTrinhDuocLapDatTrongNgay_Model> _listDanhSachCongTrinh;
        public ObservableCollection<DanhSachCongTrinhDuocLapDatTrongNgay_Model> ListDanhSachCongTrinh { 
            get => _listDanhSachCongTrinh;
            set
            {
                SetProperty(ref _listDanhSachCongTrinh, value);
            }
        }               
        #endregion

        

        public DanhSachCongTrinhDuocLapDatTrongNgay_ViewModel()  
        {
            try
            {
                ListDanhSachCongTrinh = new ObservableCollection<DanhSachCongTrinhDuocLapDatTrongNgay_Model>();                          
                Title = "Danh sách công trình được lắp đặt";
                LoadData(null);
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
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
                ListDanhSachCongTrinh.Clear();
                var a = await RunHttpClientGet<DanhSachCongTrinhDuocLapDatTrongNgay_Model>("DanhSachCongTrinhDuocLapDatTrongNgay");
                ListDanhSachCongTrinh = a.Lists;                
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
