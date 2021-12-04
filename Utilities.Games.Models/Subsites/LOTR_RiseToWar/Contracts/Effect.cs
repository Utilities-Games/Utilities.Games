namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar.Contracts
{
    /// <summary>
    /// An altered effect.
    /// </summary>
    public class Effect
    {
        /// <summary>
        /// Specifies the target type. For example, Might or Speed.
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// The amount that the target type is affected.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Specifies what scope of the target type. For example, Commander or Army.
        /// </summary>
        public string TargetScope { get; set; }

        /// <summary>
        /// Unit of measurement for the effect amount.
        /// </summary>
        public string Units { get; set; }
    }
}
