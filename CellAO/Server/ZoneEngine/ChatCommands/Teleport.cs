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
    using System.Globalization;

    using CellAO.Core.Entities;
    using CellAO.Core.Vector;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.MessageHandlers;
    using ZoneEngine.Core.Playfields;

    #endregion

    /// <summary>
    /// </summary>
    public class ChatCommandTeleport : AOChatCommand
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        /// <returns>
        /// </returns>
        public override bool CheckCommandArguments(string[] args)
        {
            var check = new List<Type> { typeof(float), typeof(float), typeof(int) };
            bool check1 = CheckArgumentHelper(check, args);

            check.Clear();
            check.Add(typeof(float));
            check.Add(typeof(float));
            check.Add(typeof(string));
            check.Add(typeof(float));
            check.Add(typeof(int));
            check1 |= CheckArgumentHelper(check, args);

            return check1;
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(
                ChatTextMessageHandler.Default.CreateIM(
                    character,
                    "Teleports you\r\n" + "Usage: /tp [float] [float] [int] (X, Z, Playfield)\r\n"
                    + "Or:    /tp [float] [float] y [float] [int] (X, Z, Y, Playfield)"));
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="args">
        /// </param>
        public override void ExecuteCommand(ICharacter character, Identity target, string[] args)
        {
            var check = new List<Type> { typeof(float), typeof(float), typeof(int) };

            var coord = new Coordinate();
            int pf = character.Playfield.Identity.Instance;
            if (CheckArgumentHelper(check, args))
            {
                coord = new Coordinate(
                    float.Parse(args[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                    character.Coordinates.y,
                    float.Parse(args[2], NumberStyles.Any, CultureInfo.InvariantCulture));
                pf = int.Parse(args[3]);
            }

            check.Clear();
            check.Add(typeof(float));
            check.Add(typeof(float));
            check.Add(typeof(string));
            check.Add(typeof(float));
            check.Add(typeof(int));

            if (CheckArgumentHelper(check, args))
            {
                coord = new Coordinate(
                    float.Parse(args[1], NumberStyles.Any, CultureInfo.InvariantCulture),
                    float.Parse(args[4], NumberStyles.Any, CultureInfo.InvariantCulture),
                    float.Parse(args[2], NumberStyles.Any, CultureInfo.InvariantCulture));
                pf = int.Parse(args[5]);
            }

            if (!Playfields.ValidPlayfield(pf))
            {
                FeedbackMessageHandler.Default.Send(character, 110, 188845972);
            }
            else
            {
                character.Playfield.Teleport(
                    (Character)character,
                    coord,
                    character.Heading,
                    new Identity() { Type = IdentityType.Playfield, Instance = pf });
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override int GMLevelNeeded()
        {
            return 1;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override List<string> ListCommands()
        {
            var temp = new List<string> { "tp", "teleport" };
            return temp;
        }

        #endregion
    }
}