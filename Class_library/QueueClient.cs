using System.Collections.Generic;

namespace Class_library
{
    public class QueueClient
    {
        public Client[] Clients { get; set; }
        private int firstClient { get; set; }
        private int lastClient { get; set; }
        public int CountClient { get; set; }

        public bool AddClient(string name)
        {
            var newClient = new Client(name);
            if(CountClient == 0)
            {
                firstClient = (firstClient + 1) % Clients.Length;
                lastClient = firstClient;
                Clients[lastClient] = newClient;
                return true;
            }
            lastClient = (lastClient + 1) % Clients.Length;
            if(lastClient == firstClient)
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
            if(CountClient != 1)
                firstClient = (firstClient + 1) % Clients.Length;
            else
            {
                firstClient = -1;
                lastClient = -1;
            }
            CountClient--;
            return true;
        }
    }
}
