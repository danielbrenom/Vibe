using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Vibe.Models;
using Vibe.Models.Clientes;
using Vibe.Services;
using Vibe.Views;

namespace Vibe.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public ObservableCollection<Cliente> Clientes { get; set; }
        private readonly ISessionStorage _sessionStorage;
        private bool _authenticated;
        public bool Authenticated
        {
            get => _authenticated;
            set => SetProperty(ref _authenticated, value);
        }

        public ItemsViewModel(ISessionStorage sessionStorage)
        {
            Title = "Clientes";
            Items = new ObservableCollection<Item>();
            Clientes = new ObservableCollection<Cliente>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            _sessionStorage = sessionStorage;
            sessionStorage.PropertyChanged += SessionStorageOnPropertyChanged;
        }

        private void SessionStorageOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Authenticated))
            {
                Authenticated = _sessionStorage.Authenticated;
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (!IsBusy && !_sessionStorage.Authenticated)
                return;

            IsBusy = true;

            try
            {
                Clientes.Clear();
                var clientes = await ApiServices.LoadClientes();
                foreach (var cliente in clientes.clienteList)
                {
                    await DataStore.AddItemAsync(cliente);
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
    }
}