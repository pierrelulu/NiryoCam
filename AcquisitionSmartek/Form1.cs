﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Threading;
using libImage;
using TcpIp;

namespace AcquisitionSmartek
{
    public partial class Form1 : Form
    {
        gige.IDevice m_device;
        Rectangle m_rect;
        PixelFormat m_pixelFormat;
        UInt32 m_pixelType;
        TCP tcp;

        public Form1()
        {
            InitializeComponent();
            tcp = new TCP(client: IPAddress.Loopback, server: IPAddress.Loopback, port: 8001);
        }

        private void boutInit_Click(object sender, EventArgs e)
        {
            bool cameraConnected = false;

            // initialize GigEVision API
            gige.GigEVisionSDK.InitGigEVisionAPI();
            gige.IGigEVisionAPI smcsVisionApi = gige.GigEVisionSDK.GetGigEVisionAPI();

            if (!smcsVisionApi.IsUsingKernelDriver())
            {
                MessageBox.Show("Warning: Smartek Filter Driver not loaded.");
            }

            // discover all devices on network
            smcsVisionApi.FindAllDevices(3.0);
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
                    this.lblConnection.BackColor = Color.LimeGreen;
                    this.lblConnection.Text = "Connection établie";
                    this.lblAdrIP.BackColor = Color.LimeGreen;
                    this.lblAdrIP.Text = "Adresse IP : " + Common.IpAddrToString(m_device.GetIpAddress());
                    this.lblNomCamera.Text = m_device.GetManufacturerName() + " : " + m_device.GetModelName();

                    // disable trigger mode
                    bool status = m_device.SetStringNodeValue("TriggerMode", "Off");
                    // set continuous acquisition mode
                    status = m_device.SetStringNodeValue("AcquisitionMode", "Continuous");
                    // start acquisition
                    status = m_device.SetIntegerNodeValue("TLParamsLocked", 1);
                    status = m_device.CommandNodeExecute("AcquisitionStart");
                    cameraConnected = true;
                }
            }

            if (!cameraConnected)
            {
                this.lblAdrIP.BackColor = Color.Red;
                this.lblAdrIP.Text = "Erreur de connection!";
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

        private void timAcq_Tick(object sender, EventArgs e)
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
                            ClImage Img = ClImage.traiter(bitmap);
                            bitmap = (Bitmap)Img.result;
                            //this.pbImage.Height = bitmap.Height;
                            //this.pbImage.Width = bitmap.Width;
                            tcp.sendImage(bitmap);
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

            gige.GigEVisionSDK.ExitGigEVisionAPI();

            this.Close();
        }

    }
}
