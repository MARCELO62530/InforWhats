using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WhatsAppApi.Helper;
using WhatsAppApi.Parser;
using WhatsAppApi.Response;
using WhatsAppApi.Settings;

namespace WhatsAppApi
{
    public class WhatsSendBase : WhatsAppBase
    {
        public void Login(byte[] nextChallenge = null)
        {
            //reset stuff
            reader.Key = null;
            BinWriter.Key = null;
            _challengeBytes = null;

            if (nextChallenge != null)
            {
                _challengeBytes = nextChallenge;
            }

            string resource = string.Format(@"{0}-{1}-{2}",
                WhatsConstants.Device,
                WhatsConstants.WhatsAppVer,
                WhatsConstants.WhatsPort);
            var data = BinWriter.StartStream(WhatsConstants.WhatsAppServer, resource);
            var feat = addFeatures();
            var auth = addAuth();
            SendData(data);
            SendData(BinWriter.Write(feat, false));
            SendData(BinWriter.Write(auth, false));

            pollMessage();//stream start
            pollMessage();//features
            pollMessage();//challenge or success

            if (loginStatus != CONNECTION_STATUS.LOGGEDIN)
            {
                //oneshot failed
                ProtocolTreeNode authResp = addAuthResponse();
                SendData(BinWriter.Write(authResp, false));
                pollMessage();
            }

            SendAvailableForChat(name, hidden);
        }

        public void PollMessages(bool autoReceipt = true)
        {
            while (pollMessage(autoReceipt)) ;
        }

        public bool pollMessage(bool autoReceipt = true)
        {
            if (loginStatus == CONNECTION_STATUS.CONNECTED || loginStatus == CONNECTION_STATUS.LOGGEDIN)
            {
                byte[] nodeData;
                try
                {
                    nodeData = whatsNetwork.ReadNextNode();
                    if (nodeData != null)
                    {
                        return processInboundData(nodeData, autoReceipt);
                    }
                }
                catch (ConnectionException)
                {
                    Disconnect();
                }
            }
            return false;
        }

        protected ProtocolTreeNode addFeatures()
        {
            return new ProtocolTreeNode("stream:features", null);
        }

        protected ProtocolTreeNode addAuth()
        {
            List<KeyValue> attr = new List<KeyValue>(new[] {
                new KeyValue("mechanism", KeyStream.AuthMethod),
                new KeyValue("user", phoneNumber)});
            if (hidden)
            {
                attr.Add(new KeyValue("passive", "true"));
            }
            var node = new ProtocolTreeNode("auth", attr.ToArray(), null, getAuthBlob());
            return node;
        }

        protected byte[] getAuthBlob()
        {
            byte[] data = null;
            if (_challengeBytes != null)
            {
                byte[][] keys = KeyStream.GenerateKeys(encryptPassword(), _challengeBytes);

                reader.Key = new KeyStream(keys[2], keys[3]);

                outputKey = new KeyStream(keys[0], keys[1]);

                PhoneNumber pn = new PhoneNumber(phoneNumber);

                List<byte> b = new List<byte>();
                b.AddRange(new byte[] { 0, 0, 0, 0 });
                b.AddRange(SYSEncoding.GetBytes(phoneNumber));
                b.AddRange(_challengeBytes);
                b.AddRange(SYSEncoding.GetBytes(Func.GetNowUnixTimestamp().ToString()));
                b.AddRange(SYSEncoding.GetBytes(WhatsConstants.UserAgent));
                b.AddRange(SYSEncoding.GetBytes(String.Format(" MccMnc/{0}001", pn.MCC)));
                data = b.ToArray();

                _challengeBytes = null;

                outputKey.EncodeMessage(data, 0, 4, data.Length - 4);

                BinWriter.Key = outputKey;
            }

            return data;
        }

        protected ProtocolTreeNode addAuthResponse()
        {
            if (_challengeBytes != null)
            {
                byte[][] keys = KeyStream.GenerateKeys(encryptPassword(), _challengeBytes);

                reader.Key = new KeyStream(keys[2], keys[3]);
                BinWriter.Key = new KeyStream(keys[0], keys[1]);

                List<byte> b = new List<byte>();
                b.AddRange(new byte[] { 0, 0, 0, 0 });
                b.AddRange(SYSEncoding.GetBytes(phoneNumber));
                b.AddRange(_challengeBytes);


                byte[] data = b.ToArray();
                BinWriter.Key.EncodeMessage(data, 0, 4, data.Length - 4);
                var node = new ProtocolTreeNode("response",
                    new[] { new KeyValue("xmlns", "urn:ietf:params:xml:ns:xmpp-sasl") },
                    data);

                return node;
            }
            throw new Exception("Auth response error");
        }

