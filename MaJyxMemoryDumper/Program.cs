﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MaJyxMemoryDumper
{

    unsafe class Program
    {

        #region Declaration Memory Manager

        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);


        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }

        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }

        #endregion

        static void Main(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("No process ID specified"); // Check for null array
            }
            else
            {
                int ID = 0;

                if (int.TryParse(args[0], out ID))
                {
                    Console.WriteLine("Dump start");
                    try
                    {
                        Process process = Process.GetProcessById(ID);
                        SYSTEM_INFO sys_info = new SYSTEM_INFO();
                        GetSystemInfo(out sys_info);

                        IntPtr proc_min_address = sys_info.minimumApplicationAddress;
                        IntPtr proc_max_address = sys_info.maximumApplicationAddress;

                        long proc_min_address_l = (long)proc_min_address;
                        long proc_max_address_l = (long)proc_max_address;

                        IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);

                        StreamWriter sw = new StreamWriter("dump_" + process.ProcessName + "_" + process.Id + ".txt");

                        MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

                        int bytesRead = 0;

                        while (proc_min_address_l < proc_max_address_l)
                        {
                            VirtualQueryEx(processHandle, proc_min_address, out mem_basic_info, (uint)sizeof(MEMORY_BASIC_INFORMATION));

                            if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                            {
                                byte[] buffer = new byte[mem_basic_info.RegionSize];

                                ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);

                                for (int i = 0; i < mem_basic_info.RegionSize; i++)
                                    sw.Write((char)buffer[i]);
                            }

                            proc_min_address_l += mem_basic_info.RegionSize;
                            proc_min_address = new IntPtr(proc_min_address_l);
                        }

                        sw.Close();
                        Console.WriteLine("Dump finished");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Wrong argument format");
                }
                

            }
        }
    }
}
