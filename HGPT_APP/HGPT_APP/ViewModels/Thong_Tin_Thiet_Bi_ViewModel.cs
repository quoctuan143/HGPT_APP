using HGPT_APP.Global;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using HGPT_APP.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HGPT_APP.ViewModels
{
  public  class Thong_Tin_Thiet_Bi_ViewModel : BaseViewModel
    {        
            public DanhMuc_ThietBi Item { get; set; }        
        ObservableCollection<Danh_Muc_Phu_Tung_Model> _listPhuTung;
        public ObservableCollection<Danh_Muc_Phu_Tung_Model> ListPhuTung  { get => _listPhuTung; set { _listPhuTung = value; OnPropertyChanged("ListPhuTung"); } }
        ObservableCollection<LICH_SU_BAO_TRI> lichsubaotri;
        public ObservableCollection<LICH_SU_BAO_TRI> lICH_SU_BAO_TRIs { get => lichsubaotri; set { lichsubaotri = value; OnPropertyChanged("lICH_SU_BAO_TRIs"); } }
        ObservableCollection<QUY_TRINH_BAO_TRI> _quytrinhbaotri;
        public ObservableCollection<QUY_TRINH_BAO_TRI> QUY_TRINH_BAO_TRIs
         {
            get => _quytrinhbaotri;
            set
            {
                _quytrinhbaotri = value;  
                OnPropertyChanged(nameof(QUY_TRINH_BAO_TRIs));
            }
        }
         ObservableCollection<KeHoachBaoTri> _kehoachbaotris;


        public ObservableCollection<KeHoachBaoTri> KE_HOACH_BAO_TRIs
        {
            get => _kehoachbaotris;
            set
            {
                _kehoachbaotris = value;
                OnPropertyChanged(nameof(KE_HOACH_BAO_TRIs));
            }
        }
        Danh_Muc_Phu_Tung_Model _selectPhuTung;
        public Danh_Muc_Phu_Tung_Model SelectPhuTung { get => _selectPhuTung; set { _selectPhuTung = value; OnPropertyChanged("SelectPhuTung"); } }
        public Command<string> LoadKeHoachBaoTri { get; set; }
        public Command<string> LoadLichSuBaoTri { get; set; }
        public Command<string> LoadQuyTrinhBaoTri { get; set; }
        public Command<string> LoadPhuTUng { get; set; }
        public Thong_Tin_Thiet_Bi_ViewModel(DanhMuc_ThietBi _item) 
        {

            Item = _item;
            Title = Item.NameVN;
            KE_HOACH_BAO_TRIs = new ObservableCollection<KeHoachBaoTri>();
            ListPhuTung = new ObservableCollection<Danh_Muc_Phu_Tung_Model>();
            lICH_SU_BAO_TRIs = new ObservableCollection<LICH_SU_BAO_TRI>();
            QUY_TRINH_BAO_TRIs = new ObservableCollection<QUY_TRINH_BAO_TRI>();
            LoadLichSuBaoTri = new Command<string>(async (p) => await ExcuteLoadLichSuBaoTri(p));
            LoadQuyTrinhBaoTri = new Command<string>(async (p) => await ExcuteLoadQuyTrinhBaoTri(p));
            LoadKeHoachBaoTri = new Command<string>(async (p) => await ExcuteKeHoachBaoTri(p));
            LoadPhuTUng = new Command<string>(async (p) => await ExcuteLoadPhuTung(p)); 

            MessagingCenter.Subscribe<ThongTinThietBi_Edit, DanhMuc_ThietBi>(this, "UpdateThietBi", (ojb, tbi) => {
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            if (tbi != null)
                            {
                                Item = tbi;
                                OnPropertyChanged(nameof(Item));
                            }
                        }
                        catch (Exception)
                        {


                        }



                    });
                }
                catch (Exception)
                {

                }


            });

            MessagingCenter.Subscribe<Nhap_Phu_Tung, Danh_Muc_Phu_Tung_Model>(this, "UpdatePhuTung", (ojb, item) => {
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            if (item != null)
                            {
                               ExcuteLoadPhuTung(item.Ma_Thiet_Bi);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    });
                }
                catch (Exception)
                {

                }


            });
        }

        async  Task ExcuteLoadPhuTung(string p)
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                ListPhuTung.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getPhuTung?mathietbi=" + Item.No_).Result;
                //  await Task.Delay(3000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListPhuTung = JsonConvert.DeserializeObject<ObservableCollection<Danh_Muc_Phu_Tung_Model>>(result);
                }
                 
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

        async Task ExcuteLoadQuyTrinhBaoTri(string mathietbi)
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                QUY_TRINH_BAO_TRIs.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getQuyTrinhBaoTri?mathietbi=" + Item.No_).Result;
                //  await Task.Delay(3000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    QUY_TRINH_BAO_TRIs = JsonConvert.DeserializeObject<ObservableCollection<QUY_TRINH_BAO_TRI>>(result);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

        async Task ExcuteLoadLichSuBaoTri(string mathietbi)
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                lICH_SU_BAO_TRIs.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getLichSuBaoTri?mathietbi=" + Item.No_).Result;
                //  await Task.Delay(3000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    lICH_SU_BAO_TRIs = JsonConvert.DeserializeObject<ObservableCollection<LICH_SU_BAO_TRI>>(result);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

        async Task ExcuteKeHoachBaoTri(string year)
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                KE_HOACH_BAO_TRIs.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getKeHoachBaoTri_ThietBi?mathietbi=" + Item.No_ + "&nam=" + year).Result;
                // await Task.Delay(3000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    KE_HOACH_BAO_TRIs = JsonConvert.DeserializeObject<ObservableCollection<KeHoachBaoTri>>(result);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }
    }
}
