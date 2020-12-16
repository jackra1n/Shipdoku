using System;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Shipdoku.Enums;
using Shipdoku.Interfaces;
using Shipdoku.Models;

namespace Shipdoku.ViewModels
{
    public class ShipdokuViewModel : BindableBase, INavigationAware
    {
        private readonly IShipdokuGenerator _shipdokuGenerator;
        private readonly IExportService _exportService;
        private readonly IRegionManager _regionManager;
        private ShipdokuModel _shipdokuModel;
        private EShipdokuField _currentFieldType;
        private bool _isCreateEmpty;
        private bool _showSolution;
        private bool _canShowSolution;

        public ShipdokuViewModel(IShipdokuGenerator shipdokuGenerator, IExportService exportService, IRegionManager regionManager)
        {
            _shipdokuGenerator = shipdokuGenerator;
            _exportService = exportService;
            _regionManager = regionManager;

            ShipdokuModel = new ShipdokuModel();

            ExportCommand = new DelegateCommand(Export);
            GenerateNewFieldCommand = new DelegateCommand(GenerateNewField);
            SetCurrentFieldTypeCommand = new DelegateCommand<string>(SetCurrentFieldType);
            SetFieldCommand = new DelegateCommand<string>(SetField);
            StartMenuNavigateCommand = new DelegateCommand(StartMenuNavigate);
        }

        #region Properties

        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand GenerateNewFieldCommand { get; set; }
        public DelegateCommand<string> SetCurrentFieldTypeCommand { get; set; }
        public DelegateCommand<string> SetFieldCommand { get; set; }
        public DelegateCommand StartMenuNavigateCommand { get; }

        public ShipdokuModel ShipdokuModel
        {
            get => _shipdokuModel;
            set
            {
                SetProperty(ref _shipdokuModel, value);
                RaisePropertyChanged(nameof(PlayingField));
            }
        }

        public EShipdokuField CurrentFieldType
        {
            get => _currentFieldType;
            set => SetProperty(ref _currentFieldType, value);
        }

        public bool IsCreateEmpty
        {
            get => _isCreateEmpty;
            set => SetProperty(ref _isCreateEmpty ,value);
        }

        public EShipdokuField[,] PlayingField => ShowSolution ? ShipdokuModel.SolvedShipdokuField : ShipdokuModel.ShipdokuField;

        public bool ShowSolution
        {
            get => _showSolution;
            set
            {
                SetProperty(ref _showSolution, value);
                RaisePropertyChanged(nameof(PlayingField));
            }
        }

        public bool CanShowSolution
        {
            get => _canShowSolution;
            set => SetProperty(ref _canShowSolution, value);
        }

        #endregion

        private void SetCurrentFieldType(string fieldType)
        {
            CurrentFieldType = Enum.Parse<EShipdokuField>(fieldType);
        }

        private void SetField(string commandParameter)
        {
            var array = commandParameter.Split(',').Select(int.Parse).ToArray();
            PlayingField[array[0], array[1]] = _currentFieldType;
            RaisePropertyChanged(nameof(PlayingField));
        }

        private void GenerateNewField()
        {
            ShipdokuModel = _shipdokuGenerator.GenerateShipdokuModel();
            CanShowSolution = true;
            IsCreateEmpty = false;
        }

        private void Export()
        {
            _exportService.ExportPlayingFieldToPng(ShipdokuModel, ShowSolution);
        }

        private void StartMenuNavigate()
        {
            _regionManager.RequestNavigate("ContentRegion", "StartMenu");
            ShipdokuModel = new ShipdokuModel();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsCreateEmpty = (bool)(navigationContext.Parameters["createEmpty"]);
            CanShowSolution = !IsCreateEmpty;
            ShowSolution = false;
            if (!IsCreateEmpty)
            {
                GenerateNewField();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
