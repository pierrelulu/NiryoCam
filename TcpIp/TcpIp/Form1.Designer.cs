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
            this.SuspendLayout();
            // 
            // tbCom
            // 
            this.tbCom.Location = new System.Drawing.Point(20, 27);
            this.tbCom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbCom.Multiline = true;
            this.tbCom.Name = "tbCom";
            this.tbCom.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbCom.Size = new System.Drawing.Size(262, 270);
            this.tbCom.TabIndex = 0;
            // 
            // boutClient
            // 
            this.boutClient.Location = new System.Drawing.Point(359, 56);
            this.boutClient.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.boutClient.Name = "boutClient";
            this.boutClient.Size = new System.Drawing.Size(56, 19);
            this.boutClient.TabIndex = 1;
            this.boutClient.Text = "Client";
            this.boutClient.UseVisualStyleBackColor = true;
            this.boutClient.Click += new System.EventHandler(this.boutClient_Click);
            // 
            // boutServeur
            // 
            this.boutServeur.Location = new System.Drawing.Point(359, 27);
            this.boutServeur.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.boutQuit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.tbMessage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(262, 20);
            this.tbMessage.TabIndex = 4;
            this.tbMessage.Text = "Ceci est un message test";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 392);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.boutQuit);
            this.Controls.Add(this.boutServeur);
            this.Controls.Add(this.boutClient);
            this.Controls.Add(this.tbCom);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
    }
}

