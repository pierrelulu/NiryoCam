using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

public enum TCPstatus
{
    CLIENT_OPEN,
    CLIENT_CONNECTED,
    
    SERVER_OPEN,
    SERVER_CONNECTED,
    
    CLOSE,

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
        private Socket sock = null;
        private int m_numPort = 8001;

        private TCPstatus status = TCPstatus.CLOSE;

        public TCP()
        {

        }

        public TCP(IPAddress client, IPAddress server, int port = 8001)
        {
            m_ipAdrClient = client;
            m_ipAdrServeur = server;
            m_numPort = 8001;
        }

        public TCPstatus Status()
        {
            return status;
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

        async public Task<bool> openClient(TextBox logger = null)
        {
            if (status == TCPstatus.SERVER_OPEN || status == TCPstatus.SERVER_CONNECTED)
            {
                if(logger != null)
                    logger.AppendText("Serveur déjà en cours d'exécution\r\n");
                return false;
            }

            if(tcpClient != null || status == TCPstatus.CLIENT_OPEN || status == TCPstatus.CLIENT_CONNECTED)
            {
                if (logger != null)
                    logger.AppendText("Connexion déjà établie\r\n");
                return false;
            }

            tcpClient = new TcpClient();
            if (logger != null)
                logger.AppendText("Connexion en cours...\r\n");
            status = TCPstatus.CLIENT_OPEN;

            try
            {
                await tcpClient.ConnectAsync(m_ipAdrClient, m_numPort);
                if (logger != null)
                    logger.AppendText("Connexion établie\r\n");
                status = TCPstatus.CLIENT_CONNECTED;

                return true;
            }
            catch (Exception e)
            {
                if (logger != null)
                    logger.AppendText($"Erreur lors de l'envoi de l'image : {e.Message}\r\n");
                tcpClient = null;
                status = TCPstatus.CLOSE;
                return false;
            }
        }

        public bool closeClient(TextBox logger = null)
        {
            if (tcpClient == null || status == TCPstatus.CLOSE || status == TCPstatus.UNKNOWN)
            {
                if (logger != null)
                    logger.AppendText("Connexion déjà fermée\r\n");
            }
            else
            {
                tcpClient.Close();
                if (logger != null)
                    logger.AppendText("Connexion fermée\r\n");
            }

            tcpClient = null;
            status = TCPstatus.CLOSE;

            return true;
        }

        public bool sendData(byte[] load, TextBox logger = null)
        {
            if(tcpClient == null || status != TCPstatus.CLIENT_CONNECTED)
            {
                if (logger != null)
                    logger.AppendText("Connexion non établie\r\n");
                return false;
            }

            try
            {
                NetworkStream stream = tcpClient.GetStream();
                byte[] sizeInfo = BitConverter.GetBytes(load.Length);
                stream.Write(sizeInfo, 0, sizeInfo.Length);

                stream.Write(load, 0, load.Length);
                if (logger != null)
                    logger.AppendText($"Image envoyée : {load.Length} bytes\r\n");

                return true;

            }
            catch (Exception e)
            {
                if (logger != null)
                    logger.AppendText($"Erreur lors de l'envoi de l'image : {e.Message}\r\n");
                return false;
            }
        }


        async public Task<bool> sendDataOnce(byte[] load, TextBox logger = null)
        {
            bool success = true;

            success &= await openClient(logger);

            if(success) 
                success &= sendData(load, logger);

            if (success)
                success &= closeClient(logger);
            return success;

        }

        async public Task<bool> sendImageOnce(Image img, TextBox logger = null)
        {
            ImageConverter converter = new ImageConverter();
            byte[] imgBytes = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return await sendDataOnce(imgBytes, logger);
        }

        async public Task<bool> sendImageOnce(Bitmap bmp, TextBox logger = null)
        {
            ImageConverter converter = new ImageConverter();
            byte[] imgBytes = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            return await sendDataOnce(imgBytes, logger);
        }

        async public Task<bool> openServer(TextBox logger = null)
        {
            if(status == TCPstatus.CLIENT_OPEN || status == TCPstatus.CLIENT_CONNECTED)
            {
                if (logger != null)
                    logger.AppendText("Client déjà en cours d'exécution\r\n");
                return false;
            }   

            if (tcpServer != null || status == TCPstatus.SERVER_OPEN || status == TCPstatus.SERVER_CONNECTED)
            {
                if (logger != null)
                    logger.AppendText("Serveur déjà en cours d'exécution\r\n");
                return false;
            }

            tcpServer = new TcpListener(m_ipAdrServeur, m_numPort);
            tcpServer.Start();
            if (logger != null)
                logger.AppendText("Serveur en cours d'exécution...\r\n");
            status = TCPstatus.SERVER_OPEN;

            try
            {
                sock = await tcpServer.AcceptSocketAsync();
                status = TCPstatus.SERVER_CONNECTED;
            }
            catch (Exception e)
            {
                if (logger != null)
                    logger.AppendText($"Erreur lors de l'acceptation de la connexion : {e.Message}\r\n");
                tcpServer.Stop();
                tcpServer = null;
                status = TCPstatus.CLOSE;
                return false;
            }

            return true;
        }

        public bool closeServer(TextBox logger = null)
        {
            if(sock != null)
            {
                sock.Close();
            }

            if (tcpServer == null || status == TCPstatus.CLOSE || status == TCPstatus.UNKNOWN)
            {
                if (logger != null)
                    logger.AppendText("Serveur déjà fermé\r\n");
            }
            else
            {
                if (logger != null)
                    logger.AppendText("Serveur fermé\r\n");
                tcpServer.Stop();
            }
            
            tcpServer = null;
            status = TCPstatus.CLOSE;
            
            return true;
        }

        public byte[] receiveData(TextBox logger = null)
        {
            if (tcpServer == null || status != TCPstatus.SERVER_CONNECTED)
            {
                if (logger != null)
                    logger.AppendText("Serveur non en cours d'exécution\r\n");
                return null;
            }
            
            if (sock == null)
            {
                //closeServer();
                if (logger != null)
                    logger.AppendText("Aucune connexion acceptée\r\n");
                return null;
            }

            if (logger != null)
                logger.AppendText($"Connexion acceptée de {sock.RemoteEndPoint}\r\n");

            NetworkStream stream = new NetworkStream(sock);

            // Lire la taille des datas
            byte[] sizeInfo = new byte[4];
            try
            {
                stream.Read(sizeInfo, 0, sizeInfo.Length);
            }
            catch (Exception e)
            {
                if (logger != null)
                    logger.AppendText($"Erreur lors de la lecture de la taille des données : {e.Message}\r\n");
                return null;
            }

            int dataSize = BitConverter.ToInt32(sizeInfo, 0);

            byte[] data = new byte[dataSize];
            try
            {
                stream.Read(data, 0, data.Length);
            }
            catch (Exception e)
            {
                if (logger != null)
                    logger.AppendText($"Erreur lors de la lecture des données : {e.Message}\r\n");
                return null;
            }   

            return data;
        }

        async public Task<byte[]> receiveDataOnce(TextBox logger = null)
        {
            await openServer(logger);

            byte[] data = receiveData(logger);

            closeServer(logger);

            return data;
        }

    }
}
