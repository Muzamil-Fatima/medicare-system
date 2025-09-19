namespace ClinicMangSystem
{
    partial class OrderTest
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.EditBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.AddBtn = new System.Windows.Forms.Button();
            this.DelBtn = new System.Windows.Forms.Button();
            this.PayCb = new System.Windows.Forms.ComboBox();
            this.TestGV = new System.Windows.Forms.DataGridView();
            this.TestCb = new System.Windows.Forms.ComboBox();
            this.TestCon = new System.Windows.Forms.DateTimePicker();
            this.TestOrd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.patId = new System.Windows.Forms.TextBox();
            this.docId = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TestGV)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.EditBtn);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.AddBtn);
            this.panel1.Controls.Add(this.DelBtn);
            this.panel1.Controls.Add(this.PayCb);
            this.panel1.Controls.Add(this.TestGV);
            this.panel1.Controls.Add(this.TestCb);
            this.panel1.Controls.Add(this.TestCon);
            this.panel1.Controls.Add(this.TestOrd);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.patId);
            this.panel1.Controls.Add(this.docId);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(248, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1252, 749);
            this.panel1.TabIndex = 122;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(585, 170);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(114, 26);
            this.label12.TabIndex = 86;
            this.label12.Text = "Patient Id";
            // 
            // EditBtn
            // 
            this.EditBtn.BackColor = System.Drawing.Color.LimeGreen;
            this.EditBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.EditBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.EditBtn.FlatAppearance.BorderSize = 0;
            this.EditBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.EditBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.EditBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EditBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.EditBtn.Location = new System.Drawing.Point(898, 331);
            this.EditBtn.Name = "EditBtn";
            this.EditBtn.Size = new System.Drawing.Size(142, 44);
            this.EditBtn.TabIndex = 82;
            this.EditBtn.Text = "Edit";
            this.EditBtn.UseVisualStyleBackColor = false;
            this.EditBtn.Click += new System.EventHandler(this.EditBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(585, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 26);
            this.label5.TabIndex = 85;
            this.label5.Text = "Doctor Id";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(69, 259);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(172, 26);
            this.label11.TabIndex = 83;
            this.label11.Text = "Test ordered on";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(46, 315);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 26);
            this.label3.TabIndex = 84;
            this.label3.Text = "Test conducted on";
            // 
            // AddBtn
            // 
            this.AddBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.AddBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.AddBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.AddBtn.FlatAppearance.BorderSize = 0;
            this.AddBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.AddBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.AddBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.AddBtn.Location = new System.Drawing.Point(731, 331);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(142, 44);
            this.AddBtn.TabIndex = 81;
            this.AddBtn.Text = "Add";
            this.AddBtn.UseVisualStyleBackColor = false;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // DelBtn
            // 
            this.DelBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.DelBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.DelBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.DelBtn.FlatAppearance.BorderSize = 3;
            this.DelBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.DelBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.DelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DelBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DelBtn.Location = new System.Drawing.Point(554, 331);
            this.DelBtn.Name = "DelBtn";
            this.DelBtn.Size = new System.Drawing.Size(142, 44);
            this.DelBtn.TabIndex = 80;
            this.DelBtn.Text = "Delete";
            this.DelBtn.UseVisualStyleBackColor = false;
            this.DelBtn.Click += new System.EventHandler(this.DelBtn_Click);
            // 
            // PayCb
            // 
            this.PayCb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PayCb.FormattingEnabled = true;
            this.PayCb.Items.AddRange(new object[] {
            "Paid",
            "Un-Paid",
            "Pending"});
            this.PayCb.Location = new System.Drawing.Point(252, 187);
            this.PayCb.Name = "PayCb";
            this.PayCb.Size = new System.Drawing.Size(250, 37);
            this.PayCb.TabIndex = 79;
            // 
            // TestGV
            // 
            this.TestGV.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TestGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TestGV.Location = new System.Drawing.Point(51, 393);
            this.TestGV.Name = "TestGV";
            this.TestGV.RowHeadersWidth = 62;
            this.TestGV.RowTemplate.Height = 28;
            this.TestGV.Size = new System.Drawing.Size(1088, 258);
            this.TestGV.TabIndex = 75;
            this.TestGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TestGV_CellContentClick);
            // 
            // TestCb
            // 
            this.TestCb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestCb.FormattingEnabled = true;
            this.TestCb.Items.AddRange(new object[] {
            "Pending",
            "Completed",
            "Cancelled"});
            this.TestCb.Location = new System.Drawing.Point(252, 113);
            this.TestCb.Name = "TestCb";
            this.TestCb.Size = new System.Drawing.Size(250, 37);
            this.TestCb.TabIndex = 78;
            // 
            // TestCon
            // 
            this.TestCon.CalendarFont = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestCon.CalendarForeColor = System.Drawing.SystemColors.HotTrack;
            this.TestCon.CalendarMonthBackground = System.Drawing.SystemColors.GradientInactiveCaption;
            this.TestCon.CalendarTitleForeColor = System.Drawing.SystemColors.HotTrack;
            this.TestCon.CalendarTrailingForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TestCon.Location = new System.Drawing.Point(257, 315);
            this.TestCon.Name = "TestCon";
            this.TestCon.Size = new System.Drawing.Size(245, 26);
            this.TestCon.TabIndex = 77;
            // 
            // TestOrd
            // 
            this.TestOrd.CalendarFont = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestOrd.CalendarForeColor = System.Drawing.SystemColors.HotTrack;
            this.TestOrd.CalendarMonthBackground = System.Drawing.SystemColors.GradientInactiveCaption;
            this.TestOrd.CalendarTitleForeColor = System.Drawing.SystemColors.HotTrack;
            this.TestOrd.CalendarTrailingForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.TestOrd.Location = new System.Drawing.Point(257, 253);
            this.TestOrd.Name = "TestOrd";
            this.TestOrd.Size = new System.Drawing.Size(245, 26);
            this.TestOrd.TabIndex = 76;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(68, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 26);
            this.label2.TabIndex = 74;
            this.label2.Text = " Payment status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(92, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 26);
            this.label4.TabIndex = 73;
            this.label4.Text = "Test Status";
            // 
            // patId
            // 
            this.patId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patId.Location = new System.Drawing.Point(731, 170);
            this.patId.Name = "patId";
            this.patId.Size = new System.Drawing.Size(254, 35);
            this.patId.TabIndex = 72;
            // 
            // docId
            // 
            this.docId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.docId.Location = new System.Drawing.Point(731, 108);
            this.docId.Name = "docId";
            this.docId.Size = new System.Drawing.Size(254, 35);
            this.docId.TabIndex = 71;
            // 
            // OrderTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1500, 800);
            this.Controls.Add(this.panel1);
            this.Name = "OrderTest";
            this.Text = "OrderTest";
            this.Load += new System.EventHandler(this.OrderTest_Load);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TestGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button EditBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button DelBtn;
        private System.Windows.Forms.ComboBox PayCb;
        private System.Windows.Forms.DataGridView TestGV;
        private System.Windows.Forms.ComboBox TestCb;
        private System.Windows.Forms.DateTimePicker TestCon;
        private System.Windows.Forms.DateTimePicker TestOrd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox patId;
        private System.Windows.Forms.TextBox docId;
    }
}