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

    #endregion

    /// <summary>
    /// </summary>
    public interface IStat
    {
        #region Public Events

        /// <summary>
        /// </summary>
        event EventHandler<StatChangedEventArgs> AfterStatChangedEvent;

        /// <summary>
        /// </summary>
        event EventHandler<StatChangedEventArgs> BeforeStatChangedEvent;

        /// <summary>
        /// </summary>
        event EventHandler<StatChangedEventArgs> CalculateStatEvent;

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        bool AnnounceToPlayfield { get; set; }

        /// <summary>
        /// </summary>
        uint BaseValue { get; set; }

        /// <summary>
        /// </summary>
        bool Changed { get; set; }

        /// <summary>
        /// </summary>
        int Modifier { get; set; }

        /// <summary>
        /// </summary>
        int PercentageModifier { get; set; }

        /// <summary>
        /// </summary>
        int StatId { get; }

        /// <summary>
        /// </summary>
        IStatList Stats { get; }

        /// <summary>
        /// </summary>
        int Trickle { get; set; }

        /// <summary>
        /// </summary>
        int Value { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        void CalcTrickle();

        /// <summary>
        /// </summary>
        /// <param name="old">
        /// </param>
        /// <returns>
        /// </returns>
        uint GetMaxValue(uint old);

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="starting">
        /// </param>
        void Set(uint value, bool starting = false);

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        void SetBaseValue(uint value);

        #endregion
    }
}