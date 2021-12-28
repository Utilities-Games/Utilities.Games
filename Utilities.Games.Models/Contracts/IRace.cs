namespace Utilities.Games.Models.Contracts
{
    /// <summary>
    /// Describes a race of creatures, characters, and other NPCs within a game.
    /// </summary>
    public interface IRace {
        /// <summary>
        /// Name of the type of race.
        /// </summary>
        string Name { get; set; }
    }
}
