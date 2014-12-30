using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InforWhats;
using System.Reflection;

namespace TesteFrm
{
    public partial class Form1 : Form
    {
        private int autoescola = 2;
        private int atend = 1000;
        string nickname = "marcelo";
        string Remetente = "5511948327016"; // Mobile number with country code (but without + or 00)
        string password = "KaXsQ/AflWDP8LyiMkQ9UvDIFvA=";//v2 password
        //string target = "5511950373839";// Mobile number to send the message to
        //string target = "5511942659372";// Mobile number to send the message to

        Whats w;

        public Form1()
        {
            InitializeComponent();
        }



        void Conect()
        {

            using (w = new Whats(new Credencial
           {
               Nome = nickname,
               Telefone = Remetente,
               Senha = password
           }))
            {
                w.Conectar();
                w.TrataMsgRecebidaArgs = TrataMsgRecebida;
            }


        }

        void TrataMsgRecebida(Mensagem msg)
        {
            MsgWhats mw = new MsgWhats();
            mw.Descricao = msg.Descricao;
            mw.Remetente = msg.Remetente;
            mw.Data = msg.Data;
            mw.Aluno = 1111;
            mw.Atend = atend;
            mw.NomeAtend = "nome atend";
            //salvar


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Conect();
            w.Enviar(new Mensagem
            {
                Descricao = txtSend.Text,
                Destinatario = new Pessoa
                {
                    Telefone = txtTelDestinatario.Text
                }

            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            Conect();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dataGridView1.DataSource = w.BuscarUltimos();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Conect();
        }



    }
}
