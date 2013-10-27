namespace CellAO.Interfaces
{
    using System.Collections.Generic;

    public interface IActions
    {
        /// <summary>
        /// Type of Action (constants in ItemLoader)
        /// </summary>
        int ActionType { get; set; }

        /// <summary>
        /// List of Requirements for this action
        /// </summary>
        List<IRequirements> Requirements { get; set; }
    }
}