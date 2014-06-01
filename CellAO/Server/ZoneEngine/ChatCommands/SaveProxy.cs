using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.ChatCommands
{
    using CellAO.Core.Entities;
    using CellAO.Core.Playfields;
    using CellAO.Core.Statels;
    using CellAO.Core.Vector;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.Functions.GameFunctions;
    using ZoneEngine.Core.MessageHandlers;
    using ZoneEngine.Core.Playfields;

    public class SaveProxy : AOChatCommand
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
            if ((character.Stats[StatIds.externaldoorinstance].Value == 0)
                || (character.Stats[StatIds.externalplayfieldinstance].Value == 0))
            {
                ChatTextMessageHandler.Default.Create(character, "Please enter a proxyfied playfield first.");
            }

            Coordinate tempCoordinate = character.Coordinates();
            PlayfieldData pfData = PlayfieldLoader.PFData[character.Playfield.Identity.Instance];
            StatelData o = null;
            foreach (StatelData s in pfData.Statels)
            {
                if (o == null)
                {
                    o = s;
                }
                else
                {
                    if (Coordinate.Distance2D(tempCoordinate, s.Coord())
                        < Coordinate.Distance2D(tempCoordinate, o.Coord()))
                    {
                        o = s;
                    }
                }
            }
            if (o == null)
            {

                ChatTextMessageHandler.Default.Create(
                    character,
                    "No statel on this playfield... Very odd, where exactly are you???");

            }
            else
            {
                DBTeleport tel = new DBTeleport();
                tel.playfield = character.Stats[StatIds.externalplayfieldinstance].Value;
                tel.statelType = 0xc748; // Door only for now
                tel.statelInstance = character.Stats[StatIds.externaldoorinstance].BaseValue;
                tel.destinationPlayfield = o.PlayfieldId;
                tel.destinationType = (int)o.Identity.Type;
                tel.destinationInstance = BitConverter.ToUInt32(BitConverter.GetBytes(o.Identity.Instance), 0);

                var temp = TeleportDao.Instance.GetWhere(new { tel.playfield, tel.statelType, tel.statelInstance });
                foreach (var t in temp)
                {
                    TeleportDao.Instance.Delete(t.Id);
                }
                TeleportDao.Instance.Add(tel);
                character.Playfield.Publish(
    ChatTextMessageHandler.Default.CreateIM(character, "Proxy saved"));

            }

        }

        public override int GMLevelNeeded()
        {
            return 1;
        }

        public override List<string> ListCommands()
        {
            return new List<string>() { "saveproxy" };
        }
    }
}
