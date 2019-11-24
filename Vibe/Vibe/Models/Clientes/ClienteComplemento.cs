using System;
using System.Collections.Generic;
using System.Text;

namespace Vibe.Models.Clientes
{
    public class ClienteComplemento
    {
        public string urlImagem { get; set; }
        public string empresa { get; set; }
        public ClienteEndereco endereco { get; set; }
    }
}
