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

    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    using ZoneEngine.Core.Controllers;
    using ZoneEngine.Core.KnuBot;
    using ZoneEngine.Script;

    #endregion

    public class KnuBotFlappy : BaseKnuBot
    {
        public KnuBotFlappy(Identity identity)
            : base(identity)
        {
            KnuBotDialogTree temp = new KnuBotDialogTree(
                "0",
                this.TreeCondition0,
                new[]
                {
                    // This is the main dialog (send text in here)
                    this.CAS(this.StartDialog, "self"), 
                    // This will be called on the first option
                    this.CAS(this.Option0_1, "01"),
                    // this will be called on the second option
                    this.CAS(this.Option0_2, "02")
                });

            // SetRootNode needs to be done after building the first node, so AddNode can put in neccesary information in subsequent nodes
            this.SetRootNode(temp);
            
            temp.AddNode(
                new KnuBotDialogTree(
                    "01",
                    this.TreeCondition01,
                    new[]
                    {
                        // This is the main dialog (send text in here)
                        this.CAS(this.Dialog1, "self"),
                        // This will be called on the first option
                        this.CAS(this.Option01_1, "root"),
                        // this will be called on the second option
                        this.CAS(this.Option01_2, "self")
                    }));
            temp.AddNode(
                new KnuBotDialogTree(
                    "02",
                    this.TreeCondition02,
                    new[]
                    {
                        // This sends only back to the root node
                        this.CAS(this.Dialog02, "root")
                    }));
        }

        #region Node 0

        private KnuBotAction TreeCondition0(KnuBotOptionId id)
        {
            switch (id)
            {
                case KnuBotOptionId.DialogStart:
                    return this.StartDialog;
                case KnuBotOptionId.Option1:
                    return this.Option0_1;
                case KnuBotOptionId.Option2:
                    return this.Option0_2;
            }
            return null;
        }

        private void StartDialog()
        {
            // Send blabla
            this.WriteLine("Knubot test script");
            this.WriteLine();
            this.WriteLine("Option 1 -> Node 01");
            this.WriteLine("Option 2 -> Node 02");
            this.WriteLine();
            this.SendAnswerList("Calls Option0_1", "Calls Option0_2");
        }

        private void Option0_1()
        {
            this.WriteLine("Option 0_1 text");
            this.WriteLine("Go to node 01");
            this.WriteLine();
        }

        private void Option0_2()
        {
            this.WriteLine("Option 0_2 text");
            this.WriteLine("Go to node 02");
            this.WriteLine();
        }

        #endregion

        #region Node 01

        private KnuBotAction TreeCondition01(KnuBotOptionId knuBotOptionId)
        {
            switch (knuBotOptionId)
            {
                case KnuBotOptionId.DialogStart:
                    return this.Dialog1;
                case KnuBotOptionId.Option1:
                    return this.Option01_1;
                case KnuBotOptionId.Option2:
                    return this.Option01_2;
            }
            return null;
        }

        private void Dialog1()
        {
            this.WriteLine("Dialog 01");
            this.WriteLine("Option 1 -> Node 0");
            this.WriteLine("Option 2 -> Node 01");
            this.WriteLine();
            this.SendAnswerList("Calls Option01_1", "Calls Option01_2");
        }

        private void Option01_1()
        {
            this.WriteLine("Option 01_1 text");
            this.WriteLine("Go to root node (0)");
            this.WriteLine();
        }

        private void Option01_2()
        {
            this.WriteLine("Option 01_2 text");
            this.WriteLine("Go to same node (01)");
            this.WriteLine();
        }

        #endregion

        #region Node 02

        private KnuBotAction TreeCondition02(KnuBotOptionId optionId)
        {
            return this.Dialog02;
        }

        private void Dialog02()
        {
            this.WriteLine("Dialog 02");
            this.WriteLine("This node only transfers back to root node");
            this.WriteLine("and it starts with DialogStart again");
            this.WriteLine();
        }

        #endregion
    }
}