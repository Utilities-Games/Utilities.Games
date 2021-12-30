using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts;

namespace Utilities.Games.Models.Subsites.TheLegendOfZelda
{
    /// <summary>
    /// A character or NPC race type.
    /// </summary>
    public class Race : IRace {
        /// <summary>
        /// Name of the type of the character or NPC.
        /// </summary>
        [Key]
        public string Name { get; set; }
    }
}
