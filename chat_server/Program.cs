using System.Net;
using System.Net.Sockets;
using System.Text;

const int port = 3737;
const string JOIN_CMD = "<join>";
const string LEAVE_CMD = "<leave>";
int count = 0;

UdpClient server = new(port);

//ignore dublicates
HashSet<IPEndPoint> members = new HashSet<IPEndPoint>();

while (true)
{
    IPEndPoint clientIp = null;
    byte[] data = server.Receive(ref clientIp);
    string message = Encoding.UTF8.GetString(data);
    Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] - {message} | from {clientIp}");
    switch (message)
    {
        case JOIN_CMD:
            if(count >= 10)
            {
                foreach (IPEndPoint ip in members)
                    server.Send(Encoding.UTF8.GetBytes("Wait untill someone leave chat!!!"), ip);
            }
            else
            {
                members.Add(clientIp);
                count++;
            }
            break;
        case LEAVE_CMD:
            members.Remove(clientIp);
            count--;
            break;
        default:
            foreach (IPEndPoint ip in members)
                server.Send(data, ip);
            break;
    }
}