using HGPT_APP.Global;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using Newtonsoft.Json;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lich_Su_Bao_Tri_Chi_Tiet : ContentPage, INotifyPropertyChanged
    {
        public string mathietbi;

        ObservableCollection<LICH_SU_BAO_TRI> _lichsu;
        public ObservableCollection<LICH_SU_BAO_TRI> LichSus { get => _lichsu; set { _lichsu = value; OnPropertyChanged("LichSus"); } }
        bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; OnPropertyChanged("IsRunning"); }
        }

        public Command LoadLichSuBaoTri { get; set; }
        public Lich_Su_Bao_Tri_Chi_Tiet(string _mathietbi)
        {
            InitializeComponent();
            mathietbi = _mathietbi;            
            listThietBi.QueryRowHeight += DataGrid_QueryRowHeight;
            LichSus = new ObservableCollection<LICH_SU_BAO_TRI>();
            LoadLichSuBaoTri = new Command(async () => await ExcuteLoadLichSuBaoTri());
            BindingContext = this;
        }
        private void DataGrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            try
            {
                if (e.RowIndex != 0)
                {
                    //Calculates and sets height of the row based on its content 
                    e.Height = listThietBi.GetRowHeight(e.RowIndex);
                    e.Handled = true;
                }
            }
            catch (Exception)
            {


            }

        }

        async Task ExcuteLoadLichSuBaoTri()
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                LichSus.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getLichSuBaoTri?mathietbi=" + mathietbi).Result;
                //  await Task.Delay(3000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    LichSus = JsonConvert.DeserializeObject<ObservableCollection<LICH_SU_BAO_TRI>>(result);
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (LichSus.Count == 0)
                LoadLichSuBaoTri.Execute(null);
        }

        private async void listThietBi_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            LICH_SU_BAO_TRI item = listThietBi.SelectedItem as LICH_SU_BAO_TRI;
            if (item != null)
            {
                if (item.IMAGE_LINK != "http://erp.hoatho.com.vn/ObjectSpace/")
                    await new ShowImage(item.IMAGE_LINK).Show();
            }
        }
    }
}