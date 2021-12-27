using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using HGPT_APP.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Phan_Chia_Cong_Viec : ContentPage
    {
        Phan_Chia_Cong_Viec_ViewModel viewModel;
        LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN _selectItem;
        string filterText;       
        public Phan_Chia_Cong_Viec()
        {
            InitializeComponent();
            BindingContext = viewModel = new Phan_Chia_Cong_Viec_ViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.IsBusy == true) return;
            if (viewModel.ListCong_Doan_Cong_Nhans.Count == 0)
            {
                viewModel.LoadCongDoanCongNhan.Execute(null);
            }    
        }

        private void listCongDoanCongNhan_SwipeEnded(object sender, Syncfusion.SfDataGrid.XForms.SwipeEndedEventArgs e)
        {           
            _selectItem = e.RowData as LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN;
        }        

        private async void Edit_Tapped(object sender, EventArgs e)
        {
            try
            {
                var result = await new Sua_Cong_Doan_Cong_Nhan(_selectItem).Show();
                if (result != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        var ok = client.PostAsJsonAsync("api/hgpt/update_cap_nhat_cong_doan?nguoitao=" + Preferences.Get(Config.User, ""), result);
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
            catch (Exception ex )
            {
                await new MessageBox("Thông báo", ex.Message ).Show();

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
                        var ok = client.PostAsJsonAsync("api/hgpt/delete_Cong_Doan_Cong_Nhan?nguoitao=" + Preferences.Get(Config.User, ""), _selectItem);

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

                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
             
        }

        private async void stop_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ask = await new MessageYesNo("Thông báo", "Bạn có muốn kết thúc không").Show();
                if (ask == Global.DialogReturn.OK)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        var ok = client.PostAsJsonAsync("api/hgpt/post_Ket_Thuc_Cong_Viec?nguoitao=" + Preferences.Get(Config.User, ""), _selectItem);

                        if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                        {
                            viewModel.ListCong_Doan_Cong_Nhans.Remove(_selectItem);
                            DependencyService.Get<IMessage>().ShortAlert("đã kết thúc");
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

                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
           
        }
        public bool FilterRecords(object o)
        {

            var item = o as LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN;

            if (item != null)
            {

                if (item.ExternalDocumentNo_.ToLower().Contains(filterText) || item.TEN_NHAN_VIEN.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listCongDoanCongNhan.View.Filter = FilterRecords;
            listCongDoanCongNhan.View.RefreshFilter();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result = await new MessageYesNo("Thông báo", "Bạn có muốn khởi tạo công việc không?").Show();
                if (result == DialogReturn.OK)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Config.URL);
                        var ok = client.PostAsJsonAsync("api/hgpt/post_All_Bat_Dau_Cong_Viec?ma_nha_may=" + Preferences.Get(Config.NhaMay, "") + "&nguoitao=" + Preferences.Get(Config.User, ""), _selectItem);
                        if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                        {
                            DependencyService.Get<IMessage>().ShortAlert("đã khởi tạo thành công");
                            viewModel.LoadCongDoanCongNhan.Execute(null);
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

                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
            
        }

        private async void start_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ask = await new MessageYesNo("Thông báo", "Bạn có muốn bắt đầu không").Show();
                if (ask == Global.DialogReturn.OK)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var ok = client.PostAsJsonAsync("api/hgpt/start_mot_cong_doan?nguoitao=" + Preferences.Get(Config.User, ""), _selectItem);

                        if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                        {
                            foreach (LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN lx in viewModel.ListCong_Doan_Cong_Nhans)
                            {
                                if (_selectItem.RowID == lx.RowID)
                                {
                                    lx.NGAY_TAO = DateTime.Now;
                                    lx.TRANG_THAI = 1;
                                }
                            }
                            DependencyService.Get<IMessage>().ShortAlert("đã bắt đầu");
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

                await new MessageBox("Thông báo", ex.ToString()).Show();
            }
           
        }

        private async  void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                //bool isChooose = false;
                //foreach (LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN item in viewModel.ListCong_Doan_Cong_Nhans)
                //{
                //    if (item.UnCheck == 1)
                //    {
                //        isChooose = true;
                //        break;
                //    }
                //}
                //if (isChooose == false )
                //{
                //    DependencyService.Get<IMessage>().ShortAlert("Vui lòng chọn nhân viên cần cập nhật");
                //    return;                    
                //}    
                var result = await new Cap_Nhat_Thoi_Gian_Bat_Dau ().Show();
                if (result != null)
                {
                    try
                    {
                        if (viewModel.ListCong_Doan_Cong_Nhans.Count > 0)
                        {
                            if (IsBusy == true) return;

                            ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN> listUpdate = new ObservableCollection<LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN>();
                            foreach (LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN item in viewModel.ListCong_Doan_Cong_Nhans)
                            {
                                if ( item.UnCheck == 1)
                                {
                                    item.GIO_BAT_DAU = Convert.ToDateTime( result);
                                    listUpdate.Add(item);
                                }
                            }
                            if (listUpdate.Count == 0)
                            {
                                foreach (LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN item in viewModel.ListCong_Doan_Cong_Nhans)
                                {
                                    item.GIO_BAT_DAU = Convert.ToDateTime(result);
                                    listUpdate.Add(item);
                                }
                            }    
                               
                            if (listUpdate.Count > 0)
                            {
                                var OK = await new MessageYesNo("Thông báo", "Bạn có muốn cập nhật lại thời gian không?").Show();
                                if (OK == DialogReturn.OK)
                                {
                                    IsBusy = true;
                                    using (HttpClient client = new HttpClient())
                                    {
                                        client.BaseAddress = new Uri(Config.URL);
                                        var ok = client.PostAsJsonAsync("api/hgpt/put_Cap_Nhat_Thoi_Gian_Bat_Dau?nguoitao=" + Preferences.Get(Config.User, ""), listUpdate);
                                        if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("ok"))
                                        {
                                            IsBusy = false;
                                            viewModel.LoadCongDoanCongNhan.Execute(null);
                                            DependencyService.Get<IMessage>().ShortAlert("cập nhật thành công");
                                        }
                                        else
                                        {
                                            await new MessageBox("Thông báo", ok.Result.Content.ReadAsStringAsync().Result.ToLower()).Show();
                                        }
                                        client.Dispose();
                                    }    
                                  
                                }
                            }
                            else
                            {
                                await new MessageBox("Thông báo", "Bạn chưa chọn nhân viên để cập nhật.!").Show();
                            }

                        }

                    }
                    catch (Exception ex)
                    {

                        await new MessageBox("Thông báo", ex.ToString()).Show();
                    }
                    finally
                    {
                        IsBusy = false;
                    }


                }
            }
            catch (Exception ex)
            {
                await new MessageBox("Thông báo", ex.Message).Show();

            }
        }
    }
}