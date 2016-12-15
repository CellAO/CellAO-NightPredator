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

namespace CellAO.Core.Components
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Linq;
    using System.Reflection;

    #endregion

    /// <summary>
    /// Component Model composition container class
    /// </summary>
    public class MefContainer : IContainer
    {
        #region Fields

        /// <summary>
        /// Lazy container
        /// </summary>
        private readonly Lazy<CompositionContainer> lazyContainer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public MefContainer()
        {
            this.lazyContainer = new Lazy<CompositionContainer>(this.Initialize);
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        private CompositionContainer Container
        {
            get
            {
                return this.lazyContainer.Value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="serviceType">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return this.Container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        /// <summary>
        /// </summary>
        /// <param name="serviceType">
        /// </param>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public object GetInstance(Type serviceType, string key = null)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            IEnumerable<object> exports = this.Container.GetExportedValues<object>(contract);

            object instance = exports.FirstOrDefault();

            if (instance != null)
            {
                return instance;
            }

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        /// <summary>
        /// </summary>
        /// <param name="key">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public T GetInstance<T>(string key = null)
        {
            return (T)this.GetInstance(typeof(T), key);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private CompositionContainer Initialize()
        {
            var catalog = new AggregateCatalog();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var assemblyCatalog = new AssemblyCatalog(assembly);
                    ComposablePartDefinition[] assemblyParts = assemblyCatalog.Parts.ToArray();
                    catalog.Catalogs.Add(assemblyCatalog);
                }
                catch (Exception)
                {
                    // Dont catch not importable assemblies errors
                }
            }

            var container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();
            batch.AddExportedValue<IContainer>(this);
            container.Compose(batch);
            return container;
        }

        #endregion
    }
}