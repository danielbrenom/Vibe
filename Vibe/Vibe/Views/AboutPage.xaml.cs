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
        public AboutPage()
        {
            InitializeComponent();

            BindingContext = aboutViewModel = (AboutViewModel)ServiceLocator.Current.GetInstance(typeof(AboutViewModel));
        }

        async void Cadastro_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CadastroPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var user = aboutViewModel;
        }
    }
}