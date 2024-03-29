﻿using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// The alignment of an entity. (ie. Good, Evil, Neutral)
    /// </summary>
    public class AlignmentType {
        /// <summary>
        /// Name of the alignment.
        /// </summary>
        [Key]
        public string Name { get; set; }
    }
}
