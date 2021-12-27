using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using HGPT_APP.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public partial class Lich_Su_Phan_Chia_Cong_Viec : ContentPage
    {
        Lich_Su_Phan_Chia_Cong_Viec_ViewModel viewModel;
        LICH_SU_PHAN_CONG_VIEC _selectItem;
        string filterText;
        public Lich_Su_Phan_Chia_Cong_Viec()
        {
            InitializeComponent();
            BindingContext = viewModel = new Lich_Su_Phan_Chia_Cong_Viec_ViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.IsBusy == true) return;
            if (viewModel.ListCong_Doan_Cong_Nhans.Count == 0)
            {
                viewModel.LoadCongDoanCongNhan.Execute(DateTime.Now.Date );
            }
        }

        private async void listCongDoanCongNhan_SwipeEnded(object sender, Syncfusion.SfDataGrid.XForms.SwipeEndedEventArgs e)
        {
            try
            {
                _selectItem = e.RowData as LICH_SU_PHAN_CONG_VIEC;
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
            }
           
        }

        private async void Edit_Tapped(object sender, EventArgs e)
        {
            try
            {
                var result = await new Sua_Lich_Su_Cong_Doan(_selectItem).Show();
                if (result != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        var ok = client.PostAsJsonAsync("api/hgpt/update_lich_su_cong_doan?nguoitao=" + Preferences.Get(Config.User, ""), result);
                        if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                        {
                            viewModel.ListCong_Doan_Cong_Nhans.Remove(_selectItem);
                            viewModel.ListCong_Doan_Cong_Nhans.Add(result);
                            DependencyService.Get<IMessage>().ShortAlert("đã cập nhật");
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
                await new MessageBox("Thông báo", ex.Message).Show();

            }

        }

        private void listCongDoanCongNhan_GridTapped(object sender, Syncfusion.SfDataGrid.XForms.GridTappedEventArgs e)
        {
            
        }

        private async void delete_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ask = await new MessageYesNo("Thông báo", "Bạn có muốn xóa không").Show();
                if (ask == Global.DialogReturn.OK)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        var ok = client.PostAsJsonAsync("api/hgpt/delete_Lich_Su_Cong_Doan?nguoitao=" + Preferences.Get(Config.User, ""), _selectItem);

                        if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                        {
                            viewModel.ListCong_Doan_Cong_Nhans.Remove(_selectItem);
                            DependencyService.Get<IMessage>().ShortAlert("đã xóa");
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

                await new MessageBox("Thông báo", ex.Message).Show();

            }

        }

      
        public bool FilterRecords(object o)
        {

            var item = o as LICH_SU_PHAN_CONG_VIEC;

            if (item != null)
            {

                if (item.ExternalDocumentNo_.ToLower().Contains(filterText) || item.TEN_NHAN_VIEN.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
       

        private void search_DateSelected(object sender, DateChangedEventArgs e)
        {            
            viewModel.LoadCongDoanCongNhan.Execute(e.NewDate);

        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listCongDoanCongNhan.View.Filter = FilterRecords;
            listCongDoanCongNhan.View.RefreshFilter();
        }

        private void searchTenNV_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}