namespace CellAO_Launcher
{
    #region Usings ...

    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Text;

    using Microsoft.Win32.SafeHandles;

    #endregion

    #endregion

    #region ProcessAccessFlags

    #region MemoryReleaseFlags

    /// <summary>
    /// Memory-release options list.
    /// </summary>
    [Flags]
    public enum MemoryReleaseFlags
    {
        /// <summary>
        /// Decommits the specified region of committed pages. After the operation, the pages are in the reserved state.
        /// The function does not fail if you attempt to decommit an uncommitted page. 
        /// This means that you can decommit a range of pages without first determining their current commitment state.
        /// Do not use this value with MEM_RELEASE.
        /// </summary>
        Decommit = 0x4000, 

        /// <summary>
        /// Releases the specified region of pages. After the operation, the pages are in the free state.
        /// If you specify this value, dwSize must be 0 (zero), and lpAddress must point to the base address returned by the VirtualAllocEx function when the region is reserved. 
        /// The function fails if either of these conditions is not met.
        /// If any pages in the region are committed currently, the function first decommits, and then releases them.
        /// The function does not fail if you attempt to release pages that are in different states, some reserved and some committed. 
        /// This means that you can release a range of pages without first determining the current commitment state.
        /// Do not use this value with MEM_DECOMMIT.
        /// </summary>
        Release = 0x8000
    }

    #endregion

    #region MemoryStateFlags

    /// <summary>
    /// Memory-state options list.
    /// </summary>
    [Flags]
    public enum MemoryStateFlags
    {
        /// <summary>
        /// Indicates committed pages for which physical storage has been allocated, either in memory or in the paging file on disk.
        /// </summary>
        Commit = 0x1000, 

        /// <summary>
        /// Indicates free pages not accessible to the calling process and available to be allocated. 
        /// For free pages, the information in the AllocationBase, AllocationProtect, Protect, and Type members is undefined.
        /// </summary>
        Free = 0x10000, 

        /// <summary>
        /// Indicates reserved pages where a range of the process's virtual address space is reserved without any physical storage being allocated. 
        /// For reserved pages, the information in the Protect member is undefined.
        /// </summary>
        Reserve = 0x2000
    }

    #endregion

    #region MemoryTypeFlags

    /// <summary>
    /// Memory-type options list.
    /// </summary>
    [Flags]
    public enum MemoryTypeFlags
    {
        /// <summary>
        /// This value is not officially present in the Microsoft's enumeration but can occur after testing.
        /// </summary>
        None = 0x0, 

        /// <summary>
        /// Indicates that the memory pages within the region are mapped into the view of an image section.
        /// </summary>
        Image = 0x1000000, 

        /// <summary>
        /// Indicates that the memory pages within the region are mapped into the view of a section.
        /// </summary>
        Mapped = 0x40000, 

        /// <summary>
        /// Indicates that the memory pages within the region are private (that is, not shared by other processes).
        /// </summary>
        Private = 0x20000
    }

    #endregion/// <summary>

    /// <summary>
    /// </summary>
    [Flags]
    public enum ProcessAccessFlags
    {
        /// <summary>
        /// All possible access rights for a process object.
        /// </summary>
        AllAccess = 0x001F0FFF, 

        /// <summary>
        /// Required to create a process.
        /// </summary>
        CreateProcess = 0x0080, 

        /// <summary>
        /// Required to create a thread.
        /// </summary>
        CreateThread = 0x0002, 

        /// <summary>
        /// Required to duplicate a handle using DuplicateHandle.
        /// </summary>
        DupHandle = 0x0040, 

        /// <summary>
        /// Required to retrieve certain information about a process, such as its token, exit code, and priority class (see OpenProcessToken).
        /// </summary>
        QueryInformation = 0x0400, 

        /// <summary>
        /// Required to retrieve certain information about a process (see GetExitCodeProcess, GetPriorityClass, IsProcessInJob, QueryFullProcessImageName). 
        /// A handle that has the PROCESS_QUERY_INFORMATION access right is automatically granted PROCESS_QUERY_LIMITED_INFORMATION.
        /// </summary>
        QueryLimitedInformation = 0x1000, 

