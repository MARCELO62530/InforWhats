namespace TesteFrm
{
    partial class FrmInicial
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gvMsgRecebe = new System.Windows.Forms.DataGridView();
            this.txtMensagem = new System.Windows.Forms.TextBox();
            this.btnMsgEnviar = new System.Windows.Forms.Button();
            this.lvDestinatario = new System.Windows.Forms.ListView();
            this.clNome = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clTelefone = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.gvMsgRecebe)).BeginInit();
            this.SuspendLayout();
            // 
            // gvMsgRecebe
            // 
            this.gvMsgRecebe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvMsgRecebe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMsgRecebe.Location = new System.Drawing.Point(12, 12);
            this.gvMsgRecebe.Name = "gvMsgRecebe";
            this.gvMsgRecebe.Size = new System.Drawing.Size(429, 174);
            this.gvMsgRecebe.TabIndex = 0;
            // 
            // txtMensagem
            // 
            this.txtMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMensagem.Location = new System.Drawing.Point(12, 192);
            this.txtMensagem.Name = "txtMensagem";
            this.txtMensagem.Size = new System.Drawing.Size(429, 20);
            this.txtMensagem.TabIndex = 1;
            // 
            // btnMsgEnviar
            // 
            this.btnMsgEnviar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMsgEnviar.Location = new System.Drawing.Point(366, 218);
            this.btnMsgEnviar.Name = "btnMsgEnviar";
            this.btnMsgEnviar.Size = new System.Drawing.Size(75, 23);
            this.btnMsgEnviar.TabIndex = 2;
            this.btnMsgEnviar.Text = "Enviar";
            this.btnMsgEnviar.UseVisualStyleBackColor = true;
            this.btnMsgEnviar.Click += new System.EventHandler(this.btnMsgEnviar_Click);
            // 
            // lvDestinatario
            // 
            this.lvDestinatario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDestinatario.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clNome,
            this.clTelefone});
            this.lvDestinatario.Location = new System.Drawing.Point(447, 12);
            this.lvDestinatario.Name = "lvDestinatario";
            this.lvDestinatario.Size = new System.Drawing.Size(107, 174);
            this.lvDestinatario.TabIndex = 3;
            this.lvDestinatario.UseCompatibleStateImageBehavior = false;
            this.lvDestinatario.View = System.Windows.Forms.View.Tile;
            // 
            // clNome
            // 
            this.clNome.Text = "Nome";
            // 
            // clTelefone
            // 
            this.clTelefone.Text = "Telefone";
            // 
            // FrmInicial
            // 
            this.ClientSize = new System.Drawing.Size(566, 262);
            this.Controls.Add(this.lvDestinatario);
            this.Controls.Add(this.btnMsgEnviar);
            this.Controls.Add(this.txtMensagem);
            this.Controls.Add(this.gvMsgRecebe);
            this.Name = "FrmInicial";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvMsgRecebe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gvMsgRecebe;
        private System.Windows.Forms.TextBox txtMensagem;
        private System.Windows.Forms.Button btnMsgEnviar;
        private System.Windows.Forms.ListView lvDestinatario;
        private System.Windows.Forms.ColumnHeader clNome;
        private System.Windows.Forms.ColumnHeader clTelefone;
    }
}

