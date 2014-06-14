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

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Utility;

    #endregion

    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    public class SimpleItemFullUpdateMessageHandler :
        BaseMessageHandler<SimpleItemFullUpdateMessage, SimpleItemFullUpdateMessageHandler>
    {
        public void Send(ICharacter character, StaticDynel dynel)
        {
            this.Send(character, this.DynelFiller(dynel));
        }

        private MessageDataFiller DynelFiller(StaticDynel dynel)
        {
            return x =>
            {
                try
                {
                    x.Coordinate = new Vector3(dynel.Coordinate.x, dynel.Coordinate.y, dynel.Coordinate.z);
                    x.Heading = new Quaternion()
                                {
                                    X = dynel.Heading.X,
                                    Y = dynel.Heading.Y,
                                    Z = dynel.Heading.Z,
                                    W = dynel.Heading.W
                                };

                    x.Identity = dynel.Identity;
                    x.Owner = Identity.None;
                    x.Playfield = dynel.Parent.Instance;
                    x.Unknown1 = new Identity() { Type = (IdentityType)1000015, Instance = 0 };
                    x.Unknown2 = 0;
                    x.Unknown3 = 111;
                    x.MsgVersion = 0xb;
                    x.Unknown = 0;
                    List<GameTuple<CharacterStat, UInt32>> temp = new List<GameTuple<CharacterStat, uint>>();
                    foreach (KeyValuePair<int, int> stat in dynel.Stats)
                    {
                        temp.Add(
                            new GameTuple<CharacterStat, uint>()
                            {
                                Value1 = (CharacterStat)stat.Key,
                                Value2 = (uint)stat.Value
                            });
                    }
                    x.Stats = temp.ToArray();
                    x.Name = "";
                }
                catch (Exception e)
                {
                    LogUtil.Debug(DebugInfoDetail.Error, e.Message + e.StackTrace);
                }
            };
        }
    }
}