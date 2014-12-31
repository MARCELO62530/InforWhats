using System;
using System.Collections.Generic;
using System.Linq;

namespace WhatsAppApi.Helper
{
    public class BinTreeNodeWriter
    {
        private List<byte> buffer;
        public KeyStream Key;

        public BinTreeNodeWriter()
        {
            buffer = new List<byte>();
        }

        public byte[] StartStream(string domain, string resource)
        {
            var attributes = new List<KeyValue>();
            buffer = new List<byte>();
            
            attributes.Add(new KeyValue("to", domain));
            attributes.Add(new KeyValue("resource", resource));
            writeListStart(attributes.Count * 2 + 1);

            buffer.Add(1);
            writeAttributes(attributes.ToArray());

            byte[] ret = flushBuffer();
            buffer.Add((byte)'W');
            buffer.Add((byte)'A');
            buffer.Add(0x1);
            buffer.Add(0x4);
            buffer.AddRange(ret);
            ret = buffer.ToArray();
            buffer = new List<byte>();
            return ret;
        }

        public byte[] Write(ProtocolTreeNode node, bool encrypt = true)
        {
            if (node == null)
            {
                buffer.Add(0);
            }
            else
            {
                DebugPrint(node.NodeString("SENT: "));
                writeInternal(node);
            }
            return flushBuffer(encrypt);
        }

        protected byte[] flushBuffer(bool encrypt = true)
        {
            byte[] data = buffer.ToArray();
            byte[] data2 = new byte[data.Length + 4];
            Buffer.BlockCopy(data, 0, data2, 0, data.Length);

            byte[] size = GetInt24(data.Length);
            if (encrypt && Key != null)
            {
                byte[] paddedData = new byte[data.Length + 4];
                Array.Copy(data, paddedData, data.Length);
                Key.EncodeMessage(paddedData, paddedData.Length - 4, 0, paddedData.Length - 4);
                data = paddedData;

                //add encryption signature
                uint encryptedBit = 0u;
                encryptedBit |= 8u;
                long dataLength = data.Length;
                size[0] = (byte)((ulong)encryptedBit << 4 | (ulong)((dataLength & 16711680L) >> 16));
                size[1] = (byte)((dataLength & 65280L) >> 8);
                size[2] = (byte)(dataLength & 255L);
            }
            byte[] ret = new byte[data.Length + 3];
            Buffer.BlockCopy(size, 0, ret, 0, 3);
            Buffer.BlockCopy(data, 0, ret, 3, data.Length);
            buffer = new List<byte>();
            return ret;
        }

        protected void writeAttributes(IEnumerable<KeyValue> attributes)
        {
            if (attributes != null)
            {
                foreach (var item in attributes)
                {
                    writeString(item.Key);
                    writeString(item.Value);
                }
            }
        }

        private byte[] GetInt16(int len)
        {
            byte[] ret = new byte[2];
            ret[0] = (byte)((len & 0xff00) >> 8);
            ret[1] = (byte)(len & 0x00ff);
            return ret;
        }

        private byte[] GetInt24(int len)
        {
            byte[] ret = new byte[3];
            ret[0] = (byte)((len & 0xf0000) >> 16);
            ret[1] = (byte)((len & 0xff00) >> 8);
            ret[2] = (byte)(len & 0xff);
            return ret;
        }

        protected void writeBytes(string bytes)
        {
            writeBytes(WhatsApp.SYSEncoding.GetBytes(bytes));
        }
        protected void writeBytes(byte[] bytes)
        {
            int len = bytes.Length;
            if (len >= 0x100)
            {
                buffer.Add(0xfd);
                writeInt24(len);
            }
            else
            {
                buffer.Add(0xfc);
                writeInt8(len);
            }
            buffer.AddRange(bytes);
        }

        protected void writeInt16(int v)
        {
            buffer.Add((byte)((v & 0xff00) >> 8));
            buffer.Add((byte)(v & 0x00ff));
        }

        protected void writeInt24(int v)
        {
            buffer.Add((byte)((v & 0xff0000) >> 16));
            buffer.Add((byte)((v & 0x00ff00) >> 8));
            buffer.Add((byte)(v & 0x0000ff));
        }

        protected void writeInt8(int v)
        {
            buffer.Add((byte)(v & 0xff));
        }

        protected void writeInternal(ProtocolTreeNode node)
        {
            int len = 1;
            if (node.attributeHash != null)
            {
                len += node.attributeHash.Count() * 2;
            }
            if (node.children.Any())
            {
                len += 1;
            }
            if (node.data.Length > 0)
            {
                len += 1;
            }
            writeListStart(len);
            writeString(node.tag);
            writeAttributes(node.attributeHash);
            if (node.data.Length > 0)
            {
                writeBytes(node.data);
            }
            if (node.children != null && node.children.Any())
            {
                writeListStart(node.children.Count());
                foreach (var item in node.children)
                {
                    writeInternal(item);
                }
            }
        }
        protected void writeJid(string user, string server)
        {
            buffer.Add(0xfa);
            if (user.Length > 0)
            {
                writeString(user);
            }
            else
            {
                writeToken(0);
            }
            writeString(server);
        }

        protected void writeListStart(int len)
        {
            if (len == 0)
            {
                buffer.Add(0x00);
            }
            else if (len < 256)
            {
                buffer.Add(0xf8);
                writeInt8(len);
            }
            else
            {
                buffer.Add(0xf9);
                writeInt16(len);
            }
        }

        protected void writeString(string tag)
        {
            int intValue = -1;
            int num = -1;
            if (new TokenDictionary().TryGetToken(tag, ref num, ref intValue))
            {
                if (num >= 0)
                {
                    writeToken(num);
                }
                writeToken(intValue);
                return;
            }
            int num2 = tag.IndexOf('@');
            if (num2 < 1)
            {
                writeBytes(tag);
                return;
            }
            string server = tag.Substring(num2 + 1);
            string user = tag.Substring(0, num2);
            writeJid(user, server);
        }

        protected void writeToken(int token)
        {
            if (token < 0xf5)
            {
                buffer.Add((byte)token);
            }
            else if (token <= 0x1f4)
            {
                buffer.Add(0xfe);
                buffer.Add((byte)(token - 0xf5));
            }
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
