using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using static System.Windows.Forms.AxHost;
using System.Data;
using System.IO;
public enum TCPstatus
{
    CLIENT_OPEN,
    CLIENT_CONNECTED,
    SERVER_OPEN,
    SERVER_CONNECTED,
    CLOSED,
    UNKNOWN
}
namespace TcpIp
{
    public class TCP
    {
        private IPAddress m_ipAdrServeur = IPAddress.Loopback;
        private IPAddress m_ipAdrClient = IPAddress.Loopback;
        private TcpClient tcpClient = null;
        private TcpListener tcpServer = null;
        private CancellationTokenSource _cts;
        private readonly object _lock = new object();
        private const int KeepAliveInterval = 1000; // 1 second
        private const int KeepAliveTimeout = 10000;  // 5 seconds
        private DateTime _lastKeepAliveReceived;
        private int m_numPort = 8001;
        private TextBox _logger;
        private TCPstatus status = TCPstatus.CLOSED;
        public event Action<Image> OnImageReceived;
        public TCP()
        {
        }
        public TCP(IPAddress client, IPAddress server, int port = 8001, TextBox logger = null)
        {
            m_ipAdrClient = client;
            m_ipAdrServeur = server;
            m_numPort = port;
            _logger = logger;
        }
        public TCPstatus Status()
        {
            updateStatus();
            return status;
        }
        private void LogMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            //string tag = types[(int)type];

