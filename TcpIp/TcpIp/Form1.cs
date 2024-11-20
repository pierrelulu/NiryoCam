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

        private void boutClient_Click(object sender, System.EventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            this.tbCom.AppendText("Connexion en cours...\r\n");

            tcpClient.Connect(m_ipAdrClient, m_numPort);
            this.tbCom.AppendText("Connexion établie\r\n");

            // Charger l'image depuis un fichier
            string imagePath = "C:/Users/luttm/Documents/Devoirs/FISA3/Application Csharp/NiryoCam/TcpIp/image_test.png"; // Remplacez par le chemin de votre image
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            // Envoyer la taille de l'image
            NetworkStream stream = tcpClient.GetStream();
            byte[] sizeInfo = BitConverter.GetBytes(imageBytes.Length);
            stream.Write(sizeInfo, 0, sizeInfo.Length);

            // Envoyer l'image
            stream.Write(imageBytes, 0, imageBytes.Length);
            this.tbCom.AppendText($"Image envoyée : {imagePath}\r\n");

            tcpClient.Close();
        }

        private void boutServeur_Click(object sender, System.EventArgs e)
        {
            TcpListener tcpList = new TcpListener(m_ipAdrServeur, m_numPort);
            tcpList.Start();
            this.tbCom.AppendText("Serveur en cours d'exécution...\r\n");
            this.tbCom.AppendText("Attente de connexion...\r\n");

            Socket sock = tcpList.AcceptSocket();
            this.tbCom.AppendText($"Connexion acceptée de {sock.RemoteEndPoint}\r\n");

            NetworkStream stream = new NetworkStream(sock);

            // Lire la taille de l'image
            byte[] sizeInfo = new byte[4];
            stream.Read(sizeInfo, 0, sizeInfo.Length);
            int imageSize = BitConverter.ToInt32(sizeInfo, 0);

            // Lire l'image
            byte[] imageBytes = new byte[imageSize];
            int bytesRead = 0;
            while (bytesRead < imageSize)
            {
                bytesRead += stream.Read(imageBytes, bytesRead, imageSize - bytesRead);
            }

            // Sauvegarder l'image reçue
            string outputPath = "received_image.png"; // Chemin de sauvegarde
            File.WriteAllBytes(outputPath, imageBytes);
            this.tbCom.AppendText($"Image reçue et sauvegardée : {outputPath}\r\n");

            tcpList.Stop();
            sock.Close();
        }

        private void boutQuit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}

