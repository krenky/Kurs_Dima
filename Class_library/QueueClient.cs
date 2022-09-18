using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace Class_library
{
    /// <summary>
    /// Класс очереди клиентов
    /// </summary>
    public class QueueClient : IEnumerable<Client>, INotifyPropertyChanged
    {
        private Client[] clients;
        /// <summary>
        /// Контсрукор
        /// </summary>
        /// <param name="maxClient">Максимальное кол-во клиентов</param>
        public QueueClient(int maxClient)
        {
            Clients = new Client[maxClient];
            this.firstClient = -1;
            this.lastClient = -1;
            CountClient = 0;
        }
        /// <summary>
        /// Массив кленов
        /// </summary>
        public Client[] Clients { get => clients; 
            set 
            { 
                clients = value;
                OnPropertyChanged("Clients");
            } 
        }
        private int firstClient { get; set; }
        private int lastClient { get; set; }
        /// <summary>
        /// Кол-во клиентов
        /// </summary>
        public int CountClient { get; set; }
        /// <summary>
        /// Добавление клиента
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool AddClient(string name)
        {
            var newClient = new Client(name);
            if (CountClient == 0)
            {
                firstClient = (firstClient + 1) % Clients.Length;
                lastClient = firstClient;
                Clients[lastClient] = newClient;
                CountClient++;
                return true;
            }
            lastClient = (lastClient + 1) % Clients.Length;
            if (lastClient == firstClient)
                firstClient = (firstClient + 1) % Clients.Length;

            if (CountClient == Clients.Length)
            {
                Clients[lastClient] = newClient;
                return true;
            }
            else
            {
                Clients[lastClient] = newClient;
                CountClient++;
                return true;
            }
        }
        /// <summary>
        /// Добавление клентов
        /// !!!Для дессериализации!!!
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private bool AddClient(Client client)
        {
            var newClient = client;

            Clients = new Client[Clients.Length];
            this.firstClient = -1;
            this.lastClient = -1;
            CountClient = 0;

            if (CountClient == 0)
            {
                firstClient = (firstClient + 1) % Clients.Length;
                lastClient = firstClient;
                Clients[lastClient] = newClient;
                CountClient++;
                return true;
            }
            lastClient = (lastClient + 1) % Clients.Length;
            if (lastClient == firstClient)
                firstClient = (firstClient + 1) % Clients.Length;

            if (CountClient == Clients.Length)
            {
                Clients[lastClient] = newClient;
                return true;
            }
            else
            {
                Clients[lastClient] = newClient;
                CountClient++;
                return true;
            }
        }
        /// <summary>
        /// Удаление клиента из очереди
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            if (CountClient == 0)
                return false;
            Clients[firstClient] = null;
            if (CountClient != 1)
                firstClient = (firstClient + 1) % Clients.Length;
            else
            {
                firstClient = -1;
                lastClient = -1;
            }
            CountClient--;
            return true;
        }
        /// <summary>
        /// Сохранение структуры 
        /// </summary>
        /// <param name="fileStream"></param>
        public void Save(FileStream fileStream)
        {
            
            JsonSerializer.Serialize<Client[]>(new Utf8JsonWriter(fileStream), clients);
        }
        /// <summary>
        /// Загрузка структуры
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task<bool> Load(FileStream fileStream)
        {
            Client[] Clients = await JsonSerializer.DeserializeAsync<Client[]>(fileStream);
            foreach(var client in Clients.Where(x => x != null))
            {
                AddClient(client);
            }
            return true;
        }

        /// <summary>
        /// For Tests
        /// </summary>
        /// <returns></returns>
        public int GetIndexFirstClient()
        {
            return firstClient;
        }

        /// <summary>
        /// For Tests
        /// </summary>
        /// <returns></returns>
        public int GetIndexLastClient()
        {
            return lastClient;
        }
        /// <summary>
        /// Реализация интрфейса ienumeratr
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Client> GetEnumerator()
        {
            foreach (var client in clients)
            {
                if (client != null)
                {
                    yield return client;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach(var client in clients)
            {
                if (client != null)
                {
                    yield return client;
                }
            }
        }
        /// <summary>
        /// Событие изменения объекта
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Метод вызова события
        /// </summary>
        /// <param name="prop"></param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
