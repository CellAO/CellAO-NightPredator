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

    using CellAO.Core.Components;
    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Utility;

    #endregion

    [MessageHandler(MessageHandlerDirection.OutboundOnly)]
    public class WeatherControlMessageHandler : BaseMessageHandler<WeatherControlMessage, WeatherControlMessageHandler>
    {
        public void Send(ICharacter character)
        {
            this.Send(character, this.Filler1(character));
        }
        private static byte version = 0;
        private MessageDataFiller Filler1(ICharacter character)
        {

            return x =>
            {
                Random rnd = new Random();
                x.Heading = new Quaternion();
                x.Heading.X = character.Coordinates().x;
                x.Heading.Y = character.Coordinates().y;
                x.Heading.Z = character.Heading.zf;
                x.Heading.W = character.Heading.wf;
                x.Unknown = 0;
                x.Identity = character.Identity;
                x.Unknown1 = 100;
                x.Unknown2 = 10000;
                x.Unknown3 = 100;
                x.Unknown4 = 1000.0f;

                x.Unknown5 = WeatherControlMessageHandler.version++;

                x.Unknown6 = (byte)rnd.Next(10);
                x.Unknown7 = (byte)rnd.Next(20);
                x.Unknown8 = (byte)rnd.Next(30);
                x.Unknown9 = (byte)rnd.Next(40);
                x.Unknown10 = (byte)rnd.Next(50);
                x.Unknown11 = (byte)rnd.Next(60);
                x.Unknown12 = (byte)rnd.Next(70);
                x.Unknown13 = (byte)rnd.Next(255);
                x.Unknown14 = (byte)rnd.Next(255);
                x.Unknown15 = (byte)rnd.Next(255);
                x.Unknown16 = (byte)rnd.Next(255);
                x.Unknown17 = (byte)rnd.Next(255);
                x.Unknown18 = (byte)rnd.Next(255);
                x.Unknown19 = (byte)rnd.Next(100);
                LogUtil.Debug(DebugInfoDetail.Memory,x.DebugString());
            };
        }
    }
}