using System;
using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts.Attributes;

namespace Utilities.Games.Models.Subsites.TheLegendOfZelda
{
    /// <summary>
    /// A cooking ingredient as introduced in Breath of the Wild
    /// </summary>
    public class Ingredient
    {
        /// <summary>
        /// Name of the ingredient 
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Inventory category for the ingredient.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Type of ingredient.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Name of the Ingredient Effect that the ingredient introduces to the recipe.
        /// </summary>
        [PseudoForeignKey(typeof(IngredientEffect), nameof(IngredientEffect.Name))]
        public string Effect { get; set; }

        /// <summary>
        /// Duration that the effect lasts.
        /// </summary>
        public string EffectDuration { get; set; }

        public string[] PotencyLevels { get; set; }

        /// <summary>
        /// Amount, in rupees, that the item can be resold.
        /// </summary>
        public double? ResaleValue { get; set; }
    }
}
