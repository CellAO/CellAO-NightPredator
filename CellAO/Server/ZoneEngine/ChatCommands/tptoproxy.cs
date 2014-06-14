using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.ChatCommands
{
    using CellAO.Core.Entities;
    using CellAO.Core.Events;
    using CellAO.Core.Functions;
    using CellAO.Core.Playfields;
    using CellAO.Core.Statels;
    using CellAO.Core.Vector;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    using ZoneEngine.Core.Playfields;

    public class tptoproxy : AOChatCommand
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
            var proxies = TeleportDao.Instance.GetAll();
            List<StatelData> statels = new List<StatelData>();
            Function lastFound = null;
            foreach (PlayfieldData pf in PlayfieldLoader.PFData.Values)
            {
                bool isProxy = false;
                foreach (StatelData sd in pf.Statels)
                {
                    lastFound = null;
                    isProxy = false;
                    if (sd.Events.Count > 0)
                    {
                        foreach (Event e in sd.Events)
                        {
                            foreach (Function f in e.Functions)
                            {
                                if (f.FunctionType == (int)FunctionType.TeleportProxy)
                                {
                                    isProxy = true;
                                    lastFound = f;
                                    if (f.Arguments.Values[1].AsInt32() < 0)
                                    {
                                        lastFound = null;
                                        isProxy = false;
                                    }
                                    break;
                                }
                            }
                            if (isProxy)
                            {
                                break;
                            }
                        }
                    }
                    if (isProxy)
                    {
                        if (
                            !proxies.Any(
                                x =>
                                    (x.statelType == (int)sd.Identity.Type)
                                    && (x.statelInstance == (uint)sd.Identity.Instance)
                                    && (x.playfield == sd.PlayfieldId)))
                        {


                            PlayfieldData dest = PlayfieldLoader.PFData[lastFound.Arguments.Values[1].AsInt32()];
                            if (dest.Statels.Count(x => x.Identity.Type == IdentityType.Door) == 1)
                            {
                                StatelData sddest = dest.Statels.First(x => x.Identity.Type == IdentityType.Door);
                                DBTeleport tel = new DBTeleport();
                                tel.playfield = sd.PlayfieldId;
                                tel.statelType = 0xc748; // Door only for now
                                tel.statelInstance = (uint)sd.Identity.Instance;
                                tel.destinationPlayfield = sddest.PlayfieldId;
                                tel.destinationType = (int)sddest.Identity.Type;
                                tel.destinationInstance = BitConverter.ToUInt32(BitConverter.GetBytes(sddest.Identity.Instance), 0);
                                var temp = TeleportDao.Instance.GetWhere(new { tel.playfield, tel.statelType, tel.statelInstance });
                                foreach (var t in temp)
                                {
                                    TeleportDao.Instance.Delete(t.Id);
                                }
                                TeleportDao.Instance.Add(tel);
                                isProxy = false;
                            }
                        }

                    }


                    if (isProxy)
                    {
                        // Check against proxies already found
                        if (
                            !proxies.Any(
                                x =>
                                    (x.statelType == (int)sd.Identity.Type)
                                    && (x.statelInstance == (uint)sd.Identity.Instance) && (x.playfield == sd.PlayfieldId)))
                        {
                            PlayfieldData dest = PlayfieldLoader.PFData[lastFound.Arguments.Values[1].AsInt32()];
                            StatelData door1 = null;
                            if (dest.Statels.Count(x => x.Identity.Type == IdentityType.Door) > 0)
                            {
                                 door1 = dest.Statels.First(x => x.Identity.Type == IdentityType.Door);
                                 LogUtil.Debug(DebugInfoDetail.Error, sd.PlayfieldId + " " + sd.Identity.ToString(true));
                                 character.Stats[StatIds.externaldoorinstance].BaseValue = (uint)sd.Identity.Instance;
                                 character.Stats[StatIds.externalplayfieldinstance].BaseValue = (uint)sd.PlayfieldId;
                                 character.Playfield.Teleport(
                                     (Dynel)character,
                                     new Coordinate(door1.X, door1.Y+1.0f, door1.Z),
                                     character.Heading,
                                     new Identity() { Type = (IdentityType)lastFound.Arguments.Values[0].AsInt32() , Instance = door1.PlayfieldId });
                                return;
                            }
                            LogUtil.Debug(DebugInfoDetail.Error, sd.PlayfieldId + " " + sd.Identity.ToString(true));
                            character.Stats[StatIds.externaldoorinstance].BaseValue = 0;
                            character.Stats[StatIds.externalplayfieldinstance].BaseValue = 0;
                            character.Playfield.Teleport(
                                (Dynel)character,
                                new Coordinate(sd.X, sd.Y, sd.Z),
                                character.Heading,
                                new Identity() { Type = IdentityType.Playfield, Instance = sd.PlayfieldId });
                            return;
                        }
                    }
                }
            }
        }

        public override int GMLevelNeeded()
        {
            return 1;
        }

        public override List<string> ListCommands()
        {
            return new List<string>() { "tpt", "tp2" };
        }
    }
}
