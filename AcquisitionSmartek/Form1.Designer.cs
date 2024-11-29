namespace AcquisitionSmartek
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.boutInit = new System.Windows.Forms.Button();
            this.boutAcquisition = new System.Windows.Forms.Button();
            this.boutQuit = new System.Windows.Forms.Button();
            this.timAcq = new System.Windows.Forms.Timer(this.components);
            this.lblComCam = new System.Windows.Forms.Label();
            this.gbCamera = new System.Windows.Forms.GroupBox();
            this.lblNomCamera = new System.Windows.Forms.Label();
            this.lblIPcam = new System.Windows.Forms.Label();
            this.boutStop = new System.Windows.Forms.Button();
            this.tcpLogger = new System.Windows.Forms.TextBox();
            this.initComImage = new System.Windows.Forms.Button();
            this.labelComTcp = new System.Windows.Forms.Label();
            this.tcpgroup = new System.Windows.Forms.GroupBox();
            this.ip4 = new System.Windows.Forms.TextBox();
            this.ip3 = new System.Windows.Forms.TextBox();
            this.ip1 = new System.Windows.Forms.TextBox();
            this.ip2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.serialLogger = new System.Windows.Forms.TextBox();
            this.initSerial = new System.Windows.Forms.Button();
            this.lblComSerial = new System.Windows.Forms.Label();
            this.nCOM = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.gbCamera.SuspendLayout();
            this.tcpgroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.Location = new System.Drawing.Point(298, 12);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(587, 500);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            this.pbImage.Click += new System.EventHandler(this.pbImage_Click);
            // 
            // boutInit
            // 
            this.boutInit.Location = new System.Drawing.Point(176, 19);
            this.boutInit.Name = "boutInit";
            this.boutInit.Size = new System.Drawing.Size(75, 23);
            this.boutInit.TabIndex = 1;
            this.boutInit.Text = "Init";
            this.boutInit.UseVisualStyleBackColor = true;
            this.boutInit.Click += new System.EventHandler(this.boutInit_Click);
            // 
            // boutAcquisition
            // 
            this.boutAcquisition.Location = new System.Drawing.Point(14, 96);
            this.boutAcquisition.Name = "boutAcquisition";
            this.boutAcquisition.Size = new System.Drawing.Size(75, 23);
            this.boutAcquisition.TabIndex = 2;
            this.boutAcquisition.Text = "Acquisition";
            this.boutAcquisition.UseVisualStyleBackColor = true;
            this.boutAcquisition.Click += new System.EventHandler(this.boutAcquisition_Click);
            // 
            // boutQuit
            // 
            this.boutQuit.Location = new System.Drawing.Point(12, 489);
            this.boutQuit.Name = "boutQuit";
            this.boutQuit.Size = new System.Drawing.Size(75, 23);
            this.boutQuit.TabIndex = 3;
            this.boutQuit.Text = "Fermer";
            this.boutQuit.UseVisualStyleBackColor = true;
            this.boutQuit.Click += new System.EventHandler(this.boutQuit_Click);
            // 
            // timAcq
            // 
            this.timAcq.Interval = 1000;
            this.timAcq.Tick += new System.EventHandler(this.timAcq_Tick);
            // 
            // lblComCam
            // 
            this.lblComCam.AutoSize = true;
            this.lblComCam.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblComCam.Location = new System.Drawing.Point(11, 21);
            this.lblComCam.Name = "lblComCam";
            this.lblComCam.Size = new System.Drawing.Size(75, 13);
            this.lblComCam.TabIndex = 4;
            this.lblComCam.Text = "Non connecté";
            // 
            // gbCamera
            // 
            this.gbCamera.Controls.Add(this.lblNomCamera);
            this.gbCamera.Controls.Add(this.lblIPcam);
            this.gbCamera.Controls.Add(this.boutStop);
            this.gbCamera.Controls.Add(this.lblComCam);
            this.gbCamera.Controls.Add(this.boutInit);
            this.gbCamera.Controls.Add(this.boutAcquisition);
            this.gbCamera.Location = new System.Drawing.Point(13, 12);
            this.gbCamera.Name = "gbCamera";
            this.gbCamera.Size = new System.Drawing.Size(279, 129);
            this.gbCamera.TabIndex = 5;
            this.gbCamera.TabStop = false;
            this.gbCamera.Text = "Caméra ";
            // 
            // lblNomCamera
            // 
            this.lblNomCamera.AutoSize = true;
            this.lblNomCamera.BackColor = System.Drawing.SystemColors.Control;
            this.lblNomCamera.Location = new System.Drawing.Point(11, 73);
            this.lblNomCamera.Name = "lblNomCamera";
            this.lblNomCamera.Size = new System.Drawing.Size(43, 13);
            this.lblNomCamera.TabIndex = 7;
            this.lblNomCamera.Text = "Camera";
            // 
            // lblIPcam
            // 
            this.lblIPcam.AutoSize = true;
            this.lblIPcam.BackColor = System.Drawing.SystemColors.Control;
            this.lblIPcam.Location = new System.Drawing.Point(11, 47);
            this.lblIPcam.Name = "lblIPcam";
            this.lblIPcam.Size = new System.Drawing.Size(100, 13);
            this.lblIPcam.TabIndex = 6;
            this.lblIPcam.Text = "Adresse IP : 0.0.0.0";
            // 
            // boutStop
            // 
            this.boutStop.Location = new System.Drawing.Point(176, 96);
            this.boutStop.Name = "boutStop";
            this.boutStop.Size = new System.Drawing.Size(75, 23);
            this.boutStop.TabIndex = 6;
            this.boutStop.Text = "Arret";
            this.boutStop.UseVisualStyleBackColor = true;
            this.boutStop.Click += new System.EventHandler(this.boutStop_Click);
            // 
            // tcpLogger
            // 
            this.tcpLogger.AcceptsReturn = true;
            this.tcpLogger.Location = new System.Drawing.Point(6, 84);
            this.tcpLogger.Multiline = true;
            this.tcpLogger.Name = "tcpLogger";
            this.tcpLogger.Size = new System.Drawing.Size(267, 71);
            this.tcpLogger.TabIndex = 8;
            // 
            // initComImage
            // 
            this.initComImage.Location = new System.Drawing.Point(198, 15);
            this.initComImage.Name = "initComImage";
            this.initComImage.Size = new System.Drawing.Size(75, 23);
            this.initComImage.TabIndex = 7;
            this.initComImage.Text = "Init";
            this.initComImage.UseVisualStyleBackColor = true;
            this.initComImage.Click += new System.EventHandler(this.initComImage_Click);
            // 
            // labelComTcp
            // 
            this.labelComTcp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.labelComTcp.Location = new System.Drawing.Point(6, 42);
            this.labelComTcp.Name = "labelComTcp";
            this.labelComTcp.Size = new System.Drawing.Size(267, 33);
            this.labelComTcp.TabIndex = 4;
            this.labelComTcp.Text = "Non Connecté";
            this.labelComTcp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tcpgroup
            // 
            this.tcpgroup.Controls.Add(this.tcpLogger);
            this.tcpgroup.Controls.Add(this.initComImage);
            this.tcpgroup.Controls.Add(this.ip4);
            this.tcpgroup.Controls.Add(this.labelComTcp);
            this.tcpgroup.Controls.Add(this.ip3);
            this.tcpgroup.Controls.Add(this.ip1);
            this.tcpgroup.Controls.Add(this.ip2);
            this.tcpgroup.Controls.Add(this.label1);
            this.tcpgroup.Location = new System.Drawing.Point(13, 147);
            this.tcpgroup.Name = "tcpgroup";
            this.tcpgroup.Size = new System.Drawing.Size(279, 161);
            this.tcpgroup.TabIndex = 9;
            this.tcpgroup.TabStop = false;
            this.tcpgroup.Text = "TCP Connection";
            // 
            // ip4
            // 
            this.ip4.Location = new System.Drawing.Point(138, 18);
            this.ip4.MaxLength = 3;
            this.ip4.Name = "ip4";
            this.ip4.Size = new System.Drawing.Size(30, 20);
            this.ip4.TabIndex = 3;
            this.ip4.Text = "1";
            this.ip4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ip4.WordWrap = false;
            // 
            // ip3
            // 
            this.ip3.Location = new System.Drawing.Point(102, 18);
            this.ip3.MaxLength = 3;
            this.ip3.Name = "ip3";
            this.ip3.Size = new System.Drawing.Size(30, 20);
            this.ip3.TabIndex = 2;
            this.ip3.Text = "0";
            this.ip3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ip3.WordWrap = false;
            // 
            // ip1
            // 
            this.ip1.Location = new System.Drawing.Point(30, 18);
            this.ip1.MaxLength = 3;
            this.ip1.Name = "ip1";
            this.ip1.Size = new System.Drawing.Size(30, 20);
            this.ip1.TabIndex = 0;
            this.ip1.Text = "127";
            this.ip1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ip1.WordWrap = false;
            // 
            // ip2
            // 
            this.ip2.Location = new System.Drawing.Point(66, 18);
            this.ip2.MaxLength = 3;
            this.ip2.Name = "ip2";
            this.ip2.Size = new System.Drawing.Size(30, 20);
            this.ip2.TabIndex = 1;
            this.ip2.Text = "0";
            this.ip2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ip2.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP:             .           .           .";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.serialLogger);
            this.groupBox1.Controls.Add(this.initSerial);
            this.groupBox1.Controls.Add(this.lblComSerial);
            this.groupBox1.Controls.Add(this.nCOM);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(13, 322);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 161);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Connection";
            // 
            // serialLogger
            // 
            this.serialLogger.AcceptsReturn = true;
            this.serialLogger.Location = new System.Drawing.Point(6, 84);
            this.serialLogger.Multiline = true;
            this.serialLogger.Name = "serialLogger";
            this.serialLogger.Size = new System.Drawing.Size(267, 71);
            this.serialLogger.TabIndex = 8;
            // 
            // initSerial
            // 
            this.initSerial.Location = new System.Drawing.Point(198, 15);
            this.initSerial.Name = "initSerial";
            this.initSerial.Size = new System.Drawing.Size(75, 23);
            this.initSerial.TabIndex = 7;
            this.initSerial.Text = "Init";
            this.initSerial.UseVisualStyleBackColor = true;
            this.initSerial.Click += new System.EventHandler(this.initSerial_Click);
            // 
            // lblComSerial
            // 
            this.lblComSerial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblComSerial.Location = new System.Drawing.Point(6, 42);
            this.lblComSerial.Name = "lblComSerial";
            this.lblComSerial.Size = new System.Drawing.Size(267, 33);
            this.lblComSerial.TabIndex = 4;
            this.lblComSerial.Text = "Non Connecté";
            this.lblComSerial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nCOM
            // 
            this.nCOM.Location = new System.Drawing.Point(66, 18);
            this.nCOM.MaxLength = 3;
            this.nCOM.Name = "nCOM";
            this.nCOM.Size = new System.Drawing.Size(30, 20);
            this.nCOM.TabIndex = 0;
            this.nCOM.Text = "3";
            this.nCOM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nCOM.WordWrap = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Port : COM";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 521);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tcpgroup);
            this.Controls.Add(this.pbImage);
            this.Controls.Add(this.gbCamera);
            this.Controls.Add(this.boutQuit);
            this.Name = "Form1";
            this.Text = "Acquisition SMARTEK";
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.gbCamera.ResumeLayout(false);
            this.gbCamera.PerformLayout();
            this.tcpgroup.ResumeLayout(false);
            this.tcpgroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Button boutInit;
        private System.Windows.Forms.Button boutAcquisition;
        private System.Windows.Forms.Button boutQuit;
        private System.Windows.Forms.Timer timAcq;
        private System.Windows.Forms.Label lblComCam;
        private System.Windows.Forms.GroupBox gbCamera;
        private System.Windows.Forms.Label lblNomCamera;
        private System.Windows.Forms.Label lblIPcam;
        private System.Windows.Forms.Button boutStop;
        private System.Windows.Forms.Label labelComTcp;
        private System.Windows.Forms.Button initComImage;
        private System.Windows.Forms.TextBox tcpLogger;
        private System.Windows.Forms.GroupBox tcpgroup;
        private System.Windows.Forms.TextBox ip4;
        private System.Windows.Forms.TextBox ip3;
        private System.Windows.Forms.TextBox ip1;
        private System.Windows.Forms.TextBox ip2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox serialLogger;
        private System.Windows.Forms.Button initSerial;
        private System.Windows.Forms.Label lblComSerial;
        private System.Windows.Forms.TextBox nCOM;
        private System.Windows.Forms.Label label3;
    }
}

