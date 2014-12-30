namespace TesteFrm
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.txtTelDestinatario = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Abrir = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colRemetente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Resposta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(12, 277);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(507, 20);
            this.txtSend.TabIndex = 0;
            this.txtSend.Text = "oi";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(444, 303);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Enviar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(845, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(245, 259);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // txtTelDestinatario
            // 
            this.txtTelDestinatario.Location = new System.Drawing.Point(641, 305);
            this.txtTelDestinatario.Name = "txtTelDestinatario";
            this.txtTelDestinatario.Size = new System.Drawing.Size(177, 20);
            this.txtTelDestinatario.TabIndex = 5;
            this.txtTelDestinatario.Text = "5511950373839";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(896, 303);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Refresh";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Abrir,
            this.colRemetente,
            this.colDescricao,
            this.Resposta});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(806, 259);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Abrir
            // 
            dataGridViewCellStyle2.NullValue = "Abrir";
            this.Abrir.DefaultCellStyle = dataGridViewCellStyle2;
            this.Abrir.HeaderText = "Abrir";
            this.Abrir.Name = "Abrir";
            this.Abrir.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Abrir.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colRemetente
            // 
            this.colRemetente.DataPropertyName = "Remetente";
            this.colRemetente.HeaderText = "Remetente";
            this.colRemetente.Name = "colRemetente";
            // 
            // colDescricao
            // 
            this.colDescricao.DataPropertyName = "Descricao";
            this.colDescricao.HeaderText = "Descricao";
            this.colDescricao.Name = "colDescricao";
            // 
            // Resposta
            // 
            this.Resposta.HeaderText = "Resposta";
            this.Resposta.Name = "Resposta";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1102, 368);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtTelDestinatario);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSend);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Button button1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtTelDestinatario;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewLinkColumn Abrir;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemetente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn Resposta;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

