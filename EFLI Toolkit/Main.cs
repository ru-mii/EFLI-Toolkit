using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFLI_Toolkit
{
    public partial class Main : Form
    {
        public Main()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        public static Process gameProcess = null;
        public static bool formHotkeysOpen = false;

        public static int hotkey_SavePlayerPosition = 0;
        public static int hotkey_LoadPlayerPosition = 0;
        public int[] keyboardTableReady = new int[999];

        long oldProcessSession = 0;
        long processSession = 0;

        IntPtr mainModule = IntPtr.Zero;
        IntPtr sizeMainModule = IntPtr.Zero;

        IntPtr pillSizeAllocated = IntPtr.Zero;
        IntPtr pillSizePointer = IntPtr.Zero;

        IntPtr playerPointer = IntPtr.Zero;

        private void Main_Load(object sender, EventArgs e)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            selector.Select();

            // fill out keyboard table
            for (int i = 0; i < keyboardTableReady.Length; i++)
                keyboardTableReady[i] = 0;

            // load hotkeys
            int.TryParse(Saves.Read("settings", "KeyValueSavePlayerPosition"), out hotkey_SavePlayerPosition);
            int.TryParse(Saves.Read("settings", "KeyValueLoadPlayerPosition"), out hotkey_LoadPlayerPosition);
            label_HotkeySavePlayerPosition.Text = Saves.Read("settings", "KeyCodeSavePlayerPosition");
            label_HotkeyLoadPlayerPosition.Text = Saves.Read("settings", "KeyCodeLoadPlayerPosition");

            // load program settings
            if (Saves.Read("settings", "WindowAlwaysOnTop") != "")
            {
                bool tempRead = bool.Parse(Saves.Read("settings", "WindowAlwaysOnTop"));

                if (tempRead)
                {
                    checkBox_WindowAlwaysOnTop.Checked = true;
                    TopMost = true;
                }
                else
                {
                    checkBox_WindowAlwaysOnTop.Checked = false;
                    TopMost = false;
                }
            }


            backgroundWorker_CheckProcess.RunWorkerAsync();
            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (!formHotkeysOpen)
                {
                    if (Toolkit.IsKeyPressed(hotkey_SavePlayerPosition))
                    {
                        if (keyboardTableReady[hotkey_SavePlayerPosition] != 1)
                        {
                            button_SavePlayerPosition.PerformClick();
                            keyboardTableReady[hotkey_SavePlayerPosition] = 1;
                        }
                    }
                    else keyboardTableReady[hotkey_SavePlayerPosition] = 0;

                    if (Toolkit.IsKeyPressed(hotkey_LoadPlayerPosition))
                    {
                        if (keyboardTableReady[hotkey_LoadPlayerPosition] != 1)
                        {
                            button_LoadPlayerPosition.PerformClick();
                            keyboardTableReady[hotkey_LoadPlayerPosition] = 1;
                        }
                    }
                    else keyboardTableReady[hotkey_LoadPlayerPosition] = 0;
                }

                Thread.Sleep(5);
            }
        }

        private void backgroundWorker_CheckProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                long tempProcessSession = 0;
                bool foundProcess = false;

                foreach (Process process in Process.GetProcesses())
                {
                    if (process.ProcessName == "LavenderIsland-Win64-Shipping")
                    {
                        gameProcess = process;
                        tempProcessSession = ((DateTimeOffset)gameProcess.StartTime).ToUnixTimeMilliseconds();
                        foundProcess = true;
                    }
                }

                if (foundProcess && processSession != tempProcessSession)
                {
                    foreach (ProcessModule module in gameProcess.Modules)
                    {
                        if (module.ModuleName.Equals("LavenderIsland-Win64-Shipping.exe", StringComparison.OrdinalIgnoreCase))
                        {
                            mainModule = module.BaseAddress;
                            sizeMainModule = (IntPtr)module.ModuleMemorySize;
                            break;
                        }
                    }

                    processSession = tempProcessSession;
                }

                // game not found
                if (!foundProcess)
                {
                    // disable PillSize UI
                    checkBox_PillSize.Enabled = false;
                    checkBox_PillSize.Checked = false;
                    textBox_PillSize.Enabled = false;
                    button_PillSizeApply.Enabled = false;
                    textBox_PillSize.Text = "";
                }

                // game found
                if (processSession != oldProcessSession && foundProcess)
                {
                    // make new allocations
                    pillSizeAllocated = Toolkit.AllocateMemory();

                    #region StaticPatches

                    byte[] defaultOp = { 0x0F, 0x10, 0x88, 0xD0, 0x01, 0x00, 0x00, 0x0F, 0x28, 0xC1, 0xF3, 0x0F, 0x11, 0x4D, 0xE0 };
                    Toolkit.WriteMemory(mainModule + 0x9AEC57, defaultOp);
                    playerPointer = Toolkit.SaveRegister(mainModule + 0x9AEC57, 15, "eax", true, IntPtr.Zero);

                    #endregion

                    // enable UI
                    checkBox_PillSize.Enabled = true;

                    // save process session
                    oldProcessSession = processSession;
                }

                Thread.Sleep(5);
            }
        }

        private void checkBox_PillSize_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_PillSize.Checked)
            {
                // patch with default op code
                IntPtr pillSizeFunctionPointer = mainModule + 0x32FEEC0;
                byte[] pillSizeDefaultOp = { 0x48, 0x83, 0xEC, 0x38, 0x48, 0x8B, 0x89, 0x30, 0x01, 0x00, 0x00, 0x48, 0x85, 0xC9 };
                Toolkit.WriteMemory(pillSizeFunctionPointer, pillSizeDefaultOp);

                byte[] pillSizeTemp1 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
                byte[] pillSizeTemp2 = BitConverter.GetBytes((ulong)pillSizeAllocated);
                byte[] pillSizeEntry = Toolkit.MergeByteArrays(pillSizeTemp1, pillSizeTemp2);

                byte[] pillSizeTemp3 = { 0x48, 0xBA };
                byte[] pillSizeTemp4 = BitConverter.GetBytes((ulong)pillSizeAllocated + 0x100);
                byte[] pillSizeTemp5 = { 0x48, 0x83, 0xEC, 0x38, 0x48, 0x8B, 0x89, 0x30, 0x01, 0x00, 0x00, 0x48, 0x85, 0xC9 };
                byte[] pillSizeTemp6 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
                byte[] pillSizeTemp7 = BitConverter.GetBytes((ulong)pillSizeFunctionPointer + 14);
                byte[] pillSizeExit = Toolkit.MergeByteArrays(pillSizeTemp3, pillSizeTemp4, pillSizeTemp5, pillSizeTemp6, pillSizeTemp7);

                // set default value for resize
                pillSizePointer = pillSizeAllocated + 0x100;
                Toolkit.WriteMemory(pillSizePointer, BitConverter.GetBytes(3f));
                Toolkit.WriteMemory(pillSizePointer + 0x4, BitConverter.GetBytes(3f));
                Toolkit.WriteMemory(pillSizePointer + 0x8, BitConverter.GetBytes(3f));
                textBox_PillSize.Text = "3";

                // write memory
                Toolkit.WriteMemory(pillSizeAllocated, pillSizeExit);
                Toolkit.WriteMemory(pillSizeFunctionPointer, pillSizeEntry);

                textBox_PillSize.Enabled = true;
                button_PillSizeApply.Enabled = true;
            }
            else
            {
                // patch with default op code
                IntPtr pillSizeFunctionPointer = mainModule + 0x32FEEC0;
                byte[] pillSizeDefaultOp = { 0x48, 0x83, 0xEC, 0x38, 0x48, 0x8B, 0x89, 0x30, 0x01, 0x00, 0x00, 0x48, 0x85, 0xC9 };
                Toolkit.WriteMemory(pillSizeFunctionPointer, pillSizeDefaultOp);

                textBox_PillSize.Enabled = false;
                button_PillSizeApply.Enabled = false;
            }
        }

        private void button_PillSizeApply_Click(object sender, EventArgs e)
        {
            float toWrite;
            if (float.TryParse(textBox_PillSize.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out toWrite))
            {
                if (toWrite != 0)
                {
                    Toolkit.WriteMemory(pillSizePointer, BitConverter.GetBytes(toWrite));
                    Toolkit.WriteMemory(pillSizePointer + 0x4, BitConverter.GetBytes(toWrite));
                    Toolkit.WriteMemory(pillSizePointer + 0x8, BitConverter.GetBytes(toWrite));
                }
            }
            else Toolkit.ShowError("could not parse pill size value");
        }

        private void button_SavePlayerPosition_Click(object sender, EventArgs e)
        {
            byte[] rawInsidePointer = Toolkit.ReadMemoryBytes(playerPointer, 8);
            IntPtr aPointer = (IntPtr)BitConverter.ToInt64(rawInsidePointer, 0);

            INF.Vector3 temp = new INF.Vector3(
                Toolkit.ReadMemoryFloat(aPointer + 0x1D0),
                Toolkit.ReadMemoryFloat(aPointer + 0x1D0 + 0x4),
                Toolkit.ReadMemoryFloat(aPointer + 0x1D0 + 0x8));

            textBox_PlayerPositionX.Text = temp.X.ToString(CultureInfo.InvariantCulture);
            textBox_PlayerPositionY.Text = temp.Y.ToString(CultureInfo.InvariantCulture);
            textBox_PlayerPositionZ.Text = temp.Z.ToString(CultureInfo.InvariantCulture);
            selector.Select();
        }

        private void button_LoadPlayerPosition_Click(object sender, EventArgs e)
        {
            INF.Vector3 vector = new INF.Vector3();

            string posX = textBox_PlayerPositionX.Text.Replace(",", ".");
            string posY = textBox_PlayerPositionY.Text.Replace(",", ".");
            string posZ = textBox_PlayerPositionZ.Text.Replace(",", ".");

            if (float.TryParse(posX, NumberStyles.Float, CultureInfo.InvariantCulture, out vector.X) &&
            float.TryParse(posY, NumberStyles.Float, CultureInfo.InvariantCulture, out vector.Y) &&
            float.TryParse(posZ, NumberStyles.Float, CultureInfo.InvariantCulture, out vector.Z))
            {
                byte[] rawInsidePointer = Toolkit.ReadMemoryBytes(playerPointer, 8);
                IntPtr aPointer = (IntPtr)BitConverter.ToInt64(rawInsidePointer, 0);

                if (vector.X != 0 && vector.Y != 0 && vector.Z != 0)
                {
                    Toolkit.WriteMemory(aPointer + 0x1D0, BitConverter.GetBytes(vector.X));
                    Toolkit.WriteMemory(aPointer + 0x1D0 + 0x4, BitConverter.GetBytes(vector.Y));
                    Toolkit.WriteMemory(aPointer + 0x1D0 + 0x8, BitConverter.GetBytes(vector.Z));
                }
                else Toolkit.ShowError("did not load position as one of the values was 0");
            }
            else Toolkit.ShowError("one of the position values is incorrect");
            selector.Select();
        }

        private void checkBox_WindowAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_WindowAlwaysOnTop.Checked) TopMost = true;
            else TopMost = false;

            Saves.Save("settings", "WindowAlwaysOnTop", TopMost.ToString());
        }

        private void button_Hotkeys_Click(object sender, EventArgs e)
        {
            formHotkeysOpen = true;
            HotkeysPositionManager hotkeysForm = new HotkeysPositionManager();
            hotkeysForm.ShowDialog();
        }
    }
}
