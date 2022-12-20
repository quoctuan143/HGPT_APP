using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using HGPT_APP.ViewModels;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KeHoachBaoTriPage : ContentPage
    {
        KeHoachBaoTriViewModel viewModel;
        public KeHoachBaoTriPage()
        {
            InitializeComponent();
            listThietBi.ItemsSourceChanged += ListThietBi_ItemsSourceChanged;
            listThietBi.QueryRowHeight += ListThietBi_QueryRowHeight;
            entryNam.Text = DateTime.Now.Year.ToString();
            BindingContext = viewModel = new KeHoachBaoTriViewModel();

        }

        private void ListThietBi_QueryRowHeight(object sender, Syncfusion.SfDataGrid.XForms.QueryRowHeightEventArgs e)
        {
            if (e.RowIndex != 0)
            {
                //Calculates and sets height of the row based on its content 
                e.Height = listThietBi.GetRowHeight(e.RowIndex);
                e.Handled = true;
            }
        }

        private void ListThietBi_ItemsSourceChanged(object sender, Syncfusion.SfDataGrid.XForms.GridItemsSourceChangedEventArgs e)
        {
            try
            {
                entryThang.SelectedIndex = DateTime.Now.Month;
                listThietBi.View.Filter = FilterRecords;
                listThietBi.View.RefreshFilter();
            }
            catch (Exception)
            {


            }

        }

        private void Nam_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                if (IsBusy == true) return;
                if (viewModel.KE_HOACH_BAO_TRI.Count == 0)
                    viewModel.LoadKeHoachBaoTri.Execute(DateTime.Now.Year.ToString());


            }
            catch (Exception ex)
            {

                new MessageBox("Lỗi", ex.Message).Show();
            }


        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
               KeHoachBaoTri item = listThietBi.SelectedItem as KeHoachBaoTri;
                if (item != null)
                {
                    if (item.Da_Bao_Tri == true)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Kế hoạch này đã được bảo trì rồi!");
                        return;
                    }
                    await Navigation.PushAsync(new TaoLichSuBaoTri(item));
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Vui lòng chọn kế hoạch");
                }
            }
            catch (Exception)
            {


            }


        }
        string filtertext;
        Boolean filter;
        int thang;
        public bool FilterRecords(object o)
        {
            try
            {
                var item = o as KeHoachBaoTri;
                thang = entryThang.SelectedIndex;
                if (item != null)
                {


                    if (radChuaBaoTri.IsChecked == true)
                    {

                        filter = false;

                    }
                    else if (radDaBaoTri.IsChecked == true)
                    {
                        filter = true;
                    }

                    if (thang == 0)
                    {
                        if (radTatCa.IsChecked == true)
                            return true;
                        else if (item.Da_Bao_Tri == filter) return true;
                    }
                    else if (radTatCa.IsChecked == true)
                    {
                        if (item.Thang == thang) return true;
                    }
                    else
                    {
                        if (item.Da_Bao_Tri == filter && item.Thang == thang) return true;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        public bool FilterRecords1(object o)
        {

            var item = o as KeHoachBaoTri;
            thang = entryThang.SelectedIndex;
            if (item != null)
            {
                if ((item.No_2 .ToLower().Contains(filtertext.ToLower()) || item.No_3.ToLower().Contains(filtertext.ToLower()) || item.NameVN.ToLower().Contains(filtertext.ToLower()) || item.No_.ToLower().Contains(filtertext.ToLower())) && item.Thang == thang)
                    return true;
            }
            return false;
        }
        private void SfRadioGroup_CheckedChanged(object sender, Syncfusion.XForms.Buttons.CheckedChangedEventArgs e)
        {
            try
            {
                listThietBi.View.Filter = FilterRecords;
                listThietBi.View.RefreshFilter();
            }
            catch (Exception ex)
            {


            }

        }

        private void entryNam_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void entryThang_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            listThietBi.View.Filter = FilterRecords;
            listThietBi.View.RefreshFilter();
        }

        private void entryNam_Unfocused(object sender, FocusEventArgs e)
        {
            if (viewModel != null)
            {
                viewModel.LoadKeHoachBaoTri.Execute(entryNam.Text);
            }
        }

        private void entryMaTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            filtertext = e.NewTextValue;
            listThietBi.View.Filter = FilterRecords1;
            listThietBi.View.RefreshFilter();
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                var scan = new ZXingScannerPage();
                await Navigation.PushAsync(scan);
                scan.OnScanResult += (result) =>
                {
                    Device.BeginInvokeOnMainThread(async () => {
                        await Navigation.PopAsync();
                        //show form lên 
                        string ma = result.Text.Split('=')[1];
                        entryMaTB.Text = ma;

                    });

                };
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
            }
        }

        private async void btnViewLichSu_Tapped(object sender, EventArgs e)
        {
            if (_selectItem != null)
            {
                await Navigation.PushAsync(new Lich_Su_Bao_Tri_Chi_Tiet(_selectItem.No_));
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Không có lịch sử");
            }
        }

        KeHoachBaoTri _selectItem { get; set; }
        private void listThietBi_SwipeEnded(object sender, Syncfusion.SfDataGrid.XForms.SwipeEndedEventArgs e)
        {
            _selectItem = e.RowData as KeHoachBaoTri;
        }
    }
}