namespace AspNetScaffolding.Extensions.RequestKey
{
    public class RequestKey
    {
        public RequestKey() { }

        public RequestKey(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
