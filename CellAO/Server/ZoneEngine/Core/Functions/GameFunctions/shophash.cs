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

namespace ZoneEngine.Core.Functions.GameFunctions
{
    #region Usings ...

    using CellAO.Core.Entities;
    using CellAO.Enums;
    using CellAO.Interfaces;

    using MsgPack;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using ZoneEngine.Core.MessageHandlers;

    #endregion

    public class shophash : FunctionPrototype
    {
        public override FunctionType FunctionId
        {
            get
            {
                return FunctionType.Shophash;
            }
        }

        public override bool Execute(
            INamedEntity self,
            IEntity caller,
            IInstancedEntity target,
            MessagePackObject[] arguments)
        {
            Vendor temp = caller as Vendor;
            if (temp != null)
            {
                if (temp.BaseInventory.Pages[temp.BaseInventory.StandardPage].List().Count == 0)
                {
                    if (temp.OriginalIdentity.Equals(Identity.None))
                    {
                    }
                    else
                    {
                        int id = temp.Playfield.Identity.Instance << 16
                                 | ((temp.OriginalIdentity.Instance >> 16) & 0xff);
                        ((ICharacter)self).Playfield.Publish(
                            ChatTextMessageHandler.Default.CreateIM(
                                ((ICharacter)self),
                                "This shop has no entry in the database yet. Please enter a new entry with the id "
                                + id.ToString() + "."));
                    }
                }
                else
                {
                    ShopUpdateMessageHandler.Default.Send((ICharacter)self, caller, ((Vendor)caller).BaseInventory.Pages[((Vendor)caller).BaseInventory.StandardPage]);
                }
            }
            return true;
        }
    }
}