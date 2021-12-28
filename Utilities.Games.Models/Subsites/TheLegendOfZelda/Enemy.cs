using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts.Attributes;

namespace Utilities.Games.Models.Subsites.TheLegendOfZelda
{
    /// <summary>
    /// An enemy creature or character of a specific race.
    /// </summary>
    public class Enemy {
        /// <summary>
        /// Reference to the race of the creature or character.
        /// </summary>
        [PseudoForeignKey(typeof(TheLegendOfZelda.Race), nameof(TheLegendOfZelda.Race.Name))]
        public string Race { get; set; }

        /// <summary>
        /// Full name of the creature or character.
        /// </summary>
        [Key]
        public string Name { get; set; }

        public BaseStatistics BaseStats { get; set; }
    }
}
