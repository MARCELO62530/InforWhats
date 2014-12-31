namespace WhatsAppApi.Account
{
    public enum Status
    {
        Online,
        OffLine
    }
    public class Pessoa
    {
       
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public Status Status { get; set; }
       
    }
}
