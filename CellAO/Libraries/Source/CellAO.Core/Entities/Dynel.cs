#region License

// Copyright (c) 2005-2013, CellAO Team
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
// Last modified: 2013-10-30 22:52
// Created:       2013-10-30 21:43

#endregion

namespace CellAO.Core.Entities
{
    #region Usings ...

    using CellAO.Stats;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    public partial class Dynel : INamedEntity
    {
        #region Constructors and Destructors

        public Dynel(Identity id)
        {
            this.Starting = true;
            this.DoNotDoTimers = true;

            this.Identity = id;
            this.Stats = new Stats(this.Identity);
            this.InitializeStats();

            this.DoNotDoTimers = false;
            this.Starting = false;
        }

        #endregion

        #region Public Properties

        public bool DoNotDoTimers { get; set; }

        public string FirstName { get; set; }

        public Identity Identity { get; private set; }

        public string LastName { get; set; }

        public string Name { get; set; }

        public bool Starting { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            // Write stats to database
            this.Stats.Write();

            // Send last farewell to playfield
            // TODO: Send vanish packet to all in playfield (ranged)
        }

        #endregion
    }
}