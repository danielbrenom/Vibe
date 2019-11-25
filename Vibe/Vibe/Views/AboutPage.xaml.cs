using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Practices.ServiceLocation;
using Vibe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vibe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        AboutViewModel aboutViewModel;
        private bool once;

        public AboutPage()
        {
            InitializeComponent();

            BindingContext = aboutViewModel =
                (AboutViewModel) ServiceLocator.Current.GetInstance(typeof(AboutViewModel));
        }

        async void Cadastro_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroPage());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (once) return;
            once = await aboutViewModel.LoginOnStart();
        }
    }
}