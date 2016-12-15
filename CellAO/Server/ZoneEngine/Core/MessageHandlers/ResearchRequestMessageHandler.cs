#region License

// Copyright (c) 2005-2016, CellAO Team
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

    using System.Collections.Generic;

    using CellAO.Core.Components;
    using CellAO.Core.Network;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    [MessageHandler(MessageHandlerDirection.InboundOnly)]
    public class ResearchRequestMessageHandler :
        BaseMessageHandler<ResearchRequestMessage, ResearchRequestMessageHandler>
    {
        protected override void Read(ResearchRequestMessage message, IZoneClient client)
        {
            List<ResearchUpdateEntry> temp = new List<ResearchUpdateEntry>();
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3000 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3010 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3011 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3012 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3020 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3021 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3022 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3030 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3040 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3041 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3050 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3051 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3052 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3060 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3070 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3071 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3072 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3080 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3081 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3082 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3090 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3100 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3101 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3102 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3110 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3111 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3112 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3120 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3130 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3131 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3132 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3140 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3141 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3142 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3200 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3201 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3202 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3203 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3204 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3205 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3206 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3207 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3208 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3209 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3210 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3211 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3212 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3213 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3214 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3215 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3216 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3217 });
            temp.Add(new ResearchUpdateEntry() { ResearchId = 3218 });

            ResearchUpdateMessageHandler.Default.Send(client.Controller.Character, temp.ToArray());
        }
    }
}