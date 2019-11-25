using System.Collections.Generic;
using Vibe.Models.Clientes;

namespace Vibe.Models.Usuario
{
    public class UsuarioStorageData
    {
        public UsuarioAuthData StorageAuthData { get; set; }
        public Usuario UserInfo { get; set; }

        public List<Cliente> ClienteListData
        {
            get;
            set;
        }
    }
}
