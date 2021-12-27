using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using Newtonsoft.Json;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LichSuBaoTri : ContentPage, INotifyPropertyChanged
    {
        string filterText = "";
        ObservableCollection<LICH_SU_BAO_TRI> _lichsu;
        public ObservableCollection<LICH_SU_BAO_TRI> LichSus { get => _lichsu; set { _lichsu = value; OnPropertyChanged("LichSus"); } }
        bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; OnPropertyChanged("IsRunning"); }
        }

        public Command LoadLichSuBaoTri { get; set; }
        public LichSuBaoTri()
        {
            InitializeComponent();
            listThietBi.QueryRowHeight += DataGrid_QueryRowHeight;
            LichSus = new ObservableCollection<LICH_SU_BAO_TRI>();
            LoadLichSuBaoTri = new Command(async () => await ExcuteLoadLichSuBaoTri());
            BindingContext = this;

        }

        private void DataGrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            if (e.RowIndex != 0)
            {
                //Calculates and sets height of the row based on its content 
                e.Height = listThietBi.GetRowHeight(e.RowIndex);
                e.Handled = true;
            }
        }

        async Task ExcuteLoadLichSuBaoTri()
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                LichSus.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getLichSuBaoTri_NhaMay?nhamay=" + Preferences.Get(Config.NhaMay, "")).Result;
                await Task.Delay(1000);
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



        private async void listThietBi_SelectionChanged(object sender, Syncfusion.SfDataGrid.XForms.GridSelectionChangedEventArgs e)
        {
            LICH_SU_BAO_TRI item = listThietBi.SelectedItem as LICH_SU_BAO_TRI;
            if (item != null)
            {
                if (item.IMAGE_LINK != "http://erp.hoatho.com.vn/ObjectSpace/")
                    await new ShowImage(item.IMAGE_LINK).Show();
            }
        }
        public bool FilterRecords(object o)
        {
            try
            {
                var item = o as LICH_SU_BAO_TRI;

                if (item != null)
                {

                    if (item.SERIAL.ToLower().Contains(filterText) || item.MODEL.ToLower().Contains(filterText) || item.TEN_THIET_BI.ToLower().Contains(filterText))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listThietBi.View.Filter = FilterRecords;
            listThietBi.View.RefreshFilter();
        }
        LICH_SU_BAO_TRI _selectItem { get; set; }
        private void listThietBi_SwipeEnded(object sender, Syncfusion.SfDataGrid.XForms.SwipeEndedEventArgs e)
        {
            _selectItem = e.RowData as LICH_SU_BAO_TRI;
        }
        private async void btnViewLichSu_Tapped(object sender, EventArgs e)
        {
            try
            {
              
                if (_selectItem != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        var ok = client.PostAsJsonAsync("api/qltb/DeleteLichSuBaoTri" , _selectItem);
                        if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("cập nhật thành công"))
                        {
                            LichSus.Remove(_selectItem);
                            OnPropertyChanged("LichSus");
                            DependencyService.Get<IMessage>().ShortAlert("Đã xóa");
                        }
                        else
                        {
                            await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                        }
                        client.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {


            }
        }
    }
}