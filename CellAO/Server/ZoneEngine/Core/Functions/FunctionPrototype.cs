using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZoneEngine.Core.Functions
{
    using CellAO.Core.Entities;

    public abstract class FunctionPrototype
    {
        /// <summary>
        /// Locks function targets and executes the function
        /// </summary>
        /// <param name="self">
        /// Dynel (Character or NPC)
        /// </param>
        /// <param name="caller">
        /// Caller of the function
        /// </param>
        /// <param name="target">
        /// Target of the Function (Dynel or Statel)
        /// </param>
        /// <param name="arguments">
        /// Function Arguments
        /// </param>
        /// <returns>
        /// </returns>
        public abstract bool Execute(
            INamedEntity self, INamedEntity caller, IInstancedEntity target, object[] arguments);

        /// <summary>
        /// </summary>
        private int functionNumber = -1;

        /// <summary>
        /// </summary>
        private string functionName = string.Empty;

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public abstract int ReturnNumber();

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public abstract string ReturnName();

        /// <summary>
        /// </summary>
        public int FunctionNumber
        {
            get
            {
                return this.functionNumber;
            }

            set
            {
                this.functionNumber = value;
            }
        }

        /// <summary>
        /// </summary>
        public string FunctionName
        {
            get
            {
                return this.functionName;
            }

            set
            {
                this.functionName = value;
            }
        }
    }
}
