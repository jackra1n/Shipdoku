using Shipdoku.Models;

namespace Shipdoku.Interfaces
{
    public interface IExportService
    {
        /// <summary>
        /// Exports the specified Shipdoku Model
        /// </summary>
        /// <param name="shipdokuModel">Shipdoku Model to Export</param>
        /// <param name="exportSolved">Specifies, wheter the Solved or non Solved field shall be Exported</param>
        void ExportPlayingFieldToPng(ShipdokuModel shipdokuModel, bool exportSolved);
    }
}
