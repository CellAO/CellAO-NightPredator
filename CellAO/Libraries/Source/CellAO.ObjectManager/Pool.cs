#region License

// Copyright (c) 2005-2014, CellAO Team
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
        private readonly Dictionary<int, Dictionary<ulong, IEntity>> pool =
            new Dictionary<int, Dictionary<ulong, IEntity>>();

        #endregion

        /// <summary>
        /// </summary>
        private static readonly Pool instance = new Pool();

        private bool disposed = false;

        /// <summary>
        /// </summary>
        public static Pool Instance
        {
            get
            {
                return instance;
            }
        }

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.disposed)
                {
                    foreach (Dictionary<ulong, IEntity> list in this.pool.Values)
                    {
                        foreach (IDisposable disposable in list.Values)
                        {
                            disposable.Dispose();
                        }
                    }
                }
            }
            this.disposed = true;
        }

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
                var temp = new Dictionary<ulong, IEntity>();
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
        /// <param name="identityType">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<IEntity> GetAll(int identityType)
        {
            List<IEntity> temp = new List<IEntity>();
            if (this.pool.ContainsKey(identityType))
            {
                temp.AddRange(this.pool[identityType].Values.ToArray());
            }

            return temp;
        }

        /// <summary>
        /// </summary>
        /// <param name="identitytype">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public IEnumerable<T> GetAll<T>(int identitytype) where T : class
        {
            List<T> temp = new List<T>();
            if (this.pool.ContainsKey(identitytype))
            {
                temp.AddRange(this.pool[identitytype].Values.OfType<T>().ToArray());
            }

            return temp;
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
        public T GetObject<T>(Identity identity) where T : class, IEntity
        {
            if (this.pool.ContainsKey((int)identity.Type))
            {
                ulong id = identity.Long();
                if (this.pool[(int)identity.Type].ContainsKey(id))
                {
                    IEntity temp = this.pool[(int)identity.Type].First(x => x.Key == id).Value;
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
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        public object GetObject(Identity identity)
        {
            if (this.pool.ContainsKey((int)identity.Type))
            {
                ulong id = identity.Long();
                if (this.pool[(int)identity.Type].ContainsKey(id))
                {
                    IEntity temp = this.pool[(int)identity.Type].First(x => x.Key == id).Value;
                    return temp;
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
                    IEntity temp = this.pool[(int)obj.Identity.Type].First(x => x.Key == id).Value;
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

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Contains(Identity identity)
        {
            bool result = false;
            if (this.pool.ContainsKey((int)identity.Type))
            {
                if (this.pool[(int)identity.Type].ContainsKey(identity.Long()))
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

        public int GetFreeInstance<T>(int minId, IdentityType type)
        {
            int newId = minId;
            Identity temp = new Identity() { Type = type, Instance = minId };
            if (this.pool.ContainsKey((int)type))
            {
                while (this.pool[(int)type].ContainsKey(temp.Long()))
                {
                    // TODO: Prevent overflow....
                    temp.Instance++;
                }
            }
            return temp.Instance;
        }
    }
}