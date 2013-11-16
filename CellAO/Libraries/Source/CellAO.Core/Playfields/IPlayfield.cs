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

#endregion

namespace CellAO.Core.Playfields
{
    #region Usings ...

    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Core.Functions;
    using CellAO.Enums;

    using MemBus;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages;

    #endregion

    /// <summary>
    /// </summary>
    public interface IPlayfield
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        List<PlayfieldDistrict> Districts { get; }

        /// <summary>
        /// </summary>
        HashSet<IInstancedEntity> Entities { get; }

        /// <summary>
        /// </summary>
        List<Functions> EnvironmentFunctions { get; }

        /// <summary>
        /// </summary>
        Expansions Expansion { get; set; }

        /// <summary>
        /// </summary>
        Identity Identity { get; set; }

        /// <summary>
        /// </summary>
        IBus PlayfieldBus { get; set; }

        /// <summary>
        /// </summary>
        float X { get; set; }

        /// <summary>
        /// </summary>
        float XScale { get; set; }

        /// <summary>
        /// </summary>
        float Z { get; set; }

        /// <summary>
        /// </summary>
        float ZScale { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        void Announce(Message message);

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        void Announce(MessageBody messageBody);

        /// <summary>
        /// </summary>
        /// <param name="messageBody">
        /// </param>
        /// <param name="dontSend">
        /// </param>
        void AnnounceOthers(MessageBody messageBody, Identity dontSend);

        /// <summary>
        /// </summary>
        void DisconnectAllClients();

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        IInstancedEntity FindByIdentity(Identity identity);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        bool IsInstancedPlayfield();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        int NumberOfDynels();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        int NumberOfPlayers();

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        void Publish(object obj);

        #endregion

        // <summary>
        // </summary>
        // <param name="sendSCFUs">
        // </param>
        // TODO: Reactivate
        // void SendSCFUsToClient(IMSendPlayerSCFUs sendSCFUs);
    }
}