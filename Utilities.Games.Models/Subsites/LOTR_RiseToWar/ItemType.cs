using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// Represents an item type.
    /// </summary>
    public class ItemType {
        /// <summary>
        /// Name of the item type. For example: Equipment, Respect, Boost, Special.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Available sub-types. For example: Head, Armour, Hand, Accessory.
        /// </summary>
        public string[] SubTypes { get; set; }
    }
}
