﻿using System.ComponentModel.DataAnnotations;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// A skill available to a commander.
    /// </summary>
    public class CommanderSkill
    {
        /// <summary>
        /// Name of the skill.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Specifies where the skill aligns within the skill tree.
        /// </summary>
        public int Tier { get; set; }
    }
}
