using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFLI_Toolkit
{
    public partial class HotkeysPositionManager : Form
    {
        public HotkeysPositionManager()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void HotkeysPositionManager_Load(object sender, EventArgs e)
        {

        }

        private void textBox_SavePlayerPosition_KeyDown(object sender, KeyEventArgs e)
        {
            Label tempLabel = Application.OpenForms["Main"].Controls["label_HotkeySavePlayerPosition"] as Label;
            tempLabel.Text = e.KeyCode.ToString();

            Main.hotkey_SavePlayerPosition = e.KeyValue;
            Saves.Save("settings", "KeyCodeSavePlayerPosition", e.KeyCode.ToString());
            Saves.Save("settings", "KeyValueSavePlayerPosition", e.KeyValue.ToString());
            textBox_SavePlayerPosition.Text = e.KeyCode.ToString();
        }

        private void textBox_LoadPlayerPosition_KeyDown(object sender, KeyEventArgs e)
        {
            Label tempLabel = Application.OpenForms["Main"].Controls["label_HotkeyLoadPlayerPosition"] as Label;
            tempLabel.Text = e.KeyCode.ToString();

            Main.hotkey_LoadPlayerPosition = e.KeyValue;
            Saves.Save("settings", "KeyCodeLoadPlayerPosition", e.KeyCode.ToString());
            Saves.Save("settings", "KeyValueLoadPlayerPosition", e.KeyValue.ToString());
            textBox_LoadPlayerPosition.Text = e.KeyCode.ToString();
        }

        private void HotkeysPositionManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.formHotkeysOpen = false;
        }
    }
}
