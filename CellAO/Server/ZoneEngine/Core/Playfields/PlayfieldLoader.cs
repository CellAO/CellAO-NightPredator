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

namespace ZoneEngine.Core.Playfields
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Events;
    using CellAO.Core.Functions;
    using CellAO.Core.Items;
    using CellAO.Core.Playfields;
    using CellAO.Core.Statels;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.Enums;

    using MsgPack;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    #endregion

    /// <summary>
    /// </summary>
    public static class PlayfieldLoader
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        public static Dictionary<int, PlayfieldData> PFData = new Dictionary<int, PlayfieldData>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static int CacheAllPlayfieldData()
        {
            return CacheAllPlayfieldData("playfields.dat");
        }

        /// <summary>
        /// </summary>
        /// <param name="fname">
        /// </param>
        /// <returns>
        /// </returns>
        public static int CacheAllPlayfieldData(string fname)
        {
            PFData = new Dictionary<int, PlayfieldData>();

            MessagePackZip.UncompressData<PlayfieldData>(fname).ForEach(x => PFData.Add(x.PlayfieldId, x));

            Console.WriteLine("Tweaking in some Statel functions");

            // Now lets do some tweaking

            foreach (PlayfieldData pfd in PFData.Values)
            {
                foreach (StatelData sd in pfd.Statels)
                {
                    bool foundproxyteleport = false;
                    int playfieldid = 0;
                    int doorinstance = 0;
                    foreach (Event ev in sd.Events)
                    {
                        foreach (Function f in ev.Functions)
                        {
                            if (f.FunctionType == (int)FunctionType.TeleportProxy)
                            {
                                foundproxyteleport = true;
                                playfieldid = f.Arguments.Values[1].AsInt32();
                                doorinstance =
                                    (int)
                                        ((uint)0xC0000000 | f.Arguments.Values[1].AsInt32()
                                         | (f.Arguments.Values[2].AsInt32() << 16));
                                DBTeleport teleporter =
                                    TeleportDao.Instance.GetWhere(new { statelInstance = (uint)sd.Identity.Instance })
                                        .FirstOrDefault();
                                if (teleporter != null)
                                {
                                    doorinstance = (int)teleporter.destinationInstance;
                                    f.Arguments.Values[2] = new MessagePackObject(((doorinstance >> 16) & 0xff));
                                }
                                break;
                            }
                            if (f.FunctionType == (int)FunctionType.TeleportProxy2)
                            {
                                playfieldid = f.Arguments.Values[1].AsInt32();
                                doorinstance =
                                    (int)
                                        ((uint)0xC0000000 | f.Arguments.Values[1].AsInt32()
                                         | (f.Arguments.Values[2].AsInt32() << 16));
                                DBTeleport teleporter =
                                    TeleportDao.Instance.GetWhere(new { statelInstance = (uint)sd.Identity.Instance })
                                        .FirstOrDefault();
                                if (teleporter != null)
                                {
                                    doorinstance = (int)teleporter.destinationInstance;
                                    f.Arguments.Values[2] = new MessagePackObject(((doorinstance >> 16) & 0xff));
                                }
                            }
                        }
                        if (foundproxyteleport)
                        {
                            break;
                        }
                    }

                    if (ItemLoader.ItemList.ContainsKey(sd.TemplateId))
                    {
                        if (ItemLoader.ItemList[sd.TemplateId].WantsCollision()
                            && !ItemLoader.ItemList[sd.TemplateId].StatelCollisionDisabled()
                            && (sd.Events.All(x => x.EventType != EventType.OnCollide))
                            && sd.Events.Any(x => x.EventType == EventType.OnUse))
                        {
                            Event ev = sd.Events.First(x => x.EventType == EventType.OnUse).Copy();
                            ev.EventType = EventType.OnCollide;
                            sd.Events.Add(ev);
                        }
                    }

                    if (foundproxyteleport)
                    {
                        if (PFData.ContainsKey(playfieldid))
                        {
                            StatelData internalDoor = PFData[playfieldid].GetDoor(doorinstance);
                            if (internalDoor != null)
                            {
                                if (internalDoor.Events.All(x => x.EventType != EventType.OnEnter))
                                {
                                    Event ev = new Event();
                                    ev.EventType = EventType.OnEnter;
                                    ev.Functions.Add(
                                        new Function() { FunctionType = (int)FunctionType.ExitProxyPlayfield });
                                    internalDoor.Events.Add(ev);
                                }
                            }
                        }
                    }
                }
            }

            return PFData.Count;
        }

        #endregion
    }

    /// <summary>
    /// </summary>
    public class EqualityComparer : IEqualityComparer<Identity>
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="x">
        /// </param>
        /// <param name="y">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Equals(Identity x, Identity y)
        {
            return (x.Type == y.Type) && (x.Instance == y.Instance);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetHashCode(Identity obj)
        {
            return obj.Type.GetHashCode() ^ obj.Instance.GetHashCode();
        }

        #endregion
    }
}