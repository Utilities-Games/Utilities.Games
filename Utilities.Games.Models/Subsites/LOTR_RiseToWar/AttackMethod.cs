using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// Reference to the type that an attack incurs.
    /// </summary>
    public class AttackMethod {
        /// <summary>
        /// Name of the attack type.
        /// </summary>
        [Key]
        public string Name { get; set; }
    }
}
