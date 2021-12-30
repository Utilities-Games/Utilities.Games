namespace Utilities.Games.Models.Contracts
{
    /// <summary>
    /// Describes a developer of a video game. See <c>author</c> schema <seealso href="https://schema.org/author"/>
    /// </summary>
    public interface IDeveloper
    {
        /// <summary>
        /// Name of the developer
        /// </summary>
        string Name { get; set; }
    }
}
