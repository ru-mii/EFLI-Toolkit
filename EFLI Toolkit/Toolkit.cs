using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace EFLI_Toolkit
{
    public static class Toolkit
    {
        /* THIS IS FOR 32-BIT */
        /*[StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public uint BaseAddress;
            public uint AllocationBase;
            public uint AllocationProtect;
            public uint RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }*/

        /* THIS IS FOR 64-BIT */
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public ulong BaseAddress;
            public ulong AllocationBase;
            public int AllocationProtect;
            public int __alignment1;
            public ulong RegionSize;
            public int State;
            public int Protect;
            public int Type;
            public int __alignment2;
        }

        // exported read memory function
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
        IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesRead);

        // exported virtual query function
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress,
        out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        // imported memory allocation
        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
        uint dwSize, uint flAllocationType, uint flProtect);

        // imported protection change
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress,
        int dwSize, uint flNewProtect, out IntPtr lpflOldProtect);

        // imported write memory 
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
        IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

        // imported key state
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        // signature is just a signature in this type "8B CF 8B D0 E8 ?? ?? ?? ?? 85 C0 74 19 A1"
        // process handle is handle of a process really nothing to say about it
        // foundAddressOffset is being added to the returned address, if it's on minus obv it will substract
        // foundAddressOffset helps with finding and returning pointers easier
        // start address is the address when it starts scanning from, try using it with blocks of 0x1000
        // maxAddress is the last address it will try scanning in, if it goes above below function will return 0
        public static IntPtr SignatureScan(string signature, IntPtr processHandle, IntPtr foundAddressOffset, IntPtr startAddress, IntPtr maxAddress, int protectionType)
        {
            // this is a first memory address of currently used block of 0x1000 addresses
            IntPtr lastAddressBlock = (IntPtr)0;

            // we are splitting signature to bytes in string array
            string[] explodedSignature = signature.Split(' ');

            // array of bytes that will hold 
            byte[] byteSignature = new byte[explodedSignature.Length];

            // mask can have either "?" or "x" type of characters, if it's "?" it means we skip this byte and
            // act like we found it, where for "x" the byte actually has to match, it's used for signatures
            // where some bytes inside might be changing per game session and we want to skip them
            // mask will look like this "xxxxxxxxxx?xxx?"
            string mask = "";

            // memory basic information for region access level information
            MEMORY_BASIC_INFORMATION memoryInfo = new MEMORY_BASIC_INFORMATION();

            // converting string byte array to just normal byte array above
            for (int i = 0; i < byteSignature.Length; i++)
            {
                if (explodedSignature[i] != "??") byteSignature[i] = byte.Parse(explodedSignature[i], System.Globalization.NumberStyles.HexNumber);
                else byteSignature[i] = 0; // doesn't matter if it's 0 because mask will make it skip this byte anyway
            }

            // creating mask based on provided signatures and question marks instead of bytes
            // for full explanation read above initialization of "mask" variable 
            foreach (string character in explodedSignature)
            {
                if (character != "??") mask += "x";
                else mask += "?";
            }

            // changing starting point for scanning
            lastAddressBlock = startAddress;

            // interlan max address for blocks
            IntPtr internalMaxAddress = (IntPtr)0;

            while ((long)lastAddressBlock < (long)maxAddress)
            {
                // amount of successful bytes found, like 8B or CF, if all of them found
                // meaning it exceeds certain number then we found address of signature
                int foundBytes = 0;

                // this is offset used for for specific indication where signature was found
                // for example we know block is 0x5000 but it could be then between 0x5000 and 0x6000
                // if we know the byte offset we can then add it to block and have eg 0x5202
                int blockReturnOffset = 0;

                // checking memory info to skip blocks that have wrong type
                VirtualQueryEx(processHandle, lastAddressBlock, out memoryInfo, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                // no protection info required
                int tempProtection = memoryInfo.Protect;
                if (protectionType == 0) tempProtection = 0;

                // memory block checks
                if (memoryInfo.State == 0x1000 && tempProtection == protectionType && memoryInfo.Protect != 0x1)
                {
                    internalMaxAddress = (IntPtr)(memoryInfo.BaseAddress + memoryInfo.RegionSize);

                    while ((long)lastAddressBlock < (long)internalMaxAddress)
                    {
                        // chunk of returned bytes from read process memory function
                        // it take program a lot longer for program to read memory bytes one by one
                        // instead of reading it through array of bytes, it's just for optimization
                        byte[] blockReturn = new byte[0x1000];

                        // below reads 0x1000 bytes and puts them in an array variable "blockReturn"
                        if (ReadProcessMemory(processHandle, lastAddressBlock, blockReturn, 0x1000, out IntPtr nullification))
                        {

                            // restarting offset to -1 with every new block of bytes
                            blockReturnOffset = 0;

                            foreach (byte singleByte in blockReturn)
                            {
                                // "foundBytes" is also being used as offset for signature & mask arrays
                                // because every time it succeds we want to look for next byte in an array
                                // and every time we find one we add 1 to "foundBytes"

                                // we don't skip the byte and act like we found it if mask doesn't say so
                                if (mask[foundBytes] != '?')
                                {
                                    // if byte matches byte in signature (then goes to next one on next iteration)
                                    if (singleByte == byteSignature[foundBytes])
                                        foundBytes += 1;

                                    // we reset to 0 because if found signature breaks it's no longer the signature we're looking for
                                    else foundBytes = 0;

                                    // if number of successfully found bytes in order reaches signature length it means we found the signature
                                    // so we're returning block + block offset + found address offset and minus signature length
                                    if (foundBytes == byteSignature.Length)
                                        return (IntPtr)(((long)lastAddressBlock + blockReturnOffset + (long)foundAddressOffset - byteSignature.Length) + 1);
                                }
                                // if mask says to skip byte and act like we found them without even searching => we do
                                else foundBytes += 1;

                                // adding offset to narrow down when returning to address and not block of 0x1000
                                blockReturnOffset += 1;
                            }
                        }

                        // setting up variable for reading next block when done with current one
                        lastAddressBlock += 0x1000;
                    }
                }

                // going to the next memory block
                if (memoryInfo.BaseAddress + memoryInfo.RegionSize == 0x7fffffff0000) break;
                lastAddressBlock = (IntPtr)(memoryInfo.BaseAddress + (ulong)memoryInfo.RegionSize);
            }

            // it went through all of the addresses in provided range startAddress <-> maxAddress
            // and couldn't find anything so returning 0 meaning it failed to find signature
            return (IntPtr)0;
        }

        public static IntPtr SaveRegister(IntPtr address, int stolenLength, string register, bool gameCodeFirst, IntPtr allocated)
        {
            DisableMemoryProtection(address, 0x1000);

            if (allocated == IntPtr.Zero) allocated = AllocateMemory();
            register = register.ToLower();

            byte[] temp1 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] temp2 = BitConverter.GetBytes((ulong)allocated);
            byte[] entry = MergeByteArrays(temp1, temp2);

            if (entry.Length < stolenLength)
            {
                byte[] additional = new byte[stolenLength - entry.Length];
                for (int i = 0; i < additional.Length; i++) additional[i] = 0x90;
                entry = MergeByteArrays(entry, additional);
            }

            byte[] steal = ReadMemoryBytes(address, stolenLength);
            byte[] movReg = new byte[2];
            if (register == "eax" || register == "rax") movReg = new byte[] { 0x48, 0xA3 };
            else if (register == "rdi" || register == "edi") movReg = new byte[] { 0x0, 0x0 };
            byte[] temp4 = BitConverter.GetBytes((long)(allocated + 0x100));

            byte[] last1 = { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 };
            byte[] last2 = BitConverter.GetBytes((ulong)address + (ulong)stolenLength);

            byte[] exit = new byte[0];
            if (gameCodeFirst) exit = MergeByteArrays(steal, movReg, temp4, last1, last2);
            else exit = MergeByteArrays(movReg, temp4, steal, last1, last2);

            WriteMemory(allocated, exit);
            WriteMemory(address, entry);

            return allocated + 0x100;
        }

        public static byte[] GetPtrBytes(IntPtr ptr)
        {
            int size = Marshal.SizeOf(typeof(IntPtr));
            byte[] bytes = new byte[size];
            Marshal.Copy(ptr, bytes, 0, size);
            return bytes;
        }

        // reads float value from memory
        public static float ReadMemoryFloat(IntPtr address)
        {
            // 4 because float is 4
            byte[] rawBytes = new byte[4];

            // read raw bytes
            ReadProcessMemory(Main.gameProcess.Handle, address,
            rawBytes, rawBytes.Length, out IntPtr nullification);

            // return parsed bytes to float
            return BitConverter.ToSingle(rawBytes, 0);
        }

        public static int GetRelativeAddress(IntPtr start, IntPtr destination)
        {
            // Cast IntPtr to long to perform arithmetic
            long srcAddress = (long)start;
            long dstAddress = (long)destination;

            // Calculate relative address
            int relativeAddress = (int)(dstAddress - srcAddress - 5);
            return relativeAddress;
        }

        public static byte[] ReadMemoryBytes(IntPtr address, int length)
        {
            byte[] toReturn = new byte[length];
            ReadProcessMemory(Main.gameProcess.Handle, address, toReturn, length, out IntPtr nullification);
            return toReturn;
        }

        // merging byte arrays with concat
        public static byte[] MergeByteArrays(params byte[][] arrays)
        {
            // create empty array
            byte[] byteArray = new byte[0];

            // go through each arrays and connect with concat
            foreach (byte[] array in arrays)
                byteArray = byteArray.Concat(array).ToArray();

            // returned merged arrays
            return byteArray;
        }

        // allocates memory
        public static IntPtr AllocateMemory()
        { return VirtualAllocEx(Main.gameProcess.Handle, IntPtr.Zero, 0x1000, 0x1000 | 0x2000, 0x40); }

        // change memory protection
        public static bool DisableMemoryProtection(IntPtr address, int size)
        { return VirtualProtectEx(Main.gameProcess.Handle, address, size, 0x40, out IntPtr zero); }

        // write memory easily, a cool wrapper
        public static bool WriteMemory(IntPtr address, byte[] array)
        {
            // longer function in wrapper
            return WriteProcessMemory(Main.gameProcess.Handle, address,
                array, array.Length, out IntPtr nullification);
        }

        public static bool IsProcessRunning(string name)
        {
            foreach (Process process in Process.GetProcesses())
                if (process.ProcessName == name) return true;

            return false;
        }

        // checks if key is pressed
        public static bool IsKeyPressed(int keyCode)
        {
            // read key status from table and compare
            short keyStatus = GetAsyncKeyState(keyCode);

            // return if pressed
            if ((keyStatus & 0x8000) > 0) return true;
            else return false;
        }

        // show custom message box with warning or error icon
        public static void ShowError(string message) { MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        public static void ShowInfo(string message) { MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); }
    }
}
