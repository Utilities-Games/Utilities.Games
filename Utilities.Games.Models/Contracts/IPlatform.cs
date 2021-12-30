using Utilities.Games.Models.Contracts.Enums;

namespace Utilities.Games.Models.Contracts
{
    public interface IPlatform {
        /// <summary>
        /// Name of the platform that supports playing of video game(s).
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Name of the marketing brand this platform was created by.
        /// </summary>
        string Brand { get; set; }

        /// <summary>
        /// Describes the line for which the platform derives from. For example, the Gameboy Color is part of the Gameboy line.
        /// </summary>
        string Line { get; set; }

        /// <summary>
        /// Describes which generation of video games this platform was released for. Typically synonymous with the Console Wars.
        /// </summary>
        int? Generation { get; set; }

        /// <summary>
        /// Describes the form type of the platform.
        /// </summary>
        PlatformType? Type { get; set; }
    }
}
