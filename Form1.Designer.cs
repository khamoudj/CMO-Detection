namespace Bearings
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
            this.pbVNSClustering = new System.Windows.Forms.PictureBox();
            this.pbCMOClustering = new System.Windows.Forms.PictureBox();
            this.btnLoadVNS = new System.Windows.Forms.Button();
            this.btnCMO = new System.Windows.Forms.Button();
            this.btnLearn = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgl = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgt = new System.Windows.Forms.DataGridView();
            this.cbFailureType = new System.Windows.Forms.ComboBox();
            this.txtDecision = new System.Windows.Forms.TextBox();
            this.lblClassNumber = new System.Windows.Forms.Label();
            this.lblFiltered = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRatio = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbVNSClustering)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCMOClustering)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgl)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgt)).BeginInit();
            this.SuspendLayout();
            // 
            // pbVNSClustering
            // 
            this.pbVNSClustering.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbVNSClustering.Location = new System.Drawing.Point(12, 12);
            this.pbVNSClustering.Name = "pbVNSClustering";
            this.pbVNSClustering.Size = new System.Drawing.Size(336, 299);
            this.pbVNSClustering.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbVNSClustering.TabIndex = 0;
            this.pbVNSClustering.TabStop = false;
            // 
            // pbCMOClustering
            // 
            this.pbCMOClustering.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCMOClustering.Location = new System.Drawing.Point(354, 12);
            this.pbCMOClustering.Name = "pbCMOClustering";
            this.pbCMOClustering.Size = new System.Drawing.Size(336, 299);
            this.pbCMOClustering.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCMOClustering.TabIndex = 1;
            this.pbCMOClustering.TabStop = false;
            // 
            // btnLoadVNS
            // 
            this.btnLoadVNS.Location = new System.Drawing.Point(12, 317);
            this.btnLoadVNS.Name = "btnLoadVNS";
            this.btnLoadVNS.Size = new System.Drawing.Size(336, 23);
            this.btnLoadVNS.TabIndex = 2;
            this.btnLoadVNS.Text = "Input VNS Clustering graphical representation";
            this.btnLoadVNS.UseVisualStyleBackColor = true;
            this.btnLoadVNS.Click += new System.EventHandler(this.btnLoadVNS_Click);
            // 
            // btnCMO
            // 
            this.btnCMO.Enabled = false;
            this.btnCMO.Location = new System.Drawing.Point(12, 346);
            this.btnCMO.Name = "btnCMO";
            this.btnCMO.Size = new System.Drawing.Size(336, 23);
            this.btnCMO.TabIndex = 3;
            this.btnCMO.Text = "Run CMO Clustering";
            this.btnCMO.UseVisualStyleBackColor = true;
            this.btnCMO.Click += new System.EventHandler(this.btnCMO_Click);
            // 
            // btnLearn
            // 
            this.btnLearn.Enabled = false;
            this.btnLearn.Location = new System.Drawing.Point(189, 374);
            this.btnLearn.Name = "btnLearn";
            this.btnLearn.Size = new System.Drawing.Size(159, 23);
            this.btnLearn.TabIndex = 4;
            this.btnLearn.Text = "Run Learning phase";
            this.btnLearn.UseVisualStyleBackColor = true;
            this.btnLearn.Click += new System.EventHandler(this.btnLearn_Click);
            // 
            // btnTest
            // 
            this.btnTest.Enabled = false;
            this.btnTest.Location = new System.Drawing.Point(12, 404);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(171, 23);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Run Test Phase";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(357, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "CMO Clustering results";
            this.label1.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(696, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(441, 442);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(433, 416);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Learning step results";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgl
            // 
            this.dgl.AllowUserToAddRows = false;
            this.dgl.AllowUserToDeleteRows = false;
            this.dgl.AllowUserToResizeColumns = false;
            this.dgl.AllowUserToResizeRows = false;
            this.dgl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgl.ColumnHeadersVisible = false;
            this.dgl.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dgl.Location = new System.Drawing.Point(6, 6);
            this.dgl.Name = "dgl";
            this.dgl.RowHeadersVisible = false;
            this.dgl.Size = new System.Drawing.Size(421, 404);
            this.dgl.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgt);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(433, 416);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Test step results";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgt
            // 
            this.dgt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgt.Location = new System.Drawing.Point(6, 6);
            this.dgt.Name = "dgt";
            this.dgt.Size = new System.Drawing.Size(421, 404);
            this.dgt.TabIndex = 1;
            // 
            // cbFailureType
            // 
            this.cbFailureType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFailureType.Enabled = false;
            this.cbFailureType.FormattingEnabled = true;
            this.cbFailureType.Items.AddRange(new object[] {
            "Select Failure Type",
            "Ball 7",
            "Ball 14",
            "Ball 21",
            "Ball 28",
            "Inner Race 7",
            "Inner Race 14",
            "Inner Race 21",
            "Inner Race 28",
            "Outer Race Centered 7",
            "Outer Race Centered 14",
            "Outer Race Centered 21",
            "Outer Race Opposite 7",
            "Outer Race Opposite 21",
            "Outer Race Orthogonal 7",
            "Outer Race Orthogonal 21"});
            this.cbFailureType.Location = new System.Drawing.Point(12, 375);
            this.cbFailureType.Name = "cbFailureType";
            this.cbFailureType.Size = new System.Drawing.Size(171, 21);
            this.cbFailureType.TabIndex = 9;
            // 
            // txtDecision
            // 
            this.txtDecision.BackColor = System.Drawing.Color.White;
            this.txtDecision.Enabled = false;
            this.txtDecision.ForeColor = System.Drawing.Color.Black;
            this.txtDecision.Location = new System.Drawing.Point(189, 405);
            this.txtDecision.Name = "txtDecision";
            this.txtDecision.ReadOnly = true;
            this.txtDecision.Size = new System.Drawing.Size(159, 20);
            this.txtDecision.TabIndex = 10;
            this.txtDecision.Text = "No decision result...";
            // 
            // lblClassNumber
            // 
            this.lblClassNumber.AutoSize = true;
            this.lblClassNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClassNumber.Location = new System.Drawing.Point(618, 301);
            this.lblClassNumber.Name = "lblClassNumber";
            this.lblClassNumber.Size = new System.Drawing.Size(69, 15);
            this.lblClassNumber.TabIndex = 11;
            this.lblClassNumber.Text = "0 Classes";
            // 
            // lblFiltered
            // 
            this.lblFiltered.AutoSize = true;
            this.lblFiltered.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFiltered.Location = new System.Drawing.Point(357, 3);
            this.lblFiltered.Name = "lblFiltered";
            this.lblFiltered.Size = new System.Drawing.Size(139, 16);
            this.lblFiltered.TabIndex = 12;
            this.lblFiltered.Text = "No selected image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(357, 320);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "CMO Configuration :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(359, 347);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Distance ratio adjustment :";
            // 
            // txtRatio
            // 
            this.txtRatio.Location = new System.Drawing.Point(540, 345);
            this.txtRatio.Name = "txtRatio";
            this.txtRatio.Size = new System.Drawing.Size(100, 20);
            this.txtRatio.TabIndex = 15;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(189, 431);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(159, 23);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "Clear the knowledge base";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 464);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtRatio);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFiltered);
            this.Controls.Add(this.lblClassNumber);
            this.Controls.Add(this.txtDecision);
            this.Controls.Add(this.cbFailureType);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnLearn);
            this.Controls.Add(this.btnCMO);
            this.Controls.Add(this.btnLoadVNS);
            this.Controls.Add(this.pbCMOClustering);
            this.Controls.Add(this.pbVNSClustering);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bearings health state learning";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbVNSClustering)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCMOClustering)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgl)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbVNSClustering;
        private System.Windows.Forms.PictureBox pbCMOClustering;
        private System.Windows.Forms.Button btnLoadVNS;
        private System.Windows.Forms.Button btnCMO;
        private System.Windows.Forms.Button btnLearn;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cbFailureType;
        private System.Windows.Forms.TextBox txtDecision;
        private System.Windows.Forms.Label lblClassNumber;
        private System.Windows.Forms.Label lblFiltered;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRatio;
        private System.Windows.Forms.DataGridView dgl;
        private System.Windows.Forms.DataGridView dgt;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}

