using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsFormsApp1
{
    class memory
    {

        const int PROCESS_WM_READ = 0x0010;


        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);



        const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte [] lpBuffer, uint dwSize, out int lpNumberOfBytesWritten);

        public string readMemory(string processString, IntPtr memoryLocation)
        {

            Process process = Process.GetProcessesByName(processString)[0];
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);

            var modBase = GetModuleBaseAddress(process, "TiberianDawn.dll");

            

            int bytesRead = 0;
            byte[] buffer = new byte[24];

            ReadProcessMemory(processHandle, modBase+(int)memoryLocation, buffer, 24, ref bytesRead);

            string hexRet = ByteArrayToString(buffer);

            return hexRet;

        }

        public void writeMemory(string processString, IntPtr memoryLocation, byte[] data, bool useBitConv)
        {

            try
            {

                Process process = Process.GetProcessesByName(processString)[0];
                IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

                var modBase = GetModuleBaseAddress(process, "TiberianDawn.dll");

                int bytesWritten = 0;

                WriteProcessMemory(processHandle, modBase + (int)memoryLocation, data, (uint)data.Length, out bytesWritten);

            }
            catch
            {

               

            }
          
            



            //int bytesRead = 0;
           // byte[] bufferX = new byte[24];

            //ReadProcessMemory(processHandle, modBase + (int)memoryLocation, bufferX, 24, ref bytesRead);



        }


            public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x1}", b);
            return hex.ToString();
        }



        public static IntPtr GetModuleBaseAddress(Process proc, string modName)
        {
            IntPtr addr = IntPtr.Zero;

            foreach (ProcessModule m in proc.Modules)
            {

               
                if (m.ModuleName == modName)
                {
                    
                    addr = m.BaseAddress;
                   
                }
            }
            return addr;
        }

        public static IntPtr FindDMAAddy(IntPtr hProc, IntPtr ptr, int[] offsets)
        {
            var buffer = new byte[IntPtr.Size];
            int bytesWritten = 0;
            foreach (int i in offsets)
            {
                ReadProcessMemory(hProc, ptr, buffer, buffer.Length, ref bytesWritten);

                ptr = (IntPtr.Size == 4)
                ? IntPtr.Add(new IntPtr(BitConverter.ToInt32(buffer, 0)), i)
                : ptr = IntPtr.Add(new IntPtr(BitConverter.ToInt64(buffer, 0)), i);
            }
            return ptr;
        }

    }
}

