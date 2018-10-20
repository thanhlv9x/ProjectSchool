namespace DesktopApplication
{
    partial class FormMain
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnChangePw = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbDate = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lbNumber = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbPw = new System.Windows.Forms.GroupBox();
            this.lbErrorChangePw = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSubmitChange = new System.Windows.Forms.Button();
            this.txtNewPw2 = new System.Windows.Forms.TextBox();
            this.txtNewPw1 = new System.Windows.Forms.TextBox();
            this.txtOldPw = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pbSetting = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lbQuay = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbPw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Location = new System.Drawing.Point(30, 34);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(192, 393);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Controls.Add(this.lbQuay);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnChangePw);
            this.groupBox1.Controls.Add(this.btnSkip);
            this.groupBox1.Controls.Add(this.lbStatus);
            this.groupBox1.Controls.Add(this.lbDate);
            this.groupBox1.Controls.Add(this.btnLogout);
            this.groupBox1.Controls.Add(this.btnComplete);
            this.groupBox1.Controls.Add(this.btnNext);
            this.groupBox1.Controls.Add(this.lbNumber);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(248, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(522, 416);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GỌI SỐ";
            // 
            // btnChangePw
            // 
            this.btnChangePw.Location = new System.Drawing.Point(261, 362);
            this.btnChangePw.Name = "btnChangePw";
            this.btnChangePw.Size = new System.Drawing.Size(138, 48);
            this.btnChangePw.TabIndex = 8;
            this.btnChangePw.Text = "ĐỔI MẬT KHẨU";
            this.btnChangePw.UseVisualStyleBackColor = true;
            this.btnChangePw.Click += new System.EventHandler(this.btnChangePw_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSkip.Enabled = false;
            this.btnSkip.Location = new System.Drawing.Point(332, 272);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(111, 48);
            this.btnSkip.TabIndex = 7;
            this.btnSkip.Text = "BỎ QUA";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(6, 34);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(88, 20);
            this.lbStatus.TabIndex = 6;
            this.lbStatus.Text = "Trạng thái: ";
            // 
            // lbDate
            // 
            this.lbDate.AutoSize = true;
            this.lbDate.Location = new System.Drawing.Point(164, 0);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(45, 20);
            this.lbDate.TabIndex = 5;
            this.lbDate.Text = "Ngày";
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(405, 362);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(111, 48);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "ĐĂNG XUẤT";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.Enabled = false;
            this.btnComplete.Location = new System.Drawing.Point(215, 272);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(111, 48);
            this.btnComplete.TabIndex = 3;
            this.btnComplete.Text = "HOÀN TẤT";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnNext
            // 
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnNext.Location = new System.Drawing.Point(98, 272);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(111, 48);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "TIẾP THEO";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lbNumber
            // 
            this.lbNumber.AutoSize = true;
            this.lbNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNumber.Location = new System.Drawing.Point(237, 160);
            this.lbNumber.Name = "lbNumber";
            this.lbNumber.Size = new System.Drawing.Size(51, 55);
            this.lbNumber.TabIndex = 1;
            this.lbNumber.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(162, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 31);
            this.label2.TabIndex = 0;
            this.label2.Text = "SỐ ĐANG GỌI";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "DANH SÁCH SỐ ĐÃ GỌI";
            // 
            // gbPw
            // 
            this.gbPw.BackColor = System.Drawing.SystemColors.Window;
            this.gbPw.Controls.Add(this.lbErrorChangePw);
            this.gbPw.Controls.Add(this.btnBack);
            this.gbPw.Controls.Add(this.btnSubmitChange);
            this.gbPw.Controls.Add(this.txtNewPw2);
            this.gbPw.Controls.Add(this.txtNewPw1);
            this.gbPw.Controls.Add(this.txtOldPw);
            this.gbPw.Controls.Add(this.label5);
            this.gbPw.Controls.Add(this.label4);
            this.gbPw.Controls.Add(this.label3);
            this.gbPw.Enabled = false;
            this.gbPw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPw.Location = new System.Drawing.Point(30, 11);
            this.gbPw.Name = "gbPw";
            this.gbPw.Size = new System.Drawing.Size(740, 416);
            this.gbPw.TabIndex = 9;
            this.gbPw.TabStop = false;
            this.gbPw.Text = "ĐỔI MẬT KHẨU";
            this.gbPw.Visible = false;
            // 
            // lbErrorChangePw
            // 
            this.lbErrorChangePw.AutoSize = true;
            this.lbErrorChangePw.ForeColor = System.Drawing.Color.Red;
            this.lbErrorChangePw.Location = new System.Drawing.Point(312, 219);
            this.lbErrorChangePw.Name = "lbErrorChangePw";
            this.lbErrorChangePw.Size = new System.Drawing.Size(0, 20);
            this.lbErrorChangePw.TabIndex = 5;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(357, 249);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(149, 56);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "QUAY LẠI";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSubmitChange
            // 
            this.btnSubmitChange.Location = new System.Drawing.Point(201, 249);
            this.btnSubmitChange.Name = "btnSubmitChange";
            this.btnSubmitChange.Size = new System.Drawing.Size(149, 56);
            this.btnSubmitChange.TabIndex = 3;
            this.btnSubmitChange.Text = "XÁC NHẬN";
            this.btnSubmitChange.UseVisualStyleBackColor = true;
            this.btnSubmitChange.Click += new System.EventHandler(this.btnSubmitChange_Click);
            // 
            // txtNewPw2
            // 
            this.txtNewPw2.Location = new System.Drawing.Point(316, 185);
            this.txtNewPw2.Name = "txtNewPw2";
            this.txtNewPw2.Size = new System.Drawing.Size(228, 26);
            this.txtNewPw2.TabIndex = 2;
            this.txtNewPw2.UseSystemPasswordChar = true;
            // 
            // txtNewPw1
            // 
            this.txtNewPw1.Location = new System.Drawing.Point(316, 151);
            this.txtNewPw1.Name = "txtNewPw1";
            this.txtNewPw1.Size = new System.Drawing.Size(228, 26);
            this.txtNewPw1.TabIndex = 1;
            this.txtNewPw1.UseSystemPasswordChar = true;
            // 
            // txtOldPw
            // 
            this.txtOldPw.Location = new System.Drawing.Point(316, 117);
            this.txtOldPw.Name = "txtOldPw";
            this.txtOldPw.Size = new System.Drawing.Size(228, 26);
            this.txtOldPw.TabIndex = 0;
            this.txtOldPw.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(169, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Mật khẩu mới:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(169, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Nhập lại mật khẩu:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(169, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mật khẩu cũ:";
            // 
            // pbSetting
            // 
            this.pbSetting.BackColor = System.Drawing.SystemColors.Window;
            this.pbSetting.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbSetting.Image = global::DesktopApplication.Properties.Resources.setting;
            this.pbSetting.Location = new System.Drawing.Point(0, 0);
            this.pbSetting.Name = "pbSetting";
            this.pbSetting.Size = new System.Drawing.Size(30, 30);
            this.pbSetting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSetting.TabIndex = 10;
            this.pbSetting.TabStop = false;
            this.pbSetting.Click += new System.EventHandler(this.pbSetting_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "Quầy số:";
            // 
            // lbQuay
            // 
            this.lbQuay.AutoSize = true;
            this.lbQuay.Location = new System.Drawing.Point(94, 65);
            this.lbQuay.Name = "lbQuay";
            this.lbQuay.Size = new System.Drawing.Size(0, 20);
            this.lbQuay.TabIndex = 10;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DesktopApplication.Properties.Resources.bg2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbSetting);
            this.Controls.Add(this.gbPw);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbPw.ResumeLayout(false);
            this.gbPw.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSetting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lbNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnChangePw;
        private System.Windows.Forms.GroupBox gbPw;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnSubmitChange;
        private System.Windows.Forms.TextBox txtNewPw2;
        private System.Windows.Forms.TextBox txtNewPw1;
        private System.Windows.Forms.TextBox txtOldPw;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbErrorChangePw;
        private System.Windows.Forms.PictureBox pbSetting;
        private System.Windows.Forms.Label lbQuay;
        private System.Windows.Forms.Label label6;
    }
}

