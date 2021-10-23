namespace Utilities.Games.Pages.Subsites.LOTR_RiseToWar.Models
{
    /// <summary>
    /// A class that a commander falls under.
    /// </summary>
    public class CommanderClass{
        /// <summary>
        /// Name of the class
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The effects this class has on the commander.
        /// </summary>
        public Effect[] Effects { get; set; }
    }
}
