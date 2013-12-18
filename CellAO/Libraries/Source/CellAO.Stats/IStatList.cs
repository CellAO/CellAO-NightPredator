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

namespace CellAO.Stats
{
    #region Usings ...

    using System;
    using System.Collections.Generic;

    using CellAO.Enums;
    using CellAO.Interfaces;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public interface IStatList : IDatabaseObject
    {
        #region Public Events

        /// <summary>
        /// </summary>
        event EventHandler<StatChangedEventArgs> AfterStatChangedEvent;

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        List<IStat> All { get; }

        /// <summary>
        /// </summary>
        GameTuple<CharacterStat, uint>[] ChangedAnnouncingStats { get; }

        /// <summary>
        /// </summary>
        GameTuple<CharacterStat, uint>[] ChangedStats { get; }

        /// <summary>
        /// </summary>
        Identity Owner { get; }

        #endregion

        #region Public Indexers

        /// <summary>
        /// Number-indexed access to Stats List
        /// </summary>
        /// <param name="index">
        /// Id of Stat
        /// </param>
        /// <returns>
        /// IStat object
        /// </returns>
        IStat this[int index] { get; }

        /// <summary>
        /// </summary>
        /// <param name="i">
        /// </param>
        /// <returns>
        /// </returns>
        IStat this[StatIds i] { get; }

        /// <summary>
        /// Name-indexed access to Stats List
        /// </summary>
        /// <param name="name">
        /// Name of the Stat
        /// </param>
        /// <returns>
        /// IStat object
        /// </returns>
        IStat this[string name] { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="e">
        /// </param>
        void AfterStatChangedEventHandler(StatChangedEventArgs e);

        /// <summary>
        /// </summary>
        void ClearChangedFlags();

        /// <summary>
        /// </summary>
        void ClearModifiers();

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        /// <returns>
        /// </returns>
        Stat GetStatByNumber(int number);

        #endregion
    }
}