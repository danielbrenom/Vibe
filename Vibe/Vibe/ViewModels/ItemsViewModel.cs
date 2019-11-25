using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Vibe.Interfaces;
using Xamarin.Forms;

using Vibe.Models;
using Vibe.Models.Clientes;
using Vibe.Services;
using Xamarin.Essentials;

namespace Vibe.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }
        public ObservableCollection<Cliente> Clientes { get; set; }
        private readonly ISessionStorage _sessionStorage;

        public ItemsViewModel(ISessionStorage sessionStorage)
        {
            Title = "Clientes";
            Clientes = new ObservableCollection<Cliente>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            _sessionStorage = sessionStorage;
            _sessionStorage.PropertyChanged += SessionStorageOnPropertyChanged;
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            if (!_sessionStorage.Authenticated)
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Usuário não autenticado", "OK");
                IsBusy = false;
                return;
            }
            
            try
            {
                Clientes.Clear();
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    if (!await ApiServices.LoadClientes())
                    {
                        await Application.Current.MainPage.DisplayAlert("Atenção", "Ocorreu um erro ao carregar a lista de clientes.", "OK");
                        IsBusy = false;
                        return;
                    }
                }
                else
                {
                    if (_sessionStorage.StorageData == null)
                    {
                        IsBusy = false;
                        await Application.Current.MainPage.DisplayAlert("Atenção", "Não foi possível recuperar a lista de clientes do cache nem da internet.", "OK");
                        return;
                    }

                    await DataStore.ReplaceListAsync(_sessionStorage.StorageData.ClienteListData);
                }
                
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Clientes.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public bool IsAuthenticated()
        {
            return _sessionStorage.Authenticated;
        }
        private void SessionStorageOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Authenticated))
            {
                Authenticated = _sessionStorage.Authenticated;
            }
        }
    }
}