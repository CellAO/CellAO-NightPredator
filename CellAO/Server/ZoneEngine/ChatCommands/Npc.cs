using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.ChatCommands
{
    using CellAO.Core.Entities;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.Controllers;
    using ZoneEngine.Core.MessageHandlers;

    public class Npc : AOChatCommand
    {
        public override bool CheckCommandArguments(string[] args)
        {
            return args.Length == 2;
        }

        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(character, "/npc [save|despawn|delete] with targeted mob"));
        }

        public override void ExecuteCommand(ICharacter character, Identity target, string[] args)
        {
            Character mob = Pool.Instance.GetObject<Character>(target);
            if (mob == null)
            {
                character.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(character, "Not a NPC?"));
                return;
            }

            if (!(mob.Controller is NPCController))
            {
                character.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(character, "Don't try to remove/save other players please."));
            }

            
            DBMobSpawn mobdbo = new DBMobSpawn();
            mobdbo.Id = mob.Identity.Instance;
            mobdbo.Name = mob.Name;
            mobdbo.Textures0 = 0;
            mobdbo.Textures1 = 0;
            mobdbo.Textures2 = 0;
            mobdbo.Textures3 = 0;
            mobdbo.Textures4 = 0;
            mobdbo.Playfield = mob.Playfield.Identity.Instance;
            mobdbo.X = mob.Coordinates.x;
            mobdbo.Y = mob.Coordinates.y;
            mobdbo.Z = mob.Coordinates.z;
            mobdbo.HeadingW = mob.Heading.wf;
            mobdbo.HeadingX = mob.Heading.xf;
            mobdbo.HeadingY = mob.Heading.yf;
            mobdbo.HeadingZ = mob.Heading.zf;

            if (MobSpawnDao.Instance.Exists(mobdbo.Id))
            {
                MobSpawnDao.Instance.Save(mobdbo);
            }
            else
            {
                MobSpawnDao.Instance.Add(mobdbo);
            }

            // Clear remnants first
            MobSpawnStatDao.Instance.Delete(new { mobdbo.Id, mobdbo.Playfield });
            var statsToSave = mob.Stats.GetStatValues();
            foreach (KeyValuePair<int, uint> kv in statsToSave)
            {
                MobSpawnStatDao.Instance.Add(
                    new DBMobSpawnStat()
                    {
                        Id = mob.Identity.Instance,
                        Playfield = mob.Playfield.Identity.Instance,
                        Stat = kv.Key,
                        Value = (int)kv.Value
                    });
            }
        }

        public override int GMLevelNeeded()
        {
            return 1;
        }

        public override List<string> ListCommands()
        {
            return new List<string> { "npc" };
        }
    }
}
