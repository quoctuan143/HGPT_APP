using HGPT_APP.Global;
using HGPT_APP.Models;
using HGPT_APP.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HGPT_APP.ViewModels
{
    public class KeHoachBaoTriViewModel : BaseViewModel
    {


        ObservableCollection<Models.KeHoachBaoTri> _kehoachbaotri;
        public ObservableCollection<Models.KeHoachBaoTri> KE_HOACH_BAO_TRI
        {
            get => _kehoachbaotri;
            set
            {
                _kehoachbaotri = value;
                OnPropertyChanged(nameof(KE_HOACH_BAO_TRI));
            }
        }
        public Command<string> LoadKeHoachBaoTri { get; set; }
        public KeHoachBaoTriViewModel()
        {


            Title = "Kế hoạch bảo trì";
            KE_HOACH_BAO_TRI = new ObservableCollection<Models.KeHoachBaoTri>();
            LoadKeHoachBaoTri = new Command<string>(async (p) => await ExcuteKeHoachBaoTri(p));
            MessagingCenter.Subscribe<TaoLichSuBaoTri, LICH_SU_BAO_TRI>(this, "AddLichSuBaoTri", (ojb, lsu) => {
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            if (lsu != null)
                            {

                                foreach (Models.KeHoachBaoTri kh in KE_HOACH_BAO_TRI)
                                {
                                    if (kh.No_ == lsu.MA_THIET_BI && kh.Thang == lsu.THANG && kh.Code == lsu.NHOM_BAO_TRI)
                                    {
                                        kh.Da_Bao_Tri = true;
                                        OnPropertyChanged(nameof(KE_HOACH_BAO_TRI));
                                    }
                                }
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

        async Task ExcuteKeHoachBaoTri(string year)
        {

            if (year == null) year = DateTime.Now.Year.ToString();
            IsBusy = true;
            IsRunning = true;
            try
            {
                KE_HOACH_BAO_TRI.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getKeHoachBaoTri?manhamay=" + Preferences.Get(Config.NhaMay, "") + "&nam=" + year +"&user=" + Preferences.Get(Config.User, "")).Result;
                await Task.Delay(1000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    KE_HOACH_BAO_TRI = JsonConvert.DeserializeObject<ObservableCollection<Models.KeHoachBaoTri>>(result);
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
