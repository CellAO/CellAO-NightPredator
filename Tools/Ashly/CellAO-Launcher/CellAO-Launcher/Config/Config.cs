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


namespace CellAO_Launcher.Config
{
    using System.Net;

    #region enum

    /// <summary>
    /// This is the Enums for the Config File..
    /// </summary>
    public enum Information
    {
        ServerIP,

        ServerPort,

        AOExecutable,

        UseEncryption,

        Debug

    }

    #endregion

    #region Config

    /// <summary>
    /// This here Sets up the Gets and Sets for the Config System.
    /// </summary>
    public class Config
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets The Server IP
        /// </summary>
        public string ServerIP { get; set; }


        public int ServerPort { get; set; }

        /// <summary>
        /// Gets or Sets the directory of the AO Exe.
        /// </summary>
        public string AOExecutable { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether to enable Encryption
        /// </summary>
        public bool UseEncryption { get; set; }

        /// <summary>
        /// Gets or sets a value indicated weather launcher is in debug mode or not.
        /// </summary>
        public bool Debug { get; set; }

        #endregion
    }

    #endregion
}
