using System;
using System.Collections.Generic;
using System.Text;

namespace Vibe.Models.Usuario
{
    public class Usuario
    {
        public string cpf { get; set; }
        public string nome { get; set; }
        public DateTime nascimento { get; set; }
        public string senha { get; set; }
    }
}
