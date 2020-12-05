using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using Shipdoku.Enums;
using Shipdoku.Interfaces;
using Shipdoku.Models;
using Prism.Commands;
using Prism.Common;
using System.Windows;
using System.Linq;
using Prism.Regions;

namespace Shipdoku.ViewModels
{
    public class ShipdokuViewModel : BindableBase, INavigationAware
    {
        private readonly IShipdokuGenerator _shipdokuGenerator;
        private readonly IExportService _exportService;
        private readonly IRegionManager _regionManager;
        private ShipdokuModel _shipdokuModel;
        private EShipdokuField _currentFieldType;

        public ShipdokuViewModel(IShipdokuGenerator shipdokuGenerator, IExportService exportService, IRegionManager regionManager)
        {
            _shipdokuGenerator = shipdokuGenerator;
            _exportService = exportService;
            _regionManager = regionManager;

            ShipdokuModel = new ShipdokuModel();

            ExportCommand = new DelegateCommand(Export);
            GenerateNewFieldCommand = new DelegateCommand(GenerateNewField);
            SetCurrentFieldTypeCommand = new DelegateCommand<string>(SetCurrentFieldType);
            ButtonCommand = new DelegateCommand<string>(btn_Click);
            StartMenuNavigateCommand = new DelegateCommand(StartMenuNavigate);
        }

        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand GenerateNewFieldCommand { get; set; }
        public DelegateCommand<string> SetCurrentFieldTypeCommand { get; set; }
        public DelegateCommand<string> ButtonCommand { get; set; }
        public DelegateCommand StartMenuNavigateCommand { get; private set; }

        public void btn_Click(string commandParameter)
        {
            var array = commandParameter.Split(',').Select(int.Parse).ToArray();
            PlayingField[array[0], array[1]] = _currentFieldType;
            RaisePropertyChanged(nameof(PlayingField));
        }

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


        public bool CreateEmpty { get; set; }
        public EShipdokuField[,] PlayingField => ShipdokuModel.ShipdokuField;

        private void GenerateNewField()
        {
            ShipdokuModel = _shipdokuGenerator.GenerateShipdokuModel();
        }

        private void Export()
        {
            _exportService.ExportPlayingFieldToPng(ShipdokuModel, false);
        }

        private void StartMenuNavigate()
        {
            _regionManager.RequestNavigate("ContentRegion", "StartMenu");
            ShipdokuModel = new ShipdokuModel();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            CreateEmpty = (bool)(navigationContext.Parameters["createEmpty"]);
            if (!CreateEmpty)
            {
                GenerateNewField();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            return;
        }
    }
}
