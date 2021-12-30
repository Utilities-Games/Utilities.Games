using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts;
using Utilities.Games.Models.Contracts.Attributes;

namespace Utilities.Games.Models.Subsites.Halo
{
    /// <summary>
    /// Reference to a specific installment of the franchise
    /// </summary>
    public class Installment : IInstallment
    {
        /// <inheritdoc/>
        [Key]
        public string Name { get; set; }

        /// <inheritdoc/>
        public string[] Platforms { get; set; }

        /// <inheritdoc/>
        [PseudoForeignKey(typeof(Saga), nameof(Halo.Saga.Name))]
        public string Saga { get; set; }
    }
}
