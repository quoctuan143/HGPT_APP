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
  public class DanhSachCongTrinhHoatDongTrongNgay_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"              
        public INavigation navigation { get; set; }
        DateTime _ngaylamviec;
        public DateTime NgayLamViec
        {
            get => _ngaylamviec;
            set 
            { 
                SetProperty(ref _ngaylamviec, value);
                LoadData(_ngaylamviec);
            }
        }
             
       ObservableCollection <DanhSachCongTrinhHoatDongTrongNgay_Model> _listDanhSachCongTrinh;
        public ObservableCollection<DanhSachCongTrinhHoatDongTrongNgay_Model> ListDanhSachCongTrinh { 
            get => _listDanhSachCongTrinh;
            set
            {
                SetProperty(ref _listDanhSachCongTrinh, value);
            }
        }               
        #endregion

        

        public DanhSachCongTrinhHoatDongTrongNgay_ViewModel() 
        {
            try
            {
                ListDanhSachCongTrinh = new ObservableCollection<DanhSachCongTrinhHoatDongTrongNgay_Model>();
                NgayLamViec = DateTime.Now.Date;                
                Title = "Danh sách công trình hôm nay";               
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
                var a = await RunHttpClientGet<DanhSachCongTrinhHoatDongTrongNgay_Model>("DanhSachCongTrinhHoatDongTrongNgay?ngay=" + string.Format("{0:yyyy-MM-dd}",ojb ));
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
