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
// Last modified: 2013-11-16 09:35

#endregion

namespace ChatEngine.Channels
{
    #region Usings ...

    using Cell.Core;

    using ChatEngine.CoreClient;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public class RestrictedChannel : ChannelBase
    {
        #region Fields

        /// <summary>
        /// </summary>
        public Side characterSide;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="side">
        /// </param>
        /// <param name="flags">
        /// </param>
        /// <param name="type">
        /// </param>
        public RestrictedChannel(Side side, ChannelFlags flags, ChannelType type)
            : base(flags, type, (uint)side)
        {
            switch (side)
            {
                case Side.Advisor:
                    this.ChannelName = "Advisor";
                    this.OnCheckRestriction += this.CheckAdvisorStatus;

                    break;
                case Side.Clan:
                    this.ChannelName = "Clan";
                    this.OnCheckRestriction += this.CheckClanStatus;
                    break;
                case Side.Gm:
                    this.ChannelName = "Suiv's Playground";
                    this.OnCheckRestriction += this.CheckGmStatus;
                    break;
                case Side.Guardian:
                    this.ChannelName = "Guardians";
                    this.OnCheckRestriction += this.CheckGuardianStatus;
                    break;
                case Side.Mixed:
                    this.ChannelName = "Mixed";
                    this.OnCheckRestriction += this.CheckMixedRestriction;
                    break;
                case Side.Monster:
                    this.ChannelName = "Monstaaaar";
                    this.OnCheckRestriction += this.CheckMonsterRestriction;
                    break;
                case Side.Neutral:
                    this.ChannelName = "Neutral";
                    this.OnCheckRestriction += this.CheckNeutralRestriction;
                    break;
                case Side.Omni:
                    this.ChannelName = "Omni-Tek";
                    this.OnCheckRestriction += this.CheckOmniRestriction;
                    break;
            }
        }

        #endregion

        #region Delegates

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        private delegate bool ChannelRestriction(Client client);

        #endregion

        #region Events

        /// <summary>
        /// </summary>
        private event ChannelRestriction OnCheckRestriction;

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
            Client cclient = (Client)client;
            if (this.OnCheckRestriction(cclient))
            {
                return base.AddClient(client);
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        private bool CheckAdvisorStatus(Client client)
        {
            return client.Character.CharacterSide == (int)Side.Advisor;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        private bool CheckClanStatus(Client client)
        {
            return client.Character.CharacterSide == (int)Side.Clan;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        private bool CheckGmStatus(Client client)
        {
            return client.Character.CharacterGMLevel > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        private bool CheckGuardianStatus(Client client)
        {
            return client.Character.CharacterSide == (int)Side.Guardian;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        private bool CheckMixedRestriction(Client client)
        {
            return client.Character.CharacterSide == (int)Side.Mixed;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        private bool CheckMonsterRestriction(Client client)
        {
            return client.Character.CharacterSide == (int)Side.Monster;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        private bool CheckNeutralRestriction(Client client)
        {
            return client.Character.CharacterSide == (int)Side.Neutral;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        private bool CheckOmniRestriction(Client client)
        {
            return client.Character.CharacterSide == (int)Side.Omni;
        }

        #endregion
    }
}