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

namespace ZoneEngine.Core.KnuBot
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public enum KnuBotOptionId
    {
        // Self made from other input
        /// <summary>
        /// </summary>
        FinishTrade = -3,

        /// <summary>
        /// </summary>
        StartTrade = -2,

        /// <summary>
        /// </summary>
        WindowClosed = -1,

        /// <summary>
        /// </summary>
        DialogStart = 99,

        // END Self made
        /// <summary>
        /// </summary>
        Option1 = 0,

        /// <summary>
        /// </summary>
        Option2 = 1,

        /// <summary>
        /// </summary>
        Option3 = 2,

        /// <summary>
        /// </summary>
        Option4 = 3,

        /// <summary>
        /// </summary>
        Option5 = 4,

        /// <summary>
        /// </summary>
        Option6 = 5,

        /// <summary>
        /// </summary>
        Option7 = 6,

        /// <summary>
        /// </summary>
        Option8 = 7,

        /// <summary>
        /// </summary>
        Option9 = 8,
        Option10 = 9,
        Option11 = 10,
        Option12 = 11,
        Option13 = 12,
        Option14 = 13,
        Option15 = 14,
        Option16 = 15,
        Option17 = 16,
        Option18 = 17,
        Option19 = 18,
        Option20 = 19,
        Option21 = 20,
        Option22 = 21,
        Option23 = 22,
        Option24 = 23,
        Option25 = 24,
        Option26 = 25,
        Option27 = 26,
        Option28 = 27,
        Option29 = 28,

    }

    /// <summary>
    /// Returns the id of the KnuBotAction to execute
    /// </summary>
    /// <param name="character">
    /// Character object to check on
    /// </param>
    public delegate KnuBotAction KnuBotCondition(KnuBotOptionId optionId);

    /// <summary>
    /// KnuBot Action (stepping into tree, stepping out of tree, executing game functions etc.)
    /// </summary>
    /// <param name="character">
    /// Character to apply to
    /// </param>
    /// <param name="knuBotIdentity">
    /// Identity of the KnuBot Dialog holder
    /// </param>
    public delegate void KnuBotAction();

    /// <summary>
    /// </summary>
    public class KnuBotDialogTree
    {
        /// <summary>
        /// </summary>
        private readonly List<KnuBotDialogTree> nodes = new List<KnuBotDialogTree>();

        /// <summary>
        /// </summary>
        private KnuBotDialogTree parent = null;

        /// <summary>
        /// </summary>
        private readonly KnuBotCondition condition;

        /// <summary>
        /// </summary>
        private BaseKnuBot knuBot = null;

        /// <summary>
        /// </summary>
        public readonly string id = string.Empty;

        /// <summary>
        /// </summary>
        private readonly List<KnuBotActionStruct> knuBotActions = new List<KnuBotActionStruct>();

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        /// <param name="condition">
        /// </param>
        /// <param name="actions">
        /// </param>
        public KnuBotDialogTree(string id, KnuBotCondition condition, IEnumerable<KnuBotActionStruct> actions)
        {
            this.id = id;
            this.condition = condition;
            this.knuBotActions.AddRange(actions);
        }

        /// <summary>
        /// </summary>
        public KnuBotDialogTree Parent
        {
            get
            {
                KnuBotDialogTree p;
                p = this.parent;
                return this.parent;
            }

            protected set
            {
                this.parent = value;
            }
        }

        /// <summary>
        /// </summary>
        private ICharacter Character
        {
            get
            {
                return this.GetBaseKnuBot().GetCharacter();
            }
        }

        /// <summary>
        /// </summary>
        private Identity KnuBotIdentity
        {
            get
            {
                return this.GetBaseKnuBot().KnuBotIdentity;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private BaseKnuBot GetBaseKnuBot()
        {
            // Get down to root
            KnuBotDialogTree temp = this;
            while (temp.Parent != null)
            {
                temp = temp.Parent;
            }

            BaseKnuBot knu = this.knuBot;
            if (knu == null)
            {
                throw new Exception("Base KnuBot gone away.");
            }

            return knu;
        }

        /// <summary>
        /// </summary>
        /// <param name="knu">
        /// </param>
        internal void SetKnuBot(BaseKnuBot knu)
        {
            this.knuBot = knu;
        }

        /// <summary>
        /// </summary>
        /// <param name="optionId">
        /// </param>
        /// <returns>
        /// </returns>
        public string Execute(KnuBotOptionId optionId = KnuBotOptionId.DialogStart)
        {
            string nextDialogId = string.Empty;
            if (this.Character != null)
            {
                KnuBotAction action = this.condition(optionId);
                if (action != null)
                {
                    string actionId = action.Method.Name;
                    if (actionId != string.Empty)
                    {
                        // Execute Action
                        // No checks for empty results here, since we did this all in the Validate method already
                        this.knuBotActions.Where(x => x.ActionId == actionId).Select(x => x.BotAction).First()();
                        nextDialogId = this.knuBotActions.First(x => x.ActionId == actionId).NextDialogId;
                    }
                }
            }

            return nextDialogId;
        }

        /// <summary>
        /// </summary>
        /// <param name="dialogTree">
        /// </param>
        /// <returns>
        /// </returns>
        public KnuBotDialogTree AddNode(KnuBotDialogTree dialogTree)
        {
            this.nodes.Add(dialogTree);
            dialogTree.Parent = this;
            dialogTree.SetKnuBot(this.GetBaseKnuBot());

            // Get down to root node
            KnuBotDialogTree temp = this;
            while (temp.Parent != null)
            {
                temp = temp.Parent;
            }

            // And lets validate the whole tree again
            // Better to do this on add, so we see from the stacktrace where it really happened
            temp.ValidateTree();

            return dialogTree;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public bool ValidateTree()
        {
            if (this.Parent != null)
            {
                throw new Exception("Please use the root node for validation only");
            }

            // This code will only be useable from the ROOT node!
            // It will throw errors if you run it from a deeper node, because of the data we want to put in!
            bool result = true;

            // Check for "loose ends"
            string[] nextDialogIds = this.FlattenNextDialogIds();

            /*
            // Check for double Id's
            if (nextDialogIds.GroupBy(n => n).Any(c => c.Count() > 1))
            {
                throw new Exception(
                    "Please check your Dialog Ids: " + Environment.NewLine
                    + string.Join(Environment.NewLine, nextDialogIds.GroupBy(n => n).Select(c => c.Count() > 1)));
            }
            */
            // Now check if there are actions pointing to a non existing Node
            // When creating the tree, this is perfectly fine. But there has to be a check in the end.
            string[] dialogIds = this.FlattenDialogIds();

            // Check for double Id's
            if (dialogIds.GroupBy(n => n).Any(c => c.Count() > 1))
            {
                throw new Exception(
                    "Please check your Dialog Ids: " + Environment.NewLine
                    + string.Join(Environment.NewLine, dialogIds.GroupBy(n => n).Select(c => c.Count() > 1)));
            }

            foreach (string dialogId in dialogIds)
            {
                // Checking for Dialogs which are not called
                result &= nextDialogIds.Any(c => c == dialogId);
            }

            // Check for nextDialogIds which are not in the dialogIds list
            foreach (string nextDialogId in nextDialogIds)
            {
                result &= dialogIds.Any(c => c == nextDialogId);
            }

            // Final check: parent calls in root node and empty action id's
            foreach (KnuBotActionStruct kbas in this.knuBotActions)
            {
                if (kbas.NextDialogId == "parent")
                {
                    throw new Exception("'parent' called from root node, huh?");
                }

                if (string.IsNullOrEmpty(kbas.ActionId))
                {
                    throw new Exception("Action id is null or empty.");
                }
            }

            // Tree traversal too? No, since circular would be perfectly fine (if there is an exit option)

            return result;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public string[] FlattenNextDialogIds()
        {
            var temp = new List<string>();
            foreach (KnuBotActionStruct kbas in this.knuBotActions)
            {
                // Discard "parent" and "root" identifiers, we check the root node for "parent" calls later
                if ((kbas.NextDialogId != "parent") && (kbas.NextDialogId != "root") && (kbas.NextDialogId != "self"))
                {
                    temp.Add(kbas.NextDialogId);
                }
            }

            foreach (KnuBotDialogTree dlg in this.nodes)
            {
                temp.AddRange(dlg.FlattenNextDialogIds());
            }

            return temp.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public string[] FlattenDialogIds()
        {
            // We ONLY collect children here
            // Since the root node is never called by a action, only by the initial KnuBotStart call
            // Parents are called by the keyword "parent"
            var temp = new List<string>();
            foreach (KnuBotDialogTree dlg in this.nodes)
            {
                temp.Add(dlg.id);
                temp.AddRange(dlg.FlattenDialogIds());
            }

            return temp.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="nextId">
        /// </param>
        /// <returns>
        /// </returns>
        internal KnuBotDialogTree GetNode(string nextId)
        {
            return this.nodes.FirstOrDefault(x => x.id == nextId);
        }
    }

    /// <summary>
    /// </summary>
    public struct KnuBotActionStruct
    {
        /// <summary>
        /// </summary>
        public string ActionId;

        /// <summary>
        /// </summary>
        public KnuBotAction BotAction;

        /// <summary>
        /// </summary>
        public string NextDialogId;
    }
}