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

namespace ZoneEngine.Core.KnuBot
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using CellAO.Core.Entities;
    using CellAO.Core.Items;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    using ZoneEngine.Core.MessageHandlers;

    #endregion

    /// <summary>
    /// </summary>
    public class BaseKnuBot
    {
        /// <summary>
        /// </summary>
        public WeakReference<ICharacter> Character;

        /// <summary>
        /// </summary>
        public Identity KnuBotIdentity;

        /// <summary>
        /// </summary>
        private KnuBotDialogTree rootNode;

        /// <summary>
        /// </summary>
        private KnuBotDialogTree selectedNode;

        /// <summary>
        /// </summary>
        /// <param name="knubotIdentity">
        /// </param>
        /// <param name="root">
        /// </param>
        protected BaseKnuBot(Identity knubotIdentity, KnuBotDialogTree root)
            : this(knubotIdentity)
        {
            this.SetRootNode(root);
        }

        /// <summary>
        /// </summary>
        /// <param name="knubotIdentity">
        /// </param>
        protected BaseKnuBot(Identity knubotIdentity)
        {
            this.Character = new WeakReference<ICharacter>(null);
            this.KnuBotIdentity = knubotIdentity;
        }

        /// <summary>
        /// </summary>
        /// <param name="node">
        /// </param>
        protected void SetRootNode(KnuBotDialogTree node)
        {
            node.ValidateTree();
            this.rootNode = node;
            this.selectedNode = this.rootNode;
            this.rootNode.SetKnuBot(this);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public ICharacter GetCharacter()
        {
            /*            if (this.Character.Target == null)
                        {
                            throw new Exception("Character has gone away.");
                        }
                        */
            return this.Character.Target;
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <returns>
        /// </returns>
        public bool StartDialog(ICharacter character)
        {
            bool result = false;

            // Does the starting character exist?
            if (character != null)
            {
                // if (this.GetCharacter() == null)
                {
                    // OK, no one is talking, lets initialize
                    result = true;
                    this.Character.Target = character;
                    this.selectedNode = this.rootNode;
                    this.OpenWindow();
                    this.Answer(KnuBotOptionId.DialogStart);
                    LogUtil.Debug(DebugInfoDetail.KnuBot, string.Format("KnuBut Start Dialog"));
                }
            }

            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="id">
        /// </param>
        public void Answer(KnuBotOptionId id)
        {
            this.Answer((int)id);
        }

        /// <summary>
        /// </summary>
        /// <param name="answer">
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        public void Answer(int answer)
        {
            KnuBotDialogTree oldNode = this.selectedNode;
            // Only do talk if window is still open
            if (answer != (int)KnuBotOptionId.WindowClosed)
            {
                string nextId = this.selectedNode.Execute((KnuBotOptionId)answer);
                LogUtil.Debug(
                    DebugInfoDetail.KnuBot,
                    string.Format(
                        "Received KnuBot Answer {0} for node {1} -> {2}",
                        answer,
                        this.selectedNode.id,
                        nextId));
                if (nextId == "parent")
                {
                    this.selectedNode = this.selectedNode.Parent;
                }
                else
                {
                    if (nextId == "root")
                    {
                        this.selectedNode = this.rootNode;
                    }
                    else
                    {
                        if (nextId != "self")
                        {
                            KnuBotDialogTree nextSelectedNode = this.selectedNode.GetNode(nextId);
                            if (nextSelectedNode == null)
                            {
                                throw new Exception(
                                    "Could not find dialog id '" + nextId + "' in tree '"
                                    + string.Join(Environment.NewLine, this.selectedNode.FlattenDialogIds()) + "'");
                            }

                            this.selectedNode = nextSelectedNode;
                        }
                    }
                }

                // Only start over if its not the same node or option
                if (this.Character.Target != null)
                {
                    if ((answer != (int)KnuBotOptionId.DialogStart) || (oldNode != this.selectedNode))
                    {
                        this.Answer(KnuBotOptionId.DialogStart);
                    }
                }
            }
            else
            {
                // Remove link to conversation partner
                this.Character = new WeakReference<ICharacter>(null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <param name="nextId">
        /// </param>
        /// <returns>
        /// </returns>
        protected KnuBotActionStruct CAS(KnuBotAction action, string nextId)
        {
            return new KnuBotActionStruct() { ActionId = action.Method.Name, BotAction = action, NextDialogId = nextId };
        }

        /// <summary>
        /// </summary>
        /// <param name="choices">
        /// </param>
        protected void SendAnswerList(params string[] choices)
        {
            KnuBotAnswerListMessageHandler.Default.Send(this.GetCharacter(), this.KnuBotIdentity, choices);
            LogUtil.Debug(
                DebugInfoDetail.KnuBot,
                string.Format("Sending KnuBot Choice List ({0} choices)", choices.Length));
        }

        /// <summary>
        /// </summary>
        /// <param name="text">
        /// </param>
        protected void Write(string text)
        {
            KnuBotAppendTextMessageHandler.Default.Send(this.GetCharacter(), this.KnuBotIdentity, text);
            LogUtil.Debug(DebugInfoDetail.KnuBot, string.Format("KnuBut Write"));
            // Need to sleep here, else packets will be jumbled...
            Thread.Sleep(20);
        }

        protected void WriteLine(string text = "")
        {
            this.Write(text + "\n");
        }

        /// <summary>
        /// </summary>
        protected void OpenWindow()
        {
            KnuBotOpenChatWindowMessageHandler.Default.Send(this.GetCharacter(), this.KnuBotIdentity);
            LogUtil.Debug(DebugInfoDetail.KnuBot, "Opening KnuBot window");
        }

        /// <summary>
        /// </summary>
        /// <param name="items">
        /// </param>
        protected void RejectItems(IEnumerable<Item> items)
        {
            KnuBotRejectedItemsMessageHandler.Default.Send(this.GetCharacter(), this.KnuBotIdentity, items);
            LogUtil.Debug(DebugInfoDetail.KnuBot, string.Format("KnuBut Reject {0} items", items.Count()));
        }

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="numberOfSlots">
        /// </param>
        protected void StartTrade(string message, int numberOfSlots = 6)
        {
            KnuBotStartTradeMessageHandler.Default.Send(
                this.GetCharacter(),
                this.KnuBotIdentity,
                message,
                numberOfSlots);
            LogUtil.Debug(DebugInfoDetail.KnuBot, string.Format("KnuBut Start trade ({0} slots)", numberOfSlots));
        }

        /// <summary>
        /// </summary>
        /// <param name="item">
        /// </param>
        protected void Trade(IdentityType container, int slotNumber)
        {
            IItem temp = this.GetCharacter().BaseInventory.Pages[(int)container][slotNumber];
            // TODO: Remove item from Character's inventory and check against script reqs
            LogUtil.Debug(
                DebugInfoDetail.KnuBot,
                string.Format("KnuBut Trade item in container {0} slot {1}", container.ToString(), slotNumber));
        }

        public void CloseChatWindow()
        {
            KnuBotCloseChatWindowMessageHandler.Default.Send(this.Character.Target, this.KnuBotIdentity);
            this.Character = new WeakReference<ICharacter>(null);
            LogUtil.Debug(DebugInfoDetail.KnuBot, string.Format("Close KnuBot window"));
        }
    }
}