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

namespace CellAO.Core.Textures
{
    #region Usings ...

    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Entities;
    using CellAO.Enums;

    #endregion

    /// <summary>
    /// </summary>
    public class MeshLayers
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly SortedList<int, int> mesh = new SortedList<int, int>();

        /// <summary>
        /// </summary>
        private readonly SortedList<int, int> meshOverride = new SortedList<int, int>();

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public SortedList<int, int> Mesh
        {
            get
            {
                return this.mesh;
            }
        }

        /// <summary>
        /// </summary>
        public SortedList<int, int> MeshOverride
        {
            get
            {
                return this.meshOverride;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="placement">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetLayer(int placement)
        {
            switch (placement)
            {
                    // Equipment pages
                case 18: // Head
                    return 0;
                case 19: // Back
                    return 4;
                case 20: // Shoulders
                case 22:
                    return 4;
                case 6: // Hands (Weapons)
                case 8:
                    return 4;

                    // Social page
                case 50: // Head
                    return 0;
                case 51: // Back
                    return 4;
                case 52: // Shoulders
                case 54:
                    return 4;
                case 56: // Hands (Weapons)
                case 58:
                    return 4;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <param name="showsocial">
        /// </param>
        /// <param name="socialonly">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<AOMeshs> GetMeshs(Character character, bool showsocial, bool socialonly)
        {
            List<AOMeshs> meshs;
            List<AOMeshs> socials;
            List<AOMeshs> output = new List<AOMeshs>();

            bool leftPadVisible;
            bool rightPadVisible;
            bool doubleLeftPad;
            bool doubleRightPad;

            lock (character)
            {
                meshs = character.MeshLayer.GetMeshs();
                socials = character.SocialMeshLayer.GetMeshs();

                int visualFlags = character.Stats[StatIds.visualflags].Value;
                rightPadVisible = (visualFlags & 0x1) > 0;
                leftPadVisible = (visualFlags & 0x2) > 0;
                bool showHelmet = (visualFlags & 0x4) > 0;
                doubleLeftPad = (visualFlags & 0x8) > 0;
                doubleRightPad = (visualFlags & 0x10) > 0;

                if (!showHelmet)
                {
                    if (meshs.ElementAt(0).Position == 0)
                    {
                        // Helmet there?
                        // This probably needs to be looked at (glasses/visors)
                        if (meshs.ElementAt(0).Mesh != character.Stats[StatIds.headmesh].BaseValue)
                        {
                            // Dont remove the head :)
                            meshs.RemoveAt(0);
                        }
                    }

                    if (socials.ElementAt(0).Position == 0)
                    {
                        // Helmet there?
                        // This probably needs to be looked at (glasses/visors)
                        if (socials.ElementAt(0).Mesh != character.Stats[StatIds.headmesh].BaseValue)
                        {
                            // Dont remove the head :)
                            socials.RemoveAt(0);
                        }
                    }
                }
            }

            socialonly &= showsocial; // Disable socialonly flag if showsocial is false

            // preapply visual flags
            if (socialonly)
            {
                meshs.Clear();
            }

            if ((!socialonly) && (!showsocial))
            {
                socials.Clear();
            }

            AOMeshs leftShoulder = null;
            AOMeshs rightShoulder = null;
            int rightShoulderNum = -1;
            int leftShoulderNum = -1;

            for (int counter1 = 0; counter1 < meshs.Count; counter1++)
            {
                AOMeshs m = meshs.ElementAt(counter1);
                if (m.Position == 3)
                {
                    if (!rightPadVisible)
                    {
                        meshs.RemoveAt(counter1);
                        counter1--;
                        continue;
                    }
                    else
                    {
                        rightShoulder = m;
                        rightShoulderNum = counter1;
                    }
                }

                if (m.Position == 4)
                {
                    if (!leftPadVisible)
                    {
                        meshs.RemoveAt(counter1);
                        counter1--;
                        continue;
                    }
                    else
                    {
                        leftShoulderNum = counter1;
                        leftShoulder = m;
                    }
                }
            }

            int counter;
            for (counter = 0; counter < 7; counter++)
            {
                AOMeshs cloth = null;
                AOMeshs social = null;

                foreach (AOMeshs aoMeshs in meshs)
                {
                    if (aoMeshs.Position == counter)
                    {
                        cloth = aoMeshs;
                        break;
                    }
                }

                foreach (AOMeshs aoMeshs in socials)
                {
                    if (aoMeshs.Position == counter)
                    {
                        social = aoMeshs;
                    }
                }

                if (social != null)
                {
                    if ((cloth != null) && (social != null))
                    {
                        // Compare layer only when both slots are set
                        if (cloth.Position == 0)
                        {
                            if (social.Mesh != character.Stats[StatIds.headmesh].BaseValue)
                            {
                                cloth = social;
                            }
                        }
                        else if (social.Layer - 8 < cloth.Layer)
                        {
                            if (cloth.Position == 5)
                            {
                                // Backmeshs act different
                                output.Add(cloth);
                            }

                            cloth = social;
                        }
                    }
                    else
                    {
                        if (showsocial || socialonly)
                        {
                            cloth = social;
                        }
                    }
                }

                if (cloth != null)
                {
                    output.Add(cloth);

                    // Moved check for Double pads here
                    if ((cloth.Position == 3) && doubleRightPad)
                    {
                        AOMeshs temp = new AOMeshs();
                        temp.Position = 4;
                        temp.Layer = cloth.Layer;
                        temp.Mesh = cloth.Mesh;
                        temp.OverrideTexture = cloth.OverrideTexture;
                        output.Add(temp);
                    }

                    if ((cloth.Position == 4) && doubleLeftPad)
                    {
                        AOMeshs temp = new AOMeshs();
                        temp.Position = 3;
                        temp.Layer = cloth.Layer;
                        temp.Mesh = cloth.Mesh;
                        temp.OverrideTexture = cloth.OverrideTexture;
                        output.Add(temp);
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// </summary>
        /// <param name="position">
        /// </param>
        /// <param name="meshToAdd">
        /// </param>
        /// <param name="overridetexture">
        /// </param>
        /// <param name="layer">
        /// </param>
        public void AddMesh(int position, int meshToAdd, int overridetexture, int layer)
        {
            int key = (position << 16) + layer;
            if (this.mesh.ContainsKey(key))
            {
                this.mesh[key] = meshToAdd;
            }
            else
            {
                this.mesh.Add(key, meshToAdd);
            }

            if (this.meshOverride.ContainsKey(key))
            {
                this.meshOverride[key] = overridetexture;
            }
            else
            {
                this.meshOverride.Add(key, overridetexture);
            }
        }

        /// <summary>
        /// </summary>
        public void Clear()
        {
            this.mesh.Clear();
            this.meshOverride.Clear();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int Count()
        {
            return this.mesh.Count;
        }

        /// <summary>
        /// </summary>
        /// <param name="pos">
        /// </param>
        /// <returns>
        /// </returns>
        public AOMeshs GetMeshAtPosition(int pos)
        {
            foreach (int key in this.mesh.Keys)
            {
                if ((key >> 16) == pos)
                {
                    // Just return mesh with highest priority (0 = highest)
                    AOMeshs aoMeshs = new AOMeshs();
                    aoMeshs.Layer = key & 0xffff;
                    aoMeshs.Position = key >> 16;
                    aoMeshs.Mesh = this.mesh[key];
                    aoMeshs.OverrideTexture = this.meshOverride[key];
                    return aoMeshs;
                }
            }

            // No mesh at this position found
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetMeshKey(int number)
        {
            return this.mesh.ElementAt(number).Key;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public List<AOMeshs> GetMeshs()
        {
            List<AOMeshs> meshList = new List<AOMeshs>();
            int counter;
            for (counter = 0; counter < this.mesh.Count; counter++)
            {
                AOMeshs aoMesh = new AOMeshs();
                aoMesh.Position = this.mesh.ElementAt(counter).Key >> 16;
                aoMesh.Mesh = this.mesh.ElementAt(counter).Value;
                aoMesh.OverrideTexture = this.meshOverride.ElementAt(counter).Value;
                aoMesh.Layer = this.mesh.ElementAt(counter).Key & 0xffff;
                meshList.Add(aoMesh);
            }

            return meshList;
        }

        /// <summary>
        /// </summary>
        /// <param name="position">
        /// </param>
        /// <param name="meshToRemove">
        /// </param>
        /// <param name="overridetexture">
        /// </param>
        /// <param name="layer">
        /// </param>
        public void RemoveMesh(int position, int meshToRemove, int overridetexture, int layer)
        {
            int key = (position << 16) + layer;
            this.mesh.Remove(key);
            this.meshOverride.Remove(key);
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        /// <returns>
        /// </returns>
        public int ReturnMesh(int number)
        {
            return this.mesh.ElementAt(number).Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        /// <returns>
        /// </returns>
        public int ReturnOverrideMesh(int number)
        {
            return this.meshOverride.ElementAt(number).Value;
        }

        #endregion
    }
}