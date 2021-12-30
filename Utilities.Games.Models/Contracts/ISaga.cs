namespace Utilities.Games.Models.Contracts
{
    /// <summary>
    /// Describes a saga of game installments within a franchise.
    /// </summary>
    public interface ISaga {
        /// <summary>
        /// Name of the franchise saga.
        /// </summary>
        string Name { get; set; }
    }
}
