#region License

// Copyright (c) 2005-2013, CellAO Team
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
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

#endregion

namespace ZoneEngine.Core.PacketHandlers
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Items;
    using CellAO.Enums;

    using ZoneEngine.Core.Packets;

    #endregion

    /// <summary>
    /// </summary>
    public static class TradeSkillReceiver
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static readonly List<TradeSkillInfo> TradeSkillInfos = new List<TradeSkillInfo>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="sourceItem">
        /// </param>
        /// <param name="targetItem">
        /// </param>
        /// <param name="newItem">
        /// </param>
        /// <returns>
        /// </returns>
        public static string SuccessMessage(Item sourceItem, Item targetItem, Item newItem)
        {
            return string.Format(
                "You combined \"{0}\" with \"{1}\" and the result is a quality level {2} \"{3}\".", 
                TradeSkill.Instance.GetItemName(sourceItem.LowID, sourceItem.HighID, sourceItem.Quality), 
                TradeSkill.Instance.GetItemName(targetItem.LowID, targetItem.HighID, targetItem.Quality), 
                newItem.Quality, 
                TradeSkill.Instance.GetItemName(newItem.LowID, newItem.HighID, newItem.Quality));
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="quality">
        /// </param>
        public static void TradeSkillBuildPressed(ZoneClient client, int quality)
        {
            TradeSkillInfo source = client.Character.TradeSkillSource;
            TradeSkillInfo target = client.Character.TradeSkillTarget;

            Item sourceItem = client.Character.BaseInventory.GetItemInContainer(source.Container, source.Placement);
            Item targetItem = client.Character.BaseInventory.GetItemInContainer(target.Container, target.Placement);

            TradeSkillEntry ts = TradeSkill.Instance.GetTradeSkillEntry(sourceItem.HighID, targetItem.HighID);

            quality = Math.Min(quality, ItemLoader.ItemList[ts.ResultHighId].Quality);
            if (ts != null)
            {
                if (WindowBuild(client, quality, ts, sourceItem, targetItem))
                {
                    Item newItem = new Item(quality, ts.ResultLowId, ts.ResultHighId);
                    InventoryError inventoryError = client.Character.BaseInventory.TryAdd(newItem);
                    if (inventoryError == InventoryError.OK)
                    {
                        AddTemplate.Send(client, newItem);

                        // Delete source?
                        if ((ts.DeleteFlag & 1) == 1)
                        {
                            client.Character.BaseInventory.RemoveItem(source.Container, source.Placement);
                            DeleteItem.Send(client, source.Container, source.Placement);
                        }

                        // Delete target?
                        if ((ts.DeleteFlag & 2) == 2)
                        {
                            client.Character.BaseInventory.RemoveItem(target.Container, target.Placement);
                            DeleteItem.Send(client, target.Container, target.Placement);
                        }

                        client.Character.Playfield.Publish(ChatText.CreateIM(client.Character, SuccessMessage(sourceItem, targetItem, new Item(quality, ts.ResultLowId, ts.ResultHighId))));

                        client.Character.Stats[StatIds.xp].Value += CalculateXP(quality, ts);
                    }
                }
            }
            else
            {
                client.Character.Playfield.Publish(ChatText.CreateIM(client.Character, "It is not possible to assemble those two items. Maybe the order was wrong?"));
                client.Character.Playfield.Publish(ChatText.CreateIM(client.Character, "No combination found!"));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="container">
        /// </param>
        /// <param name="placement">
        /// </param>
        public static void TradeSkillSourceChanged(ZoneClient client, int container, int placement)
        {
            if ((container != 0) && (placement != 0))
            {
                client.Character.TradeSkillSource = new TradeSkillInfo(0, container, placement);

                Item item = client.Character.BaseInventory.GetItemInContainer(container, placement);
                TradeSkillPacket.SendSource(client.Character, TradeSkill.Instance.SourceProcessesCount(item.HighID));

                TradeSkillChanged(client);
            }
            else
            {
                client.Character.TradeSkillSource = null;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="container">
        /// </param>
        /// <param name="placement">
        /// </param>
        public static void TradeSkillTargetChanged(ZoneClient client, int container, int placement)
        {
            if ((container != 0) && (placement != 0))
            {
                client.Character.TradeSkillTarget = new TradeSkillInfo(0, container, placement);

                Item item = client.Character.BaseInventory.GetItemInContainer(container, placement);
                TradeSkillPacket.SendTarget(client.Character, TradeSkill.Instance.TargetProcessesCount(item.HighID));

                TradeSkillChanged(client);
            }
            else
            {
                client.Character.TradeSkillTarget = null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="quality">
        /// </param>
        /// <param name="ts">
        /// </param>
        /// <returns>
        /// </returns>
        private static int CalculateXP(int quality, TradeSkillEntry ts)
        {
            int absMinQL = ItemLoader.ItemList[ts.ResultLowId].Quality;
            int absMaxQL = ItemLoader.ItemList[ts.ResultHighId].Quality;
            if (absMaxQL == absMinQL)
            {
                return ts.MaxXP;
            }

            return
                (int)
                    Math.Floor(
                        (double)((ts.MaxXP - ts.MinXP) / (absMaxQL - absMinQL)) * (quality - absMinQL) + ts.MinXP);
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        private static void TradeSkillChanged(ZoneClient client)
        {
            TradeSkillInfo source = client.Character.TradeSkillSource;
            TradeSkillInfo target = client.Character.TradeSkillTarget;

            if ((source != null) && (target != null))
            {
                Item sourceItem = client.Character.BaseInventory.GetItemInContainer(source.Container, source.Placement);
                Item targetItem = client.Character.BaseInventory.GetItemInContainer(target.Container, target.Placement);

                TradeSkillEntry ts = TradeSkill.Instance.GetTradeSkillEntry(sourceItem.HighID, targetItem.HighID);
                if (ts != null)
                {
                    if (ts.ValidateRange(sourceItem.Quality, targetItem.Quality))
                    {
                        foreach (TradeSkillSkill tsi in ts.Skills)
                        {
                            int skillReq = (int)Math.Ceiling(tsi.Percent / 100M * targetItem.Quality);
                            if (skillReq > client.Character.Stats[tsi.StatId].Value)
                            {
                                TradeSkillPacket.SendRequirement(client.Character, tsi.StatId, skillReq);
                            }
                        }

                        int leastbump = 0;
                        int maxbump = 0;
                        if (ts.IsImplant)
                        {
                            if (targetItem.Quality >= 250)
                            {
                                maxbump = 5;
                            }
                            else if (targetItem.Quality >= 201)
                            {
                                maxbump = 4;
                            }
                            else if (targetItem.Quality >= 150)
                            {
                                maxbump = 3;
                            }
                            else if (targetItem.Quality >= 100)
                            {
                                maxbump = 2;
                            }
                            else if (targetItem.Quality >= 50)
                            {
                                maxbump = 1;
                            }
                        }

                        foreach (TradeSkillSkill tsSkill in ts.Skills)
                        {
                            if (tsSkill.SkillPerBump != 0)
                            {
                                leastbump =
                                    Math.Min(
                                        Convert.ToInt32(
                                            (client.Character.Stats[tsSkill.StatId].Value
                                             - (tsSkill.Percent / 100M * targetItem.Quality)) / tsSkill.SkillPerBump), 
                                        maxbump);
                            }
                        }

                        TradeSkillPacket.SendResult(
                            client.Character, 
                            targetItem.Quality, 
                            Math.Min(targetItem.Quality + leastbump, ItemLoader.ItemList[ts.ResultHighId].Quality), 
                            ts.ResultLowId, 
                            ts.ResultHighId);
                    }
                    else
                    {
                        TradeSkillPacket.SendOutOfRange(
                            client.Character, 
                            Convert.ToInt32(
                                Math.Round((double)targetItem.Quality - ts.QLRangePercent * targetItem.Quality / 100)));
                    }
                }
                else
                {
                    TradeSkillPacket.SendNotTradeskill(client.Character);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <param name="desiredQuality">
        /// </param>
        /// <param name="ts">
        /// </param>
        /// <param name="sourceItem">
        /// </param>
        /// <param name="targetItem">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool WindowBuild(
            ZoneClient client, 
            int desiredQuality, 
            TradeSkillEntry ts, 
            Item sourceItem, 
            Item targetItem)
        {
            if (!((ts.MinTargetQL >= targetItem.Quality) || (ts.MinTargetQL == 0)))
            {
                return false;
            }

            if (!ts.ValidateRange(sourceItem.Quality, targetItem.Quality))
            {
                return false;
            }

            foreach (TradeSkillSkill tss in ts.Skills)
            {
                if (client.Character.Stats[tss.StatId].Value < Convert.ToInt32(tss.Percent / 100M * targetItem.Quality))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}