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

namespace ChatEngine.PacketHandlers
{
    #region Usings ...

    using CellAO.Database.Dao;
    using CellAO.Database.Entities;

    using ChatEngine.CoreClient;
    using ChatEngine.Packets;

    using Utility.Config;

    #endregion

    /// <summary>
    /// The player name lookup.
    /// </summary>
    public class PlayerNameLookup
    {
        #region Public Methods and Operators

        /// <summary>
        /// Read and send back Player name lookup packet
        /// </summary>
        /// <param name="client">
        /// Client sending
        /// </param>
        /// <param name="packet">
        /// Packet data
        /// </param>
        public static void Read(Client client, byte[] packet)
        {
            PacketReader reader = new PacketReader(ref packet);

            reader.ReadUInt16(); // packet ID
            reader.ReadUInt16(); // data length
            uint playerId = uint.MaxValue;
            string playerName = reader.ReadString();
            if (playerName == string.Empty)
            {
                return;
            }

            if (playerName == ConfigReadWrite.Instance.CurrentConfig.RelayBotNick)
            {
                byte[] botlookup = NameLookupResult.Create(
                    0x80000000, 
                    ConfigReadWrite.Instance.CurrentConfig.RelayBotNick);
                client.Send(botlookup);
                client.Send(BuddyOnlineStatus.Create(0x80000000, 1, new byte[] { 0x00, 0x01, 0x00 }));
                return;
            }

            client.Server.Debug(
                client, 
                "{0} >> PlayerNameLookup: PlayerName: {1}", 
                client.Character.characterName, 
                playerName);
            reader.Finish();

            DBCharacter character = CharacterDao.GetByCharName(playerName);
            if (character != null)
            {
                playerId = (uint)character.Id;
            }

            byte[] namelookup = NameLookupResult.Create(playerId, playerName);
            client.Send(namelookup);
            client.Send(
                BuddyOnlineStatus.Create(
                    playerId, 
                    (uint)OnlineDao.IsOnline((int)playerId).Online, 
                    new byte[] { 0x00, 0x01, 0x00 }));
            client.KnownClients.Add(playerId);
        }

        #endregion
    }
}