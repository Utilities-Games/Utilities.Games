using System;
using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts.Attributes;
using Utilities.Games.Models.Subsites.LOTR_RiseToWar.Contracts;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// A commander unit.
    /// </summary>
    public class Commander
    {
        /// <summary>
        /// Name of the commander.
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Where the commander aligns between Good and Evil.
        /// </summary>
        [PseudoForeignKey(typeof(AlignmentType), nameof(AlignmentType.Name))]
        public string Alignment { get; set; }

        /// <summary>
        /// The race of the commander.
        /// </summary>
        [PseudoForeignKey(typeof(RaceType), nameof(RaceType.Name))]
        public string Race { get; set; }

        /// <summary>
        /// Collection of special classes for the commander.
        /// </summary>
        [PseudoForeignKey(typeof(CommanderClass), nameof(CommanderClass.Name))]
        public string[] Classes { get; set; }

        /// <summary>
        /// Collection of commander skills.
        /// </summary>
        public CommanderSkill[] Skills { get; set; }

        /// <summary>
        /// Base stats for a commander.
        /// </summary>
        public BaseStats Stats { get; set; }
    }
    /// <summary>
    /// Base stats for a commander.
    /// </summary>
    public class BaseStats
    {
        /// <summary>
        /// Base stats for Might.
        /// </summary>
        public BaseStat Might { get; set; }

        /// <summary>
        /// Base stats for Focus.
        /// </summary>
        public BaseStat Focus { get; set; }

        /// <summary>
        /// Base stats for Speed.
        /// </summary>
        public BaseStat Speed { get; set; }
    }
}
