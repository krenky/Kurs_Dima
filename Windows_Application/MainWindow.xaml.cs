using Class_library;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        }
        /// <summary>
        /// Обрабочик смены фокуса в таблице клиентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var client = ClientsGrid.SelectedItem as Client;
            if (client == null)
                return;

            if(OperationGrid.ItemsSource == null)
                OperationGrid.ItemsSource = client.Operations.Operations;
            else
            {
                OperationGrid.ItemsSource = null;
                OperationGrid.ItemsSource = client.Operations.Operations;
            }
            OperationGrid.Items.Refresh();
            //bindOperation(client);
        }
        /// <summary>
        /// Обработчик нажатия кнопки добавления клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Обработчик нажатия кнопки удаления клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Обработчик нажатия кнопки добавления операции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOperation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = ClientsGrid.SelectedItem as Client;
                if (client == null)
                {
                    MessageBox.Show("Выберите пользователя");
                    return;
                }
                client = queueClient.Where(x => x.ClientId == client.ClientId).FirstOrDefault();
                int amount = Convert.ToInt32(Amount_TextBox.Text);
                client.Operations.AddOperation(amount);
                //bindOperation(client);
                ClientsGrid.Items.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Обработчик нажатия кнопки изменения операции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeOperation_Click(object sender, RoutedEventArgs e)
        {
            var operation = OperationGrid.SelectedItem as Operation;
            var client = ClientsGrid.SelectedItem as Client;
            try
            {
                client.Operations.ChangeOperation(operation.OperationId, Convert.ToInt32(Amount_TextBox.Text));
                ClientsGrid.Items.Refresh();
                OperationGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Выберите операцию которую хотите изменить");
            }
        }
        /// <summary>
        /// Обработчик нажатия кнопки сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if ((bool)saveFileDialog.ShowDialog())
                using (FileStream fs = (FileStream)saveFileDialog.OpenFile())
                {
                    queueClient.Save(fs);
                }
            MessageBox.Show("Сохранение успешно");
        }
        /// <summary>
        /// Обработчик нажатия кнопки загрузки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Load_button_Click(object sender, RoutedEventArgs e)
        {
            operations.Clear();
            clients.Clear();
            queueClient = new QueueClient(10);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
                using (FileStream fs = (FileStream)openFileDialog.OpenFile())
                {
                    await queueClient.Load(fs);
                }
            foreach(var client in queueClient)
            {
                clients.Add(client);
            }
            ClientsGrid.ItemsSource = null;
            ClientsGrid.ItemsSource = clients;
            ClientsGrid.Items.Refresh();
            MessageBox.Show("Загрузка успешна");
        }
        /// <summary>
        /// Проверка значения нажатой кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameClient_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsLetter(e.Text, 0));
        }
        /// <summary>
        /// Проверка значения нажатой кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameClient_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
        /// <summary>
        /// Проверка значения нажатой кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Amount_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
        /// <summary>
        /// Проверка значения нажатой кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Amount_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0) || e.Text[0] == '-');
        }
    }
}
