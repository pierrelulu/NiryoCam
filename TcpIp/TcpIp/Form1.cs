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
        private TCP tcp;

        public Form1()
        {
            InitializeComponent();


                            // Adresse distante             // Adresse locale      // Port
            tcp = new TCP(IPAddress.Parse("127.0.0.1"), IPAddress.Parse("127.0.0.1"), 8001);
        }

        private void boutClient_Click(object sender, System.EventArgs e)
        {
            // Charger l'image depuis un fichier
            string imagePath = "C:\\Users\\loris\\Downloads\\2225070_logo.png"; //"C:/Users/luttm/Documents/Devoirs/FISA3/Application Csharp/NiryoCam/TcpIp/image_test.png"; // Remplacez par le chemin de votre image
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            tcp.sendData(imageBytes, this.tbCom);
        }

        private void boutServeur_Click(object sender, System.EventArgs e)
        {
            byte[] imageBytes = tcp.receiveData(this.tbCom);
            
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

