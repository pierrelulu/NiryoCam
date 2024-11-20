using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TcpIp
{
    public class TCP
    {
        private IPAddress m_ipAdrServeur = IPAddress.Loopback;
        private IPAddress m_ipAdrClient = IPAddress.Loopback;
        private int m_numPort = 8001;

        public TCP()
        {

        }

        public TCP(IPAddress client, IPAddress server, int port = 8001)
        {
            m_ipAdrClient = client;
            m_ipAdrServeur = server;
            m_numPort = 8001;
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

        public void sendData(byte[] load, TextBox logger = null)
        {
            TcpClient tcpClient = new TcpClient();
            if (logger != null)
                logger.AppendText("Connexion en cours...\r\n");

            tcpClient.Connect(m_ipAdrClient, m_numPort);
            if (logger != null)
                logger.AppendText("Connexion établie\r\n");

            NetworkStream stream = tcpClient.GetStream();
            byte[] sizeInfo = BitConverter.GetBytes(load.Length);
            stream.Write(sizeInfo, 0, sizeInfo.Length);

            stream.Write(load, 0, load.Length);
            if (logger != null)
                logger.AppendText($"Image envoyée : {load.Length} bytes\r\n");

            tcpClient.Close();

        }

        public void sendImage(Image img, TextBox logger = null)
        {
            ImageConverter converter = new ImageConverter();
            byte[] imgBytes = (byte[])converter.ConvertTo(img, typeof(byte[]));
            sendData(imgBytes, logger);
        }

        public void sendImage(Bitmap bmp, TextBox logger = null)
        {
            ImageConverter converter = new ImageConverter();
            byte[] imgBytes = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            sendData(imgBytes, logger);
        }

        public byte[] receiveData(TextBox logger = null)
        {
            TcpListener tcpList = new TcpListener(m_ipAdrServeur, m_numPort);
            tcpList.Start();
            if (logger != null)
                logger.AppendText("Serveur en cours d'exécution...\r\n");

            Socket sock = tcpList.AcceptSocket();
            if (logger != null)
                logger.AppendText($"Connexion acceptée de {sock.RemoteEndPoint}\r\n");

            NetworkStream stream = new NetworkStream(sock);

            // Lire la taille des datas
            byte[] sizeInfo = new byte[4];
            stream.Read(sizeInfo, 0, sizeInfo.Length);
            int dataSize = BitConverter.ToInt32(sizeInfo, 0);


            byte[] data = new byte[dataSize];
            stream.Read(data, 0, data.Length);

            tcpList.Stop();
            sock.Close();

            return data;
        }

    }
}
