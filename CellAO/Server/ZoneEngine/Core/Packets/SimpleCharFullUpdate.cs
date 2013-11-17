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

namespace ZoneEngine.Core.Packets
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Entities;
    using CellAO.Core.Nanos;
    using CellAO.Core.Textures;
    using CellAO.Core.Vector;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    using Quaternion = CellAO.Core.Vector.Quaternion;
    using Vector3 = SmokeLounge.AOtomation.Messaging.GameData.Vector3;

    #endregion

    /// <summary>
    /// </summary>
    public static class SimpleCharFullUpdate
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <returns>
        /// </returns>
        public static SimpleCharFullUpdateMessage ConstructMessage(Character character)
        {
            // Character Variables
            bool socialonly;
            bool showsocial;

            int charPlayfield;
            Coordinate charCoord;
            Identity charId;
            Quaternion charHeading;

            uint sideValue;
            uint fatValue;
            uint breedValue;
            uint sexValue;
            uint raceValue;

            string charName;
            int charFlagsValue;
            int accFlagsValue;

            int expansionValue;
            int currentNano;
            int currentHealth;

            uint strengthBaseValue;
            uint staminaBaseValue;
            uint agilityBaseValue;
            uint senseBaseValue;
            uint intelligenceBaseValue;
            uint psychicBaseValue;

            string firstName;
            string lastName;
            int orgNameLength;
            string orgName;
            int levelValue;
            int healthValue;
            int losHeight;

            int monsterData;
            int monsterScale;
            int visualFlags;

            int currentMovementMode;
            uint runSpeedBaseValue;

            int texturesCount;

            int headMeshValue;

            // NPC Values
            int NPCFamily;

            var socialTab = new Dictionary<int, int>();

            var textures = new List<AOTextures>();

            List<AOMeshs> meshs;

            var nanos = new List<AONano>();

            lock (character)
            {
                socialonly = (character.Stats["VisualFlags"].Value & 0x40) > 0;
                showsocial = (character.Stats["VisualFlags"].Value & 0x20) > 0;

                charPlayfield = character.Playfield.Identity.Instance;
                charCoord = character.Coordinates;
                charId = character.Identity;
                charHeading = character.Heading;

                sideValue = character.Stats["Side"].BaseValue;
                fatValue = character.Stats["Fatness"].BaseValue;
                breedValue = character.Stats["Breed"].BaseValue;
                sexValue = character.Stats["Sex"].BaseValue;
                raceValue = character.Stats["Race"].BaseValue;

                charName = character.Name;
                charFlagsValue = character.Stats["Flags"].Value;
                accFlagsValue = character.Stats["AccountFlags"].Value;

                expansionValue = character.Stats["Expansion"].Value;
                currentNano = character.Stats["CurrentNano"].Value;

                strengthBaseValue = character.Stats["Strength"].BaseValue;
                staminaBaseValue = character.Stats["Strength"].BaseValue;
                agilityBaseValue = character.Stats["Strength"].BaseValue;
                senseBaseValue = character.Stats["Strength"].BaseValue;
                intelligenceBaseValue = character.Stats["Strength"].BaseValue;
                psychicBaseValue = character.Stats["Strength"].BaseValue;

                firstName = character.FirstName;
                lastName = character.LastName;
                orgNameLength = character.OrganizationName.Length;
                orgName = character.OrganizationName;
                levelValue = character.Stats["Level"].Value;
                healthValue = character.Stats["Life"].Value;

                monsterData = character.Stats["MonsterData"].Value;
                monsterScale = character.Stats["MonsterScale"].Value;
                visualFlags = character.Stats["VisualFlags"].Value;

                currentMovementMode = character.Stats["CurrentMovementMode"].Value;
                runSpeedBaseValue = character.Stats["RunSpeed"].BaseValue;

                texturesCount = character.Textures.Count;

                headMeshValue = character.Stats["HeadMesh"].Value;

                foreach (int num in character.SocialTab.Keys)
                {
                    socialTab.Add(num, character.SocialTab[num]);
                }

                foreach (AOTextures at in character.Textures)
                {
                    textures.Add(new AOTextures(at.place, at.Texture));
                }

                meshs = MeshLayers.GetMeshs(character, showsocial, socialonly);

                foreach (AONano nano in character.ActiveNanos)
                {
                    var tempNano = new AONano();
                    tempNano.ID = nano.ID;
                    tempNano.Instance = nano.Instance;
                    tempNano.NanoStrain = nano.NanoStrain;
                    tempNano.Nanotype = nano.Nanotype;
                    tempNano.Time1 = nano.Time1;
                    tempNano.Time2 = nano.Time2;
                    tempNano.Value3 = nano.Value3;

                    nanos.Add(tempNano);
                }

                losHeight = character.Stats["LosHeight"].Value;
                NPCFamily = character.Stats["NpcFamily"].Value;
                currentHealth = character.Stats["Health"].Value;
            }

            var scfu = new SimpleCharFullUpdateMessage();

            // affected identity
            scfu.Identity = charId;

            scfu.Version = 57; // SCFU packet version (57/0x39)
            scfu.Flags = SimpleCharFullUpdateFlags.None; // Try setting to 0x042062C8 if you have problems (old value)
            scfu.Flags |= SimpleCharFullUpdateFlags.HasPlayfieldId; // Has Playfield ID
            scfu.PlayfieldId = charPlayfield; // playfield

            if (character.FightingTarget.Instance != 0)
            {
                scfu.Flags |= SimpleCharFullUpdateFlags.HasFightingTarget;
                scfu.FightingTarget = new Identity
                                      {
                                          Type = character.FightingTarget.Type, 
                                          Instance = character.FightingTarget.Instance
                                      };
            }

            // Coordinates
            scfu.Coordinates = new Vector3 { X = charCoord.x, Y = charCoord.y, Z = charCoord.z };

            // Heading Data
            scfu.Flags |= SimpleCharFullUpdateFlags.HasHeading;
            scfu.Heading = new SmokeLounge.AOtomation.Messaging.GameData.Quaternion
                           {
                               W = charHeading.wf, 
                               X = charHeading.xf, 
                               Y = charHeading.yf, 
                               Z = charHeading.zf
                           };

            // Race
            scfu.Appearance = new Appearance
                              {
                                  Side = (Side)sideValue, 
                                  Fatness = (Fatness)fatValue, 
                                  Breed = (Breed)breedValue, 
                                  Gender = (Gender)sexValue, 
                                  Race = raceValue
                              }; // appearance

            // Name
            scfu.Name = charName;

            scfu.CharacterFlags = (CharacterFlags)charFlagsValue; // Flags
            scfu.AccountFlags = (short)accFlagsValue;
            scfu.Expansions = (short)expansionValue;

            bool isNpc = character is INonPlayerCharacter;

            if (isNpc)
            {
                // Are we a NPC (i think anyway)? So far this is _NOT_ used at all
                scfu.Flags |= SimpleCharFullUpdateFlags.IsNpc;

                var snpc = new SimpleNpcInfo { Family = (short)NPCFamily, LosHeight = (short)losHeight };
                scfu.CharacterInfo = snpc;
            }
            else
            {
                // Are we a player?
                var spc = new SimplePcInfo();

                spc.CurrentNano = (uint)currentNano; // CurrentNano
                spc.Team = 0; // team?
                spc.Swim = 5; // swim?

                // The checks here are to prevent the client doing weird things if the character has really large or small base attributes
                spc.StrengthBase = (short)Math.Min(strengthBaseValue, short.MaxValue); // Strength
                spc.AgilityBase = (short)Math.Min(agilityBaseValue, short.MaxValue); // Agility
                spc.StaminaBase = (short)Math.Min(staminaBaseValue, short.MaxValue); // Stamina
                spc.IntelligenceBase = (short)Math.Min(intelligenceBaseValue, short.MaxValue); // Intelligence
                spc.SenseBase = (short)Math.Min(senseBaseValue, short.MaxValue); // Sense
                spc.PsychicBase = (short)Math.Min(psychicBaseValue, short.MaxValue); // Psychic

                if (scfu.CharacterFlags.HasFlag(CharacterFlags.HasVisibleName))
                {
                    // has visible names? (Flags)
                    spc.FirstName = firstName;
                    spc.LastName = lastName;
                }

                if (orgNameLength != 0)
                {
                    scfu.Flags |= SimpleCharFullUpdateFlags.HasOrgName; // Has org name data
                    spc.OrgName = orgName;
                }

                scfu.CharacterInfo = spc;
            }

            // Level
            scfu.Level = (short)levelValue;
            if (scfu.Level > sbyte.MaxValue)
            {
                scfu.Flags |= SimpleCharFullUpdateFlags.HasExtendedLevel;
            }

            // Health
            scfu.Health = (uint)healthValue;
            if (scfu.Health <= short.MaxValue)
            {
                scfu.Flags |= SimpleCharFullUpdateFlags.HasSmallHealth;
            }

            scfu.HealthDamage = healthValue - currentHealth;
            if (scfu.HealthDamage <= byte.MaxValue)
            {
                scfu.Flags |= SimpleCharFullUpdateFlags.HasSmallHealthDamage;
            }

            // If player is in grid or fixer grid
            // make him/her/it a nice upside down pyramid
            if ((charPlayfield == 152) || (charPlayfield == 4107))
            {
                scfu.MonsterData = 99902;
            }
            else
            {
                scfu.MonsterData = (uint)monsterData; // Monsterdata
            }

            scfu.MonsterScale = (short)monsterScale; // Monsterscale
            scfu.VisualFlags = (short)visualFlags; // VisualFlags
            scfu.VisibleTitle = 0; // visible title?

            // 42 bytes long
            scfu.Unknown1 = new byte[]
                            {
                                0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 
                                (byte)currentMovementMode, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 
                                0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                                0x00, 0x00, 0x00, 0x00
                            };

            if (headMeshValue != 0)
            {
                scfu.Flags |= SimpleCharFullUpdateFlags.HasHeadMesh; // Has HeadMesh Flag
                scfu.HeadMesh = (uint?)headMeshValue; // Headmesh
            }

            // Runspeed
            scfu.RunSpeedBase = (short)runSpeedBaseValue;
            if (runSpeedBaseValue > sbyte.MaxValue)
            {
                scfu.Flags |= SimpleCharFullUpdateFlags.HasExtendedRunSpeed;
            }

            scfu.ActiveNanos = (from nano in nanos
                select
                    new ActiveNano
                    {
                        NanoId = nano.ID, 
                        NanoInstance = nano.Instance, 
                        Time1 = nano.Time1, 
                        Time2 = nano.Time2
                    }).ToArray();

            // Texture/Cloth Data
            var scfuTextures = new List<Texture>();

            var aotemp = new AOTextures(0, 0);
            for (int c = 0; c < 5; c++)
            {
                aotemp.Texture = 0;
                aotemp.place = c;
                for (int c2 = 0; c2 < texturesCount; c2++)
                {
                    if (textures[c2].place != c)
                    {
                        continue;
                    }

                    aotemp.Texture = textures[c2].Texture;
                    break;
                }

                if (showsocial)
                {
                    if (socialonly)
                    {
                        aotemp.Texture = socialTab[c];
                    }
                    else
                    {
                        if (socialTab[c] != 0)
                        {
                            aotemp.Texture = socialTab[c];
                        }
                    }
                }

                scfuTextures.Add(new Texture { Place = aotemp.place, Id = aotemp.Texture, Unknown = 0 });
            }

            scfu.Textures = scfuTextures.ToArray();

            // End Textures

            // ############
            // # Meshs
            // ############
            scfu.Meshes = (from aoMesh in meshs
                select
                    new Mesh
                    {
                        Position = (byte)aoMesh.Position, 
                        Id = (uint)aoMesh.Mesh, 
                        OverrideTextureId = aoMesh.OverrideTexture, 
                        Layer = (byte)aoMesh.Layer
                    }).ToArray();

            // End Meshs
            scfu.Flags2 = 0; // packetFlags2
            scfu.Unknown2 = 0;

            return scfu;
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        /// <returns>
        /// </returns>
        public static SimpleCharFullUpdateMessage ConstructMessage(ZoneClient client)
        {
            return ConstructMessage(client.Character);
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="receiver">
        /// </param>
        public static void SendToOne(Character character, ZoneClient receiver)
        {
            SimpleCharFullUpdateMessage message = ConstructMessage(character);
            receiver.SendCompressed(message);
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        public static void SendToPlayfield(ZoneClient client)
        {
            SimpleCharFullUpdateMessage message = ConstructMessage(client);
            client.Character.Playfield.Announce(message);
        }

        #endregion
    }
}