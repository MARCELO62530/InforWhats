using System;
using System.Windows.Forms;

namespace TesteFrm
{
    public partial class FrmInicial : Form
    {
        private int _autoescola = 2;
        private int _atend = 1000;
        string _nickname = "marcelo";
        string _remetente = "5511948327016"; // Mobile number with country code (but without + or 00)
        string _password = "KaXsQ/AflWDP8LyiMkQ9UvDIFvA=";
          public FrmInicial()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            lvDestinatario.Items.AddRange( WhatsStatic.PopulaListaRemetente());


        }

        private void btnMsgEnviar_Click(object sender, EventArgs e)
        {

        }




    }
}
