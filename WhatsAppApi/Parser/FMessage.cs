using System;
using WhatsAppApi.Account;
using WhatsAppApi.Helper;

namespace WhatsAppApi.Parser
{
    public class FMessage
    {
        public bool gap_behind;
        public FMessageIdentifierKey identifier_key;
        public double latitude;
        public string location_details;
        public string location_url;
        public double longitude;
        public int media_duration_seconds;
        public string media_mime_type;
        public string media_name;
        public long media_size;
        public string media_url;
        public Type media_wa_type;
        public bool offline;
        public string remote_resource;
        public Status status;
        public object thumb_image;
        public DateTime? timestamp;
        public bool wants_receipt;

        public WhatsUser User { get; private set; }

        public FMessage(FMessageIdentifierKey key)
        {
            status = Status.Undefined;
            gap_behind = true;
            identifier_key = key;
        }

        internal FMessage(WhatsUser remote_user, bool from_me)
        {
            status = Status.Undefined;
            gap_behind = true;
            User = remote_user;
            identifier_key = new FMessageIdentifierKey(remote_user.GetFullJid(), from_me, TicketManager.GenerateId());
        }
        internal FMessage(string remote_jid, bool from_me)
        {
            status = Status.Undefined;
            gap_behind = true;
            identifier_key = new FMessageIdentifierKey(remote_jid, from_me, TicketManager.GenerateId());
        }

        public FMessage(string remote_jid, string data, object image)
            : this(remote_jid, true)
        {
            this.data = data;
            thumb_image = image;
            timestamp = DateTime.Now;
        }
        public FMessage(WhatsUser remote_user, string data, object image)
            : this(remote_user, true)
        {
            this.data = data;
            thumb_image = image;
            timestamp = DateTime.Now;
        }

        public void AcceptVisitor(FMessageVisitor visitor)
        {
            switch (media_wa_type)
            {
                case Type.Image:
                    visitor.Image(this);
                    return;

                case Type.Audio:
                    visitor.Audio(this);
                    return;

                case Type.Video:
                    visitor.Video(this);
                    return;

                case Type.Contact:
                    visitor.Contact(this);
                    return;

                case Type.Location:
                    visitor.Location(this);
                    return;

                case Type.System:
                    visitor.System(this);
                    return;
            }
            visitor.Undefined(this);
        }

        public static Type GetMessage_WA_Type(string type)
        {
            if ((type != null) && (type.Length != 0))
            {
                if (type.ToUpper().Equals("system".ToUpper()))
                {
                    return Type.System;
                }
                if (type.ToUpper().Equals("image".ToUpper()))
                {
                    return Type.Image;
                }
                if (type.ToUpper().Equals("audio".ToUpper()))
                {
                    return Type.Audio;
                }
                if (type.ToUpper().Equals("video".ToUpper()))
                {
                    return Type.Video;
                }
                if (type.ToUpper().Equals("vcard".ToUpper()))
                {
                    return Type.Contact;
                }
                if (type.ToUpper().Equals("location".ToUpper()))
                {
                    return Type.Location;
                }
            }
            return Type.Undefined;
        }

        public static string GetMessage_WA_Type_StrValue(Type type)
        {
            if (type != Type.Undefined)
            {
                if (type == Type.System)
                {
                    return "system";
                }
                if (type == Type.Image)
                {
                    return "image";
                }
                if (type == Type.Audio)
                {
                    return "audio";
                }
                if (type == Type.Video)
                {
                    return "video";
                }
                if (type == Type.Contact)
                {
                    return "vcard";
                }
                if (type == Type.Location)
                {
                    return "location";
                }
            }
            return null;
        }

        public byte[] binary_data { get; set; }

        public string data { get; set; }

        public class Builder
        {
            internal byte[] binary_data;
            internal string data;
            internal bool? from_me;
            internal string id;
            internal double? latitude;
            internal string location_details;
            internal string location_url;
            internal double? longitude;
            internal int? media_duration_seconds;
            internal string media_name;
            internal long? media_size;
            internal string media_url;
            internal Type? media_wa_type;
            internal FMessage message;
            internal bool? offline;
            internal string remote_jid;
            internal string remote_resource;
            internal string thumb_image;
            internal DateTime? timestamp;
            internal bool? wants_receipt;

