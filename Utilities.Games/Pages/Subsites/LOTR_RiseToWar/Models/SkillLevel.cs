namespace Utilities.Games.Pages.Subsites.LOTR_RiseToWar.Models
{
    /// <summary>
    /// An progression level for a Skill.
    /// </summary>
    public class SkillLevel {
        /// <summary>
        /// An achievable level for a Skill.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The effects gained when the level is achieved.
        /// </summary>
        public Effect[] Effects { get; set; }
    }
}