        /// <summary>
        /// Required to set certain information about a process, such as its priority class (see SetPriorityClass).
        /// </summary>
        SetInformation = 0x0200, 

        /// <summary>
        /// Required to set memory limits using SetProcessWorkingSetSize.
        /// </summary>
        SetQuota = 0x0100, 

        /// <summary>
        /// Required to suspend or resume a process.
        /// </summary>
        SuspendResume = 0x0800, 

        /// <summary>
        /// Required to terminate a process using TerminateProcess.
        /// </summary>
        Terminate = 0x0001, 

        /// <summary>
        /// Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).
        /// </summary>
        VmOperation = 0x0008, 

        /// <summary>
        /// Required to read memory in a process using <see cref="NativeMethods.ReadProcessMemory"/>.
        /// </summary>
        VmRead = 0x0010, 

        /// <summary>
        /// Required to write to memory in a process using WriteProcessMemory.
        /// </summary>
        VmWrite = 0x0020, 

        /// <summary>
        /// Required to wait for the process to terminate using the wait functions.
        /// </summary>
        Synchronize = 0x00100000
    }

    #endregion

    #region MemoryProtectionFlags

    /// <summary>
    /// Memory-protection options list.
    /// </summary>
    [Flags]
    public enum MemoryProtectionFlags
    {
        /// <summary>
        /// Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed region results in an access violation.
        /// This value is not officially present in the Microsoft's enumeration but can occur according to the MEMORY_BASIC_INFORMATION structure documentation.
        /// </summary>
        ZeroAccess = 0x0, 

        /// <summary>
        /// Enables execute access to the committed region of pages. An attempt to read from or write to the committed region results in an access violation.
        /// This flag is not supported by the CreateFileMapping function.
        /// </summary>
        Execute = 0x10, 

        /// <summary>
        /// Enables execute or read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation.
        /// </summary>
        ExecuteRead = 0x20, 

        /// <summary>
        /// Enables execute, read-only, or read/write access to the committed region of pages.
        /// </summary>
        ExecuteReadWrite = 0x40, 

        /// <summary>
        /// Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object. 
        /// An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process. 
        /// The private page is marked as PAGE_EXECUTE_READWRITE, and the change is written to the new page.
        /// This flag is not supported by the VirtualAlloc or <see cref="NativeMethods.VirtualAllocEx"/> functions. 
        /// </summary>
        ExecuteWriteCopy = 0x80, 

        /// <summary>
        /// Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed region results in an access violation.
        /// This flag is not supported by the CreateFileMapping function.
        /// </summary>
        NoAccess = 0x01, 

        /// <summary>
        /// Enables read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation. 
        /// If Data Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.
        /// </summary>
        ReadOnly = 0x02, 

        /// <summary>
        /// Enables read-only or read/write access to the committed region of pages. 
        /// If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
        /// </summary>
        ReadWrite = 0x04, 

        /// <summary>
        /// Enables read-only or copy-on-write access to a mapped view of a file mapping object. 
        /// An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process. 
        /// The private page is marked as PAGE_READWRITE, and the change is written to the new page. 
        /// If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
        /// This flag is not supported by the VirtualAlloc or <see cref="NativeMethods.VirtualAllocEx"/> functions.
        /// </summary>
        WriteCopy = 0x08, 

        /// <summary>
        /// Pages in the region become guard pages. 
        /// Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION exception and turn off the guard page status. 
        /// Guard pages thus act as a one-time access alarm. For more information, see Creating Guard Pages.
        /// When an access attempt leads the system to turn off guard page status, the underlying page protection takes over.
        /// If a guard page exception occurs during a system service, the service typically returns a failure status indicator.
        /// This value cannot be used with PAGE_NOACCESS.
        /// This flag is not supported by the CreateFileMapping function.
        /// </summary>
        Guard = 0x100, 

