
namespace EFLI_Toolkit
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.selector = new System.Windows.Forms.Label();
            this.backgroundWorker_CheckProcess = new System.ComponentModel.BackgroundWorker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_PillSize = new System.Windows.Forms.TextBox();
            this.button_PillSizeApply = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_PillSize = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_Hotkeys = new System.Windows.Forms.Button();
            this.button_LoadPlayerPosition = new System.Windows.Forms.Button();
            this.button_SavePlayerPosition = new System.Windows.Forms.Button();
            this.textBox_PlayerPositionZ = new System.Windows.Forms.TextBox();
            this.textBox_PlayerPositionY = new System.Windows.Forms.TextBox();
            this.textBox_PlayerPositionX = new System.Windows.Forms.TextBox();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.label_HotkeyLoadPlayerPosition = new System.Windows.Forms.Label();
            this.label_HotkeySavePlayerPosition = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox_WindowAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.backgroundWorker_CheckUpdates = new System.ComponentModel.BackgroundWorker();
            this.label_Version = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // selector
            // 
            this.selector.AutoSize = true;
            this.selector.Location = new System.Drawing.Point(32000, 32000);
            this.selector.Name = "selector";
            this.selector.Size = new System.Drawing.Size(0, 13);
            this.selector.TabIndex = 0;
            // 
            // backgroundWorker_CheckProcess
            // 
            this.backgroundWorker_CheckProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_CheckProcess_DoWork);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(-12, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(500, 2);
            this.label2.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(16, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Viagra Pill Size Effect";
            // 
            // textBox_PillSize
            // 
            this.textBox_PillSize.Enabled = false;
            this.textBox_PillSize.Location = new System.Drawing.Point(43, 41);
            this.textBox_PillSize.Name = "textBox_PillSize";
            this.textBox_PillSize.Size = new System.Drawing.Size(100, 20);
            this.textBox_PillSize.TabIndex = 5;
            this.textBox_PillSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_PillSizeApply
            // 
            this.button_PillSizeApply.Enabled = false;
            this.button_PillSizeApply.Location = new System.Drawing.Point(149, 39);
            this.button_PillSizeApply.Name = "button_PillSizeApply";
            this.button_PillSizeApply.Size = new System.Drawing.Size(175, 23);
            this.button_PillSizeApply.TabIndex = 6;
            this.button_PillSizeApply.Text = "apply";
            this.button_PillSizeApply.UseVisualStyleBackColor = true;
            this.button_PillSizeApply.Click += new System.EventHandler(this.button_PillSizeApply_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "size";
            // 
            // checkBox_PillSize
            // 
            this.checkBox_PillSize.AutoSize = true;
            this.checkBox_PillSize.Location = new System.Drawing.Point(309, 12);
            this.checkBox_PillSize.Name = "checkBox_PillSize";
            this.checkBox_PillSize.Size = new System.Drawing.Size(15, 14);
            this.checkBox_PillSize.TabIndex = 8;
            this.checkBox_PillSize.UseVisualStyleBackColor = true;
            this.checkBox_PillSize.CheckedChanged += new System.EventHandler(this.checkBox_PillSize_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label4.Location = new System.Drawing.Point(16, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Position Manager";
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(-12, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(500, 2);
            this.label5.TabIndex = 9;
            // 
            // button_Hotkeys
            // 
            this.button_Hotkeys.Location = new System.Drawing.Point(224, 120);
            this.button_Hotkeys.Name = "button_Hotkeys";
            this.button_Hotkeys.Size = new System.Drawing.Size(100, 23);
            this.button_Hotkeys.TabIndex = 21;
            this.button_Hotkeys.Text = "hotkeys";
            this.button_Hotkeys.UseVisualStyleBackColor = true;
            this.button_Hotkeys.Click += new System.EventHandler(this.button_Hotkeys_Click);
            // 
            // button_LoadPlayerPosition
            // 
            this.button_LoadPlayerPosition.Location = new System.Drawing.Point(118, 120);
            this.button_LoadPlayerPosition.Name = "button_LoadPlayerPosition";
            this.button_LoadPlayerPosition.Size = new System.Drawing.Size(100, 23);
            this.button_LoadPlayerPosition.TabIndex = 20;
            this.button_LoadPlayerPosition.Text = "load position";
            this.button_LoadPlayerPosition.UseVisualStyleBackColor = true;
            this.button_LoadPlayerPosition.Click += new System.EventHandler(this.button_LoadPlayerPosition_Click);
            // 
            // button_SavePlayerPosition
            // 
            this.button_SavePlayerPosition.Location = new System.Drawing.Point(12, 120);
            this.button_SavePlayerPosition.Name = "button_SavePlayerPosition";
            this.button_SavePlayerPosition.Size = new System.Drawing.Size(100, 23);
            this.button_SavePlayerPosition.TabIndex = 19;
            this.button_SavePlayerPosition.Text = "save position";
            this.button_SavePlayerPosition.UseVisualStyleBackColor = true;
            this.button_SavePlayerPosition.Click += new System.EventHandler(this.button_SavePlayerPosition_Click);
            // 
            // textBox_PlayerPositionZ
            // 
            this.textBox_PlayerPositionZ.Location = new System.Drawing.Point(224, 149);
            this.textBox_PlayerPositionZ.Name = "textBox_PlayerPositionZ";
            this.textBox_PlayerPositionZ.Size = new System.Drawing.Size(100, 20);
            this.textBox_PlayerPositionZ.TabIndex = 18;
            this.textBox_PlayerPositionZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_PlayerPositionY
            // 
            this.textBox_PlayerPositionY.Location = new System.Drawing.Point(118, 149);
            this.textBox_PlayerPositionY.Name = "textBox_PlayerPositionY";
            this.textBox_PlayerPositionY.Size = new System.Drawing.Size(100, 20);
            this.textBox_PlayerPositionY.TabIndex = 17;
            this.textBox_PlayerPositionY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_PlayerPositionX
            // 
            this.textBox_PlayerPositionX.Location = new System.Drawing.Point(12, 149);
            this.textBox_PlayerPositionX.Name = "textBox_PlayerPositionX";
            this.textBox_PlayerPositionX.Size = new System.Drawing.Size(100, 20);
            this.textBox_PlayerPositionX.TabIndex = 16;
            this.textBox_PlayerPositionX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            // 
            // label_HotkeyLoadPlayerPosition
            // 
            this.label_HotkeyLoadPlayerPosition.Location = new System.Drawing.Point(118, 102);
            this.label_HotkeyLoadPlayerPosition.Name = "label_HotkeyLoadPlayerPosition";
            this.label_HotkeyLoadPlayerPosition.Size = new System.Drawing.Size(100, 19);
            this.label_HotkeyLoadPlayerPosition.TabIndex = 23;
            this.label_HotkeyLoadPlayerPosition.Text = "<hotkey not set>";
            this.label_HotkeyLoadPlayerPosition.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label_HotkeySavePlayerPosition
            // 
            this.label_HotkeySavePlayerPosition.Location = new System.Drawing.Point(12, 102);
            this.label_HotkeySavePlayerPosition.Name = "label_HotkeySavePlayerPosition";
            this.label_HotkeySavePlayerPosition.Size = new System.Drawing.Size(100, 19);
            this.label_HotkeySavePlayerPosition.TabIndex = 22;
            this.label_HotkeySavePlayerPosition.Text = "<hotkey not set>";
            this.label_HotkeySavePlayerPosition.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label6.Location = new System.Drawing.Point(156, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(168, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "default is RNG between (1.4 - 3.0)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label7.Location = new System.Drawing.Point(16, 186);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Program Settings";
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Location = new System.Drawing.Point(-12, 193);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(500, 2);
            this.label8.TabIndex = 25;
            // 
            // checkBox_WindowAlwaysOnTop
            // 
            this.checkBox_WindowAlwaysOnTop.AutoSize = true;
            this.checkBox_WindowAlwaysOnTop.Location = new System.Drawing.Point(15, 211);
            this.checkBox_WindowAlwaysOnTop.Name = "checkBox_WindowAlwaysOnTop";
            this.checkBox_WindowAlwaysOnTop.Size = new System.Drawing.Size(130, 17);
            this.checkBox_WindowAlwaysOnTop.TabIndex = 27;
            this.checkBox_WindowAlwaysOnTop.Text = "window always on top";
            this.checkBox_WindowAlwaysOnTop.UseVisualStyleBackColor = true;
            this.checkBox_WindowAlwaysOnTop.CheckedChanged += new System.EventHandler(this.checkBox_WindowAlwaysOnTop_CheckedChanged);
            // 
            // backgroundWorker_CheckUpdates
            // 
            this.backgroundWorker_CheckUpdates.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_CheckUpdates_DoWork);
            // 
            // label_Version
            // 
            this.label_Version.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label_Version.Location = new System.Drawing.Point(233, 221);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(100, 23);
            this.label_Version.TabIndex = 28;
            this.label_Version.Text = "v2";
            this.label_Version.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 239);
            this.Controls.Add(this.label_Version);
            this.Controls.Add(this.checkBox_WindowAlwaysOnTop);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label_HotkeyLoadPlayerPosition);
            this.Controls.Add(this.label_HotkeySavePlayerPosition);
            this.Controls.Add(this.button_Hotkeys);
            this.Controls.Add(this.button_LoadPlayerPosition);
            this.Controls.Add(this.button_SavePlayerPosition);
            this.Controls.Add(this.textBox_PlayerPositionZ);
            this.Controls.Add(this.textBox_PlayerPositionY);
            this.Controls.Add(this.textBox_PlayerPositionX);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBox_PillSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_PillSizeApply);
            this.Controls.Add(this.textBox_PillSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.selector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "EFLI Toolkit";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label selector;
        private System.ComponentModel.BackgroundWorker backgroundWorker_CheckProcess;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_PillSize;
        private System.Windows.Forms.Button button_PillSizeApply;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_PillSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_Hotkeys;
        private System.Windows.Forms.Button button_LoadPlayerPosition;
        private System.Windows.Forms.Button button_SavePlayerPosition;
        private System.Windows.Forms.TextBox textBox_PlayerPositionZ;
        private System.Windows.Forms.TextBox textBox_PlayerPositionY;
        private System.Windows.Forms.TextBox textBox_PlayerPositionX;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label label_HotkeyLoadPlayerPosition;
        private System.Windows.Forms.Label label_HotkeySavePlayerPosition;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBox_WindowAlwaysOnTop;
        private System.ComponentModel.BackgroundWorker backgroundWorker_CheckUpdates;
        private System.Windows.Forms.Label label_Version;
    }
}

