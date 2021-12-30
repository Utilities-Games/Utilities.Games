using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.TheLegendOfZelda
{
    /// <summary>
    /// Describes the effects of an ingredient on the player.
    /// </summary>
    public class IngredientEffect {
        /// <summary>
        /// The name of the effect. Ie. Attack, Defense, Heat Resistance
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// The mathematical factor for which this effect has on the player.
        /// </summary>
        public double MaxPotency { get; set; }

        /// <summary>
        /// Descriptive text of what the effect has on gameplay.
        /// </summary>
        public string Effect { get; set; }

        /// <summary>
        /// Flag for whether or not the effect lasts within a duration or if it is instant.
        /// </summary>
        public bool HasDuration { get; set; }
    }
}