        /// <summary>
        /// Sets all pages to be non-cachable. Applications should not use this attribute except when explicitly required for a device. 
        /// Using the interlocked functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
        /// The PAGE_NOCACHE flag cannot be used with the PAGE_GUARD, PAGE_NOACCESS, or PAGE_WRITECOMBINE flags.
        /// The PAGE_NOCACHE flag can be used only when allocating private memory with the VirtualAlloc, <see cref="NativeMethods.VirtualAllocEx"/>, or VirtualAllocExNuma functions. 
        /// To enable non-cached memory access for shared memory, specify the SEC_NOCACHE flag when calling the CreateFileMapping function.
        /// </summary>
        NoCache = 0x200, 

        /// <summary>
        /// Sets all pages to be write-combined.
        /// Applications should not use this attribute except when explicitly required for a device. 
        /// Using the interlocked functions with memory that is mapped as write-combined can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
        /// The PAGE_WRITECOMBINE flag cannot be specified with the PAGE_NOACCESS, PAGE_GUARD, and PAGE_NOCACHE flags.
        /// The PAGE_WRITECOMBINE flag can be used only when allocating private memory with the VirtualAlloc, <see cref="NativeMethods.VirtualAllocEx"/>, or VirtualAllocExNuma functions. 
        /// To enable write-combined memory access for shared memory, specify the SEC_WRITECOMBINE flag when calling the CreateFileMapping function.
        /// </summary>
        WriteCombine = 0x400
    }

    #endregion

    /// <summary>
    /// </summary>
    public static class Memory
    {
        /// <summary>
        /// Changes the protection on a region of committed pages in the virtual address space of a specified process.
        /// </summary>
        /// <param name="hProcess">
        /// A handle to the process whose memory protection is to be changed. The handle must have the PROCESS_VM_OPERATION access right. 
        /// For more information, see Process Security and Access Rights.
        /// </param>
        /// <param name="lpAddress">
        /// A pointer to the base address of the region of pages whose access protection attributes are to be changed. 
        /// All pages in the specified region must be within the same reserved region allocated when calling the VirtualAlloc or VirtualAllocEx function using MEM_RESERVE. 
        /// The pages cannot span adjacent reserved regions that were allocated by separate calls to VirtualAlloc or <see cref="VirtualAllocEx"/> using MEM_RESERVE.
        /// </param>
        /// <param name="dwSize">
        /// The size of the region whose access protection attributes are changed, in bytes. 
        /// The region of affected pages includes all pages containing one or more bytes in the range from the lpAddress parameter to (lpAddress+dwSize). 
        /// This means that a 2-byte range straddling a page boundary causes the protection attributes of both pages to be changed.
        /// </param>
        /// <param name="flNewProtect">
        /// The memory protection option. This parameter can be one of the memory protection constants. 
        /// For mapped views, this value must be compatible with the access protection specified when the view was mapped (see MapViewOfFile, MapViewOfFileEx, and MapViewOfFileExNuma).
        /// </param>
        /// <param name="lpflOldProtect">
        /// A pointer to a variable that receives the previous access protection of the first page in the specified region of pages. 
        /// If this parameter is NULL or does not point to a valid variable, the function fails.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero. 
        /// If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtectEx(
            SafeMemoryHandle hProcess, 
            IntPtr lpAddress, 
            int dwSize, 
            MemoryProtectionFlags flNewProtect, 
            out MemoryProtectionFlags lpflOldProtect);

