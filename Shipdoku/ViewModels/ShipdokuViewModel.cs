﻿using System;
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

            ShipdokuModel = _shipdokuGenerator.GenerateShipdokuModel();

            ExportCommand = new DelegateCommand(Export);
            GenerateNewFieldCommand = new DelegateCommand(GenerateNewField);
        }

        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand GenerateNewFieldCommand { get; set; }

        public ShipdokuModel ShipdokuModel { get; set; }

        public EShipdokuField[,] PlayingField => ShipdokuModel.SolvedShipdokuField;

        private void GenerateNewField()
        {
            ShipdokuModel = _shipdokuGenerator.GenerateShipdokuModel();
        }

        private void Export()
        {
            // ToDo: nicht immer Solved exportieren
            _exportService.ExportPlayingFieldToPng(ShipdokuModel, true);
        }
    }
}
