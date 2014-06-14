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

namespace Utility
{
    #region Usings ...

    using System;
    using System.Diagnostics;
    using System.Threading;

    #endregion

    /// <summary>
    /// </summary>
    public class CpuRamUtilization : IDisposable
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static CpuRamUtilization instance;

        #endregion

        #region Fields

        /// <summary>
        /// </summary>
        protected PerformanceCounter cpuCounter;

        /// <summary>
        /// </summary>
        protected PerformanceCounter ramCounter;

        private bool disposed = false;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        private CpuRamUtilization()
        {
            this.cpuCounter = new PerformanceCounter()
                              {
                                  CategoryName = "Processor",
                                  CounterName = "% Processor Time",
                                  InstanceName = "_Total"
                              };
            this.ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public static CpuRamUtilization Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CpuRamUtilization();
                }

                return instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static float GetCpuLoad()
        {
            float nextValue = 0.0f;
            int tries = 0;
            while ((tries < 2) && (nextValue == 0.0f))
            {
                nextValue = Instance.cpuCounter.NextValue();
                if (nextValue == 0.0f)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    return nextValue;
                }

                tries++;
            }

            return 0.0f;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static float GetRamLoad()
        {
            float nextValue = 0.0f;
            int tries = 0;
            while ((tries < 2) && (nextValue == 0.0f))
            {
                nextValue = Instance.ramCounter.NextValue();
                if (nextValue == 0.0f)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    return nextValue;
                }

                tries++;
            }

            return 0.0f;
        }

        #endregion

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
                    this.ramCounter.Close();
                }
            }
            this.disposed = true;
        }
    }
}