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

namespace ZoneEngine.Core.MessageHandlers
{
    #region Usings ...

    using CellAO.Core.Components;
    using CellAO.Core.Network;

    using SmokeLounge.AOtomation.Messaging.Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.InternalMessages;

    #endregion

    /// <summary>
    /// </summary>
    public class FollowTargetMessageHandler : BaseMessageHandler<FollowTargetMessage, FollowTargetMessageHandler>
    {
        /// <summary>
        /// </summary>
        public FollowTargetMessageHandler()
        {
            // Inbound only? We might need it for Mobs/Pets following - Algorithman
            this.Direction = MessageHandlerDirection.InboundOnly;
            this.UpdateCharacterStatsOnReceive = true;
        }

        #region Inbound

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="message">
        /// </param>
        /// <param name="updateCharacterStats">
        /// </param>
        protected override void Read(FollowTargetMessage followTargetMessage, IZoneClient client)
        {
            // REFACT can we use the base class methods here ??

            //var followTargetMessage = (FollowTargetMessage)message.Body;

            var announce = new FollowTargetMessage
                           {
                               Identity = client.Character.Identity, 
                               Unknown = 0, 
                               Unknown1 = followTargetMessage.Unknown1, 
                               Unknown2 = followTargetMessage.Unknown2, 
                               Target =     followTargetMessage.Target, 
                               Unknown3 = followTargetMessage.Unknown3, 
                               Unknown4 = followTargetMessage.Unknown4, 
                               Unknown5 = followTargetMessage.Unknown5, 
                               Unknown6 = followTargetMessage.Unknown6, 
                               Unknown7 = followTargetMessage.Unknown7
                           };

            client.Character.Playfield.Publish(new IMSendAOtomationMessageToPlayfield { Body = announce });

        }

        #endregion
    }
}