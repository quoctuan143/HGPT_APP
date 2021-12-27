using HGPT_APP.Global;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using HGPT_APP.Views.SinhNhatKhachHang;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HGPT_APP.ViewModels.SinhNhatKhachHang
{
  public  class SinhNhatKhachHang_ChuaXuLyViewModel : BaseViewModel
    {
       
        ObservableCollection<ChamSocKhachHang> _listDanhSach;
        public ObservableCollection<ChamSocKhachHang> ListDanhSach { get => _listDanhSach; set { _listDanhSach = value; OnPropertyChanged(nameof(ListDanhSach)); } }

        ChamSocKhachHang _itemSelected;
        public ChamSocKhachHang ItemSelected { get => _itemSelected; set { _itemSelected = value; OnPropertyChanged(nameof(ItemSelected)); } }

        public Command LoadItemsCommand { get; set; }
        public Command CallCommand { get; set; }
        public SinhNhatKhachHang_ChuaXuLyViewModel()
        {
            ListDanhSach = new ObservableCollection<ChamSocKhachHang>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CallCommand = new Command(OnCallClicked );
            MessagingCenter.Subscribe<NoiDungChamSocKhachHang, ChamSocKhachHang>(this, "xulysinhnhatkhachhang", (ojb, item) => {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        if (item != null)
                        {
                            ListDanhSach.Remove(item);
                            OnPropertyChanged(nameof(ListDanhSach));
                        }
                    }
                    catch
                    {


                    }

                });


            });
        }

        private void OnCallClicked(object obj)
        {
            try
            {
                ChamSocKhachHang kh = obj as ChamSocKhachHang;
                Xamarin.Essentials.PhoneDialer.Open(kh.PhoneNo_);
            }
            catch
            {

               
            }
           
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                ListDanhSach.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getChamSocKhachHangChuaHoanThanh").Result;
                await Task.Delay(1000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListDanhSach = JsonConvert.DeserializeObject<ObservableCollection<ChamSocKhachHang>>(result);
                }
                
            }
            catch (Exception ex)
            {
                await new MessageBox("Thông Báo",ex.Message ).Show();
            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

       
    }
}
