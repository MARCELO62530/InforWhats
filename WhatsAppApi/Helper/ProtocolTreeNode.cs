using System;
using System.Collections.Generic;
using System.Linq;

namespace WhatsAppApi.Helper
{
    public class ProtocolTreeNode
    {
        public string tag;
        public IEnumerable<KeyValue> attributeHash;
        public IEnumerable<ProtocolTreeNode> children;
        public byte[] data;

        public ProtocolTreeNode(string tag, IEnumerable<KeyValue> attributeHash, IEnumerable<ProtocolTreeNode> children = null,
                            byte[] data = null)
        {
            this.tag = tag ?? "";
            this.attributeHash = attributeHash ?? new KeyValue[0];
            this.children = children ?? new ProtocolTreeNode[0];
            this.data = new byte[0];
            if (data != null)
                this.data = data;
        }

        public ProtocolTreeNode(string tag, IEnumerable<KeyValue> attributeHash, ProtocolTreeNode children = null)
        {
            this.tag = tag ?? "";
            this.attributeHash = attributeHash ?? new KeyValue[0];
            this.children = children != null ? new[] { children } : new ProtocolTreeNode[0];
            data = new byte[0];
        }

        public ProtocolTreeNode(string tag, IEnumerable<KeyValue> attributeHash, byte[] data = null)
            : this(tag, attributeHash, new ProtocolTreeNode[0], data)
        { }

        public ProtocolTreeNode(string tag, IEnumerable<KeyValue> attributeHash)
            : this(tag, attributeHash, new ProtocolTreeNode[0], null)
        {
        }

        public string NodeString(string indent = "")
        {
            string ret = "\n" + indent + "<" + tag;
            if (attributeHash != null)
            {
                foreach (var item in attributeHash)
                {
                    ret += string.Format(" {0}=\"{1}\"", item.Key, item.Value);
                }
            }
            ret += ">";
            if (data.Length > 0)
            {
                if (data.Length <= 1024)
                {
                    ret += WhatsApp.SYSEncoding.GetString(data);
                }
                else
                {
                    ret += string.Format("--{0} byte--", data.Length);
                }
            }
            
            if (children != null && children.Count() > 0)
            {
                foreach (var item in children)
                {
                    ret += item.NodeString(indent + "  ");
                }
                ret += "\n" + indent;
            }
            ret += "</" + tag + ">";
            return ret;
        }

        public string GetAttribute(string attribute)
        {
            var ret = attributeHash.FirstOrDefault(x => x.Key.Equals(attribute));
            return (ret == null) ? null : ret.Value;
        }

        public ProtocolTreeNode GetChild(string tag)
        {
            if (children != null && children.Any())
            {
                foreach (var item in children)
                {
                    if (TagEquals(item, tag))
                    {
                        return item;
                    }
                    ProtocolTreeNode ret = item.GetChild(tag);
                    if (ret != null)
                    {
                        return ret;
                    }
                }
            }
            return null;
        }

        public IEnumerable<ProtocolTreeNode> GetAllChildren(string tag)
        {
            var tmpReturn = new List<ProtocolTreeNode>();
            if (children != null && children.Any())
            {
                foreach (var item in children)
                {
                    if (tag.Equals(item.tag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        tmpReturn.Add(item);
                    }
                    tmpReturn.AddRange(item.GetAllChildren(tag));
                }
            }
            return tmpReturn.ToArray();
        }

        public IEnumerable<ProtocolTreeNode> GetAllChildren()
        {
            return children.ToArray();
        }

        public byte[] GetData()
        {
            return data;
        }

        public static bool TagEquals(ProtocolTreeNode node, string _string)
        {
            return (((node != null) && (node.tag != null)) && node.tag.Equals(_string, StringComparison.OrdinalIgnoreCase));
        }
    }
}
