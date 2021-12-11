using System;
using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts.Attributes;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// Represents a building that can be constructed.
    /// </summary>
    public class BuildingType {
        /// <summary>
        /// Name of the building type.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Reference to which levels are available for this building type and the associated costs, requirements, and effects.
        /// </summary>
        public BuildingLevel[] Levels { get; set; }
    }
    /// <summary>
    /// Represents the costs, requirements, and effects associated with acquiring or achieving this level of a building type's construction.
    /// </summary>
    public class BuildingLevel
    {
        /// <summary>
        /// Reference to the building level.
        /// </summary>
        [Key]
        public int Level { get; set; }

        /// <summary>
        /// Base resource/time costs to construct this level of a building type.
        /// </summary>
        public BuildingConstructionCost Costs { get; set; }

        /// <summary>
        /// Collection of other building requirements before construction can begin on this leve.
        /// </summary>
        public MinimumConstructionRequirement[] Requirements { get; set; }

        // TODO: Effects per level
    }
    /// <summary>
    /// Represents the construction costs associated with a specific level of a building type.
    /// </summary>
    public class BuildingConstructionCost
    {

        /// <summary>
        /// Construction cost of Wood resources.
        /// </summary>
        public int Wood { get; set; }

        /// <summary>
        /// Construction cost of Stone resources.
        /// </summary>
        public int Stone { get; set; }

        /// <summary>
        /// Construction cost of Iron resources.
        /// </summary>
        public int Iron { get; set; }

        /// <summary>
        /// Construction cost of Grain resources.
        /// </summary>
        public int Grain { get; set; }

        /// <summary>
        /// Construction cost of time.
        /// </summary>
        public TimeSpan Time { get; set; }
    }
    /// <summary>
    /// Represents a single minimum building requirement for a building type.
    /// </summary>
    public class MinimumConstructionRequirement {
        /// <summary>
        /// Reference to the building type that his requirement is based on.
        /// </summary>
        [PseudoForeignKey(typeof(BuildingType), nameof(BuildingType.Name))]
        public string Building { get; set; }

        /// <summary>
        /// Represents the minimum level that the <see cref="BuildingType"/> must achieve before this requirement is satisfied.
        /// </summary>
        public int MinimumLevel { get; set; }
    }
}
