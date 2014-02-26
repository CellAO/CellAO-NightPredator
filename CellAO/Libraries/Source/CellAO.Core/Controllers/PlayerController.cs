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

namespace CellAO.Core.Controllers
{
    #region Usings ...

    using System;

    using CellAO.Core.Entities;
    using CellAO.Core.Vector;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Quaternion = CellAO.Core.Vector.Quaternion;

    #endregion

    /// <summary>
    /// </summary>
    internal class PlayerController : IController
    {
        /// <summary>
        /// </summary>
        public ICharacter Character { get; set; }

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
        public bool CastNano(int nanoId, Identity target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="parameter1">
        /// </param>
        /// <param name="parameter2">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool CharacterAction(CharacterActionType action, Identity target, int parameter1, int parameter2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="command">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool ChatCommand(string command, Identity target)
        {
            throw new NotImplementedException();
        }

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
        public bool Move(int moveType, Coordinate newCoordinates, Quaternion heading)
        {
            throw new NotImplementedException();
        }

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
        public bool ContainerAddItem(int sourceContainerType, int sourcePlacement, Identity target, int targetPlacement)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Follow(Identity target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <param name="count">
        /// </param>
        /// <param name="user">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool GenericCommand(GenericCmdAction action, int count, Identity user, Identity target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool LookAt(Identity target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool InviteToTeam(Identity target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Logout()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Login()
        {
            throw new NotImplementedException();
        }

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
        public bool SocialAction(
            SocialAction action, 
            byte parameter1, 
            byte parameter2, 
            byte parameter3, 
            byte parameter4, 
            int parameter5)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Trade(Identity target)
        {
            throw new NotImplementedException();
        }
    }
}