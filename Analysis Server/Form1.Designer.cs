namespace Analysis_Server
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
            this.ip1 = new System.Windows.Forms.TextBox();
            this.ip2 = new System.Windows.Forms.TextBox();
            this.ip3 = new System.Windows.Forms.TextBox();
            this.ip4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tcpgroup = new System.Windows.Forms.GroupBox();
            this.tcpStatus = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.anaStatus = new System.Windows.Forms.Label();
            this.logger = new System.Windows.Forms.TextBox();
            this.tcpgroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP:             .           .           .";
            // 
            // tcpgroup
            // 
            this.tcpgroup.Controls.Add(this.tcpStatus);
            this.tcpgroup.Controls.Add(this.connectButton);
            this.tcpgroup.Controls.Add(this.ip4);
            this.tcpgroup.Controls.Add(this.ip3);
            this.tcpgroup.Controls.Add(this.ip1);
            this.tcpgroup.Controls.Add(this.ip2);
            this.tcpgroup.Controls.Add(this.label1);
            this.tcpgroup.Location = new System.Drawing.Point(12, 12);
            this.tcpgroup.Name = "tcpgroup";
            this.tcpgroup.Size = new System.Drawing.Size(273, 84);
            this.tcpgroup.TabIndex = 5;
            this.tcpgroup.TabStop = false;
            this.tcpgroup.Text = "TCP Connection";
            // 
            // tcpStatus
            // 
            this.tcpStatus.BackColor = System.Drawing.Color.Orange;
            this.tcpStatus.Location = new System.Drawing.Point(6, 54);
            this.tcpStatus.Name = "tcpStatus";
            this.tcpStatus.Size = new System.Drawing.Size(261, 23);
            this.tcpStatus.TabIndex = 6;
            this.tcpStatus.Text = "Status";
            this.tcpStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(192, 16);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 5;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(14, 102);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(341, 325);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(361, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(438, 415);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // anaStatus
            // 
            this.anaStatus.BackColor = System.Drawing.Color.Orange;
            this.anaStatus.Location = new System.Drawing.Point(291, 12);
            this.anaStatus.Name = "anaStatus";
            this.anaStatus.Size = new System.Drawing.Size(64, 84);
            this.anaStatus.TabIndex = 8;
            this.anaStatus.Text = "Status";
            this.anaStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // logger
            // 
            this.logger.Location = new System.Drawing.Point(805, 12);
            this.logger.Multiline = true;
            this.logger.Name = "logger";
            this.logger.Size = new System.Drawing.Size(227, 415);
            this.logger.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 446);
            this.Controls.Add(this.logger);
            this.Controls.Add(this.anaStatus);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tcpgroup);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tcpgroup.ResumeLayout(false);
            this.tcpgroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ip1;
        private System.Windows.Forms.TextBox ip2;
        private System.Windows.Forms.TextBox ip3;
        private System.Windows.Forms.TextBox ip4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox tcpgroup;
        private System.Windows.Forms.Label tcpStatus;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label anaStatus;
        private System.Windows.Forms.TextBox logger;
    }
}

