using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Class_library
{
    /// <summary>
    /// Класс клиента
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Пследний индекс
        /// </summary>
        private static int lastId = 0;
        /// <summary>
        /// Имя клинета
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Id клиента
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Коснтруктор
        /// </summary>
        /// <param name="name"></param>
        public Client(string name)
        {
            Name = name;
            ClientId = lastId;
            lastId++;
            Operations = new ListOperation();
        }
        /// <summary>
        /// Список операций
        /// </summary>
        [JsonIgnore]
        public ListOperation Operations { get; set; }
        /// <summary>
        /// Список операций
        /// !!!Для подключения к гриду!!!
        /// </summary>
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
        /// <summary>
        /// Сумма всех операций
        /// </summary>
        [JsonIgnore]
        public int SumAmount { get => Operations == null ? 0 : Operations.SumAmount; }
    }
}
