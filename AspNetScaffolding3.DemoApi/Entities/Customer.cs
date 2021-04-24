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


        public string OnlyResponse { get; private set; } = "test";


        public string OnlyRequest { get; private set; } = "test";

        public SubTest SubTest { get; set; } = new SubTest();

        public Test TestTest { get; set; }
    }

    public enum Test
    {
        TEst_TESt,
        TESTE_TEST,
        
    }

    public class SubTest
    {
        public string OnlyResponse { get; private set; } = "test";

        public string OnlyRequest { get; private set; } = "test";
    }
}