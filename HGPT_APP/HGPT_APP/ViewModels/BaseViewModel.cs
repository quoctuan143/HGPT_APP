using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using HGPT_APP.Models;
using HGPT_APP.Services;
using HGPT_APP.Interface;
using HGPT_APP.Models.GiamSat;
using System.Collections.ObjectModel;
using HGPT_APP.Global;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace HGPT_APP.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            CongTrinhList = new ObservableCollection<DanhSachCongTrinh>();
            LoadCommand = new Command(  () =>   LoadData(null ));
        }
        public virtual async Task  LoadData(object ojb)
        {
           
        }
        public Command LoadCommand { get; set; }
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            set { SetProperty(ref isRunning, value); }
        }

        public void ShowLoading(string title)
        {
            DependencyService.Get<IProcessLoader>().Show(title);
        }
        public void HideLoading()
        {
            DependencyService.Get<IProcessLoader>().Hide();
        }
        public void ShortAlert(string title)
        {
            DependencyService.Get<IMessage>().ShortAlert(title);
        }
        ObservableCollection<DanhSachCongTrinh> _congTrinh;
        public ObservableCollection<DanhSachCongTrinh> CongTrinhList { get => _congTrinh; set => SetProperty(ref _congTrinh, value); }
        public void GetCongTrinh(string nhom)
        {
            var _jsonctrinh = Config.client.GetStringAsync(Config.URL + $"getCongTrinh?nhom={nhom}").Result;
            _jsonctrinh = _jsonctrinh.Replace("\\r\\n", "").Replace("\\", "");
            if (_jsonctrinh != "")
            {
                string result = _jsonctrinh.Substring(1, _jsonctrinh.Length - 2);
                CongTrinhList = JsonConvert.DeserializeObject<ObservableCollection<DanhSachCongTrinh>>(result);
                OnPropertyChanged("CongTrinhList");
            }
        }
        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public async Task<HttpClientResponseModel<T>> RunHttpClientGet<T>(string apiUrl) where T : class
        {
            try
            {
                var respon = await Config.client.GetAsync(apiUrl);
                HttpClientResponseModel<T> values = new HttpClientResponseModel<T>();
                values.Status = respon;
                values.Lists = new ObservableCollection<T>();
                if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string _json = await respon.Content.ReadAsStringAsync();
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        values.Lists = JsonConvert.DeserializeObject<ObservableCollection<T>>(result);
                    }
                }
                return values;
            }
            catch (Exception ex)
            {
                return new HttpClientResponseModel<T> { Status = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest } };
            }

        }

        public async Task<HttpResponseMessage> RunHttpClientPost(string apiUrl, object Value)
        {
            try
            {
                HttpResponseMessage Status = await Config.client.PostAsJsonAsync(apiUrl, Value);
                return Status;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest };
            }

        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class HttpClientResponseModel<T> where T : class
    {
        public HttpResponseMessage Status { get; set; }
        public ObservableCollection<T> Lists { get; set; }
    }
}
