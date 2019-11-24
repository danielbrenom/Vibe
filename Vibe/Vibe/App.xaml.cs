using System;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Vibe.Services;
using Vibe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vibe.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Vibe
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<ISessionStorage, SessionStorage>();
            unityContainer.RegisterInstance(typeof(ItemsViewModel));
            unityContainer.RegisterInstance(typeof(AboutViewModel));

            var unityServiceLocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(()=> unityServiceLocator);
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
