namespace PVFSimple.表格控件
{
    partial class Lst列表生成
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lst列表生成));
            this.reoGridControl1 = new unvell.ReoGrid.ReoGridControl();
            this.LST加入button = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Tooltimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // reoGridControl1
            // 
            this.reoGridControl1.BackColor = System.Drawing.Color.White;
            this.reoGridControl1.ColumnHeaderContextMenuStrip = null;
            this.reoGridControl1.LeadHeaderContextMenuStrip = null;
            this.reoGridControl1.Location = new System.Drawing.Point(-1, -1);
            this.reoGridControl1.Name = "reoGridControl1";
            this.reoGridControl1.RowHeaderContextMenuStrip = null;
            this.reoGridControl1.Script = null;
            this.reoGridControl1.SheetTabContextMenuStrip = null;
            this.reoGridControl1.SheetTabNewButtonVisible = false;
            this.reoGridControl1.SheetTabVisible = false;
            this.reoGridControl1.SheetTabWidth = 391;
            this.reoGridControl1.ShowScrollEndSpacing = true;
            this.reoGridControl1.Size = new System.Drawing.Size(524, 469);
            this.reoGridControl1.TabIndex = 58;
            this.reoGridControl1.Text = "reoGridControl1";
            // 
            // LST加入button
            // 
            this.LST加入button.Location = new System.Drawing.Point(370, 482);
            this.LST加入button.Name = "LST加入button";
            this.LST加入button.Size = new System.Drawing.Size(77, 25);
            this.LST加入button.TabIndex = 59;
            this.LST加入button.Text = "加入";
            this.LST加入button.UseVisualStyleBackColor = true;
            this.LST加入button.Click += new System.EventHandler(this.LST加入button_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(135, 484);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(234, 21);
            this.textBox1.TabIndex = 60;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 488);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 61;
            this.label1.Text = "路径增加文本：";
            // 
            // Tooltimer
            // 
            this.Tooltimer.Tick += new System.EventHandler(this.Tooltimer_Tick);
            // 
            // Lst列表生成
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 520);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.LST加入button);
            this.Controls.Add(this.reoGridControl1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Lst列表生成";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lst列表生成";
            this.Load += new System.EventHandler(this.设计图Lst列表生成_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private unvell.ReoGrid.ReoGridControl reoGridControl1;
        private System.Windows.Forms.Button LST加入button;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer Tooltimer;
    }
}