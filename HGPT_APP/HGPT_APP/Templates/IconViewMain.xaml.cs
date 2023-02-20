using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HGPT_APP.Templates
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IconViewMain : ContentView
	{
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(IconViewMain), string.Empty);
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(IconImageSource), typeof(ImageSource), typeof(IconViewMain), default(ImageSource));
        public static readonly BindableProperty BadgeTextProperty = BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(IconViewMain), string.Empty);
        public static readonly BindableProperty ActionCommandProperty = BindableProperty.Create(nameof(ActionCommand), typeof(ICommand), typeof(IconViewMain));
        public static readonly BindableProperty DisableProperty = BindableProperty.Create(nameof(Disable), typeof(bool), typeof(IconViewMain), false);
        public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(nameof(ImageWidth), typeof(int), typeof(IconViewMain), 0);
        public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(nameof(ImageHeight), typeof(int), typeof(IconViewMain), 0);
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public string BadgeText
        {
            get => (string)GetValue(BadgeTextProperty);
            set => SetValue(BadgeTextProperty, value);
        }
        public int ImageWidth
        {
            get => (int)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }
        public int ImageHeight
        {
            get => (int)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }
        public bool Disable
        {
            get => (bool)GetValue(DisableProperty);
            set => SetValue(DisableProperty, value);
        }

        public ImageSource IconImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public ICommand ActionCommand
        {
            get => (ICommand)GetValue(ActionCommandProperty);
            set => SetValue(ActionCommandProperty, value);
        }
        public IconViewMain ()
		{
			InitializeComponent ();
		}
        private void Frame_Tapped(object sender, EventArgs e)
        {
            ActionCommand.Execute(null);
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}