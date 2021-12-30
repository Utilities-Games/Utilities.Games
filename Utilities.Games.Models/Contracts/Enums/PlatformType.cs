namespace Utilities.Games.Models.Contracts.Enums
{
    /// <summary>
    /// Types of video game platforms.
    /// </summary>
    public enum PlatformType
    {
        /// <summary>
        /// A type of video game platform that is typically a stationary device connected to a display.
        /// </summary>
        CONSOLE,
        /// <summary>
        /// A type of video game platform that is contains its own display and can be played anywhere.
        /// </summary>
        HANDHELD,
        /// <summary>
        /// A hybrid type of video game platform that contains its own display and can be played anywhere as well as be docked, natively, to a connected display.
        /// </summary>
        DOCKING,
        /// <summary>
        /// A type of video game platform that consists of a Virtual Reality headset and controllers.
        /// </summary>
        VR
    }
}
