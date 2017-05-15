using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace crossPlatform
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnPushIt_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Привет!", "Это мое первое приложение для ведроид!", "OK");
            
        }
    }
}
