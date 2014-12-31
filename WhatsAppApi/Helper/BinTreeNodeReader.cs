using System;
using System.Collections.Generic;

namespace WhatsAppApi.Helper
{
    public class BinTreeNodeReader
    {
        public KeyStream Key;
        private List<byte> buffer;

        public void SetKey(byte[] key, byte[] mac)
        {
            Key = new KeyStream(key, mac);
        }

        public ProtocolTreeNode nextTree(byte[] pInput = null, bool useDecrypt = true)
        {

            if (pInput != null && pInput.Length > 0)
            {
                buffer = new List<byte>();
                buffer.AddRange(pInput);

                int stanzaFlag = (peekInt8() & 0xF0) >> 4;
                int stanzaSize = peekInt16(1);

                int flags = stanzaFlag;
                int size = stanzaSize;

                readInt24();

                bool isEncrypted = (stanzaFlag & 8) != 0;

                if (isEncrypted)
                {
                    if (Key != null)
                    {
                        var realStanzaSize = stanzaSize - 4;
                        var macOffset = stanzaSize - 4;
                        var treeData = buffer.ToArray();
                        try
                        {
                            Key.DecodeMessage(treeData, macOffset, 0, realStanzaSize);
                        }
                        catch (Exception e)
                        {
                            DebugAdapter.Instance.fireOnPrintDebug(e);
                        }
                        buffer.Clear();
                        buffer.AddRange(treeData);
                    }
                    else
                    {
                        throw new Exception("Received encrypted message, encryption key not set");
                    }
                }

                if (stanzaSize > 0)
                {
                    ProtocolTreeNode node = nextTreeInternal();
                    if (node != null)
                        DebugPrint(node.NodeString("RECVD: "));
                    return node;
                }
            }
            return null;
        }

        protected string getToken(int token)
        {
            string tokenString = null;
            int num = -1;
            new TokenDictionary().GetToken(token, ref num, ref tokenString);
            if (tokenString == null)
            {
                token = readInt8();
                new TokenDictionary().GetToken(token, ref num, ref tokenString);
            }
            return tokenString;
        }

        protected byte[] readBytes(int token)
        {
            byte[] ret = new byte[0];
            if (token == -1)
            {
                throw new Exception("BinTreeNodeReader->readString: Invalid token " + token);
            }
            if ((token > 2) && (token < 245))
            {
                ret = WhatsApp.SYSEncoding.GetBytes(getToken(token));
            }
            else if (token == 0)
            {
                ret = new byte[0];
            }
            else if (token == 252)
            {
                int size = readInt8();
                ret = fillArray(size);
            }
            else if (token == 253)
            {
                int size = readInt24();
                ret = fillArray(size);
            }
            else if (token == 254)
            {
                int tmpToken = readInt8();
                ret = WhatsApp.SYSEncoding.GetBytes(getToken(tmpToken + 0xf5));
            }
            else if (token == 250)
            {
                string user = WhatsApp.SYSEncoding.GetString(readBytes(readInt8()));
                string server = WhatsApp.SYSEncoding.GetString(readBytes(readInt8()));
                if ((user.Length > 0) && (server.Length > 0))
                {
                    ret = WhatsApp.SYSEncoding.GetBytes(user + "@" + server);
                }
                else if (server.Length > 0)
                {
                    ret = WhatsApp.SYSEncoding.GetBytes(server);
                }
            }
            return ret;
        }

        protected IEnumerable<KeyValue> readAttributes(int size)
        {
            var attributes = new List<KeyValue>();
            int attribCount = (size - 2 + size % 2) / 2;
            for (int i = 0; i < attribCount; i++)
            {
                byte[] keyB = readBytes(readInt8());
                byte[] valueB = readBytes(readInt8());
                string key = WhatsApp.SYSEncoding.GetString(keyB);
                string value = WhatsApp.SYSEncoding.GetString(valueB);
                attributes.Add(new KeyValue(key, value));
            }
            return attributes;
        }

        protected ProtocolTreeNode nextTreeInternal()
        {
            int token1 = readInt8();
            int size = readListSize(token1);
            int token2 = readInt8();
            if (token2 == 1)
            {
                var attributes = readAttributes(size);
                return new ProtocolTreeNode("start", attributes);
            }
            if (token2 == 2)
            {
                return null;
            }
            string tag = WhatsApp.SYSEncoding.GetString(readBytes(token2));
            var tmpAttributes = readAttributes(size);

            if ((size % 2) == 1)
            {
                return new ProtocolTreeNode(tag, tmpAttributes);
            }
            int token3 = readInt8();
            if (isListTag(token3))
            {
                return new ProtocolTreeNode(tag, tmpAttributes, readList(token3));
            }

            return new ProtocolTreeNode(tag, tmpAttributes, null, readBytes(token3));
        }

        protected bool isListTag(int token)
        {
            return ((token == 248) || (token == 0) || (token == 249));
        }

        protected List<ProtocolTreeNode> readList(int token)
        {
            int size = readListSize(token);
            var ret = new List<ProtocolTreeNode>();
            for (int i = 0; i < size; i++)
            {
                ret.Add(nextTreeInternal());
            }
            return ret;
        }

        protected int readListSize(int token)
        {
            int size = 0;
            if (token == 0)
            {
                size = 0;
            }
            else if (token == 0xf8)
            {
                size = readInt8();
            }
            else if (token == 0xf9)
            {
                size = readInt16();
            }
            else
            {
                throw new Exception("BinTreeNodeReader->readListSize: Invalid token " + token);
            }
            return size;
        }

        protected int peekInt8(int offset = 0)
        {
            int ret = 0;

            if (buffer.Count >= offset + 1)
                ret = buffer[offset];

            return ret;
        }

        protected int peekInt24(int offset = 0)
        {
            int ret = 0;
            if (buffer.Count >= 3 + offset)
            {
                ret = (buffer[0 + offset] << 16) + (buffer[1 + offset] << 8) + buffer[2 + offset];
            }
            return ret;
        }
        
        protected int readInt24()
        {
            int ret = 0;
            if (buffer.Count >= 3)
            {
                ret = buffer[0] << 16;
                ret |=buffer[1] << 8;
                ret |=buffer[2] << 0;
                buffer.RemoveRange(0, 3);
            }
            return ret;
        }

        protected int peekInt16(int offset = 0)
        {
            int ret = 0;
            if (buffer.Count >= offset + 2)
            {
                ret = buffer[0+offset] << 8;
                ret |= buffer[1+offset] << 0;
            }
            return ret;
        }

        protected int readInt16()
        {
            int ret = 0;
            if (buffer.Count >= 2)
            {
                ret = buffer[0] << 8;
                ret |= buffer[1] << 0;
                buffer.RemoveRange(0, 2);
            }
            return ret;
        }

        protected int readInt8()
        {
            int ret = 0;
            if (buffer.Count >= 1)
            {
                ret = buffer[0];
                buffer.RemoveAt(0);
            }
            return ret;
        }

        protected byte[] fillArray(int len)
        {
            byte[] ret = new byte[len];
            if (buffer.Count >= len)
            {
                Buffer.BlockCopy(buffer.ToArray(), 0, ret, 0, len);
                buffer.RemoveRange(0, len);
            }
            else
            {
                throw new Exception();
            }
            return ret;
        }

        protected void DebugPrint(string debugMsg)
        {
            if (WhatsApp.DEBUG && debugMsg.Length > 0)
            {
                DebugAdapter.Instance.fireOnPrintDebug(debugMsg);
            }
        }
    }
}
