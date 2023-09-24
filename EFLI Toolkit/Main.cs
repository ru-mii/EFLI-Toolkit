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

        // build version, adding new line because github adds it to their file
        // and the version is being compared with one written in github file in repo
        public static string softwareVersion = "3" + "\n";

        public static Process gameProcess = null;
        public static bool formHotkeysOpen = false;

        public static int hotkey_SavePlayerPosition = 0;
        public static int hotkey_LoadPlayerPosition = 0;
        public int[] keyboardTableReady = new int[999];

        float originalMusicLevel = 0;
        bool statsEnabled = false;

        IntPtr mainModule = IntPtr.Zero;
        IntPtr sizeMainModule = IntPtr.Zero;

        IntPtr pillSizeAllocated = IntPtr.Zero;
        IntPtr shipAllocated = IntPtr.Zero;
        IntPtr musicAllocated = IntPtr.Zero;
        IntPtr miscAllocated = IntPtr.Zero;
        IntPtr timingAllocated = IntPtr.Zero;
        IntPtr pillSizeStatsAllocated = IntPtr.Zero;
        IntPtr cameraAllocated = IntPtr.Zero;

        IntPtr pillSizePointer = IntPtr.Zero;
        IntPtr playerPointer = IntPtr.Zero;
        IntPtr shipSpeedPointer = IntPtr.Zero;
        IntPtr musicPointer = IntPtr.Zero;
        IntPtr pillSizeStatsPointer = IntPtr.Zero;
        IntPtr cameraPointer = IntPtr.Zero;

        private void Main_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            selector.Select();

            if (!Toolkit.IsProcessRunning("LavenderIsland-Win64-Shipping"))
            {
                Toolkit.ShowError("game needs to be open, closing now...");
                Environment.Exit(0);
            }
            else PatchAndAllocate();

            if (Saves.Read("flags", "visualChangesWarning") == "")
            {
                Toolkit.ShowInfo("this tool makes multiple visual changes to the game, make sure to restart it before doing runs to not have your run invalidated!");
                Saves.Save("flags", "visualChangesWarning", "1");
            }

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

            label_Version.Text = "v" + softwareVersion.ToString();

            backgroundWorker_CheckProcess.RunWorkerAsync();
            backgroundWorker.RunWorkerAsync();
            backgroundWorker_CheckUpdates.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                //
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

                //
                if (statsEnabled)
                {
                    float pillSize = Toolkit.ReadMemoryFloat(pillSizeStatsPointer);
                    label_StatsPillSize.Text = "last pill size: " + pillSize.ToString(CultureInfo.InvariantCulture);

                    //

                    byte[] rawInsidePointer = Toolkit.ReadMemoryBytes(playerPointer, 8);
                    IntPtr aPointer = (IntPtr)BitConverter.ToInt64(rawInsidePointer, 0);

                    INF.Vector3 temp = new INF.Vector3(
                        Toolkit.ReadMemoryFloat(aPointer + 0x1D0),
                        Toolkit.ReadMemoryFloat(aPointer + 0x1D0 + 0x4),
                        Toolkit.ReadMemoryFloat(aPointer + 0x1D0 + 0x8));

                    label_StatsPosX.Text = "posX: " + temp.X.ToString(CultureInfo.InvariantCulture);
                    label_StatsPosY.Text = "posY: " + temp.Y.ToString(CultureInfo.InvariantCulture);
                    label_StatsPosZ.Text = "posZ: " + temp.Z.ToString(CultureInfo.InvariantCulture);

                    //
                    byte[] rawInsidePointer2 = Toolkit.ReadMemoryBytes(cameraPointer, 8);
                    IntPtr aPointer2 = (IntPtr)BitConverter.ToInt64(rawInsidePointer2, 0) + 0x0C;

                    float camX = Toolkit.ReadMemoryFloat(aPointer2 + 0x4);
                    float camY = Toolkit.ReadMemoryFloat(aPointer2);

                    label_StatsCameraX.Text = "camX: " + camX.ToString(CultureInfo.InvariantCulture);
                    label_StatsCameraY.Text = "camY: " + camY.ToString(CultureInfo.InvariantCulture);
                }

                Thread.Sleep(10);
            }
        }

        private void PatchAndAllocate()
        {
            bool foundProcess = false;
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName == "LavenderIsland-Win64-Shipping")
                {
                    gameProcess = process;
                    foundProcess = true;
                }
            }

            if (foundProcess)
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
            }
            else Environment.Exit(0);

            // allocate for dynamic patches
            pillSizeAllocated = Toolkit.AllocateMemory();
            shipAllocated = Toolkit.AllocateMemory();
            musicAllocated = Toolkit.AllocateMemory();
            miscAllocated = Toolkit.AllocateMemory();
            timingAllocated = Toolkit.AllocateMemory();
            pillSizeStatsAllocated = Toolkit.AllocateMemory();
            cameraAllocated = Toolkit.AllocateMemory();

            // *********************

            #region StaticPatches

            // position manager
            byte[] defaultOp = { 0x0F, 0x10, 0x88, 0xD0, 0x01, 0x00, 0x00, 0x0F, 0x28, 0xC1, 0xF3, 0x0F, 0x11, 0x4D, 0xE0 };
            Toolkit.WriteMemory(mainModule + 0x9AEC57, defaultOp);
            playerPointer = Toolkit.SaveRegister(mainModule + 0x9AEC57, 15, "eax", true, IntPtr.Zero);

            // game music
            byte[] temp1 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] temp2 = BitConverter.GetBytes((ulong)musicAllocated);
            byte[] temp3 = { 0x90, 0x90 };
            byte[] start = Toolkit.MergeByteArrays(temp1, temp2, temp3);

            byte[] temp4 = { 0x48, 0x81, 0xFF, 0x88, 0x00, 0x00, 0x00, 0x75, 0x07, 0x48, 0x89, 0x1D, 0xF0, 0x00, 0x00, 0x00 };
            byte[] temp5 = { 0x0F, 0x10, 0x43, 0x28, 0x49, 0x8B, 0x07, 0x0F, 0x11, 0x44, 0x07, 0x08, 0x0F, 0x10, 0x4B, 0x38 };
            byte[] temp6 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] temp7 = BitConverter.GetBytes((ulong)mainModule + 0x33B0999);
            byte[] exit = Toolkit.MergeByteArrays(temp4, temp5, temp6, temp7);

            Toolkit.WriteMemory(musicAllocated, exit);
            Toolkit.WriteMemory(mainModule + 0x33B0989, start);
            musicPointer = musicAllocated + 0x100;

            //
            byte[] part1 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] part2 = BitConverter.GetBytes((ulong)miscAllocated);
            byte[] part3 = { 0x90 };
            byte[] start2 = Toolkit.MergeByteArrays(part1, part2, part3);

            byte[] part4 = { 0x48, 0x89, 0x1D, 0xF9, 0x00, 0x00, 0x00, 0x81, 0x3B, 0x49, 0x00, 0x73, 0x00, 0x75, 0x59, 0x48,
            0x83, 0xC3, 0x04, 0x81, 0x3B, 0x6C, 0x00, 0x61, 0x00, 0x75, 0x4D, 0x48, 0x83, 0xC3, 0x04, 0x81,
            0x3B, 0x6E, 0x00, 0x64, 0x00, 0x75, 0x41, 0x48, 0x83, 0xC3, 0x04, 0x81, 0x3B, 0x20, 0x00, 0x58,
            0x00, 0x75, 0x35, 0x48, 0x8B, 0x1D, 0xC6, 0x00, 0x00, 0x00, 0xC7, 0x03, 0x62, 0x00, 0x79, 0x00,
            0x48, 0x83, 0xC3, 0x04, 0xC7, 0x03, 0x20, 0x00, 0x72, 0x00, 0x48, 0x83, 0xC3, 0x04, 0xC7, 0x03,
            0x75, 0x00, 0x6D, 0x00, 0x48, 0x83, 0xC3, 0x04, 0xC7, 0x03, 0x69, 0x00, 0x69, 0x00, 0x48, 0x83,
            0xC3, 0x04, 0xC7, 0x03, 0x3A, 0x00, 0x20, 0x00, 0x48, 0x8B, 0x1D, 0x91, 0x00, 0x00, 0x00 };
            byte[] part5 = { 0x41, 0xBB, 0x01, 0x00, 0x00, 0x00, 0x4C, 0x89, 0x7C, 0x24, 0x20, 0x45, 0x8D, 0x6A, 0x01 };
            byte[] part6 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] part7 = BitConverter.GetBytes((ulong)mainModule + 0x1B52701);
            byte[] exit2 = Toolkit.MergeByteArrays(part4, part5, part6, part7);

            Toolkit.WriteMemory(miscAllocated, exit2);
            Toolkit.WriteMemory(mainModule + 0x1B526F2, start2);

            // 
            byte[] part11 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] part22 = BitConverter.GetBytes((ulong)timingAllocated);
            byte[] part33 = { 0x90, 0x90 };
            byte[] start22 = Toolkit.MergeByteArrays(part11, part22, part33);

            byte[] part44 = { 0x48, 0x89, 0x1D, 0xF9, 0x00, 0x00, 0x00, 0x81, 0x3B, 0x45, 0x00, 0x53, 0x00, 0x75, 0x31, 0x48,
            0x83, 0xC3, 0x04, 0x81, 0x3B, 0x43, 0x00, 0x41, 0x00, 0x75, 0x25, 0x48, 0x83, 0xC3, 0x04, 0x81,
            0x3B, 0x50, 0x00, 0x45, 0x00, 0x75, 0x19, 0x48, 0x83, 0xC3, 0x04, 0x81, 0x3B, 0x20, 0x00, 0x46,
            0x00, 0x75, 0x0D, 0x48, 0x8B, 0x1D, 0xC6, 0x00, 0x00, 0x00, 0xC7, 0x03, 0x33, 0x00, 0x53, 0x00,
            0x48, 0x8B, 0x1D, 0xB9, 0x00, 0x00, 0x00 };
            byte[] part55 = { 0x48, 0x89, 0x74, 0x24, 0x60, 0x41, 0xBA, 0x28, 0x20, 0x00, 0x00, 0x48, 0x89, 0x6C, 0x24, 0x58 };
            byte[] part66 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] part77 = BitConverter.GetBytes((ulong)mainModule + 0x1B526ED);
            byte[] exit22 = Toolkit.MergeByteArrays(part44, part55, part66, part77);

            Toolkit.WriteMemory(timingAllocated, exit22);
            Toolkit.WriteMemory(mainModule + 0x1B526DD, start22);

            //
            byte[] kekw1 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] kekw2 = BitConverter.GetBytes((ulong)pillSizeStatsAllocated);
            byte[] kekstart = Toolkit.MergeByteArrays(kekw1, kekw2);

            byte[] kek1 = { 0x66, 0x0F, 0x7E, 0x05, 0xF8, 0x00, 0x00, 0x00, 0x8B, 0x42, 0x08, 0x48,
                0x8D, 0x54, 0x24, 0x20, 0xF2, 0x0F, 0x11, 0x44, 0x24, 0x20 };
            byte[] kek2 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] kek3 = BitConverter.GetBytes((ulong)mainModule + 0x32FEEE2);
            byte[] kekexit = Toolkit.MergeByteArrays(kek1, kek2, kek3);

            Toolkit.WriteMemory(pillSizeStatsAllocated, kekexit);
            Toolkit.WriteMemory(mainModule + 0x32FEED4, kekstart);

            pillSizeStatsPointer = pillSizeStatsAllocated + 0x100;

            //
            byte[] cam1 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] cam2 = BitConverter.GetBytes((ulong)cameraAllocated);
            byte[] cam3 = { 0x90 };
            byte[] camstart = Toolkit.MergeByteArrays(cam1, cam2, cam3);

            byte[] cam4 = { 0x48, 0x89, 0x3D, 0xF9, 0x00, 0x00, 0x00 };
            byte[] cam5 = { 0x8B, 0x83, 0x98, 0x01, 0x00, 0x00, 0x44, 0x0F, 0x28, 0xBC, 0x24, 0x70, 0x03, 0x00, 0x00 };
            byte[] cam6 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] cam7 = BitConverter.GetBytes((ulong)mainModule + 0x33EFE4E);
            byte[] camexit = Toolkit.MergeByteArrays(cam4, cam5, cam6, cam7);

            cameraPointer = cameraAllocated + 0x100;
            Toolkit.WriteMemory(cameraAllocated, camexit);
            Toolkit.WriteMemory(mainModule + 0x33EFE3F, camstart);

            #endregion
        }

        private void backgroundWorker_CheckProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (!Toolkit.IsProcessRunning("LavenderIsland-Win64-Shipping")) Environment.Exit(0);
                Thread.Sleep(100);
            }
        }

        private void checkBox_PillSize_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_PillSize.Checked)
            {
                if (Toolkit.IsProcessRunning("LavenderIsland-Win64-Shipping"))
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
                }

                label_Size.Enabled = true;
                textBox_PillSize.Enabled = true;
                button_PillSizeApply.Enabled = true;
            }
            else
            {
                if (Toolkit.IsProcessRunning("LavenderIsland-Win64-Shipping"))
                {
                    // patch with default op code
                    IntPtr pillSizeFunctionPointer = mainModule + 0x32FEEC0;
                    byte[] pillSizeDefaultOp = { 0x48, 0x83, 0xEC, 0x38, 0x48, 0x8B, 0x89, 0x30, 0x01, 0x00, 0x00, 0x48, 0x85, 0xC9 };
                    Toolkit.WriteMemory(pillSizeFunctionPointer, pillSizeDefaultOp);
                }

                label_Size.Enabled = false;
                textBox_PillSize.Enabled = false;
                button_PillSizeApply.Enabled = false;
            }
        }

        private void button_PillSizeApply_Click(object sender, EventArgs e)
        {
            float toWrite;
            string converted = textBox_PillSize.Text.Replace(",", ".");
            if (float.TryParse(converted, NumberStyles.Float, CultureInfo.InvariantCulture, out toWrite))
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

        private void backgroundWorker_CheckUpdates_DoWork(object sender, DoWorkEventArgs e)
        { Updates.CheckForUpdates(); }

        private void checkBox_Ship_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Ship.Checked)
            {
                byte[] temp1 = { 0x48, 0xB8 };
                byte[] temp2 = BitConverter.GetBytes((ulong)shipAllocated);
                byte[] temp3 = { 0xFF, 0xE0 };

                byte[] temp4 = { 0x48, 0xB8 };
                byte[] temp5 = BitConverter.GetBytes((ulong)shipAllocated + 0x100);
                byte[] temp6 = { 0x66, 0x0F, 0x6E, 0x00 };
                byte[] temp7 = { 0x48, 0x83, 0xC4, 0x20 };
                byte[] temp8 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
                byte[] temp9 = BitConverter.GetBytes((ulong)mainModule + 0x342E0B2);

                byte[] start = Toolkit.MergeByteArrays(temp1, temp2, temp3);
                byte[] exit = Toolkit.MergeByteArrays(temp4, temp5, temp6, temp7, temp8, temp9);

                shipSpeedPointer = shipAllocated + 0x100;

                float lastSpeed = BitConverter.ToSingle(Toolkit.ReadMemoryBytes(shipAllocated + 0x100, 4), 0);
                if (lastSpeed == 0) Toolkit.WriteMemory(shipSpeedPointer, BitConverter.GetBytes(1000f));

                Toolkit.WriteMemory(shipAllocated, exit);
                Toolkit.WriteMemory(mainModule + 0x342E0A6, start);

                label_ShipSpeed.Enabled = true;
                textBox_ShipSpeed.Enabled = true;
                button_ApplyShipSpeed.Enabled = true;
                checkBox_InfiniteShipPower.Enabled = true;
            }
            else
            {
                byte[] temp1 = { 0xF3, 0x0F, 0x10, 0x83, 0x98, 0x01, 0x00, 0x00, 0x48, 0x83, 0xC4, 0x20 };
                Toolkit.WriteMemory(mainModule + 0x342E0A6, temp1);

                label_ShipSpeed.Enabled = false;
                textBox_ShipSpeed.Enabled = false;
                button_ApplyShipSpeed.Enabled = false;
                checkBox_InfiniteShipPower.Enabled = false;
            }
        }

        private void checkBox_InfiniteShipPower_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_InfiniteShipPower.Checked)
            {
                byte[] nops = { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                Toolkit.WriteMemory(mainModule + 0xA91EC5, nops);
            }
            else
            {
                //F3 0F 11 83 D0 05 00 00
                byte[] nops = { 0xF3, 0x0F, 0x11, 0x83, 0xD0, 0x05, 0x00, 0x00 };
                Toolkit.WriteMemory(mainModule + 0xA91EC5, nops);
            }
        }

        private void button_ApplyShipSpeed_Click(object sender, EventArgs e)
        {
            float toWrite = 0;
            string converted = textBox_ShipSpeed.Text.Replace(",", ".");

            if (float.TryParse(converted, NumberStyles.Float, CultureInfo.InvariantCulture, out toWrite))
                Toolkit.WriteMemory(shipSpeedPointer, BitConverter.GetBytes(toWrite));
            else Toolkit.ShowError("could not parse pill size value");
        }

        private void checkBox_TurnOffGameMusic_CheckedChanged(object sender, EventArgs e)
        {
            IntPtr actualClass = (IntPtr)BitConverter.ToUInt64(Toolkit.ReadMemoryBytes(musicPointer, 8), 0);

            if (checkBox_TurnOffGameMusic.Checked)
            {
                if (originalMusicLevel == 0) originalMusicLevel = Toolkit.ReadMemoryFloat(actualClass + 0x30);
                Toolkit.WriteMemory(actualClass + 0x30, BitConverter.GetBytes(0f));
            }
            else Toolkit.WriteMemory(actualClass + 0x30, BitConverter.GetBytes(originalMusicLevel));
        }

        private void checkBox_Stats_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Stats.Checked)
            {
                statsEnabled = true;
                label_StatsPosX.Enabled = true;
                label_StatsPosY.Enabled = true;
                label_StatsPosZ.Enabled = true;
                label_StatsPillSize.Enabled = true;
                label_StatsCameraX.Enabled = true;
                label_StatsCameraY.Enabled = true;
            }
            else
            {
                statsEnabled = false;
                label_StatsPosX.Enabled = false;
                label_StatsPosY.Enabled = false;
                label_StatsPosZ.Enabled = false;
                label_StatsPillSize.Enabled = false;
                label_StatsCameraX.Enabled = false;
                label_StatsCameraY.Enabled = false;
            }
        }
    }
}