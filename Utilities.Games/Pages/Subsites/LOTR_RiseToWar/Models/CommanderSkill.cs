namespace Utilities.Games.Pages.Subsites.LOTR_RiseToWar.Models
{
    /// <summary>
    /// A skill available to a commander.
    /// </summary>
    public class CommanderSkill
    {
        /// <summary>
        /// Name of the skill.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies where the skill aligns within the skill tree.
        /// </summary>
        public int Tier { get; set; }
    }
}