            string receivedData = $"[{timestamp}] {message}\r\n";
            _logger?.AppendText(receivedData);

        }
        private void updateStatus()
        {
            if (tcpClient != null)
            {
                if (tcpClient.Connected)
                {
                    status = TCPstatus.CLIENT_CONNECTED;
                }
                else
                {
                    status = TCPstatus.CLIENT_OPEN;
                }
            }
            else if (tcpServer != null)
            {
                if (tcpServer.Server.IsBound && status != TCPstatus.SERVER_CONNECTED)
                {
                    status = TCPstatus.SERVER_OPEN;
                }
                else if (tcpServer.Pending())
                {
                    status = TCPstatus.SERVER_CONNECTED;
                }
            }
            else
            {
                status = TCPstatus.CLOSED;
            }
        }
        public void changePort(int port)
        {
            m_numPort = port;
        }
        public int getPort()
        {
            return m_numPort;
        }
        public void changeClient(IPAddress client)
        {
            m_ipAdrClient = client;
        }
        public IPAddress getClient()
        {
            return m_ipAdrClient;
        }
        public void changeServer(IPAddress server)
        {
            m_ipAdrServeur = server;
        }
        public IPAddress getServer()
        {
            return m_ipAdrServeur;
        }
        public void setLogger(TextBox logger)
        {
            _logger = logger;
        }
        async public Task<bool> openClient()
        {
            if (status == TCPstatus.SERVER_OPEN || status == TCPstatus.SERVER_CONNECTED)
            {
                LogMessage("Serveur déjà en cours d'exécution");
                return false;
            }
            if (tcpClient != null || status == TCPstatus.CLIENT_CONNECTED)
            {
                LogMessage("Connexion déjà établie");
                return false;
            }
            lock (_lock)
            {
                status = TCPstatus.CLIENT_OPEN;
                tcpClient = new TcpClient();
                _cts = new CancellationTokenSource();
            }
            LogMessage("Connexion en cours...");
            while (status == TCPstatus.CLIENT_OPEN)
            {
                try
                {
                    await tcpClient.ConnectAsync(m_ipAdrServeur, m_numPort);
                    lock (_lock) { status = TCPstatus.CLIENT_CONNECTED; }
                    LogMessage("Client connected to server.");
                    _ = SendClientKeepAliveAsync();
                    _ = ReceiveKeepAliveAsync();
                }
                catch (SocketException)
                {
                    await Task.Delay(1000);
                }
            }
            return false;
        }
        public bool closeClient()
        {
            if (tcpClient != null && status == TCPstatus.CLIENT_CONNECTED)
            {
                tcpClient.Close();
                tcpClient = null;
                status = TCPstatus.CLOSED;
                LogMessage("Connexion fermée");
                return true;
            }
            tcpClient = null;
            status = TCPstatus.CLOSED;
            return false;
        }
        async public Task<bool> sendData(byte[] load)
        {
            if (tcpClient == null || status != TCPstatus.CLIENT_CONNECTED)
            {
                LogMessage("Connexion non établie");
                return false;
            }
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                byte[] sizeInfo = BitConverter.GetBytes(load.Length);
                await stream.WriteAsync(sizeInfo, 0, sizeInfo.Length);
                await stream.WriteAsync(load, 0, load.Length);
                LogMessage($"Image envoyée : {load.Length} bytes");
                return true;
            }
            catch (Exception e)
            {
                LogMessage($"Erreur lors de l'envoi de l'image : {e.Message}");
                return false;
            }
        }
        async public Task<bool> sendDataOnce(byte[] load)
        {
            bool success = true;
            success &= await openClient();
            if (success)
                success &= await sendData(load);
            if (success)
                success &= closeClient();
            return success;
        }
        async public Task<bool> sendImageOnce(Image img)
        {
            ImageConverter converter = new ImageConverter();
            byte[] imgBytes = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return await sendDataOnce(imgBytes);
        }
        async public Task<bool> sendImageOnce(Bitmap bmp)
        {
            ImageConverter converter = new ImageConverter();
            byte[] imgBytes = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            //return await sendDataOnce(imgBytes);
            return await sendData(imgBytes);
        }
        private async Task SendClientKeepAliveAsync()
        {
            while (status == TCPstatus.CLIENT_CONNECTED && !_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var keepAliveMessage = CreateMessage("KEEP_ALIVE");
                    await tcpClient.GetStream().WriteAsync(keepAliveMessage, 0, keepAliveMessage.Length);
                    LogMessage("Client sent Keep alive...");
                    await Task.Delay(KeepAliveInterval);
                    if ((DateTime.Now - _lastKeepAliveReceived).TotalMilliseconds > KeepAliveTimeout)
                    {
                        LogMessage("No keep-alive received.");
                        if (status == TCPstatus.CLIENT_CONNECTED && !_cts.Token.IsCancellationRequested)
                        {
                            closeClient();
                        }
                    }
                }
                catch (Exception e)
                {
                    LogMessage($"Failed to send keep-alive: {e.Message}");
                    if (status == TCPstatus.CLIENT_CONNECTED && !_cts.Token.IsCancellationRequested)
                    {
                        closeClient();
                    }
                }
            }
        }
        private async Task SendServerKeepAliveAsync(TcpClient client)
        {
            while (status == TCPstatus.SERVER_CONNECTED && !_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var keepAliveMessage = CreateMessage("KEEP_ALIVE");
                    await client.GetStream().WriteAsync(keepAliveMessage, 0, keepAliveMessage.Length);
                    LogMessage("Server sent Keep alive...");
                    await Task.Delay(KeepAliveInterval);
                    // Check for keep-alive timeout
                    if ((DateTime.Now - _lastKeepAliveReceived).TotalMilliseconds > KeepAliveTimeout)
                    {
                        LogMessage("No keep-alive received.");
                        if (status == TCPstatus.SERVER_CONNECTED && !_cts.Token.IsCancellationRequested)
                        {
                            closeServer();
                        }
                    }
                }
                catch (Exception e)
                {
                    LogMessage($"Failed to send keep-alive from server: {e.Message}");
                    if (status == TCPstatus.SERVER_CONNECTED && !_cts.Token.IsCancellationRequested)
                    {
                        closeServer();

                    }
                }
            }
        }
            
        async public Task<bool> openServer()
        {
            if (status == TCPstatus.CLIENT_OPEN || status == TCPstatus.CLIENT_CONNECTED)
            {
                LogMessage("Client déjà en cours d'exécution");
                return false;
            }
            if (tcpServer != null || status == TCPstatus.SERVER_CONNECTED)
            {
                LogMessage("Serveur déjà en cours d'exécution");
                return false;
            }
            lock (_lock)
            {
                status = TCPstatus.SERVER_OPEN;
                tcpServer = new TcpListener(IPAddress.Any, m_numPort);
                tcpServer.Start();
                _cts = new CancellationTokenSource();
            }
            LogMessage("Serveur en cours d'exécution...");
            while (status == TCPstatus.SERVER_OPEN)
            {
                try
                {
                    var client = await tcpServer.AcceptTcpClientAsync();
                    lock (_lock) { status = TCPstatus.SERVER_CONNECTED; }
                    LogMessage("Server connected to client.");
                    _lastKeepAliveReceived = DateTime.Now;
                    // Lancer l'envoi de keep-alive depuis le serveur
                    _ = SendServerKeepAliveAsync(client);
                    _ = receiveData(client);
                }
                catch (SocketException)
                {
                    await Task.Delay(1000); // Retry every second
                }
            }
            return true;
        }
        public bool closeServer()
        {
            if (tcpServer != null && status == TCPstatus.SERVER_CONNECTED)
            {
                tcpServer.Stop();
                status = TCPstatus.CLOSED;
                tcpServer = null;
                LogMessage("Connexion fermée");
                return true;
            }
            status = TCPstatus.CLOSED;
            tcpServer = null;
            return false;
        }
        private async Task ReceiveKeepAliveAsync()
        {
            if (tcpClient == null || status != TCPstatus.CLIENT_CONNECTED)
            {
                LogMessage("Client non connecté");
            }
            NetworkStream stream = tcpClient.GetStream();
            byte[] sizeInfo = new byte[4];
            while (status == TCPstatus.CLIENT_CONNECTED && !_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var byteCount = await stream.ReadAsync(sizeInfo, 0, sizeInfo.Length, _cts.Token);
                    int dataSize = BitConverter.ToInt32(sizeInfo, 0);
                    if (byteCount > 0 && dataSize > 0)
                    {
                        byte[] data = new byte[dataSize];
                        byteCount = await stream.ReadAsync(data, 0, data.Length, _cts.Token);
                        var message = Encoding.ASCII.GetString(data, 0, byteCount);
                        if (message == "KEEP_ALIVE")
                        {
                            _lastKeepAliveReceived = DateTime.Now;
                            LogMessage("Received keep alive from server.");
                        }
                    }
                    else
                    {
                        LogMessage("Server disconnected.");
                        if (status == TCPstatus.CLIENT_CONNECTED && !_cts.Token.IsCancellationRequested)
                        {
                            closeClient();
                        }
                    }
                }
                catch (Exception e)
                {
                    LogMessage($"Error receiving keep alive: {e.Message}");
                    if (status == TCPstatus.CLIENT_CONNECTED && !_cts.Token.IsCancellationRequested)
                    {
                        closeClient();
                    }
                }
            }
        }
        async public Task receiveData(TcpClient client)
        {
            if (tcpServer == null || status != TCPstatus.SERVER_CONNECTED)
            {
                LogMessage("Serveur non en cours d'exécution");
            }
            if (tcpServer.Server == null)
            {
                //closeServer();
                LogMessage("Aucune connexion acceptée");
            }
            
            NetworkStream stream = client.GetStream();
            byte[] sizeInfo = new byte[4];
            while (status == TCPstatus.SERVER_CONNECTED && !_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var byteCount = await stream.ReadAsync(sizeInfo, 0, sizeInfo.Length, _cts.Token);
                    int dataSize = BitConverter.ToInt32(sizeInfo, 0);
                    if (byteCount > 0 && dataSize > 0)
                    {
                        byte[] data = new byte[dataSize];
                        byteCount = await stream.ReadAsync(data, 0, data.Length, _cts.Token);
                        var message = Encoding.ASCII.GetString(data, 0, byteCount);
                        if (message == "KEEP_ALIVE")
                        {
                            _lastKeepAliveReceived = DateTime.Now;
                            LogMessage("Received keep alive from client.");
                            // Envoyer un keep-alive en réponse
                            //var keepAliveResponse = Encoding.ASCII.GetBytes("KEEP_ALIVE");
                            //await stream.WriteAsync(keepAliveResponse, 0, keepAliveResponse.Length);
                        }
                        else
                        {
                            LogMessage("Received message");
                            using (var ms = new MemoryStream(data))
                            {
                                Image img = Image.FromStream(ms);
                                OnImageReceived?.Invoke(img); // Déclencher l'événement
                            }
                        }
                    }
                    else
                    {
                        LogMessage("Client disconnected.");
                        if (status == TCPstatus.SERVER_CONNECTED && !_cts.Token.IsCancellationRequested)

                            closeClient();
                    }
                }

                catch (Exception e)
                {
                    LogMessage($"Error receiving keep alive: {e.Message}");
                    if (status == TCPstatus.CLIENT_CONNECTED && !_cts.Token.IsCancellationRequested)
                    {
                        closeClient();
                    }
                }
            }
        }
        public byte[] CreateMessage(string message)
        {
            int size = message.Length;
            byte[] sizeInfo = BitConverter.GetBytes(size);
            byte[] keepAliveMessage = sizeInfo.Concat(Encoding.ASCII.GetBytes(message)).ToArray();
            return keepAliveMessage;
        }
    }
}