            public byte[] BinaryData()
            {
                return binary_data;
            }

            public Builder BinaryData(byte[] data)
            {
                binary_data = data;
                return this;
            }

            public FMessage Build()
            {
                if (message == null)
                {
                    return null;
                }
                if (((remote_jid != null) && from_me.HasValue) && (id != null))
                {
                    message.identifier_key = new FMessageIdentifierKey(remote_jid, from_me.Value, id);
                }
                if (remote_resource != null)
                {
                    message.remote_resource = remote_resource;
                }
                if (wants_receipt.HasValue)
                {
                    message.wants_receipt = wants_receipt.Value;
                }
                if (data != null)
                {
                    message.data = data;
                }
                if (thumb_image != null)
                {
                    message.thumb_image = thumb_image;
                }
                if (timestamp.HasValue)
                {
                    message.timestamp = timestamp.Value;
                }
                if (offline.HasValue)
                {
                    message.offline = offline.Value;
                }
                if (media_wa_type.HasValue)
                {
                    message.media_wa_type = media_wa_type.Value;
                }
                if (media_size.HasValue)
                {
                    message.media_size = media_size.Value;
                }
                if (media_duration_seconds.HasValue)
                {
                    message.media_duration_seconds = media_duration_seconds.Value;
                }
                if (media_url != null)
                {
                    message.media_url = media_url;
                }
                if (media_name != null)
                {
                    message.media_name = media_name;
                }
                if (latitude.HasValue)
                {
                    message.latitude = latitude.Value;
                }
                if (longitude.HasValue)
                {
                    message.longitude = longitude.Value;
                }
                if (location_url != null)
                {
                    message.location_url = location_url;
                }
                if (location_details != null)
                {
                    message.location_details = location_details;
                }
                if (binary_data != null)
                {
                    message.binary_data = binary_data;
                }
                return message;
            }

            public string Data()
            {
                return data;
            }

            public Builder Data(string data)
            {
                this.data = data;
                return this;
            }

            public bool? From_me()
            {
                return from_me;
            }

            public Builder From_me(bool from_me)
            {
                this.from_me = from_me;
                return this;
            }

            public string Id()
            {
                return id;
            }

            public Builder Id(string id)
            {
                this.id = id;
                return this;
            }

            public bool Instantiated()
            {
                return (message != null);
            }

            public Builder Key(FMessageIdentifierKey key)
            {
                remote_jid = key.remote_jid;
                from_me = key.from_me;
                id = key.id;
                return this;
            }

            public double? Latitude()
            {
                return latitude;
            }

            public Builder Latitude(double latitude)
            {
                this.latitude = latitude;
                return this;
            }

            public string Location_details()
            {
                return location_details;
            }

            public Builder Location_details(string details)
            {
                location_details = details;
                return this;
            }

            public string Location_url()
            {
                return location_url;
            }

            public Builder Location_url(string url)
            {
                location_url = url;
                return this;
            }

            public double? Longitude()
            {
                return longitude;
            }

            public Builder Longitude(double longitude)
            {
                this.longitude = longitude;
                return this;
            }

            public int? Media_duration_seconds()
            {
                return media_duration_seconds;
            }

            public Builder Media_duration_seconds(int media_duration_seconds)
            {
                this.media_duration_seconds = media_duration_seconds;
                return this;
            }

            public string Media_name()
            {
                return media_name;
            }

            public Builder Media_name(string media_name)
            {
                this.media_name = media_name;
                return this;
            }

            public long? Media_size()
            {
                return media_size;
            }

            public Builder Media_size(long media_size)
            {
                this.media_size = media_size;
                return this;
            }

            public string Media_url()
            {
                return media_url;
            }

            public Builder Media_url(string media_url)
            {
                this.media_url = media_url;
                return this;
            }

            public Type? Media_wa_type()
            {
                return media_wa_type;
            }

            public Builder Media_wa_type(Type media_wa_type)
            {
                this.media_wa_type = media_wa_type;
                return this;
            }

