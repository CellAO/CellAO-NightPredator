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
// Last modified: 2013-11-03 10:58

#endregion

namespace ChatEngine.CoreClient
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using Cell.Core;

    using ChatEngine.CoreServer;

    using NiceHexOutput;

    using Utility;

    #endregion

    /// <summary>
    /// The client.
    /// </summary>
    public class Client : ClientBase
    {
        #region Fields

        /// <summary>
        /// Private known clients collection
        /// </summary>
        private readonly List<uint> knownClients;

        /// <summary>
        /// </summary>
        private ushort packetNumber = 1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class. 
        /// The client.
        /// </summary>
        /// <param name="srvr">
        /// </param>
        public Client(ChatServer srvr)
            : base(srvr)
        {
            this.Character = new Character(0, null);
            this.ServerSalt = string.Empty;
            this.knownClients = new List<uint>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class. 
        /// The client.
        /// </summary>
        public Client()
            : base(null)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The character.
        /// </summary>
        public Character Character { get; set; }

        /// <summary>
        /// The known clients.
        /// </summary>
        public List<uint> KnownClients
        {
            get
            {
                return this.knownClients;
            }
        }

        /// <summary>
        /// The server salt.
        /// </summary>
        public string ServerSalt { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public ChatServer ChatServer()
        {
            return this.Server as ChatServer;
        }

        /// <summary>
        /// </summary>
        /// <param name="receiver">
        /// </param>
        /// <param name="packetBytes">
        /// </param>
        public void Send(int receiver, byte[] packetBytes)
        {
            packetBytes[0] = BitConverter.GetBytes(this.packetNumber)[0];
            packetBytes[1] = BitConverter.GetBytes(this.packetNumber)[1];
            this.packetNumber++;

#if DEBUG
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(NiceHexOutput.Output(packetBytes));
            Console.ResetColor();
            LogUtil.Debug("Sent:\r\n" + NiceHexOutput.Output(packetBytes));
#endif
            if (packetBytes.Length % 4 > 0)
            {
                Array.Resize(ref packetBytes, packetBytes.Length + (4 - (packetBytes.Length % 4)));
            }

            this.Send(packetBytes);
        }

        #endregion

        // NV: Should this be here or inside Character...

        #region Methods

        /// <summary>
        /// The get message number.
        /// </summary>
        /// <param name="packet">
        /// </param>
        /// <returns>
        /// The get message number.
        /// </returns>
        protected ushort GetMessageNumber(byte[] packet)
        {
            ushort reply = BitConverter.ToUInt16(new[] { packet[1], packet[0] }, 0);
            return reply;
        }

        /// <summary>
        /// </summary>
        /// <param name="buffer">
        /// </param>
        /// <returns>
        /// </returns>
        protected override bool OnReceive(BufferSegment buffer)
        {
            return true;
        }

        #endregion
    }
}