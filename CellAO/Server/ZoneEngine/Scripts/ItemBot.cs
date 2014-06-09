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
namespace ZoneEngine.Scripts
{
    using CellAO.Core.Entities;
    using CellAO.Core.Items;
    using CellAO.Enums;
    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    using ZoneEngine.Core.Controllers;
    using ZoneEngine.Core.KnuBot;
    using ZoneEngine.Script;
    using ZoneEngine.Core.MessageHandlers;


    public class ItemBot : BaseKnuBot
    {
        private int ql = 0;

        public ItemBot(Identity identity)
            : base(identity)
        {
            KnuBotDialogTree temp = new KnuBotDialogTree("0", this.TreeCondition0, new[] {
                    this.CAS(this.MainDialog, "self"), 
                    this.CAS(this.Items, "01"), 
                    this.CAS(this.GoodBye, "self")
                });
            this.SetRootNode(temp);

            temp = temp.AddNode(new KnuBotDialogTree("01", this.TreeCondition2, new[] {
                    this.CAS(this.WantItem, "self"),
                    this.CAS(this.WantMiyMelee, "02"),
                    this.CAS(this.TooBad, "parent")

                }));
            temp = temp.AddNode(new KnuBotDialogTree("02", this.MiyMeleeQL, new[] {
                    this.CAS(this.QL, "self"),
                    this.CAS(this.QL1, "03"),
                    this.CAS(this.QL2, "03"),
                    this.CAS(this.QL3, "03"),
                    this.CAS(this.QL4, "03"),
                    this.CAS(this.QL5, "03"), 
                    this.CAS(this.TooBad, "parent")

                }));
            temp = temp.AddNode(new KnuBotDialogTree("03", this.MiyMelee, new[] {
                    this.CAS(this.GiveMiyMelee, "root")
                }));
        }


        #region node0

        private KnuBotAction TreeCondition0(KnuBotOptionId id)
        {
            switch (id)
            {
                case KnuBotOptionId.DialogStart:
                    return this.MainDialog;
                case KnuBotOptionId.Option1:
                    return this.Items;
                case KnuBotOptionId.Option2:
                    return this.GoodBye;
            }
            return null;
        }
        private void MainDialog()
        {
            // Hey
            this.WriteLine("Hello, " + this.GetCharacter().Name + " How may I help you today");
            this.WriteLine();
            this.SendAnswerList("I need a few items", "Nothing");
        }

        private void Items()
        {
        }

        private void GoodBye()
        {
            this.CloseChatWindow();
        }
        #endregion


        #region node2
        private KnuBotAction TreeCondition2(KnuBotOptionId id)
        {
            switch (id)
            {
                case KnuBotOptionId.DialogStart:
                    return this.WantItem;
                case KnuBotOptionId.Option1:
                    return this.WantMiyMelee;
                case KnuBotOptionId.Option2:
                    return this.TooBad;

            }
            return null;
        }
        void WantItem()
        {
            this.WriteLine("What items do you want?");
            this.SendAnswerList("Full Miy`s Melee", "I dont know yet");
        }

        void WantMiyMelee()
        {

        }

        void TooBad()
        {
            this.WriteLine("...");
        }

        #endregion


        #region SelectQLMiy

        private KnuBotAction MiyMeleeQL(KnuBotOptionId id)
        {
            switch (id)
            {
                case KnuBotOptionId.DialogStart:
                    return this.QL;
                case KnuBotOptionId.Option1:
                    return this.QL1;
                case KnuBotOptionId.Option2:
                    return this.QL2;
                case KnuBotOptionId.Option3:
                    return this.QL3;
                case KnuBotOptionId.Option4:
                    return this.QL4;
                case KnuBotOptionId.Option5:
                    return this.QL5;
                case KnuBotOptionId.Option6:
                    return this.TooBad;
            }
            return null;
        }
        void QL()
        {
            this.WriteLine("What QL?");
            this.SendAnswerList("50", "100", "150", "200", "250", "Other armor type please");
        }

        void QL1() { ql = 50; }
        void QL2() { ql = 100; }
        void QL3() { ql = 150; }
        void QL4() { ql = 200; }
        void QL5() { ql = 250; }


        #endregion

        #region MiyMelee
        private KnuBotAction MiyMelee(KnuBotOptionId id)
        {
            switch (id)
            {
                case KnuBotOptionId.DialogStart:
                    return this.GiveMiyMelee;
            }
            return null;
        }

        void GiveMiyMelee()
        {
            GiveItem(ql, 268850, 268851);
            GiveItem(ql, 268854, 268855);
            GiveItem(ql, 268854, 268855);
            GiveItem(ql, 268848, 268849);
            GiveItem(ql, 268852, 268853);
            GiveItem(ql, 270338, 270339);
            GiveItem(ql, 268858, 268850);
            GiveItem(ql, 268856, 268857);

        }


        #endregion


        // Give the player a item
        public void GiveItem(int qualityLevel, int lowID, int highID)
        {
            Item item = new Item(qualityLevel, lowID, highID);
            this.GetCharacter().BaseInventory.TryAdd(item);
            AddTemplateMessageHandler.Default.Send(this.GetCharacter(), item);

        }
    }
}