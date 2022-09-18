using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;

namespace Class_library
{
    public class QueueClient : IEnumerable<Client>, INotifyPropertyChanged
    {
        private Client[] clients;

        public QueueClient(int maxClient)
        {
            Clients = new Client[maxClient];
            this.firstClient = -1;
            this.lastClient = -1;
            CountClient = 0;
        }

        public Client[] Clients { get => clients; 
            set 
            { 
                clients = value;
                OnPropertyChanged("Clients");
            } 
        }
        private int firstClient { get; set; }
        private int lastClient { get; set; }
        public int CountClient { get; set; }

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

        public void Save(FileStream fileStream)
        {
            
            JsonSerializer.Serialize<Client[]>(new Utf8JsonWriter(fileStream), clients);
        }
        public async void Load(FileStream fileStream)
        {
            Client[] Clients = await JsonSerializer.DeserializeAsync<Client[]>(fileStream);
            foreach(var client in Clients)
            {
                AddClient(client);
            }
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
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
