using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WhatsAppApi;
using WhatsAppApi.Account;

namespace InforWhats
{
    public class Whats : Eventos
    {
        WhatsApp wa;
        Pessoa _remetente;
        public List<Mensagem> Mensagens { get; set; }
        public Whats(Credencial _credencial)
        {
            _remetente = _credencial;
            Mensagens = new List<Mensagem>();
            wa = new WhatsApp(_credencial.Telefone, _credencial.Senha, _credencial.Nome, true);

            //event bindings
            wa.OnLoginSuccess += wa_OnLoginSuccess;
            wa.OnLoginFailed += wa_OnLoginFailed;
            wa.OnGetMessage += wa_OnGetMessage;
            wa.OnGetMessageReceivedClient += wa_OnGetMessageReceivedClient;
            wa.OnGetMessageReceivedServer += wa_OnGetMessageReceivedServer;
            wa.OnNotificationPicture += wa_OnNotificationPicture;
            wa.OnGetPresence += wa_OnGetPresence;
            wa.OnGetGroupParticipants += wa_OnGetGroupParticipants;
            wa.OnGetLastSeen += wa_OnGetLastSeen;
            wa.OnGetTyping += wa_OnGetTyping;
            wa.OnGetPaused += wa_OnGetPaused;
            wa.OnGetMessageImage += wa_OnGetMessageImage;
            wa.OnGetMessageAudio += wa_OnGetMessageAudio;
            wa.OnGetMessageVideo += wa_OnGetMessageVideo;
            wa.OnGetMessageLocation += wa_OnGetMessageLocation;
            wa.OnGetMessageVcard += wa_OnGetMessageVcard;
            wa.OnGetPhoto += wa_OnGetPhoto;
            wa.OnGetPhotoPreview += wa_OnGetPhotoPreview;
            wa.OnGetGroups += wa_OnGetGroups;
            wa.OnGetSyncResult += wa_OnGetSyncResult;
            wa.OnGetStatus += wa_OnGetStatus;
            wa.OnGetPrivacySettings += wa_OnGetPrivacySettings;
            //DebugAdapter.Instance.OnPrintDebug += Instance_OnPrintDebug;

            Receber = PopulaMensagens;

        }

        public void Conectar()
        {
            wa.Connect();
            string datFile = getDatFileName(_remetente.Telefone);
            byte[] nextChallenge = null;
            if (File.Exists(datFile))
            {
                try
                {
                    string foo = File.ReadAllText(datFile);
                    nextChallenge = Convert.FromBase64String(foo);
                }
                catch (Exception) { };
            }

            wa.Login(nextChallenge);
            this.ProcessChat(wa);
        }

        public void Enviar(Mensagem msg)
        {
            msg.Remetente = _remetente;
            Mensagens.Add(msg);
            if (wa.ConnectionStatus == ApiBase.CONNECTION_STATUS.DISCONNECTED)
            {
              
               
                Conectar();
            }
            WhatsUserManager usrMan = new WhatsUserManager();
            var tmpUser = usrMan.CreateUser(msg.Destinatario.Telefone, "User");
            if (!string.IsNullOrEmpty(msg.Descricao))
            {
                Console.WriteLine("[] Send message to {0}: {1}", tmpUser, msg.Descricao);
                wa.SendMessage(tmpUser.GetFullJid(), msg.Descricao);
               
            }

         //   wa.Disconnect();
        }

        public void Desconectar()
        {
            wa.Disconnect();
        }

        private void PopulaMensagens(Mensagem msg)
        {
            msg.Destinatario.Telefone = _remetente.Telefone;
            TrataMsgRecebidaArgs.Invoke(msg);

            Mensagens.Add(msg);

        }

        public Action<Mensagem> TrataMsgRecebidaArgs = (Mensagem msg) => { };

        public List<Mensagem> BuscarUltimos()
        {
            var b = Mensagens.GroupBy(x => x.Destinatario.Telefone).Select(x=>x.Last()).ToList();

            return b;

        }


    }
}
