using HGPT_APP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HGPT_APP.ViewModels;
using ZXing.Net.Mobile.Forms;
using HGPT_APP.Global;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using HGPT_APP.Popup;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Danh_Muc_Thiet_Bi : ContentPage
    {
        Danh_Muc_Thiet_Bi_ViewModel viewModel;
        string filterText = "";

        public Danh_Muc_Thiet_Bi()
        {
            InitializeComponent();
            BindingContext = viewModel = new Danh_Muc_Thiet_Bi_ViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (IsBusy == true) return;
            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void btnGuiYeuCau_Clicked(object sender, EventArgs e)
        {
            DanhMuc_ThietBi item = listThietBi.SelectedItem as DanhMuc_ThietBi;
            if (item != null)
            {
                await Navigation.PushAsync(new ThongTinThietBi(item));
            }

        }
        public bool FilterRecords(object o)
        {

            var item = o as DanhMuc_ThietBi;

            if (item != null)
            {

                if (item.No_2.ToLower().Contains(filterText) || item.No_3.ToLower().Contains(filterText) || item.NameVN.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listThietBi.View.Filter = FilterRecords;
            listThietBi.View.RefreshFilter();
        }

        private async void btnScan_Clicked(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            scan.Title = "Tìm kiếm thiết bị";
            Shell.SetTabBarIsVisible(scan, false);
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    //show form lên 
                    var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getTimKiemThietBi?mathietbi=" + result.Text).Result;
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string ok = _json.Substring(from, to - from + 1);
                        ObservableCollection<DanhMuc_ThietBi> Item = JsonConvert.DeserializeObject<ObservableCollection<DanhMuc_ThietBi>>(ok);
                        await Navigation.PushAsync(new ThongTinThietBi(Item[0]));
                    }
                    else
                    {
                        await new MessageBox("Thông báo", "Không tìm thấy kết quả").Show();
                    }

                });

            };
        }
    }
}
