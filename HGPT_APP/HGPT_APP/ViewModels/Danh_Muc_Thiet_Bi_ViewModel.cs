using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using HGPT_APP.Models;
using Xamarin.Forms;
using HGPT_APP.Global;
using Xamarin.Essentials;
using Newtonsoft.Json;
using HGPT_APP.Popup;

namespace HGPT_APP.ViewModels
{
    public class Danh_Muc_Thiet_Bi_ViewModel : BaseViewModel
    {
        int size = 0;
        ObservableCollection<DanhMuc_ThietBi> _items;
        public ObservableCollection<DanhMuc_ThietBi> Items { get => _items; set { _items = value; OnPropertyChanged(nameof(Items)); } }

        public ObservableCollection<DanhMuc_ThietBi> ketqua = new ObservableCollection<DanhMuc_ThietBi>();
        public Command LoadItemsCommand { get; set; }
        public Command LoadMoreCommand { get; set; }
        public Danh_Muc_Thiet_Bi_ViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<DanhMuc_ThietBi>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadMoreCommand = new Command(async () => await ExecuteLoadMoreCommand());
        }

        async Task ExecuteLoadMoreCommand()
        {
            IsRunning = true;
            if (size + 100 < ketqua.Count)
            {
                for (int i = size; i < size + 100; i++)
                {
                    Items.Add(ketqua[i]);
                }
                size = Items.Count;
            }
            else
            {
                for (int i = size; i < ketqua.Count; i++)
                {
                    Items.Add(ketqua[i]);
                }
                size = Items.Count;
            }
            IsRunning = false;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            IsRunning = true;
            try
            {
                Items.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/qltb/getThietBi?nhamay=" + Preferences.Get(Config.NhaMay, "") +"&user=" + Preferences.Get(Config.User ,"")).Result;
                await Task.Delay(1000);
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    Items = JsonConvert.DeserializeObject<ObservableCollection<DanhMuc_ThietBi>>(result);

                }
                else
                {
                    await new MessageBox("Thông Báo", "Không tìm thấy thiệt bị nào cả").Show();

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                IsRunning = false;
            }
        }

        public bool FilterRecords(object o)
        {
            string filterText = "Germany";
            var item = o as DanhMuc_ThietBi;

            if (item != null)
            {

                if (item.No_2.Equals(filterText))
                    return true;
            }
            return false;
        }
    }
    }
