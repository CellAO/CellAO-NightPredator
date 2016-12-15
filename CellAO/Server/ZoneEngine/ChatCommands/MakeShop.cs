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

namespace ZoneEngine.ChatCommands
{
    #region Usings ...

    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Entities;
    using CellAO.Core.Statels;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.Playfields;

    #endregion

    public class MakeShop : AOChatCommand
    {
        public override bool CheckCommandArguments(string[] args)
        {
            return true;
        }

        public override void CommandHelp(ICharacter character)
        {
        }

        public override void ExecuteCommand(ICharacter character, Identity target, string[] args)
        {
            Vendor v = Pool.Instance.GetObject<Vendor>(character.Playfield.Identity, target);
            if (v != null)
            {
                int pfid = character.Playfield.Identity.Instance;
                StatelData sd =
                    PlayfieldLoader.PFData[pfid].Statels.FirstOrDefault(x => x.Identity.Equals(v.OriginalIdentity));
                if (sd != null)
                {
                    int instance = (((sd.Identity.Instance) >> 16) & 0xff
                                    | (character.Playfield.Identity.Instance << 16));
                    DBVendor dbv = new DBVendor();
                    dbv.Id = instance;
                    dbv.Playfield = pfid;
                    dbv.X = sd.X;
                    dbv.Y = sd.Y;
                    dbv.Z = sd.Z;
                    dbv.HeadingX = sd.HeadingX;
                    dbv.HeadingY = sd.HeadingY;
                    dbv.HeadingZ = sd.HeadingZ;
                    dbv.HeadingW = sd.HeadingW;
                    dbv.Name = "New shop, please fill me";
                    dbv.TemplateId = sd.TemplateId;
                    dbv.Hash = "";
                    VendorDao.Instance.Delete(dbv.Id);
                    VendorDao.Instance.Add(dbv, dontUseId: false);
                }
            }
        }

        public override int GMLevelNeeded()
        {
            return 1;
        }

        public override List<string> ListCommands()
        {
            return new List<string>() { "makeshop" };
        }
    }
}