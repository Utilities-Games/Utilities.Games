using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Subsites.LOTR_RiseToWar.Contracts;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// A skill that a commander can learn.
    /// </summary>
    public class Skill
    {
        /// <summary>
        /// Name of the skill.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Collection of sub-skills.
        /// </summary>
        public SubSkill[] SubSkills { get; set; }

        /// <summary>
        /// The achievable levels for the skill.
        /// </summary>
        public SkillLevel[] Levels { get; set; }

        /// <summary>
        /// The achievable effect once all levels have been achieved.
        /// </summary>
        public Effect MaxLevelEffect { get; set; }
    }
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
