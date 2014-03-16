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

namespace CellAO.Core.Components
{
    #region Usings ...

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    /// 
    /// Represents a weak reference, which references an object while still allowing   
    /// that object to be reclaimed by garbage collection.    
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object that is referenced.
    /// </typeparam>
    [Serializable]
    public class WeakReference<T> : WeakReference
        where T : class
    {
        /// <summary>
        /// 
        /// Initializes a new instance of the Minimal.WeakReference{T} class, referencing        
        /// the specified object.        
        /// </summary>
        /// <param name="target">
        /// The object to reference.
        /// </param>
        public WeakReference(T target)
            : base(target)
        {
        }

        /// <summary>
        /// 
        /// Initializes a new instance of the WeakReference{T} class, referencing 
        /// the specified object and using the specified resurrection tracking. 
        /// </summary>
        /// <param name="target">
        /// An object to track.
        /// </param>
        /// <param name="trackResurrection">
        /// Indicates when to stop tracking the object. If true, the object is tracked 
        /// after finalization; if false, the object is only tracked until finalization.
        /// </param>
        public WeakReference(T target, bool trackResurrection)
            : base(target, trackResurrection)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="info">
        /// </param>
        /// <param name="context">
        /// </param>
        protected WeakReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary> 
        /// Gets or sets the object (the target) referenced by the current WeakReference{T} 
        /// object. 
        /// </summary> 
        public new T Target
        {
            get
            {
                return (T)base.Target;
            }

            set
            {
                base.Target = value;
            }
        }

        /// <summary> 
        /// Casts an object of the type T to a weak reference 
        /// of T. 
        /// </summary> 
        public static implicit operator WeakReference<T>(T target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            return new WeakReference<T>(target);
        }

        /// <summary> 
        /// Casts a weak reference to an object of the type the 
        /// reference represents. 
        /// </summary> 
        public static implicit operator T(WeakReference<T> reference)
        {
            if (reference != null)
            {
                return reference.Target;
            }
            else
            {
                return null;
            }
        }
    }
}