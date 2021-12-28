namespace Utilities.Games.Models.Contracts
{
    /// <summary>
    /// Describes a character within a game. See <c>character</c> schema <seealso href="https://schema.org/character"/>
    /// </summary>
    public interface ICharacter
    {
        /// <summary>
        /// The name of the character.
        /// </summary>
        string Name { get; set; }
    }
}
