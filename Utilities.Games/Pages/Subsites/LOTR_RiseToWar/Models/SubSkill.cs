namespace Utilities.Games.Pages.Subsites.LOTR_RiseToWar.Models
{
    /// <summary>
    /// A sub-skill that a commander can learn.
    /// </summary>
    public class SubSkill {
        /// <summary>
        /// Name of the skill.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The round that this skill kicks in.
        /// </summary>
        public int Round { get; set; }
    }
}
