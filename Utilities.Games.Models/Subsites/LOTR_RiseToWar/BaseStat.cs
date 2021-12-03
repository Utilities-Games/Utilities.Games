namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// A commander's base stat.
    /// </summary>
    public class BaseStat
    {
        /// <summary>
        /// The base value for the specific stat.
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// The rate per level that the stat increases
        /// </summary>
        public double Rate { get; set; }
    }
}
