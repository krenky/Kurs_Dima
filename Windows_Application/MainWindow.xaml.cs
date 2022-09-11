using Class_library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Windows_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QueueClient queueClient = new QueueClient(10);
        ObservableCollection<Client> clients = new ObservableCollection<Client>();
        ObservableCollection<Operation> operations = new ObservableCollection<Operation>();
        public MainWindow()
        {
            InitializeComponent();
            //ClientsGrid.ItemsSource = queueClient.Clients;
        }

        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var client = ClientsGrid.SelectedItem as Client;
            if (client == null)
                return;

            if(OperationGrid.ItemsSource == null)
                OperationGrid.ItemsSource = operations;

            bindOperation(client);
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            var name = NameClient_TextBox.Text;
            queueClient.AddClient(name);
            if (ClientsGrid.ItemsSource == null)
                ClientsGrid.ItemsSource = clients;
            clients.Clear();
            foreach(var client in queueClient)
            {
                clients.Add(client);
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            queueClient.Delete();
            clients.Clear();
            operations.Clear();
            foreach (var client in queueClient)
            {
                clients.Add(client);
            }
        }

        private void bindOperation(Client client)
        {
            operations.Clear();
            client = clients.Where(x => x.ClientId == client.ClientId).FirstOrDefault();
            if(client.Operations != null)
                foreach(var operation in client.Operations)
                {
                    operations.Add(operation);
                }
        }

        private void AddOperation_Click(object sender, RoutedEventArgs e)
        {
            var client = ClientsGrid.SelectedItem as Client;
            if (client == null)
                return;
            client = queueClient.Where(x => x.ClientId == client.ClientId).FirstOrDefault();
            int amount = Convert.ToInt32(Amount_TextBox.Text);
            client.Operations.AddOperation(amount);
            bindOperation(client);
            ClientsGrid.Items.Refresh();
        }

        private void ChangeOperation_Click(object sender, RoutedEventArgs e)
        {
            var operation = OperationGrid.SelectedItem as Operation;
            operation = operations.Where(x => x.OperationId == operation.OperationId).FirstOrDefault();
            operation.Amount = Convert.ToInt32(Amount_TextBox.Text);
            ClientsGrid.Items.Refresh();
            OperationGrid.Items.Refresh();
        }

        private void AddBeforeOperation_Click(object sender, RoutedEventArgs e)
        {
            var client = ClientsGrid.SelectedItem as Client;
            var selectedOperation = OperationGrid.SelectedItem as Operation;
            if (client == null || selectedOperation == null)
                return;
            client = queueClient.Where(x => x.ClientId == client.ClientId).FirstOrDefault();
            int amount = Convert.ToInt32(Amount_TextBox.Text);
            client.Operations.AddBeforeOperation(amount, selectedOperation.OperationId);
            bindOperation(client);
            ClientsGrid.Items.Refresh();
        }
    }
}
