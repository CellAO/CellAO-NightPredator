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

namespace ZoneEngine.Core.Playfields
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Core.Entities;
    using CellAO.Core.Playfields;
    using CellAO.Core.Vector;

    #endregion

    /// <summary>
    /// </summary>
    public static class WallCollision
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static float WallCollisionThreshold = 4;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="character">
        /// </param>
        /// <returns>
        /// </returns>
        public static WallCollisionResult CheckCollision(ICharacter character)
        {
            return CheckCollision(character.Coordinates, character.Playfield.Identity.Instance);
        }

        /// <summary>
        /// </summary>
        /// <param name="c">
        /// </param>
        /// <param name="playfieldId">
        /// </param>
        /// <returns>
        /// </returns>
        public static WallCollisionResult CheckCollision(Coordinate c, int playfieldId)
        {
            // get the coords local
            float x = c.x;
            float z = c.z;

            List<PlayfieldWalls> walls = PlayfieldLoader.PFData[playfieldId].Walls;
            foreach (PlayfieldWalls pfws in walls)
            {
                int wallsInPlayfield = pfws.Walls.Count;
                for (int i = 0; i < wallsInPlayfield; i++)
                {
                    if (MinimalDistance(pfws.Walls[i], pfws.Walls[(i + 1) % wallsInPlayfield], x, z)
                        < WallCollisionThreshold)
                    {
                        WallCollisionResult wcr = new WallCollisionResult();
                        wcr.FirstWall = pfws.Walls[i];
                        wcr.SecondWall = pfws.Walls[(i + 1) % wallsInPlayfield];
                        wcr.Factor = Distance(wcr.FirstWall, x, z)
                                     / Distance(wcr.FirstWall, wcr.SecondWall.X, wcr.SecondWall.Z);
                        return wcr;
                    }
                }
            }

            return null;
        }

        #endregion

        // Compute the dot product AB . AC

        // Compute the cross product AB x AC

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="w1">
        /// </param>
        /// <param name="w2">
        /// </param>
        /// <param name="x">
        /// </param>
        /// <param name="z">
        /// </param>
        /// <returns>
        /// </returns>
        private static float CrossProduct(PlayfieldWall w1, PlayfieldWall w2, float x, float z)
        {
            float[] AB = new float[2];
            float[] AC = new float[2];
            AB[0] = w2.X - w1.X;
            AB[1] = w2.Z - w1.Z;
            AC[0] = x - w1.X;
            AC[1] = z - w1.Z;
            float cross = AB[0] * AC[1] - AB[1] * AC[0];

            return cross;
        }

        /// <summary>
        /// </summary>
        /// <param name="w1">
        /// </param>
        /// <param name="x">
        /// </param>
        /// <param name="z">
        /// </param>
        /// <returns>
        /// </returns>
        private static float Distance(PlayfieldWall w1, float x, float z)
        {
            float d1 = w1.X - x;
            float d2 = w1.Z - z;

            return (float)Math.Sqrt(d1 * d1 + d2 * d2);
        }

        public static float Distance(float x1, float z1, float x2, float z2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (z1 - z2) * (z1 - z2));
        }
        /// <summary>
        /// </summary>
        /// <param name="w1">
        /// </param>
        /// <param name="w2">
        /// </param>
        /// <param name="x">
        /// </param>
        /// <param name="z">
        /// </param>
        /// <returns>
        /// </returns>
        private static float DotProduct(PlayfieldWall w1, PlayfieldWall w2, float x, float z)
        {
            float[] AB = new float[2];
            float[] BC = new float[2];
            AB[0] = w2.X - w1.X;
            AB[1] = w2.Z - w1.Z;
            BC[0] = x - w2.X;
            BC[1] = z - w2.Z;
            float dot = AB[0] * BC[0] + AB[1] * BC[1];

            return dot;
        }

        /// <summary>
        /// </summary>
        /// <param name="w1">
        /// </param>
        /// <param name="w2">
        /// </param>
        /// <param name="x">
        /// </param>
        /// <param name="z">
        /// </param>
        /// <returns>
        /// </returns>
        private static float MinimalDistance(PlayfieldWall w1, PlayfieldWall w2, float x, float z)
        {
            if (DotProduct(w1, w2, x, z) > 0)
            {
                return Distance(w2, x, z);
            }

            if (DotProduct(w2, w1, x, z) > 0)
            {
                return Distance(w1, x, z);
            }

            return Math.Abs(CrossProduct(w1, w2, x, z) / Distance(w1, w2.X, w2.Z));
        }

        #endregion
    }
}