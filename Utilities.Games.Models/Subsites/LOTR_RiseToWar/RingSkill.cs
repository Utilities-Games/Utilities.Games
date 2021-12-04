using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts.Attributes;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// Represents a skill that can be unlocked as Ring levels progress.
    /// </summary>
    public class RingSkill {
        /// <summary>
        /// Name of the Ring skill.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Reference to the category for which this skill applies.
        /// </summary>
        [PseudoForeignKey(typeof(RingSkillCategory), nameof(RingSkillCategory.Name))]
        public string Category { get; set; }

        /// <summary>
        /// Available progressions for this skill.
        /// </summary>
        public RingSkillLevel[] Levels { get; set; }
    }
    public class RingSkillLevel {
        /// <summary>
        /// Reference to which level this object applies to.
        /// </summary>
        [Key]
        public int Level { get; set; }

        // TODO: Effects per level
    }
}
