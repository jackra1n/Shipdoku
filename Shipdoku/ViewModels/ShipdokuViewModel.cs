using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using Shipdoku.Enums;
using Shipdoku.Interfaces;
using Shipdoku.Models;
using Prism.Commands;
using Prism.Common;
using Prism.Mvvm;
using Shipdoku.Enums;

namespace Shipdoku.ViewModels
{
    public class ShipdokuViewModel : BindableBase
    {
        private readonly IShipdokuGenerator _shipdokuGenerator;
        private readonly IExportService _exportService;

        public ShipdokuViewModel(IShipdokuGenerator shipdokuGenerator, IExportService exportService)
        {
            _shipdokuGenerator = shipdokuGenerator;
            _exportService = exportService;

            Playingfield = _shipdokuGenerator.GenerateShipdokuModel();

            ExportCommand = new DelegateCommand(Export);
            GenerateNewFieldCommand = new DelegateCommand(GenerateNewField);
        }

        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand GenerateNewFieldCommand { get; set; }

        public ShipdokuModel Playingfield { get; set; }

        private void GenerateNewField()
        {
            Playingfield = _shipdokuGenerator.GenerateShipdokuModel();
        }

        private void Export()
        {
            // ToDo: nicht immer Solved exportieren
            _exportService.ExportPlayingFieldToPng(Playingfield, true);
        }
    }
}
