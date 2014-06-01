using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.ChatCommands
{
    using CellAO.Core.Entities;
    using CellAO.Core.Playfields;
    using CellAO.Core.Vector;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.MessageHandlers;

    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    public class InstaGrid : AOChatCommand
    {
        public override bool CheckCommandArguments(string[] args)
        {
            return true;
        }

        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(
                ChatTextMessageHandler.Default.CreateIM(character, "Syntax: .instagrid or .grid"));
        }

        public override void ExecuteCommand(ICharacter character, Identity target, string[] args)
        {
            Coordinate inGrid = new Coordinate(217, 4, 199);

            character.Playfield.Teleport((Dynel)character, inGrid, character.Heading, new Identity() { Type = IdentityType.Playfield, Instance = 152 });
        }

        public override int GMLevelNeeded()
        {
            return 1;
        }

        public override List<string> ListCommands()
        {
            return new List<string>() { "instagrid", "grid" };
        }
    }
}
