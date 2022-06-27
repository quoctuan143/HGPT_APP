using HGPT_APP.ViewModels.GiamSat;
using Syncfusion.SfDataGrid.XForms;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Views.GiamSat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChamCongNhanSu_Page : ContentPage
    {
        ChamCongNhanSu_ViewModel viewModel;
        List<NgayCong> NgayCong = new List<NgayCong>();
        public ChamCongNhanSu_Page() 
        {
            InitializeComponent();
            viewModel = new ChamCongNhanSu_ViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
            NgayCong.Add(new GiamSat.NgayCong { Name = "1 Ngày", Value = 1 }  ) ;
            NgayCong.Add(new GiamSat.NgayCong { Name = "0.5 Ngày", Value = 0.5 });
            NgayCong.Add(new GiamSat.NgayCong { Name = "Nghỉ vắng", Value = 0 });
           
            
        }
       

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListCapNhatGioCong.Count  == 0 )
            {
                await viewModel.LoadData(null);
                GridComboBoxColumn comboBoxColumn1 = new GridComboBoxColumn()
                {
                    DisplayMemberPath = "Description",
                    ValueMemberPath = "Code",
                    MappingName = "NhomChamCong",
                    ItemsSource = viewModel.ListNhomChamCong,
                    HeaderText = "Nhóm công",

                    HeaderFontAttribute = FontAttributes.Bold,
                    HeaderCellTextSize = 16,
                    Width = 120,
                    Padding = new Thickness(5, 0, 0, 0),
                    TextAlignment = TextAlignment.Start,
                    LoadUIView = true
                };
                listCongDoanCongNhan.Columns.Insert(3, comboBoxColumn1);
                GridComboBoxColumn comboBoxColumn = new GridComboBoxColumn()
                {

                    DisplayMemberPath = "Name",
                    ValueMemberPath = "Value",
                    MappingName = "NgayCong",
                    ItemsSource = NgayCong,
                    HeaderText = "Ngày công",
                    HeaderFontAttribute = FontAttributes.Bold,
                    HeaderCellTextSize = 16,
                    Width = 120,
                    Padding = new Thickness(5, 0, 0, 0),
                    TextAlignment = TextAlignment.Start,
                    LoadUIView = true
                };
                listCongDoanCongNhan.Columns.Insert(4, comboBoxColumn);
            }    
        }
    }
    public class NgayCong
    {        
        public string Name { get; set; }
        public double Value { get; set; }
    }
}