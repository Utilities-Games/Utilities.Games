using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.Halo
{
    /// <summary>
    /// Reference to a collectible available across the franchise.
    /// </summary>
    public class Skull {
        /// <summary>
        /// Name of the skull across the franchise.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// List of installments this skull is found in.
        /// </summary>
        public InstallmentSkullDetails[] Installments { get; set; }
    }
}
