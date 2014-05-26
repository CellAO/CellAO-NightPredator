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
        private readonly Dictionary<ulong, Dictionary<int, Dictionary<ulong, IEntity>>> pool =
            new Dictionary<ulong, Dictionary<int, Dictionary<ulong, IEntity>>>();

        #endregion

        /// <summary>
        /// </summary>
        private static readonly Pool instance = new Pool();

        private bool disposed = false;

        private readonly List<ulong> reservedIds = new List<ulong>();

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
                    lock (this.pool)
                    {
                        foreach (Dictionary<int, Dictionary<ulong, IEntity>> ownerEntries in this.pool.Values)
                        {
                            lock (ownerEntries)
                            {
                                foreach (Dictionary<ulong, IEntity> list in ownerEntries.Values)
                                {
                                    lock (list)
                                    {
                                        foreach (IDisposable disposable in list.Values)
                                        {
                                            disposable.Dispose();
                                        }
                                        list.Clear();
                                    }
                                }
                                ownerEntries.Clear();
                            }
                        }
                        this.pool.Clear();
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
        public void AddObject<T>(Identity parent, T obj) where T : class, IDisposable, IEntity
        {
            ulong parentId = parent.Long();
            Dictionary<int, Dictionary<ulong, IEntity>> parentList = null;
            lock (this.reservedIds)
            {
                lock (this.pool)
                {
                    if (!this.pool.ContainsKey(parentId))
                    {
                        parentList = new Dictionary<int, Dictionary<ulong, IEntity>>();
                        this.pool.Add(parentId, parentList);
                    }
                    else
                    {
                        parentList = this.pool[parentId];
                    }
                }
                lock (parentList)
                {
                    if (!parentList.ContainsKey((int)obj.Identity.Type))
                    {
                        var temp = new Dictionary<ulong, IEntity>();
                        parentList.Add((int)obj.Identity.Type, temp);
                    }
                    // I dont check for duplicates here, thrown exceptions should be enough
                    parentList[(int)obj.Identity.Type].Add(obj.Identity.Long(), obj);
                    this.reservedIds.Remove(obj.Identity.Long());
                }
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
            lock (this.pool)
            {
                foreach (Dictionary<int, Dictionary<ulong, IEntity>> entries in this.pool.Values)
                {
                    lock (entries)
                    {
                        if (entries.ContainsKey(identityType))
                        {
                            temp.AddRange(entries[identityType].Values);
                        }
                    }
                }
            }

            return temp;
        }

        public IEnumerable<IEntity> GetAll(Identity parentIdentity, int identitytype)
        {
            List<IEntity> temp = new List<IEntity>();
            ulong parentId = parentIdentity.Long();
            lock (this.pool)
            {
                if (this.pool.ContainsKey(parentId))
                {
                    lock (this.pool[parentId])
                    {
                        if (this.pool[parentId].ContainsKey(identitytype))
                        {
                            temp.AddRange(this.pool[parentId][identitytype].Values);
                        }
                    }
                }
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

            lock (this.pool)
            {
                foreach (Dictionary<int, Dictionary<ulong, IEntity>> list in this.pool.Values)
                {
                    lock (list)
                    {
                        if (list.ContainsKey(identitytype))
                        {
                            lock (list[identitytype])
                            {
                                temp.AddRange(list[identitytype].Values.OfType<T>());
                            }
                        }
                    }
                }
            }

            return temp;
        }

        public IEnumerable<T> GetAll<T>(Identity parent, int identitytype) where T : class
        {
            List<T> temp = new List<T>();
            ulong parentId = parent.Long();

            lock (this.pool)
            {
                if (this.pool.ContainsKey(parentId))
                {
                    lock (this.pool[parentId])
                    {
                        if (this.pool[parentId].ContainsKey(identitytype))
                        {
                            lock (this.pool[parentId][identitytype])
                            {
                                temp.AddRange(this.pool[parentId][identitytype].Values.OfType<T>());
                            }
                        }
                    }
                }
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
            ulong id = identity.Long();
            lock (this.pool)
            {
                foreach (Dictionary<int, Dictionary<ulong, IEntity>> list in this.pool.Values)
                {
                    lock (list)
                    {
                        if (list.ContainsKey((int)identity.Type))
                        {
                            lock (list[(int)identity.Type])
                            {
                                IEntity temp = null;
                                try
                                {
                                    temp = list[(int)identity.Type][id];
                                }
                                catch (Exception)
                                {
                                }
                                if (temp is T)
                                {
                                    return (T)temp;
                                }
                                if (temp != null)
                                {
                                    throw new TypeInstanceMismatchException(
                                        "Tried to retrieve " + identity.Type.ToString("X") + ":"
                                        + identity.Instance.ToString("X") + " with the wrong type ("
                                        + typeof(T).ToString() + " != " + temp.GetType().ToString() + ")");
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public T GetObject<T>(Identity parent, Identity identity)
        {
            ulong parentId = parent.Long();
            ulong id = identity.Long();

            Dictionary<int, Dictionary<ulong, IEntity>> list = null;
            lock (this.pool)
            {
                if (this.pool.ContainsKey(parentId))
                {
                    list = this.pool[parentId];
                }
            }
            if (list != null)
            {
                Dictionary<ulong, IEntity> entries = null;
                lock (list)
                {
                    if (list.ContainsKey((int)identity.Type))
                    {
                        entries = list[(int)identity.Type];
                    }
                }
                if (entries != null)
                {
                    lock (entries)
                    {
                        IEntity temp = null;
                        try
                        {
                            temp = entries[id];
                        }
                        catch (Exception)
                        {
                        }
                        if (temp is T)
                        {
                            return (T)temp;
                        }
                        throw new TypeInstanceMismatchException(
                            "Tried to retrieve " + identity.Type.ToString("X8") + ":" + identity.Instance.ToString("X8")
                            + " with the wrong type (" + typeof(T).ToString() + " != " + temp.GetType().ToString() + ")");
                    }
                }
                throw new Exception("Identity type not in this parent's list");
            }
            throw new ParentNotInPoolException("Parent " + parent.ToString(true) + " not in Pool list");
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        public object GetObject(Identity identity)
        {
            List<ulong> parentIds = new List<ulong>();
            lock (this.pool)
            {
                parentIds.AddRange(this.pool.Keys);
            }
            foreach (ulong parentId in parentIds)
            {
                Dictionary<int, Dictionary<ulong, IEntity>> list = this.pool[parentId];
                Dictionary<ulong, IEntity> entries = null;

                lock (list)
                {
                    if (list.ContainsKey((int)identity.Type))
                    {
                        entries = list[(int)identity.Type];
                    }
                }
                if (entries != null)
                {
                    lock (entries)
                    {
                        IEntity temp = null;
                        try
                        {
                            temp = entries[identity.Long()];
                        }
                        catch (Exception)
                        {
                        }
                        return temp;
                    }
                }
            }

            return null;
        }

        public IEntity GetObject(Identity parent, Identity identity)
        {
            ulong parentId = parent.Long();
            ulong id = identity.Long();

            Dictionary<int, Dictionary<ulong, IEntity>> list = null;
            lock (this.pool)
            {
                if (this.pool.ContainsKey(parentId))
                {
                    list = this.pool[parentId];
                }
            }

            if (list != null)
            {
                lock (list)
                {
                    if (list.ContainsKey((int)identity.Type))
                    {
                        lock (list[(int)identity.Type])
                        {
                            IEntity temp = null;
                            try
                            {
                                temp = list[(int)identity.Type][id];
                            }
                            catch (Exception)
                            {
                            }
                            return temp;
                        }
                    }
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
            Dictionary<int, Dictionary<ulong, IEntity>> list = null;
            lock (this.pool)
            {
                if (this.pool.ContainsKey(obj.Parent.Long()))
                {
                    list = this.pool[obj.Parent.Long()];
                }
            }
            if (list != null)
            {
                lock (list)
                {
                    if (list.ContainsKey((int)obj.Identity.Type))
                    {
                        lock (list[(int)obj.Identity.Type])
                        {
                            IEntity temp = null;
                            try
                            {
                                temp = list[(int)obj.Identity.Type][obj.Identity.Long()];
                            }
                            catch (Exception)
                            {
                            }
                            if (temp is T)
                            {
                                list[(int)obj.Identity.Type].Remove(obj.Identity.Long());
                            }
                            else
                            {
                                if (temp != null)
                                {
                                    throw new TypeInstanceMismatchException(
                                        "Tried to remove " + obj.Identity.Type.ToString("X8") + ":"
                                        + obj.Identity.Instance.ToString("X8") + " with the wrong type ("
                                        + typeof(T).ToString() + " != " + temp.GetType().ToString() + ")");
                                }
                                else
                                {
                                    throw new ArgumentNullException(
                                        "Tried to remove object, which is not in the pool: "
                                        + obj.Identity.ToString(true));
                                }
                            }
                        }
                    }
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
            List<ulong> parentIds = new List<ulong>();
            lock (this.pool)
            {
                parentIds.AddRange(this.pool.Keys);
            }

            foreach (ulong parentId in parentIds)
            {
                Dictionary<int, Dictionary<ulong, IEntity>> list = this.pool[parentId];
                Dictionary<ulong, IEntity> entries = null;
                lock (list)
                {
                    if (list.ContainsKey((int)identity.Type))
                    {
                        entries = list[(int)identity.Type];
                    }
                }
                if (entries != null)
                {
                    lock (entries)
                    {
                        if (list[(int)identity.Type].ContainsKey(identity.Long()))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public bool Contains(Identity parent, Identity identity)
        {
            bool result = false;
            ulong parentId = parent.Long();
            Dictionary<int, Dictionary<ulong, IEntity>> list = null;
            lock (this.pool)
            {
                if (this.pool.ContainsKey(parentId))
                {
                    list = this.pool[parentId];
                }
            }

            if (list != null)
            {
                Dictionary<ulong, IEntity> entries = null;
                lock (list)
                {
                    if (list.ContainsKey((int)identity.Type))
                    {
                        entries = list[(int)identity.Type];
                    }
                }
                if (entries != null)
                {
                    lock (entries)
                    {
                        if (entries.ContainsKey(identity.Long()))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        public int GetFreeInstance<T>(int minId, IdentityType type)
        {
            Identity temp = new Identity() { Type = type, Instance = minId };

            lock (this.reservedIds)
            {
                List<ulong> parentIds = new List<ulong>();
                lock (this.pool)
                {
                    parentIds.AddRange(this.pool.Keys);
                }

                bool foundEmpty = false;
                while (!foundEmpty)
                {
                    if (this.reservedIds.Contains(temp.Long()))
                    {
                        temp.Instance++;
                    }
                    else
                    {
                        foreach (ulong parentid in parentIds)
                        {
                            Dictionary<ulong, IEntity> entries = null;
                            if (this.pool[parentid].ContainsKey((int)type))
                            {
                                entries = this.pool[parentid][(int)type];
                            }
                            if (entries != null)
                            {
                                lock (entries)
                                {
                                    if (entries.ContainsKey(temp.Long()))
                                    {
                                        temp.Instance++;
                                    }
                                    else
                                    {
                                        this.reservedIds.Add(temp.Long());
                                        foundEmpty = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return temp.Instance;
        }
    }
}