            public Builder NewIncomingInstance()
            {
                if (((remote_jid == null) || !from_me.HasValue) || (id == null))
                {
                    throw new NotSupportedException(
                        "missing required property before instantiating new incoming message");
                }
                message =
                    new FMessage(new FMessageIdentifierKey(remote_jid, from_me.Value, id));
                return this;
            }

            public Builder NewOutgoingInstance()
            {
                if (((remote_jid == null) || (data == null)) || (thumb_image == null))
                {
                    throw new NotSupportedException(
                        "missing required property before instantiating new outgoing message");
                }
                if ((id != null) || (from_me.Value && !from_me.Value))
                {
                    throw new NotSupportedException("invalid property set before instantiating new outgoing message");
                }
                message = new FMessage(remote_jid, data, thumb_image);
                return this;
            }

            public bool? Offline()
            {
                return offline;
            }

            public Builder Offline(bool offline)
            {
                this.offline = offline;
                return this;
            }

            public string Remote_jid()
            {
                return remote_jid;
            }

            public Builder Remote_jid(string remote_jid)
            {
                this.remote_jid = remote_jid;
                return this;
            }

            public string Remote_resource()
            {
                return remote_resource;
            }

            public Builder Remote_resource(string remote_resource)
            {
                this.remote_resource = remote_resource;
                return this;
            }

            public Builder SetInstance(FMessage message)
            {
                this.message = message;
                return this;
            }

            public string Thumb_image()
            {
                return thumb_image;
            }

            public Builder Thumb_image(string thumb_image)
            {
                this.thumb_image = thumb_image;
                return this;
            }

            public DateTime? Timestamp()
            {
                return timestamp;
            }

            public Builder Timestamp(DateTime? timestamp)
            {
                this.timestamp = timestamp;
                return this;
            }

            public bool? Wants_receipt()
            {
                return wants_receipt;
            }

            public Builder Wants_receipt(bool wants_receipt)
            {
                this.wants_receipt = wants_receipt;
                return this;
            }
        }

        public class FMessageIdentifierKey
        {
            public bool from_me;
            public string id;
            public string remote_jid;
            public string serverNickname;

            public FMessageIdentifierKey(string remote_jid, bool from_me, string id)
            {
                this.remote_jid = remote_jid;
                this.from_me = from_me;
                this.id = id;
            }

            public override bool Equals(object obj)
            {
                if (this != obj)
                {
                    if (obj == null)
                    {
                        return false;
                    }
                    if (GetType() != obj.GetType())
                    {
                        return false;
                    }
                    FMessageIdentifierKey key = (FMessageIdentifierKey)obj;
                    if (from_me != key.from_me)
                    {
                        return false;
                    }
                    if (id == null)
                    {
                        if (key.id != null)
                        {
                            return false;
                        }
                    }
                    else if (!id.Equals(key.id))
                    {
                        return false;
                    }
                    if (remote_jid == null)
                    {
                        if (key.remote_jid != null)
                        {
                            return false;
                        }
                    }
                    else if (!remote_jid.Equals(key.remote_jid))
                    {
                        return false;
                    }
                }
                return true;
            }

            public override int GetHashCode()
            {
                int num = 0x1f;
                int num2 = 1;
                num2 = (0x1f * 1) + (from_me ? 0x4cf : 0x4d5);
                num2 = (num * num2) + ((id == null) ? 0 : id.GetHashCode());
                return ((num * num2) + ((remote_jid == null) ? 0 : remote_jid.GetHashCode()));
            }

            public override string ToString()
            {
                return
                    string.Concat("Key[id=", id, ", from_me=", from_me, ", remote_jid=", remote_jid, "]");
            }
        }

        public enum Status
        {
            UnsentOld,
            Uploading,
            Uploaded,
            SentByClient,
            ReceivedByServer,
            ReceivedByTarget,
            NeverSend,
            ServerBounce,
            Undefined,
            Unsent
        }

        public enum Type
        {
            Audio = 2,
            Contact = 4,
            Image = 1,
            Location = 5,
            System = 7,
            Undefined = 0,
            Video = 3
        }
    }
}