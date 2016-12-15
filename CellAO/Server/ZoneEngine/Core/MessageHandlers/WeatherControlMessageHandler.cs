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
        public void Send(ICharacter character, WeatherEntry w, bool announceToPlayfield = false)
        {
            double secondsLate = (DateTime.UtcNow - w.StartTime).TotalMilliseconds;
            double fadeIn = w.FadeIn / 6.0f;
            short sFadeIn = w.FadeIn;

            int duration = w.Duration;
            if (secondsLate > 0)
            {
                secondsLate -= fadeIn;

                fadeIn = secondsLate < 0 ? -secondsLate : 1 / 6;
                secondsLate = secondsLate < 0 ? 0 : secondsLate;

                secondsLate -= duration * 1000;
                duration = secondsLate < 0 ? Convert.ToInt32(-secondsLate / 1000) : 1;
                // secondsLate = secondsLate < 0 ? 0 : secondsLate;
                sFadeIn = Convert.ToInt16(fadeIn * 6);
            }

            Vector3 temp = w.Position;

            this.Send(
                character,
                w.Playfield,
                temp,
                sFadeIn,
                duration,
                w.FadeOut,
                w.Range,
                (byte)w.WeatherType,
                w.Intensity,
                w.Wind,
                w.Clouds,
                w.Thunderstrikes,
                w.Tremors,
                w.TremorPercentage,
                w.ThunderstrikePercentage,
                w.AmbientColor,
                w.FogColor,
                w.ZBufferVisibility,
                announceToPlayfield);
        }

        public void Send(
            ICharacter character,
            Identity playfield,
            Vector3 position,
            short fadeIn,
            int duration,
            short fadeOut,
            Single range,
            byte weathertype,
            byte weatherIntensity,
            byte wind,
            byte clouds,
            byte thunderstrikes,
            byte tremors,
            byte tremorPercentage,
            byte thunderstrikePercentage,
            int ambientColor,
            int fogColor,
            byte zBufferVisibility,
            bool announceToPlayfield)
        {
            this.Send(
                character,
                this.Filler1(
                    playfield,
                    position,
                    fadeIn,
                    duration,
                    fadeOut,
                    range,
                    weathertype,
                    weatherIntensity,
                    wind,
                    clouds,
                    thunderstrikes,
                    tremors,
                    tremorPercentage,
                    thunderstrikePercentage,
                    ambientColor,
                    fogColor,
                    zBufferVisibility),
                announceToPlayfield);
        }

        private MessageDataFiller Filler1(
            Identity playfield,
            Vector3 position,
            short fadeIn,
            int duration,
            short fadeOut,
            Single range,
            byte weatherType,
            byte weatherIntensity,
            byte wind,
            byte clouds,
            byte thunderstrikes,
            byte tremors,
            byte tremorPercentage,
            byte thunderstrikePercentage,
            int ambientColor,
            int fogColor,
            byte zBufferVisibility)
        {
            return x =>
            {
                x.Position = new Vector3();

                x.Unknown = 0;
                x.Identity = new Identity() { Type = IdentityType.Playfield1, Instance = playfield.Instance };
                x.FadeIn = fadeIn; // in x/100*60 seconds
                x.Duration = duration; // Duration in seconds
                x.FadeOut = fadeOut; // in x/100*60 seconds
                x.Range = range; // Range

                x.WeatherType = weatherType; // StormIntensity 0-x (probably not 100)

                x.WeatherIntensity = weatherIntensity; // Rain
                x.Wind = wind; // Wind intensity
                x.Clouds = clouds; // Cloud overlay intensity
                x.Thunderstrikes = thunderstrikes; // Thunderstrikes
                x.Tremors = tremors; // Tremors
                x.TremorPercentage = tremorPercentage;
                // Tremor percentage of occurrance every 20 (+small random) seconds
                x.ThunderstrikePercentage = thunderstrikePercentage;
                // Thunderstrike percentage of occurance every 10 (+small random) seconds

                // Cloud/Ambient color
                x.CloudColorRed = (byte)(ambientColor & 0xff);
                x.CloudColorGreen = (byte)((ambientColor >> 8) & 0xff);
                x.CloudColorBlue = (byte)(ambientColor >> 16);

                x.FogColorRed = (byte)(fogColor & 0xff);
                x.FogColorGreen = (byte)((fogColor >> 8) & 0xff);
                x.FogColorBlue = (byte)(fogColor >> 16);
                x.ZBufferVisibility = zBufferVisibility; // Visibility reduction

                x.Position.X = position.X;
                x.Position.Y = position.Y;
                x.Position.Z = position.Z;
                x.UnknownSingle = 0.0f; // 0 < x < 1.0 Dunno what it is

                LogUtil.Debug(DebugInfoDetail.Memory, x.DebugString());
            };
        }
    }
}