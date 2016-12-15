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

    using System.Linq;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.MessageHandlers;

    #endregion

    public class List : AOChatCommand
    {
        private System.Text.RegularExpressions.Regex matchLevelRange = new System.Text.RegularExpressions.Regex(@"^\d{1,3}[-]\d{1,3}$");
        private System.Text.RegularExpressions.Regex matchLevel = new System.Text.RegularExpressions.Regex(@"^\d{1,3}$");

        public override bool CheckCommandArguments(string[] args)
        {
            return args.Length <= 2;
        }

        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(
                ChatTextMessageHandler.Default.CreateIM(
                    character,
                    "/list [level range|level|name|side]"));
        }

        public override void ExecuteCommand(ICharacter character, Identity target, string[] args)
        {
            var chars = CellAO.ObjectManager.Pool.Instance.GetAll<Character>((int)IdentityType.CanbeAffected)
                    .Where(x => x.InPlayfield(character.Playfield.Identity));

            string parm1 = args.Length > 1 ? args[1] : "";

            if (!String.IsNullOrEmpty(parm1) && matchLevelRange.IsMatch(parm1))
            {
                chars = chars.Where(c =>
                    (c.Stats[CellAO.Enums.StatIds.level].Value) >= Convert.ToInt32(parm1.Split('-')[0]) &&
                    (c.Stats[CellAO.Enums.StatIds.level].Value) <= Convert.ToInt32(parm1.Split('-')[1]));
            }
            else if (!String.IsNullOrEmpty(parm1) && matchLevel.IsMatch(parm1))
            {
                chars = chars.Where(c =>
                    (c.Stats[CellAO.Enums.StatIds.level].Value) == Convert.ToInt32(parm1));
            }
            else
            {
                chars = chars.Where(c =>
                    (c.Stats[CellAO.Enums.StatIds.name].Value).ToString().ToLower().StartsWith(parm1.ToLower()) ||
                    ((Profession)c.Stats[CellAO.Enums.StatIds.profession].Value).ToString().ToLower().StartsWith(parm1.ToLower()) ||
                    ((Side)c.Stats[CellAO.Enums.StatIds.side].Value).ToString().ToLower().Equals(parm1.ToLower()));
            }

            foreach (Character entity in chars)
            {
                character.Playfield.Publish(
                ChatTextMessageHandler.Default.CreateIM(
                    character,
                    String.Format("Name: '{0}', Profession: '{1}', Level: {2}, Side: '{3}'", 
                        entity.Name,
                        ((Profession)entity.Stats[CellAO.Enums.StatIds.visualprofession].Value).ToString(),
                        (entity.Stats[CellAO.Enums.StatIds.level].Value).ToString(),
                        ((Side)entity.Stats[CellAO.Enums.StatIds.side].Value).ToString()
                        )));
            }
            character.Playfield.Publish(
            ChatTextMessageHandler.Default.CreateIM(
                character,
                String.Format("Dynels listed: {0}", chars.Count())));
        }
    

        public override int GMLevelNeeded()
        {
            return 1;
        }

        public override List<string> ListCommands()
        {
            return new List<string>(new[] { "list" });
        }
    }
}