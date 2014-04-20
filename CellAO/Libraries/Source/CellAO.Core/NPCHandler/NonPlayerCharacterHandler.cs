using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Core.NPCHandler
{
    using CellAO.Core.Entities;
    using CellAO.Core.Functions;
    using CellAO.Core.Items;
    using CellAO.Core.Playfields;
    using CellAO.Core.Vector;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.Enums;
    using CellAO.ObjectManager;

    using Dapper;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Quaternion = CellAO.Core.Vector.Quaternion;

    public static class NonPlayerCharacterHandler
    {
        public static Character SpawnMobFromTemplate(string hash, Identity playfieldIdentity, Coordinate coord, Quaternion heading, IController controller, int desiredLevel = -1)
        {
            Character mobCharacter = null;

            var mob = MobTemplateDao.Instance.GetMobTemplateByHash(hash);
            if (mob != null)
            {
                int level = desiredLevel;
                if (level == -1)
                {
                    // Get random inside level range
                    Random rnd = new Random();
                    level = mob.MinLvl + rnd.Next(mob.MaxLvl - mob.MinLvl);
                }
                else
                {
                    // Put it inside level range
                    level = (level < mob.MinLvl ? mob.MinLvl : level);
                    level = level > mob.MaxLvl ? mob.MaxLvl : level;
                }
                mobCharacter = CreateMob(mob, playfieldIdentity, coord, heading, controller, level);
            }
            return mobCharacter;
        }

        private static Character CreateMob(DBMobTemplate mob, Identity playfieldIdentity, Coordinate coord, Quaternion heading, IController controller, int level)
        {
            IPlayfield playfield = Pool.Instance.GetObject<IPlayfield>(playfieldIdentity);
            if (playfield != null)
            {
                int newInstanceId = Pool.Instance.GetFreeInstance<Character>(1000000, IdentityType.CanbeAffected);
                Identity newIdentity = new Identity() { Type = IdentityType.CanbeAffected, Instance = newInstanceId };
                Character mobCharacter = new Character(newIdentity, controller);
                mobCharacter.Coordinates = coord;
                mobCharacter.Playfield = Pool.Instance.GetObject<IPlayfield>(playfieldIdentity);
                mobCharacter.RawHeading = new Quaternion(heading.xf, heading.yf, heading.zf, heading.wf);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.life, (uint)mob.Health);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.level, (uint)level);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.npcfamily, (uint)mob.NPCFamily);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.side, (uint)mob.Side);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.fatness, (uint)mob.Fatness);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.breed, (uint)mob.Breed);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.sex, (uint)mob.Sex);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.race, (uint)mob.Race);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.flags, (uint)mob.Flags);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.monsterdata, (uint)mob.MonsterData);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.monsterscale, (uint)mob.MonsterScale);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.profession, 15);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.accountflags, 0);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.expansion, 0);
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.runspeed, 513);
                mobCharacter.Stats[StatIds.headmesh].BaseValue = (uint)mob.HeadMesh;
                mobCharacter.Stats.SetBaseValueWithoutTriggering((int)StatIds.losheight, 15);

                /*
                 * For testing only, blue trousers and a helmet
                IItem trousers = new Item(1, ItemLoader.ItemList[27350].GetLowId(1),ItemLoader.ItemList[27350].GetLowId(1));
                IItem helmet = new Item(1, ItemLoader.ItemList[85534].GetLowId(1), ItemLoader.ItemList[85534].GetHighId(1));
                mobCharacter.BaseInventory.Pages[(int)IdentityType.ArmorPage].Add((int)ArmorSlots.Legs + mobCharacter.BaseInventory.Pages[(int)IdentityType.ArmorPage].FirstSlotNumber, trousers);
                mobCharacter.BaseInventory.Pages[(int)IdentityType.ArmorPage].Add((int)ArmorSlots.Head + mobCharacter.BaseInventory.Pages[(int)IdentityType.ArmorPage].FirstSlotNumber, helmet);
                 */
                mobCharacter.BaseInventory.CalculateModifiers(mobCharacter);
                Function fc = new Function();
                fc.FunctionType = (int)FunctionType.HeadMesh;
                fc.Arguments.Values.Add(0);
                fc.Arguments.Values.Add(mob.HeadMesh);
                fc.Target = (int)ItemTarget.Self;
                fc.TickCount = 1;
                fc.TickInterval = 0;
                mobCharacter.Controller.CallFunction(fc);


                mobCharacter.Name = mob.Name;
                mobCharacter.FirstName = "";
                mobCharacter.LastName = "";
                controller.Character = mobCharacter;
                return mobCharacter;
            }
            return null;
        }
    }
}
