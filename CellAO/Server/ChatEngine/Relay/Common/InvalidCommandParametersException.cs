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
// Last modified: 2013-11-16 19:07

#endregion

namespace ChatEngine.Relay.Common
{
    #region Usings ...

    using System;
    using System.Diagnostics;

    using ChatEngine.Properties;

    #endregion

    /// <summary>
    /// </summary>
    public class InvalidCommandParametersException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="minParameters">
        /// </param>
        /// <param name="maxParameters">
        /// </param>
        public InvalidCommandParametersException(int minParameters, int? maxParameters = null)
            : base()
        {
            Debug.Assert(minParameters >= 0, "minParameters must be at least zero.");
            Debug.Assert(
                maxParameters == null || maxParameters >= minParameters, 
                "maxParameters must be at least minParameters.");

            this.MinParameters = minParameters;
            this.MaxParameters = maxParameters ?? minParameters;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public int MaxParameters { get; private set; }

        /// <summary>
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public override string Message
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// </summary>
        public int MinParameters { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="command">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetMessage(string command)
        {
            if (this.MinParameters == 0 && this.MaxParameters == 0)
            {
                return string.Format(Resources.MessageCommandTakesNoParams, command);
            }
            else if (this.MinParameters == this.MaxParameters)
            {
                return string.Format(Resources.MessageCommandTakesXParams, command, this.MinParameters);
            }
            else
            {
                return string.Format(
                    Resources.MessageCommandTakesXToYParams, 
                    command, 
                    this.MinParameters, 
                    this.MaxParameters);
            }
        }

        #endregion
    }
}