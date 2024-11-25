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

        async private void boutClient_Click(object sender, System.EventArgs e)
        {
            await tcp.openClient();
        }

        async private void boutServeur_Click(object sender, System.EventArgs e)
        {
            await tcp.openServer();
            byte[] imageBytes = null; //await tcp.receiveDataOnce(this.tbCom);
            Image received_image = null;
            try
            {
                if (imageBytes != null)
                {
                    received_image = Image.FromStream(new MemoryStream(imageBytes));
                    // Sauvegarder l'image reçue
                    string outputPath = "received_image.png"; // Chemin de sauvegarde
                    File.WriteAllBytes(outputPath, imageBytes);
                    this.tbCom.AppendText($"Image reçue et sauvegardée : {outputPath}\r\n");
                }
                else
                {
                    this.tbCom.AppendText($"Aucune image reçue\r\n");
                }
            }
            catch (Exception ex)
            {
                this.tbCom.AppendText($"Erreur lors de la conversion de l'image : {ex.Message}\r\n");
                return;
            }


        }

        private void boutQuit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Charger l'image depuis un fichier
            string imagePath = "C:\\Users\\loris\\Downloads\\2225070_logo.png"; //"C:/Users/luttm/Documents/Devoirs/FISA3/Application Csharp/NiryoCam/TcpIp/image_test.png"; // Remplacez par le chemin de votre image
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            tcp.sendData(imageBytes);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tcp.closeClient();
        }

        private void closerServer_Click(object sender, EventArgs e)
        {
            tcp.closeServer();
        }
    }
}