        /// <summary>
        /// </summary>
        /// <param name="processHandle">
        /// </param>
        /// <param name="address">
        /// </param>
        /// <param name="size">
        /// </param>
        /// <param name="protection">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static MemoryProtectionFlags ChangeProtection(
            SafeMemoryHandle processHandle, 
            IntPtr address, 
            int size, 
            MemoryProtectionFlags protection)
        {
            // Create the variable storing the old protection of the memory page
            MemoryProtectionFlags oldProtection;

            // Change the protection in the target process
            if (VirtualProtectEx(processHandle, address, size, protection, out oldProtection))
            {
                // Return the old protection
                return oldProtection;
            }

            // Else the protection couldn't be changed, throws an exception
            throw new Win32Exception(
                string.Format(
                    "Couldn't change the protection of the memory at 0x{0} of {1} byte(s) to {2}.", 
                    address.ToString("X"), 
                    size, 
                    protection));
        }

        /// <summary>
        /// </summary>
        /// <param name="dwDesiredAccess">
        /// </param>
        /// <param name="bInheritHandle">
        /// </param>
        /// <param name="dwProcessId">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeMemoryHandle OpenProcess(
            ProcessAccessFlags dwDesiredAccess, 
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, 
            int dwProcessId);

        /// <summary>
        /// </summary>
        /// <param name="processHandle">
        /// </param>
        /// <param name="address">
        /// </param>
        /// <param name="byteArray">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Win32Exception">
        /// </exception>
        public static int WriteBytes(SafeMemoryHandle processHandle, IntPtr address, byte[] byteArray)
        {
            // Create the variable storing the number of bytes written
            int nbBytesWritten;

            // Write the data to the target process
            if (WriteProcessMemory(processHandle, address, byteArray, byteArray.Length, out nbBytesWritten))
            {
                // Check whether the length of the data written is equal to the inital array
                if (nbBytesWritten == byteArray.Length)
                {
                    return nbBytesWritten;
                }
            }

            // Else the data couldn't be written, throws an exception
            throw new Win32Exception(
                string.Format("Couldn't write {0} bytes to 0x{1}", byteArray.Length, address.ToString("X")));
        }

        #region WriteProcessMemory

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <param name="hProcess">
        /// A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.
        /// </param>
        /// <param name="lpBaseAddress">
        /// A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that 
        /// all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="lpBuffer">
        /// A pointer to the buffer that contains data to be written in the address space of the specified process.
        /// </param>
        /// <param name="nSize">
        /// The number of bytes to be written to the specified process.
        /// </param>
        /// <param name="lpNumberOfBytesWritten">
        /// A pointer to a variable that receives the number of bytes transferred into the specified process. 
        /// This parameter is optional. If lpNumberOfBytesWritten is NULL, the parameter is ignored.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero. 
        /// If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteProcessMemory(
            SafeMemoryHandle hProcess, 
            IntPtr lpBaseAddress, 
            byte[] lpBuffer, 
            int nSize, 
            out int lpNumberOfBytesWritten);

        #endregion

        #region OpenProcess

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="accessFlags">
        /// The access level to the process object.
        /// </param>
        /// <param name="processId">
        /// The identifier of the local process to be opened.
        /// </param>
        /// <returns>
        /// An open handle to the specified process.
        /// </returns>
        public static SafeMemoryHandle OpenProcess(ProcessAccessFlags accessFlags, int processId)
        {
            // Get an handle from the remote process
            SafeMemoryHandle handle = OpenProcess(accessFlags, false, processId);

            // Check whether the handle is valid
            if (!handle.IsInvalid && !handle.IsClosed)
            {
                return handle;
            }

            // Else the handle isn't valid, throws an exception
            throw new Win32Exception(string.Format("Couldn't open the process {0}.", processId));
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="hProcess">
        /// </param>
        /// <param name="lpAddress">
        /// </param>
        /// <param name="lpBuffer">
        /// </param>
        /// <param name="dwLength">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int VirtualQueryEx(
            IntPtr hProcess, 
            IntPtr lpAddress, 
            out MemoryBasicInformation lpBuffer, 
            int dwLength);

        #region MemoryBasicInformation

        /// <summary>
        /// Contains information about a range of pages in the virtual address space of a process. The VirtualQuery and <see cref="NativeMethods.VirtualQueryEx"/> functions use this structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MemoryBasicInformation
        {
            /// <summary>
            /// A pointer to the base address of the region of pages.
            /// </summary>
            public IntPtr BaseAddress;

            /// <summary>
            /// A pointer to the base address of a range of pages allocated by the VirtualAlloc function. The page pointed to by the BaseAddress member is contained within this allocation range.
            /// </summary>
            public IntPtr AllocationBase;

            /// <summary>
            /// The memory protection option when the region was initially allocated. This member can be one of the memory protection constants or 0 if the caller does not have access.
            /// </summary>
            public MemoryProtectionFlags AllocationProtect;

            /// <summary>
            /// The size of the region beginning at the base address in which all pages have identical attributes, in bytes.
            /// </summary>
            public readonly int RegionSize;

            /// <summary>
            /// The state of the pages in the region.
            /// </summary>
            public MemoryStateFlags State;

            /// <summary>
            /// The access protection of the pages in the region. This member is one of the values listed for the AllocationProtect member.
            /// </summary>
            public MemoryProtectionFlags Protect;

            /// <summary>
            /// The type of pages in the region.
            /// </summary>
            public MemoryTypeFlags Type;
        }

        #endregion

        /// <summary>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            /// <summary>
            /// </summary>
            public uint dwOemId;

            /// <summary>
            /// </summary>
            public uint dwPageSize;

            /// <summary>
            /// </summary>
            public IntPtr lpMinimumApplicationAddress;

            /// <summary>
            /// </summary>
            public IntPtr lpMaximumApplicationAddress;

            /// <summary>
            /// </summary>
            public uint dwActiveProcessorMask;

            /// <summary>
            /// </summary>
            public uint dwNumberOfProcessors;

            /// <summary>
            /// </summary>
            public uint dwProcessorType;

            /// <summary>
            /// </summary>
            public uint dwAllocationGranularity;

            /// <summary>
            /// </summary>
            public uint dwProcessorLevel;

            /// <summary>
            /// </summary>
            public uint dwProcessorRevision;
        }

        /// <summary>
        /// </summary>
        /// <param name="lpSystemInfo">
        /// </param>
        [DllImport("kernel32.dll")]
        private static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        /// <summary>
        /// </summary>
        /// <param name="processHandle">
        /// </param>
        /// <param name="addressFrom">
        /// </param>
        /// <param name="addressTo">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static IEnumerable<MemoryBasicInformation> Query(
            IntPtr processHandle, 
            IntPtr addressFrom, 
            IntPtr addressTo)
        {
            // Convert the addresses to Int64
            long numberFrom = addressFrom.ToInt64();
            long numberTo = addressTo.ToInt64();

            // The first address must be lower than the second
            if (numberFrom >= numberTo)
            {
                throw new ArgumentException(
                    "The starting address must be lower than the ending address.", 
                    "addressFrom");
            }

            // Create the variable storing the result of the call of VirtualQueryEx
            int ret;

            // Enumerate the memory pages
            do
            {
                // Allocate the structure to store information of memory
                MemoryBasicInformation memoryInfo;

                // Get the next memory page
                ret = VirtualQueryEx(processHandle, new IntPtr(numberFrom), out memoryInfo, 28);

                // Increment the starting address with the size of the page
                numberFrom += memoryInfo.RegionSize;

                // Return the memory page
                if (memoryInfo.State != MemoryStateFlags.Free)
                {
                    yield return memoryInfo;
                }
            }
            while (numberFrom < numberTo && ret != 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="handle">
        /// </param>
        /// <param name="memHandle">
        /// </param>
        /// <param name="search">
        /// </param>
        /// <returns>
        /// </returns>
        internal static IntPtr Find(IntPtr handle, SafeMemoryHandle memHandle, string search)
        {
            SYSTEM_INFO pSI = new SYSTEM_INFO();
            GetSystemInfo(ref pSI);
            List<MemoryBasicInformation> memPages = new List<MemoryBasicInformation>();
            memPages.AddRange(
                Query(handle, (IntPtr)pSI.lpMinimumApplicationAddress, (IntPtr)pSI.lpMaximumApplicationAddress));
            int len = ASCIIEncoding.ASCII.GetByteCount(search);
            foreach (MemoryBasicInformation mbi in memPages)
            {
                if (mbi.RegionSize >= len)
                {
                    if (((mbi.State & MemoryStateFlags.Commit) == MemoryStateFlags.Commit)
                        && ((mbi.Type & MemoryTypeFlags.Image) == MemoryTypeFlags.Image))
                    {
                        IntPtr res = FindKey(memHandle, search, mbi);
                        if ((uint)res > 0)
                        {
                            return res;
                        }
                    }
                }
            }

            return (IntPtr)0;
        }

        /// <summary>
        /// </summary>
        /// <param name="hProcess">
        /// </param>
        /// <param name="lpBaseAddress">
        /// </param>
        /// <param name="lpBuffer">
        /// </param>
        /// <param name="dwSize">
        /// </param>
        /// <param name="lpNumberOfBytesRead">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadProcessMemory(
            SafeMemoryHandle hProcess, 
            IntPtr lpBaseAddress, 
            [Out] byte[] lpBuffer, 
            int dwSize, 
            out int lpNumberOfBytesRead);

        #region ReadBytes

        /// <summary>
        /// Reads an array of bytes in the memory form the target process.
        /// </summary>
        /// <param name="processHandle">
        /// A handle to the process with memory that is being read.
        /// </param>
        /// <param name="address">
        /// A pointer to the base address in the specified process from which to read.
        /// </param>
        /// <param name="size">
        /// The number of bytes to be read from the specified process.
        /// </param>
        /// <returns>
        /// The collection of read bytes.
        /// </returns>
        public static byte[] ReadBytes(SafeMemoryHandle processHandle, IntPtr address, int size)
        {
            // Allocate the buffer
            var buffer = new byte[size];
            int nbBytesRead;

            // Read the data from the target process
            if (ReadProcessMemory(processHandle, address, buffer, size, out nbBytesRead) && size == nbBytesRead)
            {
                return buffer;
            }

            // Else the data couldn't be read, throws an exception
            throw new Win32Exception(
                string.Format("Couldn't read {0} byte(s) from 0x{1}.", size, address.ToString("X")));
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="handle">
        /// </param>
        /// <param name="search">
        /// </param>
        /// <param name="mbi">
        /// </param>
        /// <returns>
        /// </returns>
        private static IntPtr FindKey(SafeMemoryHandle handle, string search, MemoryBasicInformation mbi)
        {
            byte[] memCopy = ReadBytes(handle, mbi.BaseAddress, mbi.RegionSize);
            int len = ASCIIEncoding.ASCII.GetByteCount(search);
            byte[] src = ASCIIEncoding.ASCII.GetBytes(search);

            int iPointer = 0;
            while (iPointer < memCopy.Length - len)
            {
                int dPointer = 0;
                while (dPointer < len)
                {
                    if (src[dPointer] != memCopy[iPointer + dPointer])
                    {
                        break;
                    }

                    dPointer++;
                    if (dPointer == len)
                    {
                        return (IntPtr)((int)mbi.BaseAddress + iPointer);
                    }
                }

                iPointer++;
            }

            return (IntPtr)0;
        }
    }

    /// <summary>
    /// Represents a Win32 handle safely managed.
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public sealed class SafeMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// </summary>
        /// <param name="hObject">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// Parameterless constructor for handles built by the system (like <see cref="NativeMethods.OpenProcess"/>).
        /// </summary>
        public SafeMemoryHandle()
            : base(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeMemoryHandle"/> class, specifying the handle to keep in safe.
        /// </summary>
        /// <param name="handle">
        /// The handle to keep in safe.
        /// </param>
        public SafeMemoryHandle(IntPtr handle)
            : base(true)
        {
            this.SetHandle(handle);
        }

        /// <summary>
        /// Executes the code required to free the handle.
        /// </summary>
        /// <returns>True if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. In this case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.</returns>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            // Check whether the handle is set AND whether the handle has been successfully closed
            return this.handle != IntPtr.Zero && CloseHandle(this.handle);
        }
    }
}