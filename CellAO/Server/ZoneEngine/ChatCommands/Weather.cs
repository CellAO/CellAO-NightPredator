using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.ChatCommands
{
    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using ZoneEngine.Core.MessageHandlers;

    public class Weather : AOChatCommand
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
            WeatherControlMessageHandler.Default.Send(character);
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
