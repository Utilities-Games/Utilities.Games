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
}
