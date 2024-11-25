namespace TcpIp
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
            this.tbCom = new System.Windows.Forms.TextBox();
            this.boutClient = new System.Windows.Forms.Button();
            this.boutServeur = new System.Windows.Forms.Button();
            this.boutQuit = new System.Windows.Forms.Button();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.closerServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbCom
            // 
            this.tbCom.Location = new System.Drawing.Point(20, 27);
            this.tbCom.Margin = new System.Windows.Forms.Padding(2);
            this.tbCom.Multiline = true;
            this.tbCom.Name = "tbCom";
            this.tbCom.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbCom.Size = new System.Drawing.Size(262, 270);
            this.tbCom.TabIndex = 0;
            // 
            // boutClient
            // 
            this.boutClient.Location = new System.Drawing.Point(400, 27);
            this.boutClient.Margin = new System.Windows.Forms.Padding(2);
            this.boutClient.Name = "boutClient";
            this.boutClient.Size = new System.Drawing.Size(56, 19);
            this.boutClient.TabIndex = 1;
            this.boutClient.Text = "Client";
            this.boutClient.UseVisualStyleBackColor = true;
            this.boutClient.Click += new System.EventHandler(this.boutClient_Click);
            // 
            // boutServeur
            // 
            this.boutServeur.Location = new System.Drawing.Point(315, 27);
            this.boutServeur.Margin = new System.Windows.Forms.Padding(2);
            this.boutServeur.Name = "boutServeur";
            this.boutServeur.Size = new System.Drawing.Size(56, 19);
            this.boutServeur.TabIndex = 2;
            this.boutServeur.Text = "Serveur";
            this.boutServeur.UseVisualStyleBackColor = true;
            this.boutServeur.Click += new System.EventHandler(this.boutServeur_Click);
            // 
            // boutQuit
            // 
            this.boutQuit.Location = new System.Drawing.Point(359, 191);
            this.boutQuit.Margin = new System.Windows.Forms.Padding(2);
            this.boutQuit.Name = "boutQuit";
            this.boutQuit.Size = new System.Drawing.Size(56, 19);
            this.boutQuit.TabIndex = 3;
            this.boutQuit.Text = "Quitter";
            this.boutQuit.UseVisualStyleBackColor = true;
            this.boutQuit.Click += new System.EventHandler(this.boutQuit_Click);
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(20, 328);
            this.tbMessage.Margin = new System.Windows.Forms.Padding(2);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(262, 20);
            this.tbMessage.TabIndex = 4;
            this.tbMessage.Text = "Ceci est un message test";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(400, 110);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 19);
            this.button1.TabIndex = 5;
            this.button1.Text = "Close (client)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(400, 68);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 19);
            this.button2.TabIndex = 6;
            this.button2.Text = "Send";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // closerServer
            // 
            this.closerServer.Location = new System.Drawing.Point(315, 110);
            this.closerServer.Margin = new System.Windows.Forms.Padding(2);
            this.closerServer.Name = "closerServer";
            this.closerServer.Size = new System.Drawing.Size(56, 19);
            this.closerServer.TabIndex = 7;
            this.closerServer.Text = "Close (server)";
            this.closerServer.UseVisualStyleBackColor = true;
            this.closerServer.Click += new System.EventHandler(this.closerServer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 392);
            this.Controls.Add(this.closerServer);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.boutQuit);
            this.Controls.Add(this.boutServeur);
            this.Controls.Add(this.boutClient);
            this.Controls.Add(this.tbCom);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCom;
        private System.Windows.Forms.Button boutClient;
        private System.Windows.Forms.Button boutServeur;
        private System.Windows.Forms.Button boutQuit;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button closerServer;
    }
}

