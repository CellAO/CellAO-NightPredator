namespace CellAO.Interfaces
{
    public interface IPlayfieldDistrict
    {
        /// <summary>
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// </summary>
        int MinLevel { get; set; }

        /// <summary>
        /// </summary>
        int MaxLevel { get; set; }

        /// <summary>
        /// </summary>
        int SuppressionGas { get; set; }
    }
}