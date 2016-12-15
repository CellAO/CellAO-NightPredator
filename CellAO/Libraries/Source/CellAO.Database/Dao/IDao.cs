#region License

// Copyright (c) 2005-2016, CellAO Team
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

namespace CellAO.Database.Dao
{
    #region Usings ...

    using System.Collections.Generic;
    using System.Data;

    using CellAO.Database.Entities;

    #endregion

    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IDao<T>
        where T : IDBEntity
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="dbentity">
        /// </param>
        /// <param name="connection">
        /// </param>
        /// <param name="transaction">
        /// </param>
        /// <returns>
        /// </returns>
        int Add(T dbentity, IDbConnection connection = null, IDbTransaction transaction = null, bool dontUseId = true);

        /// <summary>
        /// </summary>
        /// <param name="entityId">
        /// </param>
        /// <param name="connection">
        /// </param>
        /// <param name="transaction">
        /// </param>
        /// <returns>
        /// </returns>
        int Delete(int entityId, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// </summary>
        /// <param name="whereParameters">
        /// </param>
        /// <param name="connection">
        /// </param>
        /// <param name="transaction">
        /// </param>
        /// <returns>
        /// </returns>
        int Delete(object whereParameters, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// </summary>
        /// <param name="entityId">
        /// </param>
        /// <returns>
        /// </returns>
        bool Exists(int entityId);

        /// <summary>
        /// </summary>
        /// <param name="entityId">
        /// </param>
        /// <returns>
        /// </returns>
        T Get(int entityId);

        /// <summary>
        /// </summary>
        /// <param name="parameters">
        /// </param>
        /// <returns>
        /// </returns>
        IEnumerable<T> GetAll(object parameters = null);

        /// <summary>
        /// </summary>
        /// <param name="dbentity">
        /// </param>
        /// <param name="parameters">
        /// </param>
        /// <param name="connection">
        /// </param>
        /// <param name="transaction">
        /// </param>
        /// <returns>
        /// </returns>
        int Save(
            T dbentity,
            object parameters = null,
            IDbConnection connection = null,
            IDbTransaction transaction = null);

        #endregion
    }
}