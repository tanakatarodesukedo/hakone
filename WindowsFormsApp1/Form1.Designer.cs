namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbUniversity = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPlayerName = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvPlayers = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.mtbBest10kFrom = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mtbBest10kTo = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mtbBestHalfFrom = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.mtbBestHalfTo = new System.Windows.Forms.MaskedTextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlayers)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbUniversity
            // 
            this.cmbUniversity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbUniversity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbUniversity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUniversity.FormattingEnabled = true;
            this.cmbUniversity.Location = new System.Drawing.Point(79, 34);
            this.cmbUniversity.Name = "cmbUniversity";
            this.cmbUniversity.Size = new System.Drawing.Size(121, 20);
            this.cmbUniversity.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "大学名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(226, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "選手名";
            // 
            // txtPlayerName
            // 
            this.txtPlayerName.Location = new System.Drawing.Point(274, 34);
            this.txtPlayerName.MaxLength = 100;
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.Size = new System.Drawing.Size(100, 19);
            this.txtPlayerName.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(398, 32);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "検索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvPlayers
            // 
            this.dgvPlayers.AllowUserToAddRows = false;
            this.dgvPlayers.AllowUserToDeleteRows = false;
            this.dgvPlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlayers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvPlayers.Location = new System.Drawing.Point(0, 133);
            this.dgvPlayers.Name = "dgvPlayers";
            this.dgvPlayers.ReadOnly = true;
            this.dgvPlayers.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPlayers.RowTemplate.Height = 21;
            this.dgvPlayers.Size = new System.Drawing.Size(800, 317);
            this.dgvPlayers.TabIndex = 5;
            this.dgvPlayers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlayers_CellDoubleClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(713, 104);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "追加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // mtbBest10kFrom
            // 
            this.mtbBest10kFrom.Location = new System.Drawing.Point(79, 75);
            this.mtbBest10kFrom.Mask = "00:00.00";
            this.mtbBest10kFrom.Name = "mtbBest10kFrom";
            this.mtbBest10kFrom.PromptChar = '-';
            this.mtbBest10kFrom.Size = new System.Drawing.Size(100, 19);
            this.mtbBest10kFrom.TabIndex = 7;
            this.mtbBest10kFrom.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "1万m";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(185, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "～";
            // 
            // mtbBest10kTo
            // 
            this.mtbBest10kTo.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.mtbBest10kTo.Location = new System.Drawing.Point(208, 75);
            this.mtbBest10kTo.Mask = "00:00.00";
            this.mtbBest10kTo.Name = "mtbBest10kTo";
            this.mtbBest10kTo.PromptChar = '-';
            this.mtbBest10kTo.Size = new System.Drawing.Size(100, 19);
            this.mtbBest10kTo.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(342, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "ハーフ";
            // 
            // mtbBestHalfFrom
            // 
            this.mtbBestHalfFrom.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.mtbBestHalfFrom.Location = new System.Drawing.Point(381, 75);
            this.mtbBestHalfFrom.Mask = "0:00:00";
            this.mtbBestHalfFrom.Name = "mtbBestHalfFrom";
            this.mtbBestHalfFrom.PromptChar = '-';
            this.mtbBestHalfFrom.Size = new System.Drawing.Size(100, 19);
            this.mtbBestHalfFrom.TabIndex = 12;
            this.mtbBestHalfFrom.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(487, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "～";
            // 
            // mtbBestHalfTo
            // 
            this.mtbBestHalfTo.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.mtbBestHalfTo.Location = new System.Drawing.Point(510, 75);
            this.mtbBestHalfTo.Mask = "0:00:00";
            this.mtbBestHalfTo.Name = "mtbBestHalfTo";
            this.mtbBestHalfTo.PromptChar = '-';
            this.mtbBestHalfTo.Size = new System.Drawing.Size(100, 19);
            this.mtbBestHalfTo.TabIndex = 14;
            this.mtbBestHalfTo.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(489, 32);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 15;
            this.btnClear.Text = "クリア";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(688, 12);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 16;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(686, 41);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 12);
            this.labelStatus.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.mtbBestHalfTo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.mtbBestHalfFrom);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.mtbBest10kTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mtbBest10kFrom);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvPlayers);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtPlayerName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbUniversity);
            this.Name = "Form1";
            this.Text = "箱根駅伝";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlayers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbUniversity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPlayerName;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvPlayers;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.MaskedTextBox mtbBest10kFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox mtbBest10kTo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox mtbBestHalfFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MaskedTextBox mtbBestHalfTo;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelStatus;
    }
}

