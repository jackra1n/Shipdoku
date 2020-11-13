using Shipdoku.Enums;

namespace Shipdoku.Interfaces
{
    public interface IExportService
    {
        void ExportPlayingFieldToPng(EShipdokuField[,] shipdokuFields);
    }
}
