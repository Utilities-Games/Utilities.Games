using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts.Attributes;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// Represents a specific equipment item.
    /// </summary>
    public class EquipmentItem {
        /// <summary>
        /// Name of the item.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Reference to the type of item this is.
        /// </summary>
        [PseudoForeignKey(typeof(ItemType), nameof(ItemType.Name))]
        public string Type { get; set; }

        /// <summary>
        /// Reference to the sub-type of item this is.
        /// </summary>
        public string SubType { get; set; }

        /// <summary>
        /// List of race types this equipment can be given to.
        /// </summary>
        [PseudoForeignKey(typeof(RaceType), nameof(RaceType.Name))]
        public string[] SupportedRaces { get; set; }

        // TODO: Effects, Growth per sharpening

        // TODO: Skills, Growth per refinement
    }
}
