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

namespace ZoneEngine.Core.Functions.GameFunctions
{
    #region Usings ...

    using System.Linq;

    using CellAO.Core.Entities;
    using CellAO.Core.Playfields;
    using CellAO.Core.Vector;
    using CellAO.Enums;

    using MsgPack;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.Playfields;

    #endregion

    /// <summary>
    /// </summary>
    internal class lineteleport : FunctionPrototype
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        public override FunctionType FunctionId
        {
            get
            {
                return FunctionType.LineTeleport;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="self">
        /// </param>
        /// <param name="caller">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="arguments">
        /// </param>
        /// <returns>
        /// </returns>
        public override bool Execute(
            INamedEntity self,
            INamedEntity caller,
            IInstancedEntity target,
            MessagePackObject[] arguments)
        {
            if (arguments.Count() != 3)
            {
                return false;
            }

            uint arg1 = arguments[1].AsUInt32();
            int toPlayfield = arguments[2].AsInt32();

            byte destinationIndex = (byte)(arg1 >> 16);
            PlayfieldData pfd = PlayfieldLoader.PFData[toPlayfield];
            PlayfieldDestination pfDestination = pfd.Destinations[destinationIndex];

            float newX = (pfDestination.EndX - pfDestination.StartX) * 0.5f + pfDestination.StartX;
            float newZ = (pfDestination.EndZ - pfDestination.StartZ) * 0.5f + pfDestination.StartZ;
            float dist = WallCollision.Distance(
                pfDestination.StartX,
                pfDestination.StartZ,
                pfDestination.EndX,
                pfDestination.EndZ);
            float headDistX = (pfDestination.EndX - pfDestination.StartX) / dist;
            float headDistZ = (pfDestination.EndZ - pfDestination.StartZ) / dist;
            newX -= headDistZ * 8;
            newZ += headDistX * 8;

            Coordinate destCoordinate = new Coordinate(newX, pfDestination.EndY, newZ);

            ((ICharacter)self).Teleport(
                destCoordinate,
                ((ICharacter)self).Heading,
                new Identity() { Type = IdentityType.Playfield, Instance = toPlayfield });
            return true;
        }

        #endregion
    }
}