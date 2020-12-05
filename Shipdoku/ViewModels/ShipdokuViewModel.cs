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
        private ShipdokuModel _shipdokuModel;
        private EShipdokuField _currentFieldType;

        public ShipdokuViewModel(IShipdokuGenerator shipdokuGenerator, IExportService exportService)
        {
            _shipdokuGenerator = shipdokuGenerator;
            _exportService = exportService;

            ShipdokuModel = _shipdokuGenerator.GenerateShipdokuModel();

            ExportCommand = new DelegateCommand(Export);
            GenerateNewFieldCommand = new DelegateCommand(GenerateNewField);
            SetCurrentFieldTypeCommand = new DelegateCommand<string>(SetCurrentFieldType);
        }

        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand GenerateNewFieldCommand { get; set; }
        public DelegateCommand<string> SetCurrentFieldTypeCommand { get; set; }


        public void SetCurrentFieldType(string fieldType)
        {
            _currentFieldType = Enum.Parse<EShipdokuField>(fieldType);
        }

        public ShipdokuModel ShipdokuModel
        {
            get => _shipdokuModel;
            set
            {
                SetProperty(ref _shipdokuModel, value);
                RaisePropertyChanged(nameof(PlayingField));
            }
        }


        public EShipdokuField[,] PlayingField => ShipdokuModel.ShipdokuField;

        private void GenerateNewField()
        {
            ShipdokuModel = _shipdokuGenerator.GenerateShipdokuModel();
        }

        private void Export()
        {
            // ToDo: nicht immer Solved exportieren
            _exportService.ExportPlayingFieldToPng(ShipdokuModel, false);
        }
    }
}
