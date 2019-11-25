using System;
using System.Runtime.CompilerServices;
using Vibe.Models;
using Vibe.Models.Clientes;

namespace Vibe.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Cliente Cliente { get; set; }
        public ClienteComplemento ClienteComplemento { get; set; }
        public ItemDetailViewModel(Cliente item = null, ClienteComplemento complement = null)
        {
            
            Title = "Informações do cliente";
            Cliente = item;
            ClienteComplemento = complement;
        }
    }
}
