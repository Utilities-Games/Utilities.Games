using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts;

namespace Utilities.Games.Models.Subsites.TheLegendOfZelda
{
    /// <summary>
    /// Reference to a specific installment of the franchise
    /// </summary>
    public class Installment : IInstallment {
        /// <summary>
        /// Name of the game installment
        /// </summary>
        [Key]
        public string Name { get; set; }

        public string[] Platforms { get; set; }
    }
}
