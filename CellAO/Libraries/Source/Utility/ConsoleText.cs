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
// Last modified: 2013-10-30 19:54
// Created:       2013-10-30 19:48

#endregion

namespace Utility
{
    #region Usings ...

    using System;
    using System.IO;

    using Utility.Config;

    #endregion

    /// <summary>
    /// Test? this is for Console Text Translation...
    /// </summary>
    public class ConsoleText
    {
        /// <summary>
        /// </summary>
        private readonly string Locale = ConfigReadWrite.Instance.CurrentConfig.Locale;

        /// <summary>
        /// Please do not include language_  in the file name example:  main.txt"
        /// </summary>
        /// <param name="fileName">
        /// </param>
        public void TextRead(string fileName)
        {
            try
            {
                StreamReader reader =
                    File.OpenText(
                        Path.Combine(
                            Path.Combine("locale", this.Locale.ToLower()), this.Locale.ToLower() + "_" + fileName));
                string console_input = null;
                while ((console_input = reader.ReadLine()) != null)
                {
                    Console.WriteLine(console_input);
                }

                reader.Close();
            }
            catch (FileNotFoundException fnfex)
            {
                Console.WriteLine(fnfex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}