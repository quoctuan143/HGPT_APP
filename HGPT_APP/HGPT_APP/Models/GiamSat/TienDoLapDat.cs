using HGPT_APP.Popup;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HGPT_APP.Models.GiamSat
{
    public class TienDoLapDat : Bindable
    {
        public string RowID { get; set; }
        public string LenhSanXuat { get; set; }
        public string HangMuc { get; set; }
        public string Description { get; set; }
        public string QuyCach { get; set; }
        public double Quantity { get; set; }

        public double SoLuongLapDat { get; set; }
        private double _soluonglap;

        public double SoLuongCanLap
        {
            get => _soluonglap;
            set
            {
                SetProperty(ref _soluonglap, value);
                if (_soluonglap > Quantity + SoLuongLapDat)
                {
                    SoLuongCanLap = 0;
                    OnPropertyChanged("FormatSoLuongCanLap");
                    Task.Run(() => new MessageBox("Thông báo", "Số lượng vượt quá của hạng mục").Show());
                }
            }
        }
        public string FormatSoLuongCanLap
        {
            get => string.Format("{0:#,##0}", SoLuongCanLap);
            set
            {
                if (!CheckThapPhan(value))
                {
                    FormatNumberString(ref _soluonglap, value);
                    OnPropertyChanged("FormatSoLuongCanLap");
                    SoLuongCanLap = _soluonglap;
                }
            }
        }

        public bool ChuaHoanThanh { get; set; }
        public bool CapCha { get; set; }

        public int Type { get; set; }

        [JsonProperty("Order Code")]
        public string OrderCode { get; set; }

        [JsonProperty("Parent Code")]
        public string ParentCode { get; set; }

        public bool ValueCha { get; set; }
    }
}