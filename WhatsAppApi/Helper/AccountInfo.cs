namespace WhatsAppApi.Helper
{
    public class AccountInfo
    {
        public string Status { get; private set; }
        public string Kind { get; private set; }
        public string Creation { get; private set; }
        public string Expiration { get; private set; }

        public AccountInfo(string status, string kind, string creation, string expiration)
        {
            Status = status;
            Kind = kind;
            Creation = creation;
            Expiration = expiration;
        }

        public new string ToString()
        {
            return string.Format("Status: {0}, Kind: {1}, Creation: {2}, Expiration: {3}",
                                 Status,
                                 Kind,
                                 Creation,
                                 Expiration);
        }
    }
}
