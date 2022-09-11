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
        public MainWindow()
        {
            InitializeComponent();
            //ClientsGrid.ItemsSource = queueClient.Clients;
        }

        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
            foreach (var client in queueClient)
            {
                clients.Add(client);
            }
        }
    }
}
