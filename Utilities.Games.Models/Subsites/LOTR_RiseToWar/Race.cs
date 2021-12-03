using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// Race of a commander.
    /// </summary>
    public class Race
    {
        /// <summary>
        /// Name of the race.
        /// </summary>
        [Key]
        public string Name { get; set; }
    }
}
