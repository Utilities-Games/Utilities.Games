using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// Represents an instance of a level applied to a Ring of Power.
    /// </summary>
    public class RingPowerLevel {
        /// <summary>
        /// Represents the level associated with this level of Ring Power.
        /// </summary>
        [Key]
        public int Level { get; set; }

        /// <summary>
        /// Reference to the minimum amount of Ring Power is required to unlock this level of Ring Power.
        /// </summary>
        public int MinimumPowerCost { get; set; }
    }
}
