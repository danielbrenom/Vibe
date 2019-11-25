using System;
using Microsoft.Practices.ServiceLocation;
using Vibe.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vibe.Models.Clientes;
using Vibe.ViewModels;
using Xamarin.Essentials;

namespace Vibe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        readonly ItemsViewModel _viewModel;
        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = (ItemsViewModel)ServiceLocator.Current.GetInstance(typeof(ItemsViewModel));
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Cliente item))
                return;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("Atenção", "Não é possível verificar informações de clientes sem acesso a internet", "Fechar");
                ItemsListView.SelectedItem = null;
                return;
            }
            var api = (IApiService)ServiceLocator.Current.GetInstance(typeof(IApiService));
            var clientComplemento = await api.LoadClienteComplemento(item.id);
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item, clientComplemento)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Clientes.Count == 0 && _viewModel.IsAuthenticated())
                _viewModel.LoadItemsCommand.Execute(null);
        }

        private void MenuItem_OnClicked(object sender, EventArgs e)
        {
            _viewModel.LoadItemsCommand.Execute(null);
        }
    }
}