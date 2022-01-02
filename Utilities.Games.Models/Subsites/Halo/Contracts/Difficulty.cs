using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Games.Models.Subsites.Halo.Contracts
{
    /// <summary>
    /// Describes the level of difficulty of the campaign.
    /// </summary>
    public enum Difficulty
    {
        /// <summary>
        /// The lowest difficulty setting for the campaign.
        /// </summary>
        Easy,
        /// <summary>
        /// The default difficulty setting for the campaign.
        /// </summary>
        Normal,
        /// <summary>
        /// A slightly above-average difficulty setting for the campaign.
        /// </summary>
        Heroic,
        /// <summary>
        /// The highest difficulty setting for the campaign.
        /// </summary>
        Legendary,
        /// <summary>
        /// A fan-created difficulty that requires the difficulty to be set to Legendary and activating all skulls.
        /// </summary>
        LASO,
        /// <summary>
        /// A fan-created difficulty similar to LASO, but only allows for one player.
        /// </summary>
        SLASO
    }
}
