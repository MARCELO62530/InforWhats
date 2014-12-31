using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppApi.Helper;
using WhatsAppApi.Settings;

namespace WhatsAppApi
{
    public class WhatsAppBase : WhatsEventBase
    {
        protected ProtocolTreeNode uploadResponse;

        protected AccountInfo accountinfo;

        public static bool DEBUG;

        protected string password;

        protected bool hidden;

        protected CONNECTION_STATUS loginStatus;

        public CONNECTION_STATUS ConnectionStatus
        {
            get
            {
                return loginStatus;
            }
        }

        protected KeyStream outputKey;

        protected object messageLock = new object();

        protected List<ProtocolTreeNode> messageQueue;

        protected string name;

        protected string phoneNumber;

        protected BinTreeNodeReader reader;

        protected int timeout = 300000;

        protected WhatsNetwork whatsNetwork;

        public static readonly Encoding SYSEncoding = Encoding.UTF8;

        protected byte[] _challengeBytes;

        protected BinTreeNodeWriter BinWriter;

        protected void _constructBase(string phoneNum, string imei, string nick, bool debug, bool hidden)
        {
            messageQueue = new List<ProtocolTreeNode>();
            phoneNumber = phoneNum;
            password = imei;
            name = nick;
            this.hidden = hidden;
            DEBUG = debug;
            reader = new BinTreeNodeReader();
            loginStatus = CONNECTION_STATUS.DISCONNECTED;
            BinWriter = new BinTreeNodeWriter();
            whatsNetwork = new WhatsNetwork(WhatsConstants.WhatsAppHost, WhatsConstants.WhatsPort, timeout);
        }

        public void Connect()
        {
            try
            {
                whatsNetwork.Connect();
                loginStatus = CONNECTION_STATUS.CONNECTED;
                //success
                fireOnConnectSuccess();
            }
            catch (Exception e)
            {
                fireOnConnectFailed(e);
            }
        }

        public void Disconnect(Exception ex = null)
        {
            whatsNetwork.Disconenct();
            loginStatus = CONNECTION_STATUS.DISCONNECTED;
            fireOnDisconnect(ex);
        }

        protected byte[] encryptPassword()
        {
            return Convert.FromBase64String(password);
        }

        public AccountInfo GetAccountInfo()
        {
            return accountinfo;
        }

        public ProtocolTreeNode[] GetAllMessages()
        {
            ProtocolTreeNode[] tmpReturn = null;
            lock (messageLock)
            {
                tmpReturn = messageQueue.ToArray();
                messageQueue.Clear();
            }
            return tmpReturn;
        }

        protected void AddMessage(ProtocolTreeNode node)
        {
            lock (messageLock)
            {
                messageQueue.Add(node);
            }
        }

        public bool HasMessages()
        {
            if (messageQueue == null)
                return false;
            return messageQueue.Count > 0;
        }

        protected void SendData(byte[] data)
        {
            try
            {
                whatsNetwork.SendData(data);
            }
            catch (ConnectionException)
            {
                Disconnect();
            }
        }

        protected void SendNode(ProtocolTreeNode node)
        {
            SendData(BinWriter.Write(node));
        }
    }
}
