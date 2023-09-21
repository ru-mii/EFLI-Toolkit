
namespace EFLI_Toolkit
{
    partial class HotkeysPositionManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HotkeysPositionManager));
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_LoadPlayerPosition = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_SavePlayerPosition = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(15, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(255, 23);
            this.label5.TabIndex = 18;
            this.label5.Text = "changes will save automatically";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBox_LoadPlayerPosition
            // 
            this.textBox_LoadPlayerPosition.Location = new System.Drawing.Point(170, 59);
            this.textBox_LoadPlayerPosition.Name = "textBox_LoadPlayerPosition";
            this.textBox_LoadPlayerPosition.ReadOnly = true;
            this.textBox_LoadPlayerPosition.Size = new System.Drawing.Size(100, 20);
            this.textBox_LoadPlayerPosition.TabIndex = 17;
            this.textBox_LoadPlayerPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_LoadPlayerPosition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_LoadPlayerPosition_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "load position";
            // 
            // textBox_SavePlayerPosition
            // 
            this.textBox_SavePlayerPosition.Location = new System.Drawing.Point(170, 33);
            this.textBox_SavePlayerPosition.Name = "textBox_SavePlayerPosition";
            this.textBox_SavePlayerPosition.ReadOnly = true;
            this.textBox_SavePlayerPosition.Size = new System.Drawing.Size(100, 20);
            this.textBox_SavePlayerPosition.TabIndex = 15;
            this.textBox_SavePlayerPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_SavePlayerPosition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_SavePlayerPosition_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "save position";
            // 
            // HotkeysPositionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 89);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox_LoadPlayerPosition);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_SavePlayerPosition);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "HotkeysPositionManager";
            this.Text = "Hotkeys";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HotkeysPositionManager_FormClosing);
            this.Load += new System.EventHandler(this.HotkeysPositionManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_LoadPlayerPosition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_SavePlayerPosition;
        private System.Windows.Forms.Label label1;
    }
}