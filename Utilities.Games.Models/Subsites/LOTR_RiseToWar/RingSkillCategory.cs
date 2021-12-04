using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// Represents the category for various <see cref="RingSkill"/>s.
    /// </summary>
    public class RingSkillCategory {
        /// <summary>
        /// Name of the <see cref="RingSkill"/> category.
        /// </summary>
        [Key]
        public string Name { get; set; }
    }
}
