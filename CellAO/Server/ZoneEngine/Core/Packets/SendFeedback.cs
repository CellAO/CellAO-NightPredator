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

namespace ZoneEngine.Core.Packets
{
    #region Usings ...

    using CellAO.Core.Network;

    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    /// <summary>
    /// </summary>
    public static class SendFeedback
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="MsgCategory">
        /// </param>
        /// <param name="MsgNum">
        /// </param>
        /// <returns>
        /// </returns>
        public static FeedbackMessage Create(IZoneClient client, int MsgCategory, int MsgNum)
        {
            return new FeedbackMessage
                   {
                       Identity = client.Character.Identity, 
                       Unknown = 0x01, 
                       Unknown1 = 0x00000000, 
                       CategoryId = MsgCategory, 
                       MessageId = MsgNum
                   };
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="MsgCategory">
        /// </param>
        /// <param name="MsgNum">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool Send(IZoneClient client, int MsgCategory, int MsgNum)
        {
            client.Character.Send(Create(client, MsgCategory, MsgNum));
            return true;
        }

        #endregion
    }
}