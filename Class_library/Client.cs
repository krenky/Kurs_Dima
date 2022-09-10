namespace Class_library
{
    public class Client
    {
        private static int lastId = 0;
        public string Name { get; set; }
        public int ClientId { get; set; }

        public Client(string name)
        {
            Name = name;
            ClientId = lastId;
            lastId++;
        }

        public ListOperation Operations { get; set; }
        public int SumAmount { get => Operations == null ? 0 : Operations.SumAmount; }
    }
}
