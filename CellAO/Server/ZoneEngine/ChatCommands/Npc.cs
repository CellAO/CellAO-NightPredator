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

    using System.Collections.Generic;
    using System.Data.Linq;

    using CellAO.Core.Entities;
    using CellAO.Core.Vector;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.ObjectManager;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    using ZoneEngine.Core.Controllers;
    using ZoneEngine.Core.MessageHandlers;

    #endregion

    public class Npc : AOChatCommand
    {
        public override bool CheckCommandArguments(string[] args)
        {
            return args.Length == 2;
        }

        public override void CommandHelp(ICharacter character)
        {
            character.Playfield.Publish(
                ChatTextMessageHandler.Default.CreateIM(character, "/npc [save|despawn|delete] with targeted mob"));
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
                character.Playfield.Publish(
                    ChatTextMessageHandler.Default.CreateIM(character, "Don't try to remove/save other players please."));
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
            Coordinate tempCoordinate = mob.Coordinates();
            mobdbo.X = tempCoordinate.x;
            mobdbo.Y = tempCoordinate.y;
            mobdbo.Z = tempCoordinate.z;
            mobdbo.HeadingW = mob.Heading.wf;
            mobdbo.HeadingX = mob.Heading.xf;
            mobdbo.HeadingY = mob.Heading.yf;
            mobdbo.HeadingZ = mob.Heading.zf;
            if (mob.Waypoints.Count > 0)
            {
                List<MobSpawnWaypoint> temp = this.GetMobWaypoints(mob);
                mobdbo.Waypoints = new Binary(MessagePackZip.SerializeData(temp));
            }

            if (MobSpawnDao.Instance.Exists(mobdbo.Id))
            {
                MobSpawnDao.Instance.Delete(mobdbo.Id);
            }

            MobSpawnDao.Instance.Add(mobdbo);

            // Clear remnants first
            MobSpawnStatDao.Instance.Delete(new { mobdbo.Id, mobdbo.Playfield });
            Dictionary<int, uint> statsToSave = mob.Stats.GetStatValues();
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

        private List<MobSpawnWaypoint> GetMobWaypoints(Character mob)
        {
            List<MobSpawnWaypoint> temp = new List<MobSpawnWaypoint>();
            foreach (Waypoint wp in mob.Waypoints)
            {
                MobSpawnWaypoint waypoint = new MobSpawnWaypoint();
                waypoint.Identity = mob.Identity.Instance;
                waypoint.Playfield = mob.Playfield.Identity.Instance;
                waypoint.WalkMode = wp.Running ? 1 : 0;
                waypoint.X = wp.Position.xf;
                waypoint.Y = wp.Position.yf;
                waypoint.Z = wp.Position.zf;
                temp.Add(waypoint);
            }
            return temp;
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