using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
//using libImage;
using TcpIp;
using Serial;

namespace AcquisitionSmartek
{
    public partial class Form1 : Form
    {
        gige.IDevice m_device;
        Rectangle m_rect;
        PixelFormat m_pixelFormat;
        UInt32 m_pixelType;
        TCP tcp;
        SerialCOM Serial;
        TCPstatus camStatus = TCPstatus.CLOSED;

        public Form1()
        {
            InitializeComponent();
            tcp = new TCP(client: IPAddress.Loopback, server: getIP(), port: 8001, logger: tcpLogger);
            Serial = new SerialCOM(getCOMport(), serialLogger);

            Serial.OnInfoReceived += autoResponse;

            Thread backgroundThread = new Thread(UpdateLabelsInBackground);
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        private IPAddress getIP()
        {
            return new IPAddress(new byte[] { byte.Parse(ip1.Text), byte.Parse(ip2.Text), byte.Parse(ip3.Text), byte.Parse(ip4.Text) });
        }

        private string getCOMport()
        {
            return "COM" + nCOM.Text;
        }

        async private void boutInit_Click(object sender, EventArgs e)
        {
            camStatus = TCPstatus.CLIENT_OPEN;
            // initialize GigEVision API
            gige.GigEVisionSDK.InitGigEVisionAPI();
            gige.IGigEVisionAPI smcsVisionApi = gige.GigEVisionSDK.GetGigEVisionAPI();

            if (!smcsVisionApi.IsUsingKernelDriver())
            {
                //MessageBox.Show("Warning: Smartek Filter Driver not loaded.");
            }

            // discover all devices on network asynchronously
            await Task.Run(() =>
            {
                smcsVisionApi.FindAllDevices(3.0);
            });

            gige.IDevice[] devices = smcsVisionApi.GetAllDevices();

            //MessageBox.Show(devices.Length.ToString());
            if (devices.Length > 0)
            {
                // take first device in list
                m_device = devices[0];

                // uncomment to use specific model
                //for (int i = 0; i < devices.Length; i++)
                //{
                //    if (devices[i].GetModelName() == "GC652M")
                //    {
                //        m_device = devices[i];
                //    }
                //}

                // to change number of images in image buffer from default 10 images 
                // call SetImageBufferFrameCount() method before Connect() method
                //m_device.SetImageBufferFrameCount(20);

                if (m_device != null && m_device.Connect())
                {


                    // disable trigger mode
                    bool status = m_device.SetStringNodeValue("TriggerMode", "Off");
                    // set continuous acquisition mode
                    status = m_device.SetStringNodeValue("AcquisitionMode", "Continuous");
                    // start acquisition
                    status = m_device.SetIntegerNodeValue("TLParamsLocked", 1);
                    status = m_device.CommandNodeExecute("AcquisitionStart");
                    camStatus = TCPstatus.CLIENT_CONNECTED;
                }
            }

            if (camStatus != TCPstatus.CLIENT_CONNECTED)
            {
                camStatus = TCPstatus.CLOSED;
            }
        }

        private void boutAcquisition_Click(object sender, EventArgs e)
        {
            timAcq.Start();
        }

        private void boutStop_Click(object sender, EventArgs e)
        {
            timAcq.Stop();
        }

        async private void timAcq_Tick(object sender, EventArgs e)
        {
            timAcq.Stop();
            if (m_device != null && m_device.IsConnected())
            {
                if (!m_device.IsBufferEmpty())
                {
                    gige.IImageInfo imageInfo = null;
                    m_device.GetImageInfo(ref imageInfo);
                    if (imageInfo != null)
                    {
                        Bitmap bitmap = (Bitmap)this.pbImage.Image;
                        BitmapData bd = null;

                        ImageUtils.CopyToBitmap(imageInfo, ref bitmap, ref bd, ref m_pixelFormat, ref m_rect, ref m_pixelType);
                        //-------------------------------------------------------------------
                        //if (m_pixelFormat == PixelFormat.Format8bppIndexed)
                        //{
                        //    // set palette
                        //    ColorPalette palette = bitmap.Palette;
                        //    for (int i = 0; i < 256; i++)
                        //    {
                        //        palette.Entries[i] = Color.FromArgb(255 - i, 255 - i, 255 - i);
                        //    }
                        //    bitmap.Palette = palette;
                        //}
                        //-------------------------------------------------------------------
                        
                        // display image
                        if (bd != null)
                            bitmap.UnlockBits(bd);
                        
                        if (bitmap != null)
                        {
                            //ClImage Img = ClImage.traiter(bitmap);
                            //bitmap = (Bitmap)Img.result;
                            //this.pbImage.Height = bitmap.Height;
                            //this.pbImage.Width = bitmap.Width;
                            if (tcp.Status() == TCPstatus.CLIENT_CONNECTED) {
                                //tcp.sendImageOnce(bitmap);
                            }
                            this.pbImage.Image = bitmap;
                        }

                        this.pbImage.Invalidate();
                    }
                    // remove (pop) image from image buffer
                    m_device.PopImage(imageInfo);
                    // empty buffer
                    m_device.ClearImageBuffer();

                    GC.Collect();
                }
            }
            timAcq.Start();
        }

        private void boutQuit_Click(object sender, EventArgs e)
        {
            timAcq.Stop();
            if (m_device != null && m_device.IsConnected())
            {
                m_device.CommandNodeExecute("AcquisitionStop");
                m_device.SetIntegerNodeValue("TLParamsLocked", 0);
                m_device.Disconnect();
            }
            if(camStatus == TCPstatus.CLIENT_CONNECTED)
                gige.GigEVisionSDK.ExitGigEVisionAPI();

            this.Close();
        }

        private void changeCOMstatus(Label stateLabel, TCPstatus status, Label ipLabel, string ip)
        {
            if(ipLabel != null)
                ipLabel.Text = "Adresse IP : " + ip;
            switch (status)
            {
                case TCPstatus.CLIENT_OPEN:
                    stateLabel.BackColor = Color.Yellow;
                    stateLabel.Text = "Connection en cours";
                    break;

                case TCPstatus.CLIENT_CONNECTED:
                    stateLabel.BackColor = Color.LimeGreen;
                    stateLabel.Text = "Connecté";
                    break;

                case TCPstatus.CLOSED:
                    stateLabel.BackColor = Color.Orange;
                    stateLabel.Text = "Déconnecté";
                    break;

                default:
                    stateLabel.BackColor = Color.Red;
                    stateLabel.Text = "Erreur !";
                    break;
            }
        }

        public void changeTCPstatus()
        {
            changeCOMstatus(this.labelComTcp, tcp.Status(), null, tcp.getServer().ToString());
        }

        public void changeCAMstatus()
        {
            string ip = "0.0.0.0";
            if(camStatus == TCPstatus.CLIENT_CONNECTED && (m_device == null || !m_device.IsConnected()) )
            {
                camStatus = TCPstatus.UNKNOWN;
                gige.GigEVisionSDK.ExitGigEVisionAPI();

            }
            if (m_device != null)
            {
                try
                {
                    if (m_device.IsConnected())
                    {
                        camStatus = TCPstatus.CLIENT_CONNECTED;
                        ip = Common.IpAddrToString(m_device.GetIpAddress());
                        this.lblNomCamera.Text = m_device.GetManufacturerName() + " : " + m_device.GetModelName();
                    }
                }
                catch (Exception ex)
                {
                    // Log exception and handle it appropriately
                    Console.WriteLine("Error: " + ex.Message);
                }

                changeCOMstatus(lblComCam, camStatus, lblIPcam, ip);
            }
        }

        public void changeSerialStatus()
        {
            changeCOMstatus(lblComSerial, Serial.Status(), null, "");
        }

        private void UpdateLabelsInBackground()
        {
            while (true)
            {
                if (this.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        // Code de modification des éléments graphiques ici
                        changeTCPstatus();
                        changeCAMstatus();
                        changeSerialStatus();
                    });
                }
                Thread.Sleep(100); // Attendez 0.1 seconde
            }
        }

        async private void initComImage_Click(object sender, EventArgs e)
        {
            tcp.changeClient(getIP());
            await tcp.openClient();
        }

        private void pbImage_Click(object sender, EventArgs e)
        {

        }

        async private void autoResponse(Infos info)
        {
            //if (pictureBox1.InvokeRequired)
            //{
            //    pictureBox1.Invoke(new Action(() => pictureBox1.Image = img));
            //}
            //else
            {
                Infos response = Infos.NB_INFOS;
                if(info == Infos.READY_START)
                {
                    response = Infos.GO_START;
                }
                else if(info == Infos.READY_CAPTURE)
                {
                    response = Infos.GO_CAPTURE;
                }
                // Use the Timer class to delay the execution of the action
                if(response != Infos.NB_INFOS)
                {
                    //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    //timer.Interval = 5000; // Delay for 5 seconds
                    //timer.Tick += (sender, e) =>
                    //{
                        // Code to execute after the delay
                        Serial.SendData(response);

                        // Stop the timer
                    //    timer.Stop();
                    //};
                    //timer.Start();
                }
            }
        }

        private void initSerial_Click(object sender, EventArgs e)
        {
            Serial.changePort(getCOMport());
            Serial.open();
        }
    }
}
