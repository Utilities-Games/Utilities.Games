using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// An progression level for a Skill.
    /// </summary>
    public class SkillLevel
    {
        /// <summary>
        /// An achievable level for a Skill.
        /// </summary>
        [Key]
        public int Level { get; set; }

        /// <summary>
        /// The effects gained when the level is achieved.
        /// </summary>
        public Effect[] Effects { get; set; }
    }
}
