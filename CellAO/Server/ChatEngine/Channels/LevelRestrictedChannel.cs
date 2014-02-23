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

namespace ChatEngine.Channels
{
    #region Usings ...

    using Cell.Core;

    using CellAO.Database.Dao;
    using CellAO.Stats;

    using ChatEngine.CoreClient;

    #endregion

    /// <summary>
    /// </summary>
    public class LevelRestrictedChannel : ChannelBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        public int MaxLevel;

        /// <summary>
        /// </summary>
        public int MinLevel;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="channelId">
        /// </param>
        /// <param name="minLevel">
        /// </param>
        /// <param name="maxLevel">
        /// </param>
        public LevelRestrictedChannel(uint channelId, int minLevel, int maxLevel)
            : base(ChannelFlags.None, ChannelType.Shopping, channelId, "Shopping " + minLevel + "-" + maxLevel)
        {
            this.MinLevel = minLevel;
            this.MaxLevel = maxLevel;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        public override bool AddClient(IClient client)
        {
            Client cl = (Client)client;

            int charLevel =
                StatDao.Instance.GetById(50000, (int)cl.Character.CharacterId, StatNamesDefaults.GetStatNumber("Level"))
                    .StatValue;

            if ((charLevel >= this.MinLevel) && (charLevel <= this.MaxLevel))
            {
                return base.AddClient(client);
            }

            return false;
        }

        #endregion
    }
}