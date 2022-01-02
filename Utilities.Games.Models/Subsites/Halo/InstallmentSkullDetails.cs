using Utilities.Games.Models.Contracts.Attributes;
using Utilities.Games.Models.Subsites.Halo.Contracts;

namespace Utilities.Games.Models.Subsites.Halo
{
    /// <summary>
    /// Details of a skull by installment.
    /// </summary>
    public class InstallmentSkullDetails {
        /// <summary>
        /// Name of the installment.
        /// </summary>
        [PseudoForeignKey(typeof(Installment), nameof(Halo.Installment.Name))]
        public string Name { get; set; }

        /// <summary>
        /// Difficulties this skull can be acquired on.
        /// </summary>
        public string[] Difficulties { get; set; }

        /// <summary>
        /// Description of what this skull effects, if anything.
        /// </summary>
        public string Effect { get; set; }

        /// <summary>
        /// In-game text.
        /// </summary>
        public string FlavorText { get; set; }

        /// <summary>
        /// Rarity rating of the skull.
        /// </summary>
        public string Rarity { get; set; }

        /// <summary>
        /// Multiplier of point. Applicable in campaign and firefight modes.
        /// </summary>
        public double? PointMultiplier { get; set; }
    }
}
