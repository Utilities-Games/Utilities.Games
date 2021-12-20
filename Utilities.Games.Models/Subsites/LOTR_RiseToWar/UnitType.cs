using System;
using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts.Attributes;

namespace Utilities.Games.Models.Subsites.LOTR_RiseToWar
{
    /// <summary>
    /// A recruitable army unit.
    /// </summary>
    public class UnitType {
        /// <summary>
        /// Name of the UnitType
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Strength tier of the UnitType. For example: I, II, III, IV
        /// </summary>
        public int Tier { get; set; }

        /// <summary>
        /// Where the unit aligns between Good and Evil.
        /// </summary>
        [PseudoForeignKey(typeof(AlignmentType), nameof(AlignmentType.Name))]
        public string Alignment { get; set; }

        /// <summary>
        /// Reference to the primary form of attack this unit incurs.
        /// </summary>
        [PseudoForeignKey(typeof(AttackMethod), nameof(AttackMethod.Name))]
        public string AttackType { get; set; }

        /// <summary>
        /// Number of units, for this type, that make up an individual command unit.
        /// </summary>
        public int UnitsPerCommand { get; set; }

        /// <summary>
        /// Reference to the race these units represent.
        /// </summary>
        [PseudoForeignKey(typeof(RaceType), nameof(RaceType.Name))]
        public string Race { get; set; }

        /// <summary>
        /// Base, statistical characteristics of this unit type.
        /// </summary>
        public UnitStats Stats { get; set; }

        /// <summary>
        /// Base costs to conscript a single command unit for this unit type.
        /// </summary>
        public UnitCosts Costs { get; set; }

        // TODO: Skills/Perks
    }
    /// <summary>
    /// Base, statistical characteristics of a <see cref="UnitType"/>.
    /// </summary>
    public class UnitStats
    {
        /// <summary>
        /// Minimum base value for damage which affects the damage dealt by a single unit.
        /// </summary>
        public int MinimumBaseDamage { get; set; }

        /// <summary>
        /// Maximum base value for damage which affects the damage dealt by a single unit.
        /// </summary>
        public int MaximumBaseDamage { get; set; }

        /// <summary>
        /// Increases or modifies the amount of damage an individual unit can take before dying.
        /// </summary>
        public int HP { get; set; }

        /// <summary>
        /// Decides unit's top marching speed and turn order during battle.
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// After a battle is won, the Durability of a structure or Land is reduced by 1 for every 100 Siege points.
        /// </summary>
        public int Siege { get; set; }

        /// <summary>
        /// Decreases the amount of Physical Damage units take.
        /// </summary>
        public int Defence { get; set; }
    }

    /// <summary>
    /// Base costs to conscript a single command unit for a unit type.
    /// </summary>
    public class UnitCosts
    {
        /// <summary>
        /// Conscription cost per Command of Wood resources.
        /// </summary>
        public int Wood { get; set; }

        /// <summary>
        /// Conscription cost per Command of Stone resources.
        /// </summary>
        public int Stone { get; set; }

        /// <summary>
        /// Conscription cost per Command of Iron resources.
        /// </summary>
        public int Iron { get; set; }

        /// <summary>
        /// Conscription cost per Command of Grain resources.
        /// </summary>
        public int Grain { get; set; }

        /// <summary>
        /// Conscription cost per Command of time.
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        /// Conscription cost per Command of Gold.
        /// </summary>
        public int Gold { get; set; }
    }
}
