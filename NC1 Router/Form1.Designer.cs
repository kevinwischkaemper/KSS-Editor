namespace NC1_Router
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnSelectKSS = new System.Windows.Forms.Button();
            this.openKSS = new System.Windows.Forms.OpenFileDialog();
            this.btnParseDSTVs = new System.Windows.Forms.Button();
            this.btnWriteKSS = new System.Windows.Forms.Button();
            this.lblExportedKSSPath = new System.Windows.Forms.Label();
            this.lblGeneratedKSSPath = new System.Windows.Forms.Label();
            this.openExportedKSS = new System.Windows.Forms.OpenFileDialog();
            this.openGeneratedKSS = new System.Windows.Forms.OpenFileDialog();
            this.lblComplete = new System.Windows.Forms.Label();
            this.checkMultiplyBoltQuantities = new System.Windows.Forms.CheckBox();
            this.dgvSmallHoles = new System.Windows.Forms.DataGridView();
            this.lblHolesTooSmall = new System.Windows.Forms.Label();
            this.lblPL12 = new System.Windows.Forms.Label();
            this.dgvPLHalf = new System.Windows.Forms.DataGridView();
            this.dgvHeaderLengthChanges = new System.Windows.Forms.DataGridView();
            this.lblHeaderLengthChanges = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSmallHoles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLHalf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaderLengthChanges)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectKSS
            // 
            this.btnSelectKSS.Location = new System.Drawing.Point(12, 25);
            this.btnSelectKSS.Name = "btnSelectKSS";
            this.btnSelectKSS.Size = new System.Drawing.Size(104, 26);
            this.btnSelectKSS.TabIndex = 0;
            this.btnSelectKSS.Text = "Select KSS File";
            this.btnSelectKSS.UseVisualStyleBackColor = true;
            this.btnSelectKSS.Click += new System.EventHandler(this.SelectKSS);
            // 
            // openKSS
            // 
            this.openKSS.FileName = "openFileDialog1";
            // 
            // btnParseDSTVs
            // 
            this.btnParseDSTVs.Enabled = false;
            this.btnParseDSTVs.Location = new System.Drawing.Point(12, 71);
            this.btnParseDSTVs.Name = "btnParseDSTVs";
            this.btnParseDSTVs.Size = new System.Drawing.Size(104, 31);
            this.btnParseDSTVs.TabIndex = 1;
            this.btnParseDSTVs.Text = "Parse DSTVs";
            this.btnParseDSTVs.UseVisualStyleBackColor = true;
            this.btnParseDSTVs.Click += new System.EventHandler(this.ParseDSTVs);
            // 
            // btnWriteKSS
            // 
            this.btnWriteKSS.Enabled = false;
            this.btnWriteKSS.Location = new System.Drawing.Point(12, 131);
            this.btnWriteKSS.Name = "btnWriteKSS";
            this.btnWriteKSS.Size = new System.Drawing.Size(104, 32);
            this.btnWriteKSS.TabIndex = 2;
            this.btnWriteKSS.Text = "Write New KSS";
            this.btnWriteKSS.UseVisualStyleBackColor = true;
            this.btnWriteKSS.Click += new System.EventHandler(this.WriteKSS);
            // 
            // lblExportedKSSPath
            // 
            this.lblExportedKSSPath.AutoSize = true;
            this.lblExportedKSSPath.Location = new System.Drawing.Point(49, 238);
            this.lblExportedKSSPath.Name = "lblExportedKSSPath";
            this.lblExportedKSSPath.Size = new System.Drawing.Size(0, 13);
            this.lblExportedKSSPath.TabIndex = 5;
            // 
            // lblGeneratedKSSPath
            // 
            this.lblGeneratedKSSPath.AutoSize = true;
            this.lblGeneratedKSSPath.Location = new System.Drawing.Point(48, 328);
            this.lblGeneratedKSSPath.Name = "lblGeneratedKSSPath";
            this.lblGeneratedKSSPath.Size = new System.Drawing.Size(0, 13);
            this.lblGeneratedKSSPath.TabIndex = 6;
            // 
            // openExportedKSS
            // 
            this.openExportedKSS.FileName = "openFileDialog1";
            // 
            // openGeneratedKSS
            // 
            this.openGeneratedKSS.FileName = "openFileDialog1";
            // 
            // lblComplete
            // 
            this.lblComplete.AutoSize = true;
            this.lblComplete.Location = new System.Drawing.Point(14, 165);
            this.lblComplete.Name = "lblComplete";
            this.lblComplete.Size = new System.Drawing.Size(51, 13);
            this.lblComplete.TabIndex = 9;
            this.lblComplete.Text = "Complete";
            this.lblComplete.Visible = false;
            // 
            // checkMultiplyBoltQuantities
            // 
            this.checkMultiplyBoltQuantities.AutoSize = true;
            this.checkMultiplyBoltQuantities.Checked = true;
            this.checkMultiplyBoltQuantities.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkMultiplyBoltQuantities.Location = new System.Drawing.Point(12, 2);
            this.checkMultiplyBoltQuantities.Name = "checkMultiplyBoltQuantities";
            this.checkMultiplyBoltQuantities.Size = new System.Drawing.Size(210, 17);
            this.checkMultiplyBoltQuantities.TabIndex = 10;
            this.checkMultiplyBoltQuantities.Text = "Multiply bolt QTY by main member QTY";
            this.checkMultiplyBoltQuantities.UseVisualStyleBackColor = true;
            this.checkMultiplyBoltQuantities.CheckedChanged += new System.EventHandler(this.ToggleMutliplyBoltQTY);
            // 
            // dgvSmallHoles
            // 
            this.dgvSmallHoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSmallHoles.Location = new System.Drawing.Point(12, 210);
            this.dgvSmallHoles.Name = "dgvSmallHoles";
            this.dgvSmallHoles.Size = new System.Drawing.Size(283, 92);
            this.dgvSmallHoles.TabIndex = 11;
            // 
            // lblHolesTooSmall
            // 
            this.lblHolesTooSmall.AutoSize = true;
            this.lblHolesTooSmall.Location = new System.Drawing.Point(20, 190);
            this.lblHolesTooSmall.Name = "lblHolesTooSmall";
            this.lblHolesTooSmall.Size = new System.Drawing.Size(84, 13);
            this.lblHolesTooSmall.TabIndex = 12;
            this.lblHolesTooSmall.Text = "Holes Too Small";
            // 
            // lblPL12
            // 
            this.lblPL12.AutoSize = true;
            this.lblPL12.Location = new System.Drawing.Point(20, 308);
            this.lblPL12.Name = "lblPL12";
            this.lblPL12.Size = new System.Drawing.Size(57, 13);
            this.lblPL12.TabIndex = 14;
            this.lblPL12.Text = "PL1/2 > 5\'";
            this.lblPL12.Click += new System.EventHandler(this.label1_Click);
            // 
            // dgvPLHalf
            // 
            this.dgvPLHalf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPLHalf.Location = new System.Drawing.Point(12, 328);
            this.dgvPLHalf.Name = "dgvPLHalf";
            this.dgvPLHalf.Size = new System.Drawing.Size(283, 92);
            this.dgvPLHalf.TabIndex = 13;
            this.dgvPLHalf.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // dgvHeaderLengthChanges
            // 
            this.dgvHeaderLengthChanges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHeaderLengthChanges.Location = new System.Drawing.Point(322, 329);
            this.dgvHeaderLengthChanges.Name = "dgvHeaderLengthChanges";
            this.dgvHeaderLengthChanges.Size = new System.Drawing.Size(549, 149);
            this.dgvHeaderLengthChanges.TabIndex = 15;
            // 
            // lblHeaderLengthChanges
            // 
            this.lblHeaderLengthChanges.AutoSize = true;
            this.lblHeaderLengthChanges.Location = new System.Drawing.Point(328, 308);
            this.lblHeaderLengthChanges.Name = "lblHeaderLengthChanges";
            this.lblHeaderLengthChanges.Size = new System.Drawing.Size(148, 13);
            this.lblHeaderLengthChanges.TabIndex = 16;
            this.lblHeaderLengthChanges.Text = "Header Length Discrepancies";
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(883, 525);
            this.Controls.Add(this.lblHeaderLengthChanges);
            this.Controls.Add(this.dgvHeaderLengthChanges);
            this.Controls.Add(this.lblPL12);
            this.Controls.Add(this.dgvPLHalf);
            this.Controls.Add(this.lblHolesTooSmall);
            this.Controls.Add(this.dgvSmallHoles);
            this.Controls.Add(this.checkMultiplyBoltQuantities);
            this.Controls.Add(this.lblComplete);
            this.Controls.Add(this.lblGeneratedKSSPath);
            this.Controls.Add(this.lblExportedKSSPath);
            this.Controls.Add(this.btnWriteKSS);
            this.Controls.Add(this.btnParseDSTVs);
            this.Controls.Add(this.btnSelectKSS);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "KSS Editor";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSmallHoles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLHalf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaderLengthChanges)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectKSS;
        private System.Windows.Forms.OpenFileDialog openKSS;
        private System.Windows.Forms.Button btnParseDSTVs;
        private System.Windows.Forms.Button btnWriteKSS;
        private System.Windows.Forms.Label lblExportedKSSPath;
        private System.Windows.Forms.Label lblGeneratedKSSPath;
        private System.Windows.Forms.OpenFileDialog openExportedKSS;
        private System.Windows.Forms.OpenFileDialog openGeneratedKSS;
        private System.Windows.Forms.Label lblComplete;
        private System.Windows.Forms.CheckBox checkMultiplyBoltQuantities;
        private System.Windows.Forms.DataGridView dgvSmallHoles;
        private System.Windows.Forms.Label lblHolesTooSmall;
        private System.Windows.Forms.Label lblPL12;
        private System.Windows.Forms.DataGridView dgvPLHalf;
        private System.Windows.Forms.DataGridView dgvHeaderLengthChanges;
        private System.Windows.Forms.Label lblHeaderLengthChanges;
    }
}

