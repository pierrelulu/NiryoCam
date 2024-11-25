using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TcpIp;
using libImage;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using System.Threading;
using System.Net;

enum processStatus
{
    WAITING_COM,
    WAITING_IMG,
    RUNNING
}

namespace Analysis_Server
{
    public partial class Form1 : Form
    {
        private TCP tcp;
        private processStatus status = processStatus.WAITING_COM;
        private ClImage imageAnalysis;

        public Form1()
        {
            InitializeComponent();

            tcp = new TCP(client: System.Net.IPAddress.Loopback, server: getIP(), port: 8001, logger: logger);
            tcp.OnImageReceived += DisplayImageInPictureBox;

            Thread backgroundThread = new Thread(UpdateLabelsInBackground);
            backgroundThread.IsBackground = true;
            backgroundThread.Start();

            imageAnalysis = new ClImage();

        }

        private IPAddress getIP()
        {
            return new IPAddress(new byte[] { byte.Parse(ip1.Text), byte.Parse(ip2.Text), byte.Parse(ip3.Text), byte.Parse(ip4.Text) });
        }

        private void UpdateLabelsInBackground()
        {
            while (true)
            {
                updateStatus();
                Thread.Sleep(100); // Attendez 0.1 seconde
            }
        }

        private void updateStatus()
        {
            if (tcpStatus.InvokeRequired)
            {
                tcpStatus.Invoke(new Action(updateStatus));
                return;
            }

            switch (tcp.Status())
            {
                case TCPstatus.SERVER_OPEN:
                    tcpStatus.BackColor = Color.Yellow;
                    tcpStatus.Text = "Open to connection";
                    break;

                case TCPstatus.SERVER_CONNECTED:
                    tcpStatus.BackColor = Color.LimeGreen;
                    tcpStatus.Text = "Connected";
                    break;

                case TCPstatus.CLOSED:
                    tcpStatus.BackColor = Color.Orange;
                    tcpStatus.Text = "Disconnected";
                    break;

                default:
                    tcpStatus.BackColor = Color.Red;
                    tcpStatus.Text = "Error !";
                    break;
            }

            switch(status)
            {
                case processStatus.WAITING_COM:
                    anaStatus.BackColor = Color.Orange;
                    anaStatus.Text = "No connection";
                    break;

                case processStatus.WAITING_IMG:
                    anaStatus.BackColor = Color.Yellow;
                    anaStatus.Text = "Connected, no images";
                    break;

                case processStatus.RUNNING:
                    anaStatus.BackColor = Color.LimeGreen;
                    anaStatus.Text = "Running";
                    break;

                default:
                    anaStatus.BackColor = Color.Red;
                    anaStatus.Text = "Error !";
                    break;
            }
        }

        async private void connectButton_Click(object sender, EventArgs e)
        {
            tcp.changeServer(getIP());
            await tcp.openServer();
        }

        async private void DisplayImageInPictureBox(Image img)
        {
            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke(new Action(() => pictureBox1.Image = img));
            }
            else
            {
                pictureBox1.Image = img;
                imageAnalysis = await ClImage.traiter(img);
                pictureBox2.Image = imageAnalysis.result;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
