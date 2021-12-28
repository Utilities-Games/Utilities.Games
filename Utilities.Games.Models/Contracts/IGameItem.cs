namespace Utilities.Games.Models.Contracts
{
    /// <summary>
    /// Describes an in-game item. Based on <c>gameItem</c> schema <seealso href="https://schema.org/gameItem"/>
    /// </summary>
    public interface IGameItem
    {
        /// <summary>
        /// The name of the item.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// An alias for the item.
        /// </summary>
        string AlternateName { get; set; }

        /// <summary>
        /// A description of the item.
        /// </summary>
        string Description { get; set; }
    }
}
