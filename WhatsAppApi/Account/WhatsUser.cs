namespace WhatsAppApi.Account
{
    public class WhatsUser
    {
        private string serverUrl;
        public string Nickname { get; set; }
        public string Jid { get; private set; }

        public WhatsUser(string jid, string srvUrl, string nickname = "")
        {
            Jid = jid;
            Nickname = nickname;
            serverUrl = srvUrl;
        }

        public string GetFullJid()
        {
            return WhatsApp.GetJID(Jid);
        }

        internal void SetServerUrl(string srvUrl)
        {
            serverUrl = srvUrl;
        }

        public override string ToString()
        {
            return GetFullJid();
        }
    }
}
