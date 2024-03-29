﻿using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts.Attributes;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// A faction.
    /// </summary>
    public class Faction
    {
        /// <summary>
        /// Name of the Faction.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Name of the first, default commander for the Faction.
        /// </summary>
        [PseudoForeignKey(typeof(Commander), nameof(Commander.Name))]
        public string FirstCommander { get; set; }
    }
}
