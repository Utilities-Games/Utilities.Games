using Utilities.Games.Models.Contracts;
using Utilities.Games.Models.Contracts.Enums;

namespace Utilities.Games.Models
{
    /// <inheritdoc/>
    public class Platform : IPlatform
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string Brand { get; set; }

        /// <inheritdoc/>
        public string Line { get; set; }

        /// <inheritdoc/>
        public int? Generation { get; set; }

        /// <inheritdoc/>
        public PlatformType? Type { get; set; }
    }
}