        protected void processChallenge(ProtocolTreeNode node)
        {
            _challengeBytes = node.data;
        }

        protected bool processInboundData(byte[] msgdata, bool autoReceipt = true)
        {
            try
            {
                ProtocolTreeNode node = reader.nextTree(msgdata);
                if (node != null)
                {
                    if (ProtocolTreeNode.TagEquals(node, "challenge"))
                    {
                        processChallenge(node);
                    }
                    else if (ProtocolTreeNode.TagEquals(node, "success"))
                    {
                        loginStatus = CONNECTION_STATUS.LOGGEDIN;
                        accountinfo = new AccountInfo(node.GetAttribute("status"),
                                                           node.GetAttribute("kind"),
                                                           node.GetAttribute("creation"),
                                                           node.GetAttribute("expiration"));
                        fireOnLoginSuccess(phoneNumber, node.GetData());
                    }
                    else if (ProtocolTreeNode.TagEquals(node, "failure"))
                    {
                        loginStatus = CONNECTION_STATUS.UNAUTHORIZED;
                        fireOnLoginFailed(node.children.FirstOrDefault().tag);
                    }

                    if (ProtocolTreeNode.TagEquals(node, "receipt"))
                    {
                        string from = node.GetAttribute("from");
                        string id = node.GetAttribute("id");
                        string type = node.GetAttribute("type") ?? "delivery";
                        switch (type)
                        {
                            case "delivery":
                                //delivered to target
                                fireOnGetMessageReceivedClient(from, id);
                                break;
                            case "read":
                                //read by target
                                //todo
                                break;
                            case "played":
                                //played by target
                                //todo
                                break;
                        }

                        //send ack
                        SendNotificationAck(node, type);
                    }

                    if (ProtocolTreeNode.TagEquals(node, "message"))
                    {
                        handleMessage(node, autoReceipt);
                    }


                    if (ProtocolTreeNode.TagEquals(node, "iq"))
                    {
                        handleIq(node);
                    }

                    if (ProtocolTreeNode.TagEquals(node, "stream:error"))
                    {
                        var textNode = node.GetChild("text");
                        if (textNode != null)
                        {
                            string content = SYSEncoding.GetString(textNode.GetData());
                            DebugAdapter.Instance.fireOnPrintDebug("Error : " + content);
                        }
                        Disconnect();
                    }

                    if (ProtocolTreeNode.TagEquals(node, "presence"))
                    {
                        //presence node
                        fireOnGetPresence(node.GetAttribute("from"), node.GetAttribute("type"));
                    }

                    if (node.tag == "ib")
                    {
                        foreach (ProtocolTreeNode child in node.children)
                        {
                            switch (child.tag)
                            {
                                case "dirty":
                                    SendClearDirty(child.GetAttribute("type"));
                                    break;
                                case "offline":
                                    //this.SendQrSync(null);
                                    break;
                                default:
                                    throw new NotImplementedException(node.NodeString());
                            }
                        }
                    }

                    if (node.tag == "chatstate")
                    {
                        string state = node.children.FirstOrDefault().tag;
                        switch (state)
                        {
                            case "composing":
                                fireOnGetTyping(node.GetAttribute("from"));
                                break;
                            case "paused":
                                fireOnGetPaused(node.GetAttribute("from"));
                                break;
                            default:
                                throw new NotImplementedException(node.NodeString());
                        }
                    }

                    if (node.tag == "ack")
                    {
                        string cls = node.GetAttribute("class");
                        if (cls == "message")
                        {
                            //server receipt
                            fireOnGetMessageReceivedServer(node.GetAttribute("from"), node.GetAttribute("id"));
                        }
                    }

                    if (node.tag == "notification")
                    {
                        handleNotification(node);
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }

        protected void handleMessage(ProtocolTreeNode node, bool autoReceipt)
        {
            if (!string.IsNullOrEmpty(node.GetAttribute("notify")))
            {
                string name = node.GetAttribute("notify");
                fireOnGetContactName(node.GetAttribute("from"), name);
            }
            if (node.GetAttribute("type") == "error")
            {
                throw new NotImplementedException(node.NodeString());
            }
            if (node.GetChild("body") != null)
            {
                //text message
                fireOnGetMessage(node, node.GetAttribute("from"), node.GetAttribute("id"), node.GetAttribute("notify"), Encoding.UTF8.GetString(node.GetChild("body").GetData()), autoReceipt);
                if (autoReceipt)
                {
                    sendMessageReceived(node);
                }
            }
            if (node.GetChild("media") != null)
            {
                ProtocolTreeNode media = node.GetChild("media");
                //media message

                //define variables in switch
                string file, url, from, id;
                int size;
                byte[] preview, dat;
                id = node.GetAttribute("id");
                from = node.GetAttribute("from");
                switch (media.GetAttribute("type"))
                {
                    case "image":
                        url = media.GetAttribute("url");
                        file = media.GetAttribute("file");
                        size = Int32.Parse(media.GetAttribute("size"));
                        preview = media.GetData();
                        fireOnGetMessageImage(node, from, id, file, size, url, preview);
                        break;
                    case "audio":
                        file = media.GetAttribute("file");
                        size = Int32.Parse(media.GetAttribute("size"));
                        url = media.GetAttribute("url");
                        preview = media.GetData();
                        fireOnGetMessageAudio(node, from, id, file, size, url, preview);
                        break;
                    case "video":
                        file = media.GetAttribute("file");
                        size = Int32.Parse(media.GetAttribute("size"));
                        url = media.GetAttribute("url");
                        preview = media.GetData();
                        fireOnGetMessageVideo(node, from, id, file, size, url, preview);
                        break;
                    case "location":
                        double lon = double.Parse(media.GetAttribute("longitude"), CultureInfo.InvariantCulture);
                        double lat = double.Parse(media.GetAttribute("latitude"), CultureInfo.InvariantCulture);
                        preview = media.GetData();
                        name = media.GetAttribute("name");
                        url = media.GetAttribute("url");
                        fireOnGetMessageLocation(node, from, id, lon, lat, url, name, preview);
                        break;
                    case "vcard":
                        ProtocolTreeNode vcard = media.GetChild("vcard");
                        name = vcard.GetAttribute("name");
                        dat = vcard.GetData();
                        fireOnGetMessageVcard(node, from, id, name, dat);
                        break;
                }
                sendMessageReceived(node);
            }
        }

        protected void handleIq(ProtocolTreeNode node)
        {
            if (node.GetAttribute("type") == "error")
            {
                fireOnError(node.GetAttribute("id"), node.GetAttribute("from"), Int32.Parse(node.GetChild("error").GetAttribute("code")), node.GetChild("error").GetAttribute("text"));
            }
            if (node.GetChild("sync") != null)
            {
                //sync result
                ProtocolTreeNode sync = node.GetChild("sync");
                ProtocolTreeNode existing = sync.GetChild("in");
                ProtocolTreeNode nonexisting = sync.GetChild("out");
                //process existing first
                Dictionary<string, string> existingUsers = new Dictionary<string, string>();
                if (existing != null)
                {
                    foreach (ProtocolTreeNode child in existing.GetAllChildren())
                    {
                        existingUsers.Add(Encoding.UTF8.GetString(child.GetData()), child.GetAttribute("jid"));
                    }
                }
                //now process failed numbers
                List<string> failedNumbers = new List<string>();
                if (nonexisting != null)
                {
                    foreach (ProtocolTreeNode child in nonexisting.GetAllChildren())
                    {
                        failedNumbers.Add(Encoding.UTF8.GetString(child.GetData()));
                    }
                }
                int index = 0;
                Int32.TryParse(sync.GetAttribute("index"), out index);
                fireOnGetSyncResult(index, sync.GetAttribute("sid"), existingUsers, failedNumbers.ToArray());
            }
            if (node.GetAttribute("type").Equals("result", StringComparison.OrdinalIgnoreCase)
                && node.GetChild("query") != null
            )
            {
                //last seen
                DateTime lastSeen = DateTime.Now.AddSeconds(double.Parse(node.children.FirstOrDefault().GetAttribute("seconds")) * -1);
                fireOnGetLastSeen(node.GetAttribute("from"), lastSeen);
            }
            if (node.GetAttribute("type").Equals("result", StringComparison.OrdinalIgnoreCase)
                && (node.GetChild("media") != null || node.GetChild("duplicate") != null)
                )
            {
                //media upload
                uploadResponse = node;
            }
            if (node.GetAttribute("type").Equals("result", StringComparison.OrdinalIgnoreCase)
                && node.GetChild("picture") != null
                )
            {
                //profile picture
                string from = node.GetAttribute("from");
                string id = node.GetChild("picture").GetAttribute("id");
                byte[] dat = node.GetChild("picture").GetData();
                string type = node.GetChild("picture").GetAttribute("type");
                if (type == "preview")
                {
                    fireOnGetPhotoPreview(from, id, dat);
                }
                else
                {
                    fireOnGetPhoto(from, id, dat);
                }
            }
            if (node.GetAttribute("type").Equals("get", StringComparison.OrdinalIgnoreCase)
                && node.GetChild("ping") != null)
            {
                SendPong(node.GetAttribute("id"));
            }
            if (node.GetAttribute("type").Equals("result", StringComparison.OrdinalIgnoreCase)
                && node.GetChild("group") != null)
            {
                //group(s) info
                List<WaGroupInfo> groups = new List<WaGroupInfo>();
                foreach (ProtocolTreeNode group in node.children)
                {
                    groups.Add(new WaGroupInfo(
                        group.GetAttribute("id"),
                        group.GetAttribute("owner"),
                        group.GetAttribute("creation"),
                        group.GetAttribute("subject"),
                        group.GetAttribute("s_t"),
                        group.GetAttribute("s_o")
                        ));
                }
                fireOnGetGroups(groups.ToArray());
            }
            if (node.GetAttribute("type").Equals("result", StringComparison.OrdinalIgnoreCase)
                && node.GetChild("participant") != null)
            {
                //group participants
                List<string> participants = new List<string>();
                foreach (ProtocolTreeNode part in node.GetAllChildren())
                {
                    if (part.tag == "participant" && !string.IsNullOrEmpty(part.GetAttribute("jid")))
                    {
                        participants.Add(part.GetAttribute("jid"));
                    }
                }
                fireOnGetGroupParticipants(node.GetAttribute("from"), participants.ToArray());
            }
            if (node.GetAttribute("type") == "result" && node.GetChild("status") != null)
            {
                foreach (ProtocolTreeNode status in node.GetChild("status").GetAllChildren())
                {
                    fireOnGetStatus(status.GetAttribute("jid"),
                        "result",
                        null,
                        SYSEncoding.GetString(status.GetData()));
                }
            }
            if (node.GetAttribute("type") == "result" && node.GetChild("privacy") != null)
            {
                Dictionary<VisibilityCategory, VisibilitySetting> settings = new Dictionary<VisibilityCategory, VisibilitySetting>();
                foreach (ProtocolTreeNode child in node.GetChild("privacy").GetAllChildren("category"))
                {
                    settings.Add(parsePrivacyCategory(
                        child.GetAttribute("name")), 
                        parsePrivacySetting(child.GetAttribute("value"))
                    );
                }
                fireOnGetPrivacySettings(settings);
            }
        }

        protected void handleNotification(ProtocolTreeNode node)
        {
            if (!String.IsNullOrEmpty(node.GetAttribute("notify")))
            {
                fireOnGetContactName(node.GetAttribute("from"), node.GetAttribute("notify"));
            }
            string type = node.GetAttribute("type");
            switch (type)
            {
                case "picture":
                    ProtocolTreeNode child = node.children.FirstOrDefault();
                    fireOnNotificationPicture(child.tag, 
                        child.GetAttribute("jid"), 
                        child.GetAttribute("id"));
                    break;
                case "status":
                    ProtocolTreeNode child2 = node.children.FirstOrDefault();
                    fireOnGetStatus(node.GetAttribute("from"), 
                        child2.tag, 
                        node.GetAttribute("notify"), 
                        Encoding.UTF8.GetString(child2.GetData()));
                    break;
                case "subject":
                    //fire username notify
                    fireOnGetContactName(node.GetAttribute("participant"),
                        node.GetAttribute("notify"));
                    //fire subject notify
                    fireOnGetGroupSubject(node.GetAttribute("from"),
                        node.GetAttribute("participant"),
                        node.GetAttribute("notify"),
                        Encoding.UTF8.GetString(node.GetChild("body").GetData()),
                        GetDateTimeFromTimestamp(node.GetAttribute("t")));
                    break;
                case "contacts":
                    //TODO
                    break;
                case "participant":
                    string gjid = node.GetAttribute("from");
                    string t = node.GetAttribute("t");
                    foreach (ProtocolTreeNode child3 in node.GetAllChildren())
                    {
                        if (child3.tag == "add")
                        {
                            fireOnGetParticipantAdded(gjid, 
                                child3.GetAttribute("jid"), 
                                GetDateTimeFromTimestamp(t));
                        }
                        else if (child3.tag == "remove")
                        {
                            fireOnGetParticipantRemoved(gjid, 
                                child3.GetAttribute("jid"), 
                                child3.GetAttribute("author"), 
                                GetDateTimeFromTimestamp(t));
                        }
                        else if (child3.tag == "modify")
                        {
                            fireOnGetParticipantRenamed(gjid,
                                child3.GetAttribute("remove"),
                                child3.GetAttribute("add"),
                                GetDateTimeFromTimestamp(t));
                        }
                    }
                    break;
            }
            SendNotificationAck(node);
        }

        private void SendNotificationAck(ProtocolTreeNode node, string type = null)
        {
            string from = node.GetAttribute("from");
            string to = node.GetAttribute("to");
            string participant = node.GetAttribute("participant");
            string id = node.GetAttribute("id");
            if (type == null)
            {
                type = node.GetAttribute("type");
            }
            List<KeyValue> attributes = new List<KeyValue>();
            if (!string.IsNullOrEmpty(to))
            {
                attributes.Add(new KeyValue("from", to));
            }
            if (!string.IsNullOrEmpty(participant))
            {
                attributes.Add(new KeyValue("participant", participant));
            }
            attributes.AddRange(new[] {
                new KeyValue("to", from),
                new KeyValue("class", node.tag),
                new KeyValue("id", id),
                new KeyValue("type", type)
            });
            ProtocolTreeNode sendNode = new ProtocolTreeNode("ack", attributes.ToArray());
            SendNode(sendNode);
        }

        protected void sendMessageReceived(ProtocolTreeNode msg, string response = "received")
        {
            FMessage tmpMessage = new FMessage(new FMessage.FMessageIdentifierKey(msg.GetAttribute("from"), true, msg.GetAttribute("id")));
            SendMessageReceived(tmpMessage, response);
        }

        public void SendAvailableForChat(string nickName = null, bool isHidden = false)
        {
            var node = new ProtocolTreeNode("presence", new[] { new KeyValue("name", (!String.IsNullOrEmpty(nickName)?nickName:name)) });
            SendNode(node);
        }

        protected void SendClearDirty(IEnumerable<string> categoryNames)
        {
            string id = TicketCounter.MakeId("clean_dirty_");
            List<ProtocolTreeNode> children = new List<ProtocolTreeNode>();
            foreach (string category in categoryNames)
            {
                ProtocolTreeNode cat = new ProtocolTreeNode("clean", new[] { new KeyValue("type", category) });
                children.Add(cat);
            }
            var node = new ProtocolTreeNode("iq",
                                            new[]
                                                {
                                                    new KeyValue("id", id), 
                                                    new KeyValue("type", "set"),
                                                    new KeyValue("to", "s.whatsapp.net"),
                                                    new KeyValue("xmlns", "urn:xmpp:whatsapp:dirty")
                                                }, children);
            SendNode(node);
        }

        protected void SendClearDirty(string category)
        {
            SendClearDirty(new[] { category });
        }

        protected void SendDeliveredReceiptAck(string to, string id)
        {
            SendReceiptAck(to, id, "delivered");
        }

        protected void SendMessageReceived(FMessage message, string response)
        {
            ProtocolTreeNode node = new ProtocolTreeNode("receipt", new[] {
                new KeyValue("to", message.identifier_key.remote_jid),
                new KeyValue("id", message.identifier_key.id)
            });

            SendNode(node);
        }

        protected void SendNotificationReceived(string jid, string id)
        {
            var child = new ProtocolTreeNode("received", new[] { new KeyValue("xmlns", "urn:xmpp:receipts") });
            var node = new ProtocolTreeNode("message", new[] { new KeyValue("to", jid), new KeyValue("type", "notification"), new KeyValue("id", id) }, child);
            SendNode(node);
        }

        protected void SendPong(string id)
        {
            var node = new ProtocolTreeNode("iq", new[] { new KeyValue("type", "result"), new KeyValue("to", WhatsConstants.WhatsAppRealm), new KeyValue("id", id) });
            SendNode(node);
        }

        private void SendReceiptAck(string to, string id, string receiptType)
        {
            var tmpChild = new ProtocolTreeNode("ack", new[] { new KeyValue("xmlns", "urn:xmpp:receipts"), new KeyValue("type", receiptType) });
            var resultNode = new ProtocolTreeNode("message", new[]
                                                             {
                                                                 new KeyValue("to", to),
                                                                 new KeyValue("type", "chat"),
                                                                 new KeyValue("id", id)
                                                             }, tmpChild);
            SendNode(resultNode);
        }
    }
}
