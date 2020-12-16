using Shipdoku.Models;

namespace Shipdoku.Interfaces
{
    public interface IShipdokuGenerator
    {
        /// <summary>
        /// Generates a new Playingfield with all the Vertical and Horizontal Ship-Counts
        /// </summary>
        /// <returns>A new ShipdokuModel with the generated Playingfield</returns>
        ShipdokuModel GenerateShipdokuModel();
    }
}
