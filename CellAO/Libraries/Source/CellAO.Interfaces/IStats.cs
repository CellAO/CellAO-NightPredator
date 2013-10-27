using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Interfaces
{
    public interface IStats
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        IStatList Stats { get; }

        /// <summary>
        /// </summary>
        /// <param name="aof">
        /// </param>
        /// <param name="checkAll">
        /// </param>
        /// <returns>
        /// </returns>
        bool CheckRequirements(IFunctions aof, bool checkAll);

        #endregion
    }
}
