using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatsAppApi.Account;

namespace InforWhats
{
    public class Mensagem
    {
        public Mensagem()
        {
            Remetente = new Pessoa();
            Destinatario= new Pessoa();
        }
        public enum StatusMensagem
        {
            Servidor=0,
            Cliente=1,
            Lida=2
        }

        public string Descricao { get; set; }
        public Pessoa Remetente { get; set; }
        public Pessoa Destinatario { get; set; }
        public DateTime Data { get; set; }
        public StatusMensagem Status { get; set; }
        public override string ToString()
        {
            return Remetente.Telefone;
        }
    }
}
