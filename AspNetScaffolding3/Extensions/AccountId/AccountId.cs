namespace AspNetScaffolding.Extensions.AccountId
{
    public class AccountId
    {
        public AccountId() { }

        public AccountId(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }
}
