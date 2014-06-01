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

    using System;

    using CellAO.Core.Entities;
    using CellAO.Core.Statels;
    using CellAO.Core.Vector;
    using CellAO.Enums;
    using CellAO.Interfaces;

    using MsgPack;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.Playfields;

    using Quaternion = CellAO.Core.Vector.Quaternion;
    using Vector3 = CellAO.Core.Vector.Vector3;

    #endregion

    internal class teleportproxy : FunctionPrototype
    {
        private const FunctionType functionId = FunctionType.TeleportProxy;

        public override FunctionType FunctionId
        {
            get
            {
                return functionId;
            }
        }

        public override bool Execute(
            INamedEntity self,
            IEntity caller,
            IInstancedEntity target,
            MessagePackObject[] arguments)
        {
            
            ICharacter character = (ICharacter)self;

            int statelId = (int)((uint)0xC0000000 | arguments[1].AsInt32() | (arguments[2].AsInt32() << 16));
            character.Stats[StatIds.externaldoorinstance].BaseValue = (uint)caller.Identity.Instance;
            character.Stats[StatIds.externalplayfieldinstance].BaseValue = (uint)character.Playfield.Identity.Instance;

            if (arguments[1].AsInt32() > 0)
            {
                StatelData sd = PlayfieldLoader.PFData[arguments[1].AsInt32()].GetDoor(statelId);
                if (sd == null)
                {
                    throw new Exception(
                        "Statel " + arguments[3].AsInt32().ToString("X") + " not found? Check the rdb dammit");
                }

                Vector3 v = new Vector3(sd.X, sd.Y, sd.Z);

                Quaternion q = new Quaternion(sd.HeadingX, sd.HeadingY, sd.HeadingZ, sd.HeadingW);

                Quaternion.Normalize(q);
                Vector3 n = (Vector3)q.RotateVector3(Vector3.AxisX);

                v.x += n.z * 2;
                v.z += n.x * 2;
                character.Playfield.Teleport(
                    (Dynel)character,
                    new Coordinate(v),
                    q,
                    new Identity() { Type = (IdentityType)arguments[0].AsInt32(), Instance = arguments[1].AsInt32() });
            }

            return true;
        }
    }
}