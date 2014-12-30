using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InforWhats
{
  public  class Credencial:Pessoa
    {
        public string Senha { get; set; }
        public override string ToString()
        {
            return this.Telefone;
        }
    }
}
