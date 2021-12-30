using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts;

namespace Utilities.Games.Models.Subsites.Halo
{
    /// <inheritdoc/>
    public class Saga : ISaga
    {
        /// <inheritdoc/>
        [Key]
        public string Name { get; set; }


        /// <inheritdoc/>
        public string[] Installments { get; set; }
    }
}
