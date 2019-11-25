using System;
using System.Collections.Generic;
using System.Text;

namespace Vibe.Models.Clientes
{
    public class ClienteEndereco
    {
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string cidade { get; set; }

        public override string ToString()
        {
            return endereco + ", " + numero + ", " + complemento + ", " + cidade;
        }
    }
}