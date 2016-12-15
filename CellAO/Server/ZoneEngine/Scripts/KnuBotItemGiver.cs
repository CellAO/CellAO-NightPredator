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

namespace ZoneEngine.Scripts
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Items;
    using CellAO.Database.Dao;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    using ZoneEngine.Core.KnuBot;
    using ZoneEngine.Core.MessageHandlers;

    #endregion

    public class KnuBotItemGiver : BaseKnuBot
    {
        private readonly List<ItemSet> RKArmorSets = new List<ItemSet>();

        private readonly List<ItemSet> SLArmorSets = new List<ItemSet>();

        public KnuBotItemGiver(Identity identity)
            : base(identity)
        {
            this.InitializeItemSets();

            KnuBotDialogTree rootNode = new KnuBotDialogTree(
                "0",
                this.Condition0,
                new[]
                {
                    this.CAS(this.DialogGM0, "self"), this.CAS(this.TransferToRKArmorSet, "RKArmorSet"),
                    this.CAS(this.TransferToSLArmorSet, "SLArmorSet"), this.CAS(this.GoodBye, "self")
                });
            this.SetRootNode(rootNode);

            KnuBotDialogTree lastNode =
                rootNode.AddNode(
                    new KnuBotDialogTree(
                        "RKArmorSet",
                        this.Condition01,
                        new[]
                        {
                            this.CAS(this.DialogShowRKArmorSets, "self"),
                            this.CAS(this.ChooseQlFromSet, "QLChoiceRKArmorSet"), this.CAS(this.BackToRoot, "root")
                        }));

            lastNode.AddNode(
                new KnuBotDialogTree(
                    "QLChoiceRKArmorSet",
                    this.QLCondition,
                    new[]
                    {
                        this.CAS(this.ShowQLs, "self"), this.CAS(this.GiveItemSet, "root"),
                        this.CAS(this.BackToRoot, "parent")
                    }));

            lastNode =
                rootNode.AddNode(
                    new KnuBotDialogTree(
                        "SLArmorSet",
                        this.ConditionSL,
                        new[]
                        {
                            this.CAS(this.DialogShowSLArmorSets, "self"),
                            this.CAS(this.ChooseQlFromSet, "QLChoiceSLArmorSet"), this.CAS(this.BackToRoot, "root")
                        }));

            lastNode.AddNode(
                new KnuBotDialogTree(
                    "QLChoiceSLArmorSet",
                    this.QLCondition,
                    new[]
                    {
                        this.CAS(this.ShowQLs, "self"), this.CAS(this.GiveItemSet, "root"),
                        this.CAS(this.BackToRoot, "parent")
                    }));
        }

        private void InitializeItemSets()
        {
            // Some RK Armor sets
            this.RKArmorSets.Add(new ItemSet(162431, 162429, 162429, 162435, 162427, 162426, 162433));
            this.RKArmorSets.Add(new ItemSet(208286, 208284, 208284, 208288, 208290, 208292, 208294));
            this.RKArmorSets.Add(new ItemSet(208255, 208253, 208253, 208257, 208259, 208261, 208263));
            this.RKArmorSets.Add(new ItemSet(164997, 165005, 165005, 164999, 165001, 165007, 165003));
            this.RKArmorSets.Add(new ItemSet(245964, 245972, 245972, 245966, 245968, 245974, 245970));
            this.RKArmorSets.Add(new ItemSet(245123, 245119, 245119, 245125, 245125, 245122, 245118, 245124, 245120));
            this.RKArmorSets.Add(new ItemSet(164816, 164800, 164800, 164810, 164808, 164819, 164812));
            this.RKArmorSets.Add(new ItemSet(163945, 163943, 163943, 163941, 163947, 163949, 163951));
            this.RKArmorSets.Add(new ItemSet(205951, 205954, 205954, 205955, 205953, 205950, 205952));
            this.RKArmorSets.Add(new ItemSet(268850, 268854, 268854, 268858, 270338, 268852, 268848, 268856));
            this.RKArmorSets.Add(new ItemSet(245177, 245175, 245175, 245185, 245185, 245183, 245179, 245187, 245181));

            // Some SL Armor sets
            // Bellum Badonis
            this.SLArmorSets.Add(new ItemSet(245891, 245880, 245880, 260681, 260680, 260680, 245889, 245884, 245866));
            // Conscientious Knight
            this.SLArmorSets.Add(new ItemSet(225542, 225543, 225543, 225546, 225546, 225545, 225544, 257115));
            // Supply Unit Armor
            this.SLArmorSets.Add(new ItemSet(215462, 215470, 215470, 215472, 215476, 215466, 215468, 215474));
        }

        #region Root Node "0"

        private bool isGM = false;

        private KnuBotAction Condition0(KnuBotOptionId id)
        {
            this.isGM = this.GetCharacter().Stats[StatIds.gmlevel].Value > 0;

            switch (id)
            {
                case KnuBotOptionId.DialogStart:
                    return this.DialogGM0;
                case KnuBotOptionId.Option1:
                    return this.TransferToRKArmorSet;
                case KnuBotOptionId.Option2:
                    return this.TransferToSLArmorSet;
                case KnuBotOptionId.Option3:
                    return this.GoodBye;
            }

            return null;
        }

        private void TransferToSLArmorSet()
        {
            this.WriteLine("Shadowlands Armor sets:");
        }

        private void DialogGM0()
        {
            string line = "Hi " + (isGM ? "<font color=#FF0000>GM</font> " : "") + this.GetCharacter().Name;
            this.WriteLine(line);
            this.WriteLine();
            this.WriteLine("How may i help you?");

            this.SendAnswerList("Rubi-Ka Armor sets", "Shadowland Armor sets", "Good bye");
        }

        private void TransferToRKArmorSet()
        {
            this.WriteLine("Rubi-Ka Armor sets:");
        }

        private void GoodBye()
        {
            this.WriteLine("Good bye");
            this.CloseChatWindow();
        }

        #endregion

        #region Node 01 (Choose RK Armor set)

        private ItemSet selectedSet = null;

        private KnuBotAction Condition01(KnuBotOptionId id)
        {
            if (id == KnuBotOptionId.DialogStart)
            {
                return this.DialogShowRKArmorSets;
            }

            if (((int)id >= 0) && ((int)id < this.RKArmorSets.Count))
            {
                this.selectedSet = this.RKArmorSets[(int)id];
                return this.ChooseQlFromSet;
            }
            if ((int)id == this.RKArmorSets.Count)
            {
                return this.BackToRoot;
            }
            // Error if you encounter this line!
            return this.BackToRoot;
        }

        private void DialogShowRKArmorSets()
        {
            string[] options = this.RKArmorSets.Select(x => x.GetIconAndName()).ToArray();

            foreach (string s in options)
            {
                this.WriteLine(s);
            }

            List<string> temp = new List<string>();
            temp.AddRange(this.RKArmorSets.Select(x => x.GetName()).ToArray());
            temp.Add("Back");

            this.SendAnswerList(temp.ToArray());
        }

        private void BackToRoot()
        {
            this.WriteLine("Too bad...");
            this.WriteLine();
        }

        private void ChooseQlFromSet()
        {
        }

        #endregion

        #region Node QLChoiceRKArmorSet

        private int selectedQL = 0;

        private KnuBotAction QLCondition(KnuBotOptionId id)
        {
            if (id == KnuBotOptionId.DialogStart)
            {
                return this.ShowQLs;
            }
            int[] qls = this.selectedSet.GetQLs();
            if (((int)id >= 0) && ((int)id < qls.Length))
            {
                this.selectedQL = qls[(int)id];
                return this.GiveItemSet;
            }
            if ((int)id == qls.Length)
            {
                return this.BackToRoot;
            }
            return null;
        }

        private void ShowQLs()
        {
            List<string> temp = new List<string>();
            string[] qls = this.selectedSet.GetQLs().Select(x => x.ToString()).ToArray();
            temp.AddRange(qls);
            temp.Add("Back");
            this.SendAnswerList(temp.ToArray());
        }

        private void GiveItem(int qualityLevel, int id)
        {
            int lowId = ItemLoader.ItemList[id].GetLowId(qualityLevel);
            int highId = ItemLoader.ItemList[id].GetHighId(qualityLevel);
            Item item = new Item(qualityLevel, lowId, highId);
            if (this.GetCharacter().BaseInventory.TryAdd(item) == InventoryError.OK)
            {
                AddTemplateMessageHandler.Default.Send(this.GetCharacter(), item);
            }
        }

        private void GiveItemSet()
        {
            foreach (int id in this.selectedSet.ItemIds)
            {
                this.GiveItem(this.selectedQL, id);
            }
        }

        #endregion

        #region SL Armor sets

        private void DialogShowSLArmorSets()
        {
            string[] options = this.SLArmorSets.Select(x => x.GetIconAndName()).ToArray();

            foreach (string s in options)
            {
                this.WriteLine(s);
            }

            List<string> temp = new List<string>();
            temp.AddRange(this.SLArmorSets.Select(x => x.GetName()).ToArray());
            temp.Add("Back");

            this.SendAnswerList(temp.ToArray());
        }

        private KnuBotAction ConditionSL(KnuBotOptionId id)
        {
            if (id == KnuBotOptionId.DialogStart)
            {
                return this.DialogShowSLArmorSets;
            }

            if (((int)id >= 0) && ((int)id < this.SLArmorSets.Count))
            {
                this.selectedSet = this.SLArmorSets[(int)id];
                return this.ChooseQlFromSet;
            }
            if ((int)id == this.SLArmorSets.Count)
            {
                return this.BackToRoot;
            }
            // Error if you encounter this line!
            return this.BackToRoot;
        }

        #endregion

        public class ItemSet
        {
            private string setName = "";

            public List<int> ItemIds = new List<int>();

            public ItemSet(params int[] ids)
            {
                foreach (int id in ids)
                {
                    if (ItemLoader.ItemList.ContainsKey(id))
                    {
                        this.ItemIds.Add(id);
                    }
                    else
                    {
                        Exception e = new Exception("Script initilization error, item with Id " + id + " not found.");
                        LogUtil.ErrorException(e);
                        throw e;
                    }
                }
            }

            public int GetMinQl()
            {
                int minQl = 0;
                foreach (int id in this.ItemIds)
                {
                    Item temp = new Item(1, id, id);
                    minQl = minQl < temp.Quality ? temp.Quality : minQl;
                }
                return minQl;
            }

            public int GetMaxQl()
            {
                int maxQl = 400;
                foreach (int id in this.ItemIds)
                {
                    Item temp = new Item(400, id, ItemLoader.ItemList[id].Relations.Last());
                    maxQl = maxQl > temp.Quality ? temp.Quality : maxQl;
                }
                return maxQl;
            }

            public string GetName()
            {
                int id = this.ItemIds[0];
                if (this.setName == "")
                {
                    this.setName = ItemNamesDao.Instance.Get(id).Name;
                }
                return this.setName;
            }

            public string GetIconAndName()
            {
                string name = this.GetName();
                return string.Format(
                    "<img src=rdb://{0}> {1}",
                    ItemLoader.ItemList[this.ItemIds[0]].getItemAttribute((int)StatIds.icon),
                    name);
            }

            public int[] GetQLs()
            {
                int startQL = this.GetMinQl();
                int endQL = this.GetMaxQl();
                List<int> temp = new List<int>();
                while (startQL < endQL)
                {
                    temp.Add(startQL);

                    // Align to 25ish qls
                    if (startQL == 1)
                    {
                        startQL--;
                    }
                    startQL += 25;
                }
                temp.Add(endQL);
                return temp.ToArray();
            }
        }
    }
}