namespace AspNetScaffolding.DemoApi.Entities
{
    public class Customer
    {

        public Customer(string customerId, string otherProp)
        {
            CustomerId = customerId;
            OtherProp = otherProp;
        }

        public string CustomerId { get; private set; }

        public string OtherProp { get; private set; }
    }
}