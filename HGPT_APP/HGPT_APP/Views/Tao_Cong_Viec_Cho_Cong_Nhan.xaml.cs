using HGPT_APP.Global;
using HGPT_APP.Interface;
using HGPT_APP.Models;
using HGPT_APP.Popup;
using HGPT_APP.ViewModels;
using Newtonsoft.Json;
using Syncfusion.SfDataGrid.XForms;
using Syncfusion.XForms.ComboBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tao_Cong_Viec_Cho_Cong_Nhan : ContentPage
    {
        Tao_Cong_Viec_Cho_Cong_Nhan_ViewModel viewModel;    
        public Tao_Cong_Viec_Cho_Cong_Nhan(DANH_MUC_LENH_SAN_XUAT lsx)
        {
            InitializeComponent();
            listCongDoanCongNhan.QueryCellStyle += ListCongDoanCongNhan_QueryCellStyle;
            Title = lsx.Ten_Day_Du;
            BindingContext = viewModel = new Tao_Cong_Viec_Cho_Cong_Nhan_ViewModel(lsx.LENH_SAN_XUAT);
            GridComboBoxColumn comboBoxColumn = new GridComboBoxColumn()
            {
                
                DisplayMemberPath = "MA_CONG_DOAN",
                ValueMemberPath = "MA_CONG_DOAN",
                MappingName = "MA_CONG_DOAN",
                ItemsSource = viewModel.ListDMCongDoan,
                HeaderText="Công Đoạn",
                HeaderFontAttribute = FontAttributes.Bold,
                HeaderCellTextSize= 16,
                Width=120 ,
                Padding = new Thickness (5, 0, 0, 0),
                TextAlignment = TextAlignment.Start,
                LoadUIView=true
        };           
            listCongDoanCongNhan.Columns.Insert(1,comboBoxColumn);
        }

        private void ListCongDoanCongNhan_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {
           
            if (e.Column.MappingName == "MA_CONG_DOAN")
            {               
                e.Style.ForegroundColor = Color.Black;
            }
           
            e.Handled = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.IsBusy == true) return;
            if (viewModel.ListCong_Doan_Cong_Nhans.Count == 0)
            {
                viewModel.LoadCongDoanCongNhan.Execute(null);
                viewModel.LoadToSanXuatCommand.Execute(null);                
            }         
        }
        string filterText = "";
        public bool FilterRecords(object o)
        {

            var item = o as LSX_PHAN_CHIA_CONG_DOAN_CONG_NHAN;

            if (item != null)
            {

                if (item.TEN_NHAN_VIEN.ToLower().Contains(filterText))
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

    }
}