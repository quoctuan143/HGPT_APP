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
  public class TheoDoiGiamSat_ViewModel : BaseViewModel
    {

        #region "Khai báo biến"              
        public INavigation navigation { get; set; }
        public DateTime Thang { get; set; }    
        public string NgayXem  { get; set; }    
       ObservableCollection <TheoDoiGiamSat_Model> _listDanhSachCongTrinh;
        public ObservableCollection<TheoDoiGiamSat_Model> ListDanhSachCongTrinh { 
            get => _listDanhSachCongTrinh;
            set
            {
                SetProperty(ref _listDanhSachCongTrinh, value);
            }
        }              
        public Command OkCommand {get; set; }   
        #endregion

        

        public TheoDoiGiamSat_ViewModel()  
        {
            try
            {
                Thang = DateTime.Now.Date;
                ListDanhSachCongTrinh = new ObservableCollection<TheoDoiGiamSat_Model>();                          
                Title = "Theo dõi giám sát";
                LoadData(null);
                OkCommand = new Command(async () => {
                    await LoadData(Thang);
                });
            }
            catch (Exception ex)
            {

                 Task.Run(async ()   => await  new  MessageBox("Thông báo", ex.ToString()).Show());
            }
          
        }

        public override async Task LoadData(object ojb)
        {
            NgayXem = string.Format("{0:MM/yyyy}", Thang);
            OnPropertyChanged(nameof(NgayXem));
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                IsRunning = true;
                ShowLoading("Đang tải dữ liệu");
                await Task.Delay(1000);
                ListDanhSachCongTrinh.Clear();
                var a = await RunHttpClientGet<TheoDoiGiamSat_Model>($"TheoDoiGiamSat?thang={string.Format("{0:yyyy-MM-dd}",Thang)}");
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
