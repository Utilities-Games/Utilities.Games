﻿using System.ComponentModel.DataAnnotations;
using Utilities.Games.Models.Contracts;

namespace Utilities.Games.Models.Subsites.TheLegendOfZelda
{
    /// <summary>
    /// Reference to a specific installment of the franchise
    /// </summary>
    public class Installment : IInstallment
    {
        /// <inheritdoc/>
        [Key]
        public string Name { get; set; }

        /// <inheritdoc/>
        public string[] Platforms { get; set; }
    }
}
