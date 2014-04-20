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

namespace ZoneEngine.Core.MessageHandlers
{
    #region Usings ...

    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Components;
    using CellAO.Core.Entities;
    using CellAO.Core.Textures;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    #endregion

    /// <summary>
    /// </summary>
    public class AppearanceUpdateMessageHandler :
        BaseMessageHandler<AppearanceUpdateMessage, AppearanceUpdateMessageHandler>
    {
        /// <summary>
        /// </summary>
        public AppearanceUpdateMessageHandler()
        {
            this.Direction = MessageHandlerDirection.OutboundOnly;
        }

        #region Outbound

        /// <summary>
        /// </summary>
        /// <param name="iCharacter">
        /// </param>
        /// <returns>
        /// </returns>
        private static MessageDataFiller Filler(ICharacter iCharacter)
        {
            return message =>
            {
                Character character = (Character)iCharacter;
                message.Identity = character.Identity;
                message.Unknown = 0;

                List<AOMeshs> meshs;
                int counter;

                bool socialonly;
                bool showsocial;

                /*
            bool showhelmet;
            bool LeftPadVisible;
            bool RightPadVisible;
            bool DoubleLeftPad;
            bool DoubleRightPad;

            int TexturesCount;
            int HairMeshValue;
            int WeaponMeshRightValue;
            int WeaponMeshLeftValue;

            uint HeadMeshBaseValue;
            int HeadMeshValue;

            int BackMeshValue;
            int ShoulderMeshRightValue;
             */
                // int ShoulderMeshRightValue;
                int VisualFlags;
                int PlayField;
                int bodyMesh = 0;

                /*
            int OverrideTextureHead;
            int OverrideTextureWeaponRight;
            int OverrideTextureWeaponLeft;
            int OverrideTextureShoulderpadRight;
            int OverrideTextureShoulderpadLeft;
            int OverrideTextureBack;
            int OverrideTextureAttractor;
            */
                var textures = new List<AOTextures>();

                var socialTab = new Dictionary<int, int>();

                lock (character)
                {
                    VisualFlags = character.Stats[StatIds.visualflags].Value;

                    socialonly = (VisualFlags & 0x40) > 0;
                    showsocial = (VisualFlags & 0x20) > 0;
                    bodyMesh = character.Stats[StatIds.mesh].Value;

                    /*
                showhelmet = ((character.Stats.VisualFlags.Value & 0x4) > 0);
                LeftPadVisible = ((character.Stats.VisualFlags.Value & 0x1) > 0);
                RightPadVisible = ((character.Stats.VisualFlags.Value & 0x2) > 0);
                DoubleLeftPad = ((character.Stats.VisualFlags.Value & 0x8) > 0);
                DoubleRightPad = ((character.Stats.VisualFlags.Value & 0x10) > 0);

                HairMeshValue = character.Stats.HairMesh.Value;
                TexturesCount = character.Textures.Count;
                HairMeshValue = character.Stats.HairMesh.Value;
                WeaponMeshRightValue = character.Stats.WeaponMeshRight.Value;
                WeaponMeshLeftValue = character.Stats.WeaponMeshLeft.Value;

                HeadMeshBaseValue = character.Stats.HeadMesh.StatBaseValue;
                HeadMeshValue = character.Stats.HeadMesh.Value;

                BackMeshValue = character.Stats.BackMesh.Value;
                ShoulderMeshRightValue = character.Stats.ShoulderMeshRight.Value;
                 */
                    // ShoulderMeshLeftValue = character.Stats.ShoulderMeshLeft.Value;
                    /*
                OverrideTextureHead = character.Stats.OverrideTextureHead.Value;
                OverrideTextureWeaponRight = character.Stats.OverrideTextureWeaponRight.Value;
                OverrideTextureWeaponLeft = character.Stats.OverrideTextureWeaponLeft.Value;
                OverrideTextureShoulderpadRight = character.Stats.OverrideTextureShoulderpadRight.Value;
                OverrideTextureShoulderpadLeft = character.Stats.OverrideTextureShoulderpadLeft.Value;
                OverrideTextureBack = character.Stats.OverrideTextureBack.Value;
                OverrideTextureAttractor = character.Stats.OverrideTextureAttractor.Value;
                */
                    PlayField = character.Playfield.Identity.Instance;

                    foreach (int num in character.SocialTab.Keys)
                    {
                        socialTab.Add(num, character.SocialTab[num]);
                    }

                    foreach (AOTextures texture in character.Textures)
                    {
                        textures.Add(new AOTextures(texture.place, texture.Texture));

                        // REFACT why recreating a AOTexture ???
                        // Because of thread safety, player could lose texture the same time? - Algorithman
                    }

                    meshs = MeshLayers.GetMeshs(character, showsocial, socialonly);
                }

                var texturesToSend = new List<Texture>();

                var aotemp = new AOTextures(0, 0);
                for (counter = 0; counter < 5; counter++)
                {
                    aotemp.Texture = 0;
                    aotemp.place = counter;
                    int c2;
                    for (c2 = 0; c2 < textures.Count; c2++)
                    {
                        if (textures[c2].place == counter)
                        {
                            aotemp.Texture = textures[c2].Texture;
                            break;
                        }
                    }

                    if (showsocial)
                    {
                        if (socialonly)
                        {
                            aotemp.Texture = socialTab[counter];
                        }
                        else
                        {
                            if (socialTab[counter] != 0)
                            {
                                aotemp.Texture = socialTab[counter];
                            }
                        }
                    }

                    texturesToSend.Add(new Texture { Place = aotemp.place, Id = aotemp.Texture, Unknown = 0 });
                }

                message.Textures = texturesToSend.ToArray();

                message.Meshes =
                    meshs.Select(
                        mesh =>
                            new Mesh
                            {
                                Position = (byte)mesh.Position, 
                                Id = (uint)mesh.Mesh, 
                                OverrideTextureId = mesh.OverrideTexture, 
                                Layer = (byte)mesh.Layer
                            }).ToArray();
                message.VisualFlags = (short)VisualFlags;
                message.Unknown1 = (byte)bodyMesh;
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        public void Send(ICharacter character)
        {
            this.Send(character, Filler(character), true);
        }

        #endregion
    }
}