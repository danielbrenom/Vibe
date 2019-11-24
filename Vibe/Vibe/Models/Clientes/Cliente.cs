using System;
using System.Collections.Generic;
using System.Text;

namespace Vibe.Models.Clientes
{
    public class Cliente
    {
        public string id { get; set; }
        public string cpf { get; set; }
        public string nome { get; set; }
        public bool especial { get; set; }
    }
}
