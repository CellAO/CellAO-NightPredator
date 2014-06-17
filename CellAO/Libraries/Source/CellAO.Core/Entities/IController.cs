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

namespace CellAO.Core.Entities
{
    #region Usings ...

    using System;

    using CellAO.Core.Functions;
    using CellAO.Core.Network;
    using CellAO.Core.Vector;
    using CellAO.Enums;
    using CellAO.Interfaces;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Quaternion = CellAO.Core.Vector.Quaternion;
    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    public interface IController : IDisposable
    {
        CharacterState State { get; set; }

        /// <summary>
        /// ICharacter object connected to this Controller
        /// </summary>
        ICharacter Character { get; set; }

        IZoneClient Client { get; set; }

        bool SaveToDatabase { get; }

        #region Generic character actions

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        bool LookAt(Identity target);

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        bool UseStatel(Identity identity, EventType eventType = EventType.OnUse);

        /// <summary>
        /// </summary>
        /// <param name="text">
        /// </param>
        void SendChatText(string text);

        /// <summary>
        /// </summary>
        /// <param name="nanoId">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        bool CastNano(int nanoId, Identity target);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        bool Search();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        bool Sneak();

        /// <summary>
        /// </summary>
        /// <param name="visualFlag">
        /// </param>
        /// <returns>
        /// </returns>
        bool ChangeVisualFlag(int visualFlag);

        /// <summary>
        /// </summary>
        /// <param name="moveType">
        /// </param>
        /// <param name="newCoordinates">
        /// </param>
        /// <param name="heading">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        bool Move(int moveType, Coordinate newCoordinates, Quaternion heading);

        /// <summary>
        /// </summary>
        /// <param name="sourceContainerType">
        /// </param>
        /// <param name="sourcePlacement">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="targetPlacement">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        bool ContainerAddItem(int sourceContainerType, int sourcePlacement, Identity target, int targetPlacement);

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        bool Follow(Identity target);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        bool Stand();

        /// <summary>
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <param name="parameter1">
        /// </param>
        /// <param name="parameter2">
        /// </param>
        /// <param name="parameter3">
        /// </param>
        /// <param name="parameter4">
        /// </param>
        /// <param name="parameter5">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        bool SocialAction(
            SocialAction action,
            byte parameter1,
            byte parameter2,
            byte parameter3,
            byte parameter4,
            int parameter5);

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        bool Trade(Identity target);

        #endregion

        #region Player specific actions

        /// <summary>
        /// </summary>
        /// <param name="itemPosition">
        /// </param>
        /// <returns>
        /// </returns>
        bool UseItem(Identity itemPosition);

        /// <summary>
        /// </summary>
        /// <param name="container">
        /// </param>
        /// <param name="slotNumber">
        /// </param>
        /// <returns>
        /// </returns>
        bool DeleteItem(int container, int slotNumber);

        /// <summary>
        /// </summary>
        /// <param name="targetItem">
        /// </param>
        /// <param name="stackCount">
        /// </param>
        /// <returns>
        /// </returns>
        bool SplitItemStack(Identity targetItem, int stackCount);

        /// <summary>
        /// </summary>
        /// <param name="sourceItem">
        /// </param>
        /// <param name="targetItem">
        /// </param>
        /// <returns>
        /// </returns>
        bool JoinItemStack(Identity sourceItem, Identity targetItem);

        /// <summary>
        /// </summary>
        /// <param name="sourceItem">
        /// </param>
        /// <param name="targetItem">
        /// </param>
        /// <returns>
        /// </returns>
        bool CombineItems(Identity sourceItem, Identity targetItem);

        /// <summary>
        /// </summary>
        /// <param name="inventoryPageId">
        /// </param>
        /// <param name="slotNumber">
        /// </param>
        /// <returns>
        /// </returns>
        bool TradeSkillSourceChanged(int inventoryPageId, int slotNumber);

        /// <summary>
        /// </summary>
        /// <param name="inventoryPageId">
        /// </param>
        /// <param name="slotNumber">
        /// </param>
        /// <returns>
        /// </returns>
        bool TradeSkillTargetChanged(int inventoryPageId, int slotNumber);

        /// <summary>
        /// </summary>
        /// <param name="targetItem">
        /// </param>
        /// <returns>
        /// </returns>
        bool TradeSkillBuildPressed(Identity targetItem);

        /// <summary>
        /// </summary>
        /// <param name="command">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        bool ChatCommand(string command, Identity target);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        bool Logout();

        void LogoffCharacter();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        bool Login();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        bool StopLogout();

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        bool GetTargetInfo(Identity target);

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        bool TeamInvite(Identity target);

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        bool TeamKickMember(Identity target);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        bool TeamLeave();

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        bool TransferTeamLeadership(Identity target);

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        bool TeamJoinRequest(Identity target);

        /// <summary>
        /// </summary>
        /// <param name="accept">
        /// </param>
        /// <param name="requester">
        /// </param>
        /// <returns>
        /// </returns>
        bool TeamJoinReply(bool accept, Identity requester);

        /// <summary>
        /// </summary>
        /// <param name="newTeamMember">
        /// </param>
        /// <returns>
        /// </returns>
        bool TeamJoinAccepted(Identity newTeamMember);

        /// <summary>
        /// </summary>
        /// <param name="rejectingIdentity">
        /// </param>
        /// <returns>
        /// </returns>
        bool TeamJoinRejected(Identity rejectingIdentity);

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        void SendChangedStats();

        /// <summary>
        /// </summary>
        /// <param name="function">
        /// </param>
        void CallFunction(Function function, IEntity caller);

        /// <summary>
        /// Walk/Run Character to destination coordinates
        /// </summary>
        /// <param name="destination">
        /// </param>
        void MoveTo(Vector3 destination);

        /// <summary>
        /// Switch to Run mode
        /// </summary>
        void Run();

        /// <summary>
        /// Stop all movements
        /// </summary>
        void StopMovement();

        /// <summary>
        /// Switch to walk mode
        /// </summary>
        void Walk();

        bool IsFollowing();

        void DoFollow();

        void StartPatrolling();
    }
}