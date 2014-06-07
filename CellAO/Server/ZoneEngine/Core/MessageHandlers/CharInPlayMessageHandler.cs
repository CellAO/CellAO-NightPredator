#region License

// Copyright (c) 2005-2014, CellAO Team
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

#endregion

namespace ZoneEngine.Core.MessageHandlers
{
    #region Usings ...

    using System.Threading;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Network;
    using CellAO.Enums;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    [MessageHandler(MessageHandlerDirection.InboundOnly)]
    public class CharInPlayMessageHandler : BaseMessageHandler<CharInPlayMessage, CharInPlayMessageHandler>
    {
        /// <summary>
        /// </summary>
        public CharInPlayMessageHandler()
        {
            this.UpdateCharacterStatsOnReceive = true;
        }

        #region Inbound

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="client">
        /// </param>
        protected override void Read(CharInPlayMessage message, IZoneClient client)
        {
            LogUtil.Debug(DebugInfoDetail.NetworkMessages, "Client connected...");
            client.Controller.Character.DoNotDoTimers = true;
            Thread.Sleep(1000);
            // client got all the needed data and
            // wants to enter the world. After we
            // reply to this, the character will really be in game
            var announce = new CharInPlayMessage { Identity = client.Controller.Character.Identity, Unknown = 0x00 };
            client.Controller.Character.Playfield.Announce(announce);

            // Player is in game now, starting is over, set stats normally now
            client.Controller.Character.Starting = false;

            client.Controller.Character.Stats.ClearChangedFlags();

            // Needed fix, so gmlevel will be loaded
            client.Controller.Character.Stats[StatIds.gmlevel].Value =
                client.Controller.Character.Stats[StatIds.gmlevel].Value;
            client.Controller.Character.Stats[StatIds.expansion].Value =
                client.Controller.Character.Stats[StatIds.expansion].Value;
            client.Controller.Character.Stats[StatIds.healinterval].Value = 0;
            client.Controller.Character.Stats[StatIds.healdelta].Value = 0;
            client.Controller.Character.Stats[StatIds.nanointerval].Value = 0;
            client.Controller.Character.Stats[StatIds.nanodelta].Value = 0;

            // Extra to calculate IP
            client.Controller.Character.Stats[StatIds.ip].Value = 0;
                

            client.Controller.SendChangedStats();

            // Mobs get sent whenever player enters playfield, BUT (!) they are NOT synchronized, because the mobs don't save stuff yet.
            // for instance: the waypoints the mob went through will NOT be saved and therefore when you re-enter the PF, it will AGAIN
            // walk the same waypoints.
            // TODO: Fix it
            /*foreach (MobType mob in NPCPool.Mobs)
            {
                // TODO: Make cache - use pf indexing somehow.
                if (mob.pf == client.Character.pf)
                {
                    mob.SendToClient(client);
                }
            }*/

            foreach (WeatherEntry w in WeatherSettings.Instance.WeatherList)
            {
                WeatherControlMessageHandler.Default.Send(client.Controller.Character, w);
            }

            var list = Pool.Instance.GetAll<StaticDynel>(client.Controller.Character.Playfield.Identity);
            foreach (StaticDynel sd in list)
            {
                SimpleItemFullUpdateMessageHandler.Default.Send(client.Controller.Character, sd);
            }
            client.Controller.Character.DoNotDoTimers = false;
        }

        #endregion
    }
}