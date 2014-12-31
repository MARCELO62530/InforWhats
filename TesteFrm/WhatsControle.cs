using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InforWhats;

namespace TesteFrm
{
    class WhatsControle
    {
    }

    public static class WhatsStatic
    {
        public static ListViewItem[] PopulaListaRemetente()
        {
            return ListaRemetente().Select(x => new ListViewItem(x.Nome)
            {
                SubItems = {x.Telefone}
            }).ToArray();
          

        }

        public static List<Pessoa>  ListaRemetente()
        {
            var destinatarios = new List<Pessoa>
            {
                new Pessoa
                {
                    Nome = "Marcelo Pai",
                    Telefone = "5511950373839"
                },
                new Pessoa
                {
                    Nome = "Marcelo Junior",
                    Telefone = "5511942659372"
                }
            };
            return destinatarios;


        }


    }
}
