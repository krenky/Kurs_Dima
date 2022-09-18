using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

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
            Operations = new ListOperation();
        }

        [JsonIgnore]
        public ListOperation Operations { get; set; }

        public ObservableCollection<Operation> ArrayOperations
        {
            get
            {
                return this.Operations.Operations;
            }
            set
            {
                this.Operations.Operations = value;
            }
        }
        [JsonIgnore]
        public int SumAmount { get => Operations == null ? 0 : Operations.SumAmount; }
    }
}
