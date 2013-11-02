#region License

// Copyright (c) 2005-2013, CellAO Team
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
// Last modified: 2013-11-02 17:00

#endregion

namespace Utility
{
    #region Usings ...

    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security;

    #endregion

    /// <summary>
    /// </summary>
    public static class AssemblyInfoclass
    {
        #region Public Properties

        /// <summary>
        /// Gets the Version String
        /// </summary>
        public static string AssemblyVersion
        {
            get
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                return assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets the Company Attribute
        /// </summary>
        public static string Company
        {
            get
            {
                string result = string.Empty;
                Assembly assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        result = ((AssemblyCompanyAttribute)customAttributes[0]).Company;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the Copyright attribute
        /// </summary>
        public static string Copyright
        {
            get
            {
                string result = string.Empty;
                Assembly assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        result = ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the File Description Attribute
        /// </summary>
        public static string Description
        {
            get
            {
                string result = string.Empty;
                Assembly assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    object[] customAttributes = assembly.GetCustomAttributes(
                        typeof(AssemblyDescriptionAttribute), 
                        false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        result = ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the Filename of the assembly
        /// </summary>
        public static string FileName
        {
            [SecurityCritical]
            get
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                return fvi.OriginalFilename;
            }
        }

        /// <summary>
        /// Gets the path of the assembly
        /// </summary>
        public static string FilePath
        {
            [SecurityCritical]
            get
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                return fvi.FileName;
            }
        }

        /// <summary>
        /// Gets the FileVersion attribute
        /// </summary>
        public static string FileVersion
        {
            [SecurityCritical]
            get
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                return fvi.FileVersion;
            }
        }

        /// <summary>
        /// Gets the GUID
        /// </summary>
        public static string Guid
        {
            get
            {
                string result = string.Empty;
                Assembly assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    object[] customAttributes = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        result = ((GuidAttribute)customAttributes[0]).Value;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the Product Attribute
        /// </summary>
        public static string Product
        {
            get
            {
                string result = string.Empty;
                Assembly assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        result = ((AssemblyProductAttribute)customAttributes[0]).Product;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the revision name
        /// </summary>
        public static string RevisionName
        {
            get
            {
                string result = string.Empty;
                Assembly assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    object[] customAttributes = assembly.GetCustomAttributes(typeof(RevisionNameAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        result = ((RevisionNameAttribute)customAttributes[0]).RevisionName;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the Title of the assembly
        /// </summary>
        public static string Title
        {
            get
            {
                string result = string.Empty;
                Assembly assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        result = ((AssemblyTitleAttribute)customAttributes[0]).Title;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the Trademark Attribute
        /// </summary>
        public static string Trademark
        {
            get
            {
                string result = string.Empty;
                Assembly assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false);
                    if ((customAttributes != null) && (customAttributes.Length > 0))
                    {
                        result = ((AssemblyTrademarkAttribute)customAttributes[0]).Trademark;
                    }
                }

                return result;
            }
        }

        #endregion
    }
}