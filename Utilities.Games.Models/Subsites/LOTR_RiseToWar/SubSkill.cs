using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// A sub-skill that a commander can learn.
    /// </summary>
    public class SubSkill
    {
        /// <summary>
        /// Name of the skill.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// The round that this skill kicks in.
        /// </summary>
        public int Round { get; set; }
    }
}
