using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TesteFrm
{
   public class MsgWhats:InforWhats.Mensagem
    {
        public int Aluno { get; set; }
        public string NomeAluno { get; set; }
        public int Autoescola { get; set; }
        public int Atend { get; set; }
        public string NomeAtend { get; set; }
    }
}
