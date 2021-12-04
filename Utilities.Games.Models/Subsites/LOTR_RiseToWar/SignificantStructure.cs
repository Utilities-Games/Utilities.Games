using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Subsites.LOTR_RiseToWar.Contracts;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// Represents a significant structure on the world map.
    /// </summary>
    public class SignificantStructure {
        /// <summary>
        /// Name of the structure.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Location on the World Map that the center of this structure exists.
        /// </summary>
        public Coordinate Coordinates { get; set; }

        /// <summary>
        /// Represents the strength tier of the structure.
        /// </summary>
        public int Tier { get; set; }

        /// <summary>
        /// The hourly Ring Power output rate.
        /// </summary>
        public int HourlyPowerRate { get; set; }

        /// <summary>
        /// Base, statistical defensive characteristics for this structure.
        /// </summary>
        public StructureDefenses Defenses { get; set; }

        // TODO: Effects
    }
    
    /// <summary>
    /// The base statistical characteristics of a structure's defenses.
    /// </summary>
    public class StructureDefenses {
        /// <summary>
        /// The commander level of the army(ies) currently defending the structure.
        /// </summary>
        public int DefenderLevel { get; set; }

        /// <summary>
        /// The total number of armies stationed at the structure.
        /// </summary>
        public int TotalArmyCount { get; set; }

        /// <summary>
        /// The total amount of Siege defense is available for this structure.
        /// </summary>
        public int TotalSiegeDefenses { get; set; }
    }
}
