namespace WhatsAppApi.Account
{
  public  class Credencial:Pessoa
    {
        public string Senha { get; set; }
        public override string ToString()
        {
            return Telefone;
        }

        public bool Debug { get; set; }

        public bool Hidden { get; set; }
    }
}
