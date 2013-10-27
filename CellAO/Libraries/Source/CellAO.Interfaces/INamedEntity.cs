using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellAO.Interfaces
{
    public interface INamedEntity : IInstancedEntity
    {
        #region Public Properties

        /// <summary>
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// </summary>
        string LastName { get; set; }

        #endregion
    }
}
