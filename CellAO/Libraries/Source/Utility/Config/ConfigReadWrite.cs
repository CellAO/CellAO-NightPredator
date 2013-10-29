#region License

// Copyright (c) 2005-2013, CellAO Team
// 
// 
// All rights reserved.
// 
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
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
// 
// Last modified: 2013-10-29 22:26
// Created:       2013-10-29 19:57

#endregion

namespace Utility.Config
{
    #region Usings ...

    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class ConfigReadWrite
    {
        /// <summary>
        /// </summary>
        private Config _config;

        /// <summary>
        /// </summary>
        private static ConfigReadWrite _instance;

        /// <summary>
        /// </summary>
        private ConfigReadWrite()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static ConfigReadWrite Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConfigReadWrite();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Config CurrentConfig
        {
            get
            {
                try
                {
                    if (this._config == null)
                    {
                        this._config =
                            (Config)
                            new XmlSerializer(typeof(Config)).Deserialize(
                                new MemoryStream(File.ReadAllBytes("Config.xml")));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing configuration: {0}", ex.Message);
                    this._config = new Config();
                }

                return this._config;
            }
        }

        /// <summary>
        /// Saves the current config back to the file
        /// </summary>
        /// <returns>true, if successful</returns>
        public bool SaveConfig()
        {
            if (this._config == null)
            {
                return false;
            }

            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(Config));
                MemoryStream ms = new MemoryStream();
                ser.Serialize(ms, this._config);
                File.WriteAllText("config.xml", Encoding.UTF8.GetString(ms.GetBuffer()));
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}