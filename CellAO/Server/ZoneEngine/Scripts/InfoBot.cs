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

    public class InfoBot : IAOScript
    {
        public InfoBot()
        {
        }

        public void Main(string[] args)
        {
        }

        public void InitInfoBot(ICharacter character)
        {
            InfoBotKnu temp = new InfoBotKnu(character.Identity);
            var controller = character.Controller as NPCController;
            if (controller != null)
            {
                controller.SetKnuBot(temp);
                LogUtil.Debug(
                    DebugInfoDetail.Engine,
                    " Initialized Infobot with npc " + character.Identity.ToString(true));
            }
        }
    }

    public class InfoBotKnu : BaseKnuBot
    {
        public InfoBotKnu(Identity identity)
            : base(identity)
        {
            KnuBotDialogTree temp = new KnuBotDialogTree(
                "0",
                this.TreeCondition0,
                new[]
                {
                    // This is the main dialog
                    this.CAS(this.MainDialog, "self"), 
                    // This will be called if the player asks for commands in the main dialog
                    this.CAS(this.ChatCommands, "self"),
                    // this will be called if the player selects goodbye in the main dialog
                    this.CAS(this.GoodBye, "self")
                });

            // SetRootNode needs to be done after building the first node, so AddNode can put in neccesary information in subsequent nodes
            this.SetRootNode(temp);
        }

        #region Node 0

        private KnuBotAction TreeCondition0(KnuBotOptionId id)
        {
            switch (id)
            {
                case KnuBotOptionId.DialogStart:
                    return this.MainDialog;
                case KnuBotOptionId.Option1:
                    return this.ChatCommands;
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
            this.SendAnswerList("I need help with chat commands", "Bye");
        }

        private void ChatCommands()
        {
            this.WriteLine("A list of all commands: ");
            foreach (string command in ScriptCompiler.Instance.ChatCommands)
            {
                string formatedText = "";
                formatedText = command.Substring(command.IndexOf(":") + 1);

                this.WriteLine(formatedText);
            }
        }

        private void GoodBye()
        {
            // Exit Knubot
            this.WriteLine("test");
            this.CloseChatWindow();
        }

        #endregion
    }
}