using System.Collections.Generic;
using WhatsAppApi.Settings;

namespace WhatsAppApi.Account
{
    public class WhatsUserManager
    {
        private Dictionary<string, WhatsUser> userList;

        public WhatsUserManager()
        {
            userList = new Dictionary<string, WhatsUser>();
        }

        //public void AddUser(User user)
        //{
        //    //if(user == null || user.)
        //    //if(this.userList.ContainsKey())
        //}

        public WhatsUser CreateUser(string jid, string nickname = "")
        {
            if (userList.ContainsKey(jid))
                return userList[jid];

            string server = WhatsConstants.WhatsAppServer;
            if (jid.Contains("-"))
                server = WhatsConstants.WhatsGroupChat;

            var tmpUser = new WhatsUser(jid, server, nickname);
            userList.Add(jid, tmpUser);
            return tmpUser;
        }
    }
}
