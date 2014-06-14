#region License

// Copyright (c) 2005-2014, CellAO Team
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

namespace CellAO_Launcher
{
    #region Usings ...

    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;

    #endregion

    /// <summary>
    /// </summary>
    public static class Injector
    {
        /// <summary>
        /// </summary>
        [Flags]
        public enum ThreadAccess : int
        {
            /// <summary>
            /// </summary>
            TERMINATE = 0x0001, 

            /// <summary>
            /// </summary>
            SUSPEND_RESUME = 0x0002, 

            /// <summary>
            /// </summary>
            GET_CONTEXT = 0x0008, 

            /// <summary>
            /// </summary>
            SET_CONTEXT = 0x0010, 

            /// <summary>
            /// </summary>
            SET_INFORMATION = 0x0020, 

            /// <summary>
            /// </summary>
            QUERY_INFORMATION = 0x0040, 

            /// <summary>
            /// </summary>
            SET_THREAD_TOKEN = 0x0080, 

            /// <summary>
            /// </summary>
            IMPERSONATE = 0x0100, 

            /// <summary>
            /// </summary>
            DIRECT_IMPERSONATION = 0x0200
        }

        /// <summary>
        /// </summary>
        /// <param name="dwDesiredAccess">
        /// </param>
        /// <param name="bInheritHandle">
        /// </param>
        /// <param name="dwThreadId">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        /// <summary>
        /// </summary>
        /// <param name="hThread">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);

        /// <summary>
        /// </summary>
        /// <param name="hThread">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("kernel32.dll")]
        private static extern int ResumeThread(IntPtr hThread);

        /// <summary>
        /// </summary>
        /// <param name="PID">
        /// </param>
        public static void SuspendProcess(int PID)
        {
            Process proc = Process.GetProcessById(PID);

            if (proc.ProcessName == string.Empty)
            {
                return;
            }

            foreach (ProcessThread pT in proc.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }

                SuspendThread(pOpenThread);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="PID">
        /// </param>
        public static void ResumeProcess(int PID)
        {
            Process proc = Process.GetProcessById(PID);

            if (proc.ProcessName == string.Empty)
            {
                return;
            }

            foreach (ProcessThread pT in proc.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }

                ResumeThread(pOpenThread);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ProcessId">
        /// </param>
        /// <param name="processHandle">
        /// </param>
        /// <param name="search">
        /// </param>
        /// <param name="replace">
        /// </param>
        public static void SearchAndReplace(int ProcessId, IntPtr processHandle, string search, string replace)
        {
            SafeMemoryHandle handle = Memory.OpenProcess(ProcessAccessFlags.AllAccess, ProcessId);
            int found = 0;
            IntPtr memory = (IntPtr)1;
            while (true)
            {
                memory = Memory.Find(processHandle, handle, search);
                if ((int)memory == 0)
                {
                    break;
                }

                byte[] repl = ASCIIEncoding.ASCII.GetBytes(replace);
                MemoryProtectionFlags oldProt;
                oldProt = Memory.ChangeProtection(handle, memory, repl.Length, MemoryProtectionFlags.ExecuteReadWrite);
                Memory.WriteBytes(handle, memory, repl);
                Memory.ChangeProtection(handle, memory, repl.Length, oldProt);
                found++;
            }
        }
    }
}