using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vibe.Models.Clientes;
using Vibe.ViewModels;

namespace Vibe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Cliente
            {
                nome = "Item 1",
                cpf = "This is an item description."
            };
            var complemto = new ClienteComplemento
            {
                empresa = "empresa",
                endereco = new ClienteEndereco { cidade = "cidade", endereco = "endereco", complemento = "complemento", numero = "numero"},
                urlImagem = "imagem"
            };

            viewModel = new ItemDetailViewModel(item, complemto);
            BindingContext = viewModel;
        }
    }
}