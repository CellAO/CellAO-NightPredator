#region License

// Copyright (c) 2005-2014, CellAO Team
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

namespace ZoneEngine.Core.KnuBot
{
    #region Usings ...

    using System;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;

    using SmokeLounge.AOtomation.Messaging.GameData;

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
        public BaseKnuBot(Identity knubotIdentity, KnuBotDialogTree root)
            : this(knubotIdentity)
        {
            this.SetRootNode(root);
        }

        /// <summary>
        /// </summary>
        /// <param name="knubotIdentity">
        /// </param>
        public BaseKnuBot(Identity knubotIdentity)
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
        internal ICharacter GetCharacter()
        {
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
            if (this.GetCharacter() == null)
            {
                // OK, no one is talking, lets initialize
                result = true;
                this.Character.Target = character;
                this.selectedNode = this.rootNode;
                this.Answer(KnuBotOptionId.DialogStart);
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
            string nextId = this.selectedNode.Execute((KnuBotOptionId)answer);
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
    }
}