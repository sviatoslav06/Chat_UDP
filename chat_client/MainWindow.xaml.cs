using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace chat_client
{
    public partial class MainWindow : Window
    {
        UdpClient client = new();
        private bool inChat = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendMessageBtnClick(object sender, RoutedEventArgs e)
        {
            if (!inChat)
            {
                MessageBox.Show("You are not a member of a chat!");
                return;
            }
            SendMessage(msgTxtBox.Text);
        }

        private void JoinBtnClick(object sender, RoutedEventArgs e)
        {
            SendMessage("<join>");
            SendMessage($"'{nameTxtBox.Text}' joined chat...");
            inChat = true;
            nameTxtBox.IsEnabled = false;
            portTxtBox.IsEnabled = false;
            ipTxtBox.IsEnabled = false;
            Listen();
        }

        private void LeaveBtnClick(object sender, RoutedEventArgs e)
        {
            SendMessage("<leave>");
            SendMessage($"'{nameTxtBox.Text}' left chat...");
            inChat = false;
            nameTxtBox.IsEnabled = true;
            portTxtBox.IsEnabled = true;
            ipTxtBox.IsEnabled = true;
        }

        private async void SendMessage(string text)
        {
            IPEndPoint serverIp = new IPEndPoint(IPAddress.Parse(ipTxtBox.Text), int.Parse(portTxtBox.Text));
            byte[] data = Encoding.UTF8.GetBytes(text);
            await client.SendAsync(data, serverIp);
        }

        private async void Listen()
        {
            while (inChat)
            {
                var res = await client.ReceiveAsync();
                string message = Encoding.UTF8.GetString(res.Buffer);
                msgList.Items.Add(message);
            }
        }
    }
}
