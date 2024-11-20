using System;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TcpIp
{
    public partial class Form1 : Form
    {

        private IPAddress m_ipAdrServeur;
        private IPAddress m_ipAdrClient;
        private int m_numPort;

        public Form1()
        {
            InitializeComponent();

            m_ipAdrServeur = IPAddress.Parse("127.0.0.1");  // Adresse locale
            m_ipAdrClient = IPAddress.Parse("127.0.0.1");   // Adresse distante
            m_numPort = 8001;
        }

        public static void sendData(IPAddress client, int port, byte[] load, TextBox logger = null)
        {
            TcpClient tcpClient = new TcpClient();
            if (logger != null)
                logger.AppendText("Connexion en cours...\r\n");

            tcpClient.Connect(client, port);
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

        public static byte[] receiveData(IPAddress server, int port, TextBox logger = null)
        {
            TcpListener tcpList = new TcpListener(server, port);
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

        private void boutClient_Click(object sender, System.EventArgs e)
        {
            // Charger l'image depuis un fichier
            string imagePath = "C:\\Users\\loris\\Downloads\\2225070_logo.png"; //"C:/Users/luttm/Documents/Devoirs/FISA3/Application Csharp/NiryoCam/TcpIp/image_test.png"; // Remplacez par le chemin de votre image
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            sendData(m_ipAdrClient, m_numPort, imageBytes, this.tbCom);
        }

        private void boutServeur_Click(object sender, System.EventArgs e)
        {
            byte[] imageBytes = receiveData(m_ipAdrServeur, m_numPort, this.tbCom);
            
            Image received_image = Image.FromStream(new MemoryStream(imageBytes));

            // Sauvegarder l'image reçue
            string outputPath = "received_image.png"; // Chemin de sauvegarde
            File.WriteAllBytes(outputPath, imageBytes);
            this.tbCom.AppendText($"Image reçue et sauvegardée : {outputPath}\r\n");

        }

        private void boutQuit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}

