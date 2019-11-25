using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Vibe.Interfaces;
using Vibe.Models.Clientes;
using Vibe.Services;
using Vibe.Services.Interfaces;
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
            unityContainer.RegisterInstance<ISessionStorage>(new SessionStorage());
            unityContainer.RegisterInstance<IApiService>(new ApiServices());
            unityContainer.RegisterInstance<IDataStore<Cliente>>(new ClienteDataStore());
            unityContainer.RegisterType(typeof(ItemsViewModel));
            unityContainer.RegisterType(typeof(AboutViewModel));

            var unityServiceLocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);
            
            MainPage = new LoaginPage();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            var sessionStorage = ServiceLocator.Current.GetInstance<ISessionStorage>();
            await sessionStorage.LoadFromStorage();
            MainPage = new MainPage();
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