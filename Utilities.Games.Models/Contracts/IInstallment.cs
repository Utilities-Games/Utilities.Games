using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Games.Models.Contracts
{
    /// <summary>
    /// Describes a specific game installment either as standalone or part of a franchise.
    /// </summary>
    public interface IInstallment
    {
        /// <summary>
        /// Name of the game installment.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// List of gaming platforms the installment is capable of running on. See <c>gamePlatform</c> schema <seealso href="https://schema.org/gamePlatform"/>
        /// </summary>
        string[] Platforms { get; set; }
    }
}
