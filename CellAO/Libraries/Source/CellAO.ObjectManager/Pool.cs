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

namespace CellAO.ObjectManager
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CellAO.Core.Exceptions;
    using CellAO.Interfaces;

    using SmokeLounge.AOtomation.Messaging.GameData;

    #endregion

    /// <summary>
    /// </summary>
    public class Pool : IDisposable
    {
        #region Fields

        /// <summary>
        /// </summary>
        private Dictionary<int, Dictionary<ulong, IDisposable>> pool =
            new Dictionary<int, Dictionary<ulong, IDisposable>>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public void AddObject<T>(T obj) where T : class, IDisposable, IEntity
        {
            if (!this.pool.ContainsKey((int)obj.Identity.Type))
            {
                var temp = new Dictionary<ulong, IDisposable>();
                temp.Add(obj.Identity.Long(), obj);
                this.pool.Add((int)obj.Identity.Type, temp);
            }
            else
            {
                // I dont check for duplicates here, thrown exceptions should be enough
                this.pool[(int)obj.Identity.Type].Add(obj.Identity.Long(), obj);
            }
        }

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            foreach (Dictionary<ulong, IDisposable> list in this.pool.Values)
            {
                foreach (IDisposable disposable in list.Values)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        /// <exception cref="TypeInstanceMismatchException">
        /// </exception>
        public T GetObject<T>(Identity identity) where T : class, IDisposable, IEntity
        {
            if (this.pool.ContainsKey((int)identity.Type))
            {
                ulong id = identity.Long();
                if (this.pool[(int)identity.Type].ContainsKey(id))
                {
                    IDisposable temp = this.pool[(int)identity.Type].First(x => x.Key == id).Value;
                    if (temp is T)
                    {
                        return (T)temp;
                    }

                    throw new TypeInstanceMismatchException(
                        "Tried to retrieve " + identity.Type.ToString("X8") + ":" + identity.Instance.ToString("X8")
                        + " with the wrong type (" + typeof(T).ToString() + " != " + temp.GetType().ToString() + ")");
                }
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="TypeInstanceMismatchException">
        /// </exception>
        public void RemoveObject<T>(T obj) where T : IDisposable, IEntity
        {
            if (this.pool.ContainsKey((int)obj.Identity.Type))
            {
                ulong id = obj.Identity.Long();
                if (this.pool[(int)obj.Identity.Type].ContainsKey(id))
                {
                    IDisposable temp = this.pool[(int)obj.Identity.Type].First(x => x.Key == id).Value;
                    if (temp is T)
                    {
                        this.pool[(int)obj.Identity.Type].Remove(obj.Identity.Long());
                        return;
                    }

                    throw new TypeInstanceMismatchException(
                        "Tried to remnve " + obj.Identity.Type.ToString("X8") + ":"
                        + obj.Identity.Instance.ToString("X8") + " with the wrong type (" + typeof(T).ToString()
                        + " != " + temp.GetType().ToString() + ")");
                }
            }
        }

        #endregion
    }
}