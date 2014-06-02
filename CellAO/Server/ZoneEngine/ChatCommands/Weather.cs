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

namespace ZoneEngine.ChatCommands
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.MessageHandlers;

    #endregion

    public class Weather : AOChatCommand
    {
        public override bool CheckCommandArguments(string[] args)
        {
            var check = new List<Type>
                        {
                            typeof(short),
                            typeof(int),
                            typeof(short),
                            typeof(Single),
                            typeof(byte),
                            typeof(byte),
                            typeof(byte),
                            typeof(byte),
                            typeof(byte),
                            typeof(byte),
                            typeof(byte),
                            typeof(byte),
                            typeof(string),
                            typeof(string),
                            typeof(byte)
                        };
            bool check1 = CheckArgumentHelper(check, args);

            return check1;
        }

        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(
                ChatTextMessageHandler.Default.CreateIM(
                    character,
                    "Arguments: <fadeIn> <duration> <fadeOut> <Range> <Weathertype> <Intensity> <Wind> <Clouds> <thunderstrikes> <tremors> <tremorpercentage> <thunderstrikepercentage> <ambientColor> <fogColor> <zBufferMax>\r\n"
                    + "Weathertypes: 0 = Rain, 1 = Fog, 2 = Unknown, 3 = Quake, 4 = Sandstorm, 5 = AshStorm, 6 = RedFalloutStorm, 7 = GreenFalloutStorm\r\n"
                    + "Type is 0-7, Range is Single, all other values are 0-100"));
        }

        public override void ExecuteCommand(ICharacter character, Identity target, string[] args)
        {
            Vector3 position = new Vector3();
            position.X = character.Coordinates().x;
            position.Y = character.Coordinates().y;
            position.Z = character.Coordinates().z;
            byte type = byte.Parse(args[1]);
            /*if (type == 2)
            {
                // Set temperature
                WeatherControlMessageHandler.Default.Send(
                    character,
                    byte.Parse(args[1]),
                    (byte)(sbyte.Parse(args[2])),
                    byte.Parse(args[3]),
                    byte.Parse(args[4]),
                    byte.Parse(args[5]),
                    byte.Parse(args[6]),
                    byte.Parse(args[7]),
                    byte.Parse(args[8]));
            }
            else*/
            {
                int ambientColor = Convert.ToInt32(args[13], 16);
                int fogColor = Convert.ToInt32(args[14], 16);

                WeatherEntry newWeather = new WeatherEntry();
                newWeather.AmbientColor = ambientColor;
                newWeather.FogColor = fogColor;
                newWeather.Position = position;
                newWeather.FadeIn = short.Parse(args[1]);
                newWeather.Duration = int.Parse(args[2]);
                newWeather.FadeOut = short.Parse(args[3]);
                newWeather.Range = Single.Parse(args[4]);
                newWeather.WeatherType = (WeatherType)byte.Parse(args[5]);
                newWeather.Intensity = byte.Parse(args[6]);
                newWeather.Wind = byte.Parse(args[7]);
                newWeather.Clouds = byte.Parse(args[8]);
                newWeather.Thunderstrikes = byte.Parse(args[9]);
                newWeather.Tremors = byte.Parse(args[10]);
                newWeather.ThunderstrikePercentage = byte.Parse(args[11]);
                newWeather.TremorPercentage = byte.Parse(args[12]);
                newWeather.ZBufferVisibility = byte.Parse(args[15]);
                newWeather.Playfield = character.Playfield.Identity;

                WeatherSettings.Instance.Add(newWeather);

                WeatherControlMessageHandler.Default.Send(character, newWeather);
            }
        }

        public override int GMLevelNeeded()
        {
            return 1;
        }

        public override List<string> ListCommands()
        {
            return new List<string>(new[] { "weather" });
        }
    }
}